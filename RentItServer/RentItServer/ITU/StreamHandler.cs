using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using RentItServer.Utilities;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;

namespace RentItServer.ITU
{
    public class StreamHandler
    {
        #region Fields
        //Singleton instance of the class
        private static StreamHandler _instance;
        //FileSystemHandler
        private static FileSystemDao _fileSystemHandler;
        //DAO
        private static DatabaseDao _dao;

        // A dictionary with all running channels' id and corresponding ezprocess
        private Dictionary<int, EzProcess> runningChannelIds;

        // The logger
        private Logger _logger;

        // This timer is to detect 3AM every day in order to restart all channels
        private System.Timers.Timer timer;

        // List containing all TrackPlays which have not yet been inserted into the database
        private List<TrackPlay> newTrackPlays;

        // List containing the windows process id of all running ezstream processes
        private List<int> ezstreamProcessIds;

        #endregion

        #region Initial methods
        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private StreamHandler()
        {
            // Get singleton instancees
            _fileSystemHandler = FileSystemDao.GetInstance();
            _dao = DatabaseDao.GetInstance();

            //Initialize collections
            runningChannelIds = new Dictionary<int, EzProcess>();
            newTrackPlays = new List<TrackPlay>();
            ezstreamProcessIds = new List<int>();
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static StreamHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StreamHandler();
            }
            return _instance;
        }

        /// <summary>
        /// Initializes the timer to elapse every day at 3AM
        /// </summary>
        public void InitTimer()
        {
            _logger.AddEntry("Initialize timer");
            timer = new System.Timers.Timer();

            //Calculate how long time the first interval should be
            timer.Interval = MillisecondsUntilReset();

            //Set handler for timer elapse
            timer.Elapsed += timer_Elapsed;
            //Set the timer to elapse more than the first time
            timer.AutoReset = true;
            _logger.AddEntry("Start timer");
            timer.Start();
        }

