﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using RentItServer.ITU.Exceptions;
using RentItServer.Utilities;
using System.Threading;
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
        //
        private static TrackPrioritizer _trackPrioritizer;

        // A dictionary with all running channels' id and corresponding ezprocess
        private readonly Dictionary<int, EzProcess> runningChannelIds;

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
            _trackPrioritizer = TrackPrioritizer.GetInstance();

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

        /// <summary>
        /// Stops all running ezstream processes
        /// Removes all trackplays with playtime after the time of method call
        /// </summary>
        public void Cleanup()
        {
            _logger.AddEntry("Starting cleanup");
            //stop all ezprocesses
            CloseAllStreams();
            //remove all deletetracks
            DeleteAllFutureTrackPlays();
        }
        #endregion

        #region Properties
        /// <summary>
        /// The date that the reset of all channel streams should take place
        /// </summary>

        //The next reset date for all streams
        private DateTime ResetDate
        {
            get
            {
                const int restartHour = 4;
                //Creates the time now and adds to that value so that it is set to the next occourence of 4AM
                DateTime resetDate = DateTime.Now;
                if (resetDate.Hour >= restartHour) // in case the server is restarted before 4AM one day
                {
                    resetDate = resetDate.AddDays(1);
                }
                resetDate = resetDate.AddHours(restartHour - resetDate.Hour);
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

        #region IsChannelStreamRunning(int channelId)
        /// <summary>
        /// Check if a channel has an active ezstream process
        /// </summary>
        /// <param name="channelId">The id of the channel to check</param>
        /// <returns>If there is a running ezstream process</returns>
        public bool IsChannelStreamRunning(int channelId)
        {
            try
            {
                if (runningChannelIds[channelId] != null) // There is a ezstream process running
                {
                    return true;
                }
            }
            catch (KeyNotFoundException) //There is not a running process
            {
                //Dont do anything
            }
            return false;
        }
        #endregion

        /// <summary>
        /// Determines if it is allowed to call start and stop channels
        /// </summary>
        /// <returns>If it is allowed to start and stop channels</returns>
        public bool CanStartStopChannels()
        {
            return !(ResetDate.AddMinutes(-5) < DateTime.Now && DateTime.Now < ResetDate.AddMinutes(10)); 
        }
        #endregion

        #region Methods regarding starting a stream
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
            if (!IsChannelStreamRunning(channelId)) // Check if the channel already has a running stream
            {
                if (_dao.ChannelHasTracks(channelId)) // Check if the channel has any associated tracks
                {
                    //Start the channel stream
                    StartChannelStream(channelId);
                    //Add all new track plays to the database
                    AddTrackPlayList();
                }
                else // The channel has no associated tracks
                {
                    _logger.AddEntry("Channel with id: " + channelId + " has no associated tracks");
                    throw new NoTracksOnChannelException("Channel with id: " + channelId + " has no associated tracks");
                }
            }
            else // Channel already has a running stream
            {
                _logger.AddEntry("Channel with id: " + channelId + " is already running");
                throw new ChannelRunningException("Channel with id: " + channelId + " is already running");
            }
        }
        #endregion

        #region StartChannelStream(int channelId)
        /// <summary>
        /// Method to start a channels stream
        /// </summary>
        /// <param name="channelId"></param>
        private void StartChannelStream(int channelId)
        {
            _logger.AddEntry("Starting channel stream for channel with id: [" + channelId + "]");
            // Start stream process
            if (!IsChannelStreamRunning(channelId)) // Check if channel already has a running stream
            {
                // Make sure the channel has a config file
                string xmlFilePath;
                xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
                if (!_fileSystemHandler.Exists(xmlFilePath))
                {
                    CreateChannelConfigFile(channelId);
                }
                else
                {
                    _logger.AddEntry("Channel with id: [" + channelId + "] already has a config file");
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
                _logger.AddEntry("Channel with id: [" + channelId + "] is already running");
                throw new ChannelRunningException("Channel with id: [" + channelId + "] is already running");
            }
        }
        #endregion

        #region CreateChannelConfigfile(int channelId)
        /// <summary>
        /// Create a config file for the given channel id
        /// </summary>
        /// <param name="channelId">Channel id for the channel which should have a config file created</param>
        private void CreateChannelConfigFile(int channelId)
        {
            _logger.AddEntry("Starting generate config xml for channel with id: [" + channelId + "]");

            //Generate the xml for the config file
            string xml;
            xml = XMLGenerator.GenerateConfigXML(channelId, FilePath.ITUM3uPath.GetPath() + channelId + ".m3u");

            //Create the config filepath
            string xmlFilePath;
            xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";

            _logger.AddEntry("Writing config file for channel with id: [" + channelId + "] to file system");
            //Write the config file to the system
            try
            {
                FileSystemDao.GetInstance().WriteFile(xml, xmlFilePath);
            }
            catch (Exception e)
            {
                _logger.AddEntry("Exception when trying to write config file for channel with id: [" + channelId + "]. Xml content: [" + xml + "] to filepath: [" + xmlFilePath + "]. Exception: " + e);
                throw e;
            }
        }
        #endregion

        #region M3U methods
        #region GenerateM3UFile(int channelId, int playTime)
        /// <summary>
        /// Generates an M3U file on the system and fills it with tracks from the given channelid.
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <param name="playTime">The playtime of the generated M3U file</param>
        private void GenerateM3UFile(int channelId, int playTime)
        {
            _logger.AddEntry("Begin generate m3u file for channel with id: [" + channelId + "] with playtime: [" + playTime + "]");
            //List to contain all the trackplays for the tracks on the playlist
            List<TrackPlay> addedTrackPlays;
            //Generate the playlist
            List<Track> playlist = GeneratePlaylist(channelId, playTime, out addedTrackPlays);
            //Add the trackplays to the list of newly added trackplays
            newTrackPlays.AddRange(addedTrackPlays);

            _logger.AddEntry("Start writing m3u for channel with id: [" + channelId + "] file to filesystem");
            //Write the m3u file to the filesystem
            string filePath = FilePath.ITUM3uPath.GetPath() + channelId + ".m3u";
            try
            {
                _fileSystemHandler.WriteM3UPlaylistFile(filePath, playlist);
            }
            catch (Exception e)
            {
                _logger.AddEntry("Exception when trying to write m3u file for channel with id: [" + channelId + "] to filepath: [" + filePath + "]. m3u contains: [" + playlist.Count + "] tracks. Exception: " + e);
            }
        }
        #endregion

        #region GeneratePlaylist(int channelId, int playTime, out List<TrackPlay> addedTrackPlays)
        /// <summary>
        /// Generates an a list filled with tracks
        /// </summary>
        /// <param name="channelId">Channel id for the channel which should have a playlist generated</param>
        /// <param name="playTime">The length of the playlist</param>
        /// <param name="addedTrackPlays">List which will be filled with trackplays for all tracks in the playlist</param>
        /// <returns>A list of tracks in the order they should be played according to the trackplays generated</returns>
        private List<Track> GeneratePlaylist(int channelId, int playTime, out List<TrackPlay> addedTrackPlays)
        {
            _logger.AddEntry("Start generate playlist with playtime: [" + playTime + "] for channel with id: [" + channelId + "]");
            //Get all tracks on the channel
            List<Track> channelTracks = _dao.GetTrackList(channelId);
            if (!channelTracks.Any()) //Check that the channel has any tracks
            {
                _logger.AddEntry("Channel with id: [" + channelId + "] has no associated tracks");
                throw new NoTracksOnChannelException("Channel with id: [" + channelId + "] has no associated tracks");
            }

            //Get all trackplays for the channel
            List<TrackPlay> trackPlays = _dao.GetTrackPlays(channelId, DateTime.Now.AddDays(-2));

            //Generate the playlist
            List<Track> playlist = _trackPrioritizer.GetNextPlayList(channelTracks, trackPlays, playTime, out addedTrackPlays);
            _logger.AddEntry("Created playlist with count: [" + playlist.Count + "] tracks for channel with id: [" + channelId + "]");
            //return the playlist
            return playlist;
        }
        #endregion
        #endregion

        #region StartEzstreamProcess(int channelId)
        /// <summary>
        /// Starts an ezstream process for the given channel id
        /// </summary>
        /// <param name="channelId">Channel id for the channel which should have an ezstram started</param>
        private void StartEzstreamProcess(int channelId)
        {
            _logger.AddEntry("Start starting ezstream process for channel with id: [" + channelId + "]");
            if (!IsChannelStreamRunning(channelId)) //Check if the channel already has a running stream
            {
                //Start set up the process
                //Path to ezstream executable
                string ezPath = FilePath.ITUEzStreamPath.GetPath();

                //Create the arguments to ezstream
                string xmlFilePath;
                xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
                string arguments = "-c " + xmlFilePath;

                //Start set up process-info
                ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c " + ezPath + " " + arguments);
                //Default is true, it should be false for ezstream
                startInfo.UseShellExecute = false;
                //It should not create a new window for the ezstream process
                startInfo.CreateNoWindow = true;

                //Create the process for ezstream
                EzProcess p = new EzProcess(channelId);
                p.StartInfo = startInfo;

                _logger.AddEntry("Starting process for channel with id: [" + channelId + "]");
                try
                {
                    p.Start();
                }
                catch (Exception e)
                {
                    _logger.AddEntry("Exception when trying to start ezprocess for channel with id: [" + channelId + "]. EzStream path: [" + ezPath + "] - XmlFilePath: [" + xmlFilePath + "] - Arguments: [" + arguments + "]. Exception: " + e);
                    throw e;
                }
                _logger.AddEntry("Process started for channel with id: [" + channelId + "]");

                //Start asynchronous assignment of process id to the newly created process
                Task t = new Task(() => AssignProcessId(p));
                t.Start();

                //Add this process to the dictionary with running channels
                runningChannelIds.Add(channelId, p);
            }
            else // Channel already has a running ezstream
            {
                _logger.AddEntry("Channel with id: [" + channelId + "] is already running");
                throw new ChannelRunningException("Channel with id: [" + channelId + "] is already running");
            }
        }
        #endregion

        #region AssignProcessId(EzProcess p)
        /// <summary>
        /// Sets the RealProcessId property for the given EzProcess
        /// This is used because the default EzProcess.id is the id of "cmd" and not the "ezstream" process
        /// </summary>
        /// <param name="p">Process which should have assigned its RealProcessId property</param>
        private void AssignProcessId(EzProcess p)
        {
            _logger.AddEntry("Start assign process id for channel with id: [" + p.ChannelId + "]");
            Thread.Sleep(1000);
            _logger.AddEntry("Sleep has finished for channel with id: [" + p.ChannelId + "]");
            //Loop through all windows processes names "ezstream"
            bool sucess = false;
            foreach (Process process in Process.GetProcessesByName("ezstream"))
            {
                if (!ezstreamProcessIds.Contains(process.Id)) //If the windows process has an id which is not in the list of already running process ids
                {
                    //Set the RealProcessId
                    p.RealProcessId = process.Id;
                    //Add the id to the list of running processes ids
                    ezstreamProcessIds.Add(process.Id);
                    sucess = true;
                    _logger.AddEntry("Process for channel with id: [" + p.ChannelId + "] has been assigned process id: [" + p.RealProcessId + "]");
                    break;
                }
            }
            if (!sucess)
            {
                _logger.AddEntry("Unable to find process id for channel with id: [" + p.ChannelId + "]");
            }
        }
        #endregion
        #endregion

        #region Methods regarding stopping a stream
        #region StopStream(int channelId)
        /// <summary>
        /// Method to stop the stream process of a specific channel
        /// </summary>
        /// <param name="channelId">Channel id for the channel which should have stopped its streaming process</param>
        public void StopChannelStream(int channelId)
        {
            _logger.AddEntry("Starting stopping stream for channel with id: [" + channelId + "]");

            //The process for the given channel id
            EzProcess p;
            try
            {
                p = runningChannelIds[channelId];
            }
            catch (KeyNotFoundException) //The given channel id has no running streaming process
            {
                _logger.AddEntry("Channel with id: [" + channelId + "] is not running");
                throw new ChannelNotRunningException("Channel with id: [" + channelId + "] is not running");
            }


            //Loop through all windows processes named "ezstream"
            foreach (Process process in System.Diagnostics.Process.GetProcessesByName("ezstream"))
            {
                if (process.Id == p.RealProcessId) //if the windows process has the same id as the process associated with the given channel id
                {
                    try
                    {
                        //Kill the process
                        process.Kill();
                        _logger.AddEntry("Ezstream process for channel with id: [" + channelId + "] and ReadlProcessId: [" + p.RealProcessId + "] has been killed");
                    }
                    catch (System.ComponentModel.Win32Exception e)
                    {
                        _logger.AddEntry("Unable to kill ezstream process for channel with id: [" + channelId + "]. Exception: " + e);

                    }
                }
            }
            try
            {
                //Remove the id from active processes' widnows ids
                ezstreamProcessIds.Remove(p.RealProcessId);
                //Remove
                runningChannelIds.Remove(channelId);
                //Delete all the trackplays which have not yet been played
                DeleteTrackPlays(channelId, DateTime.Now);
                //Set success to true
                _logger.AddEntry("Ezstream process for channel with id: [" + channelId + "] has been cleaned");
            }
            catch (Exception e)
            {
                _logger.AddEntry("Unable to cleanup after killing ezstream process for channel with id: [" + channelId + "]. Exception: " + e);
                throw e;
            }

        }
        #endregion

        #region DeleteTrackPlays(int channelId, DateTime datetime)
        /// <summary>
        /// Deletes all trackplays for the given channel id which have a playtime after the fiven datetime object
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <param name="datetime">The lower bound of the time</param>
        public void DeleteTrackPlays(int channelId, DateTime datetime)
        {
            _logger.AddEntry("Start deleting trackplays for channel with id: [" + channelId + "] after datetime: [" + datetime.ToLongDateString() + " " + datetime.ToLongTimeString() + "]");
            try
            {
                _dao.DeleteTrackPlaysByChannelId(channelId, datetime);
            }
            catch (Exception e)
            {
                _logger.AddEntry("Error occoured while trying to delete trackplays in database for channel with id: [" + channelId + "]. Exception: " + e);
                throw e;
            }
        }
        #endregion
        #endregion

        #region Add TrackPlay methods
        #region AddTrackPlayList()
        /// <summary>
        /// Adds all the trackplays from newTrackPlays-list to the database
        /// </summary>
        private void AddTrackPlayList()
        {
            AddTrackPlayList(newTrackPlays);
            //Clear the list
            newTrackPlays.Clear();
        }
        #endregion

        #region AddTrackPlayList(List<TrackPlay> tracks)
        /// <summary>
        /// Adds all trackplays in the given list to the database
        /// </summary>
        /// <param name="trackPlayList"></param>
        private void AddTrackPlayList(List<TrackPlay> trackPlayList)
        {
            _logger.AddEntry("Starting adding count: [" + trackPlayList.Count + "] trackplays from given list to database");
            try
            {
                _dao.AddTrackPlayList(trackPlayList);
            }
            catch (Exception e)
            {
                _logger.AddEntry("Error occoured while trying to add trackplays in database. Tracklist contains: [" + trackPlayList.Count + "] tracks. Exception: " + e);
                throw e;
            }
        }
        #endregion
        #endregion

        #region Methods to restart all channels
        #region timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        /// <summary>
        /// Method called when timer elapses.
        /// It resets all ezstreams appropriately and start all channels with associated tracks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            timer.Interval = (ResetDate - DateTime.Now).TotalMilliseconds;//86400000;//Set timer interval to 24hours 
            _logger.AddEntry("New timer interval: [" + timer.Interval + "]");

            // Close all streams
            CloseAllStreams();

            //Remove future trackplays
            foreach (EzProcess c in runningChannelIds.Values)
            {
                //Delete all the trackplays which have not yet been played
                DeleteTrackPlays(c.ChannelId, DateTime.Now);
            }

            // clear dictionary of active streams
            _logger.AddEntry("Clearing all runningChannelIds and process ids");
            runningChannelIds.Clear();
            ezstreamProcessIds.Clear();

            // Find all channels with tracks associated
            List<Channel> channels = _dao.GetChannelsWithTracks();
            //Loop trhough all the channels and start their stream
            foreach (Channel c in channels)
            {
                _logger.AddEntry("Restarting channel with id: [" + c.Id + "]");
                StartChannelStream(c.Id);
            }

            //Add all new trackplays to the database
            AddTrackPlayList();
        }
        #endregion

        #region Cleanup
        #region CloseAllStreams()
        /// <summary>
        /// Closes all windows processes with the name "ezstream"
        /// </summary>
        private void CloseAllStreams()
        {
            _logger.AddEntry("Start killing all running ezstream processes");

            //Loop trhough all windows processes called "ezstream" and kill them
            foreach (Process p in System.Diagnostics.Process.GetProcessesByName("ezstream"))
            {
                try
                {
                    p.Kill();
                }
                catch (Exception e)
                {
                    _logger.AddEntry("Unable to kill ezstream process with id: [" + p.Id + "]. Process name: [" + p.ProcessName + "]. Exception: " + e);
                    throw e;
                }
            }
            _logger.AddEntry("All ezstream processes have been killed");
        }

        /// <summary>
        /// Delete all future trackplays in the database
        /// </summary>
        private void DeleteAllFutureTrackPlays()
        {
            DateTime datetime = DateTime.Now;
            _logger.AddEntry("Start deleting trackplays after datetime: [" + datetime.ToLongDateString() + " " + datetime.ToLongTimeString() + "]");
            try
            {
                _dao.DeleteOlderTrackPlays(datetime);
            }
            catch (Exception e)
            {
                _logger.AddEntry("Error occoured while trying to delete trackplays in database after datetime: [" + datetime.ToLongDateString() + " " + datetime.ToLongTimeString() + "] Exception: " + e);
                throw e;
            }
        }
        #endregion
        #endregion
        #endregion
    }
}