using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using RentItServer.Utilities;
using System.Threading;
using System.Timers;

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

        //List of ids of running channels;
        private Dictionary<int, EzProcess> runningChannelIds;

        //The logger
        private Logger _logger;

        private System.Timers.Timer timer;

        private List<TrackPlay> NewTrackPlays;

        #endregion

        #region Constructor
        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private StreamHandler()
        {
            _fileSystemHandler = FileSystemDao.GetInstance();
            _dao = DatabaseDao.GetInstance();
            runningChannelIds = new Dictionary<int, EzProcess>();
            NewTrackPlays = new List<TrackPlay>();

            //InitTimer();
        }
        #endregion

        #region GetInstance()
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
        #endregion

        #region InitTimer()
        public void InitTimer()
        {
            /*_logger.AddEntry("Init timer");
            timer = new System.Timers.Timer();
            timer.Interval = 86400000; //24 hours
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = false; //timer.AutoReset = true;
            _logger.AddEntry("Start timer");
            timer.Start();*/
        }
        #endregion

        #region AddLogger(Logger logger)
        public void AddLogger(Logger logger)
        {
            _logger = logger;
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

        #region ManualStreamStart(int channelId)
        /// <summary>
        /// Starts the stream of the specified channel. Runs ezstream and starts a countinous operation. Requires icecast to be running.
        /// </summary>
        /// <param name="channelId">Channel id of the channel to be started</param>
        public void ManualStreamStart(int channelId) // rename to something that says it is the first time the stream is being started and write a method for starting the stream when it has been of(is that even necessary?)
        {
            _logger.AddEntry("Manually starting stream for channel with id: " + channelId);
            if (!IsChannelPlaying(channelId)) // Check if stream is already running
            {
                if(_dao.ChannelHasTracks(channelId))
                {
                    StartChannelStream(channelId);
                    AddNewTrackPlays();
                }
                else
                {
                    _logger.AddEntry("Channel with id: " + channelId + " has no associated tracks");
                    throw new NoTracksOnChannelException("Channel with id: " + channelId + " has no associated tracks");
                }
            }
            else //channel is already running
            {
                _logger.AddEntry("Channel with id: " + channelId + " is already running");
                throw new ChannelRunningException("Channel with id: " + channelId + " is already running");
            }
        }
        #endregion

        #region StartChannelStream(int channelId)
        private void StartChannelStream(int channelId)
        {
            // make sure it has a config file
            string xmlFilePath;
            xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
            if (!_fileSystemHandler.Exists(xmlFilePath))
            {
                CreateChannelConfigFile(channelId);
            }

            // generate m3u file
            int playTime = 86400000; //24 hours
            GenerateM3UFile(channelId, playTime);


            // start stream process
            EzProcess p = StartEzstreamProcess(channelId); //CHECK AT DEN IKKE KØRER

            // add process to list of active streams
            //_logger.AddEntry("[StartChannelStream]: Adding to dictionary");
            //runningChannelIds.Add(channelId, p);
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

        #region GenerateM3UFile(int channelId, int playTime)
        private void GenerateM3UFile(int channelId, int playTime)
        {
            List<TrackPlay> addedTrackPlays;
            List<Track> playlist = GeneratePlaylist(channelId, playTime, out addedTrackPlays);
            NewTrackPlays.AddRange(addedTrackPlays);

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

        #region StartEzstreamProcess(int channelId)
        private EzProcess StartEzstreamProcess(int channelId)
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

                //startInfo.RedirectStandardInput = true; // MAYBE NEEDED FOR WHEN WE TEST CHANGE SONG VIA COMMAND LINE INPUT
                //In order to redirect the standard input for ezstream into this program
                //startInfo.RedirectStandardOutput = true;

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
                _logger.AddEntry("Process started for channel with id: " + channelId);

                //Add this process to the dictionary with running channels
                _logger.AddEntry("[StartEzstreamProcess]: Adding to dictionary");
                runningChannelIds.Add(channelId, p);
                return p;
            }
            else
            {
                _logger.AddEntry("Channel with id: " + channelId + " is already running");
                throw new ChannelRunningException("Channel with id: " + channelId + " is already running");
            }
        }
        #endregion

        #region AddNewTrackPlays()
        private void AddNewTrackPlays()
        {
            AddTrackPlayList(NewTrackPlays);
            NewTrackPlays.Clear();
        }
        #endregion

        #region AddTrackPlayList(List<TrackPlay> tracks)
        private void AddTrackPlayList(List<TrackPlay> trackPlayList)
        {
            _dao.AddTrackPlayList(trackPlayList);
        }
        #endregion
        
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


            // Close all streams
            CloseAllStreams();
            // clear dictionary of active streams
            runningChannelIds.Clear();

            // Find all channel ids for channels with tracks
            List<Channel> channels = _dao.GetChannelsWithTracks();

            foreach (Channel c in channels) // make a method which call all these
            {
                StartChannelStream(c.Id);
            }

            AddNewTrackPlays();
        }
        #endregion

        #region CloseAllStreams()
        private void CloseAllStreams()
        {
            _logger.AddEntry("Start killing all running ezstream processes");
            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
            {
                if (myProc.ProcessName == "ezstream")
                {
                    myProc.Kill();
                }
            }
            _logger.AddEntry("All ezstream processes have been killed");
        }
        #endregion




        #region StopStream(int channelId)  Out commented
        /*/// <summary>
        /// Stops the stream of a channel and sets it is not running.
        /// </summary>
        /// <param name="channelId">The id of the channel to be stopped</param>
        public void StopStream(int channelId)
        {
            //Notes: if p.Id is unique for every ezstream, then we can just loop through all running processes and kill the specific ezstream. If this is true, we can also drop the 24 hour cycle and kill a process after each song and start a new one. If this is done we should completely remove the 24 hour cycle in order to not fuck with TrackPlays and be consistent



            if (IsChannelRunning(channelId))
            {
                EzProcess p = runningChannelIds[channelId];
                p.Close();
                runningChannelIds.Remove(channelId);
            }
            else
            {
                throw new ChannelRunningException("The channel is not running");
            }
        }*/
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