        /// <summary>
        /// Set the logger which should be used in the stream handler
        /// </summary>
        /// <param name="logger"></param>
        public void SetLogger(Logger logger)
        {
            _logger = logger;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The date that the reset of all channel streams should take place
        /// </summary>
        private DateTime ResetDate
        {
            get
            {
                //For testing!
                //DateTime resetDate = DateTime.Now;
                //resetDate = resetDate.AddMinutes(15);
                //return resetDate;
                //endFor


                //Creates the time now and adds to that value
                DateTime resetDate = DateTime.Now;
                if (resetDate.Hour > 3) // in case the server is restarted before 3AM one day
                {
                    resetDate = resetDate.AddDays(1);
                }
                resetDate = resetDate.AddHours(3 - resetDate.Hour);
                resetDate = resetDate.AddMinutes(-resetDate.Minute);
                resetDate = resetDate.AddMilliseconds(-resetDate.Millisecond);
                return resetDate;
                
            }
        }
        #endregion

        #region Helper methods
        #region MillisecondsUntilReset()
        /// <summary>
        /// Calculates how many miliseconds there is until next reset if channel streams
        /// </summary>
        /// <returns></returns>
        private int MillisecondsUntilReset()
        {
            return (int)(ResetDate - DateTime.Now).TotalMilliseconds;
        }
        #endregion

        #region IsChannelPlaying(int channelId)
        public bool IsChannelPlaying(int channelId)
        {
            try
            {
                if (runningChannelIds[channelId] != null)
                {
                    return true;
                }
            }
            catch (KeyNotFoundException) 
            { 
                //_logger.AddEntry("[IsChannelRunning]: KeyNotFoundException when looking for channelId: " + channelId); 
            }
            return false;
        }
        #endregion
        #endregion

        #region ManualStreamStart(int channelId)
        /// <summary>
        /// Method to manually start the stream for a channel
        /// Throws NoTracksOnChannelException if there are no tracks on the channel with the given id
        /// Throws ChannelRunningException if the channel already has a running stream
        /// </summary>
        /// <param name="channelId">The id of the channel which should start</param>
        public void ManualStreamStart(int channelId)
        {
            _logger.AddEntry("Manually starting stream for channel with id: " + channelId);
            if (!IsChannelPlaying(channelId)) // Check if the channel already has a running stream
            {
                if(_dao.ChannelHasTracks(channelId)) // Check if the channel has any associated tracks
                {
                    _logger.AddEntry("Starting channel stream for channel with id: " + channelId);
                    //Start the channel stream
                    StartChannelStream(channelId);
                    //Add all new track plays to the database
                    AddNewTrackPlays();
                }
                else // Tjhe channel has no associated tracks
                {
                    _logger.AddEntry("Channel with id: " + channelId + " has no associated tracks");
                    throw new NoTracksOnChannelException("Channel with id: " + channelId + " has no associated tracks");
                }
            }
            else //channel already has a running stream
            {
                _logger.AddEntry("Channel with id: " + channelId + " is already running");
                throw new ChannelRunningException("Channel with id: " + channelId + " is already running");
            }
        }
        #endregion

        #region StopStream(int channelId)
        /// <summary>
        /// Method to stop the stream process of a specific channel
        /// </summary>
        /// <param name="channelId">Channel id for the channel which should have stopped its streaming process</param>
        public void StopChannelStream(int channelId)
        {
            _logger.AddEntry("Starting stopping channel with id: " + channelId);
            
            //The process for the given channel id
            EzProcess p;
            try
            {
                p = runningChannelIds[channelId];
            }
            catch(KeyNotFoundException) //The given channel id has no running streaming process
            {
                _logger.AddEntry("Channel with id: " + channelId + " is not running");
                throw new ChannelRunningException("Channel with id: " + channelId + " is not running");
            }
            
            //Loop through all windows processes named "ezstream"
            foreach (Process process in System.Diagnostics.Process.GetProcessesByName("ezstream"))
            {
                if (process.Id == p.RealProcessId) //if the windows process has the same id as the process associated with the given channel id
                {
                    //Kill the process
                    process.Kill();
                    //Remove the id from active processes' widnows ids
                    ezstreamProcessIds.Remove(p.RealProcessId);
                    //Remove
                    runningChannelIds.Remove(channelId);
                    _logger.AddEntry("Ezstream process for channel with id: " + channelId + " has been killed");
                }
            }

            //TODO: FJERN ALLE TRACKPLAYS SOM IKKE ER BLEVET SPILLET!
        }
        #endregion

        #region StartChannelStream(int channelId)
        /// <summary>
        /// Method to start a channels stream
        /// </summary>
        /// <param name="channelId"></param>
        private void StartChannelStream(int channelId)
        {
            // Start stream process
            if (!IsChannelPlaying(channelId)) // Check if channel already has a running stream
            {
                // Make sure the channel has a config file
                string xmlFilePath;
                xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
                if (!_fileSystemHandler.Exists(xmlFilePath))
                {
                    CreateChannelConfigFile(channelId);
                }
            
                // Generate m3u file for the channel
                // Calculate the playtime for the m3u file. Time until next reset
                int playTime = MillisecondsUntilReset();
                //Generate the m3u file
                GenerateM3UFile(channelId, playTime);


                // Start stream process
                StartEzstreamProcess(channelId);
            }
            else // Channel already has a running stream
            {
                _logger.AddEntry("Channel with id: " + channelId + " is already running");
                throw new ChannelRunningException("Channel with id: " + channelId + " is already running");
            }
        }
        #endregion

        #region CreateChannelConfigfile(int channelId)
        private void CreateChannelConfigFile(int channelId)
        {
            _logger.AddEntry("Generate config xml for channel with id: " + channelId);
            //Generate the xml for the config file
            string xml;
            xml = XMLGenerator.GenerateConfig(channelId, FilePath.ITUM3uPath.GetPath() + channelId + ".m3u");
            //Create the xmlFilePath
            string xmlFilePath;
            xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
            _logger.AddEntry("Generating config file for channel with id: " + channelId);
            //Write the config file to the system
            FileSystemDao.GetInstance().WriteFile(xml, xmlFilePath);
        }
        #endregion

        #region M3U methods
        #region GenerateM3UFile(int channelId, int playTime)
        private void GenerateM3UFile(int channelId, int playTime)
        {
            List<TrackPlay> addedTrackPlays;
            List<Track> playlist = GeneratePlaylist(channelId, playTime, out addedTrackPlays);
            newTrackPlays.AddRange(addedTrackPlays);

            string filePath = FilePath.ITUM3uPath.GetPath() + channelId + ".m3u";
            _fileSystemHandler.WriteM3UPlaylistFile(filePath, playlist);
        }
        #endregion

        #region GeneratePlaylist(int channelId, int playTime, out List<TrackPlay> addedTrackPlays)
        private List<Track> GeneratePlaylist(int channelId, int playTime, out List<TrackPlay> addedTrackPlays)
        {
            List<Track> channelTracks = _dao.GetTrackList(channelId);
            if (!channelTracks.Any())
            {
                _logger.AddEntry("Channel with id: " + channelId + " has no associated tracks");
                throw new NoTracksOnChannelException("Channel with id: " + channelId + " has no associated tracks");
            }

            List<TrackPlay> trackPlays = _dao.GetTrackPlays(channelId);

            List<Track> playlist = TrackPrioritizer.GetInstance().GetNextPlayList(channelTracks, trackPlays, playTime, out addedTrackPlays);

            return playlist;
        }
        #endregion
        #endregion

        #region StartEzstreamProcess(int channelId)
        private void StartEzstreamProcess(int channelId)
        {
            if (!IsChannelPlaying(channelId))
            {
                //Start set up the process
                //Path to ezstream executable
                string ezPath = FilePath.ITUEzStreamPath.GetPath();

                //Create the arguments
                string xmlFilePath;
                xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
                string arguments = "-c " + xmlFilePath;

                //Start set up process info
                ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c " + ezPath + " " + arguments);

                //Default is true, it should be false for ezstream
                startInfo.UseShellExecute = false;
                //It should not create a new window for the ezstream process
                startInfo.CreateNoWindow = true;

                //Create the process
                _logger.AddEntry("Creating process for channel with id: " + channelId);
                EzProcess p = new EzProcess(channelId);
                p.StartInfo = startInfo;

                _logger.AddEntry("Starting process for channel with id: " + channelId);
                p.Start();
                _logger.AddEntry("Process started for channel with id: " + channelId + " with process id: " + p.Id);
                //AssignProcessId(p);
                Task t = new Task(() => AssignProcessId(p));
                t.Start();
                //Task.Factory.StartNew(() => AssignProcessId(p), );

                //Add this process to the dictionary with running channels
                _logger.AddEntry("[StartEzstreamProcess]: Adding to dictionary");
                runningChannelIds.Add(channelId, p);
            }
            else
            {
                _logger.AddEntry("Channel with id: " + channelId + " is already running");
                throw new ChannelRunningException("Channel with id: " + channelId + " is already running");
            }
        }
        #endregion

        #region AssignProcessId(EzProcess p)
        private void AssignProcessId(EzProcess p)
        {
            _logger.AddEntry("Start assign process id for channel with id: " + p.ChannelId);
            Thread.Sleep(1000);
            Process[] activeProcesses = Process.GetProcessesByName("ezstream");
            foreach (Process process in activeProcesses)
            {
                if (!ezstreamProcessIds.Contains(process.Id))
                {
                    p.RealProcessId = process.Id;
                    ezstreamProcessIds.Add(process.Id);
                    _logger.AddEntry("Process for channel with id: " + p.ChannelId + " has been assign process id: " + p.RealProcessId);
                    break;
                }
            }
        }
        #endregion

        #region Add TrackPlay methods
        #region AddNewTrackPlays()
        private void AddNewTrackPlays()
        {
            AddTrackPlayList(newTrackPlays);
            newTrackPlays.Clear();
        }
        #endregion

        #region AddTrackPlayList(List<TrackPlay> tracks)
        private void AddTrackPlayList(List<TrackPlay> trackPlayList)
        {
            _logger.AddEntry("Starting adding trackplays from given list to database");
            _dao.AddTrackPlayList(trackPlayList);
        }
        #endregion
        #endregion

        #region Methods to restart all channels
        #region timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //Reset all streams
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Game plan:
            // Close all streams
            // clear dictionary of active streams
            // Find all channel ids for channels with tracks
            // Start one channel at a time
                // make sure it has a config file
                // generate m3u file
                // start stream process
                // add process to list of active streams

            _logger.AddEntry("Start restart of all streams");
            timer.Interval = 86400000; //Set timer interval to 24hours

            // Close all streams
            CloseAllStreams();
            
            // Find all channel ids for channels with tracks
            List<Channel> channels = _dao.GetChannelsWithTracks();

            foreach (Channel c in channels) // make a method which call all these
            {
                _logger.AddEntry("Restarting channel with id: " + c.Id);
                StartChannelStream(c.Id);
            }

            //Add all new trackplays to database
            AddNewTrackPlays();
        }
        #endregion

        #region CloseAllStreams()
        private void CloseAllStreams()
        {
            _logger.AddEntry("Start killing all running ezstream processes");
            foreach (Process p in System.Diagnostics.Process.GetProcessesByName("ezstream"))
            {
                p.Kill();
            }
            _logger.AddEntry("All ezstream processes have been killed");

            // clear dictionary of active streams
            _logger.AddEntry("Clearing all runningChannelIds");
            runningChannelIds.Clear();
        }
        #endregion
        #endregion
    }

    #region Custom exceptions
    class NoTracksOnChannelException : Exception
    {
        public NoTracksOnChannelException(string message)
            : base(message)
        { }
    }

    class ChannelRunningException : Exception
    {
        public ChannelRunningException(string message)
            : base(message)
        { }
    }
    #endregion
}