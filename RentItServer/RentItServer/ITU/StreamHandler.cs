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

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private StreamHandler()
        {
            _fileSystemHandler = FileSystemDao.GetInstance();
            _dao = DatabaseDao.GetInstance();
            runningChannelIds = new Dictionary<int, EzProcess>();

            InitTimer();
        }

        private void InitTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 86400000; //24 hours
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        public void AddLogger(Logger logger)
        {
            _logger = logger;
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
        /// Starts the stream of the specified channel. Runs ezstream and starts a countinous operation. Requires icecast to be running.
        /// </summary>
        /// <param name="channelId">Channel id of the channel to be started</param>
        public void ManualStreamStart(int channelId) // rename to something that says it is the first time the stream is being started and write a method for starting the stream when it has been of(is that even necessary?)
        {
            if (!IsChannelRunning(channelId)) // Check if stream is already running
            {
                if(_dao.ChannelHasTracks(channelId))
                {
                    StartEzstreamProcess(channelId);
                }
                else
                {
                    _logger.AddEntry("Channel with id: " + channelId + " has no associated tracks");
                    throw new NoTracksOnChannelException("Channel with id: " + channelId + " has no associated tracks");
                }
            }
            else //channel is already running
            {
                throw new ChannelRunningException("The channel is already running");
            }
        }

        private bool IsChannelRunning(int channelId)
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
                _logger.AddEntry("[IsChannelRunning]: KeyNotFoundException when looking for channelId: " + channelId); 
            }
            return false;
        }

        /// <summary>
        /// Stops the stream of a channel and sets it is not running.
        /// </summary>
        /// <param name="channelId">The id of the channel to be stopped</param>
        public void StopStream(int channelId)
        {
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
        }

        private void AddTrackPlay(Track track)
        {
            _dao.AddTrackPlay(track);
        }

        private void GenerateM3uWithOneTrack(int channelId, string trackFileName)
        {
            string trackPath = FilePath.ITUTrackPath.GetPath() + trackFileName;
            //FileSystemDao.GetInstance().WriteM3u(new List<string>() { trackPath }, FilePath.ITUM3uPath.GetPath() + channelId.ToString() + ".m3u");
            FileSystemDao.GetInstance().WriteM3UFile(trackPath, FilePath.ITUM3uPath.GetPath() + channelId.ToString() + ".m3u");
        }




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
        }

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
            GenerateM3U(channelId, playTime);


            // start stream process
            EzProcess p = StartEzstreamProcess(channelId); //CHECK AT DEN IKKE KØRER

            // add process to list of active streams
            runningChannelIds.Add(channelId, p);
        }

        private void CloseAllStreams()
        {
            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
            {
                if (myProc.ProcessName == "ezstream")
                {
                    myProc.Kill();
                }
            }
        }

        private void CreateChannelConfigFile(int channelId)
        {
            //Generate the xml for the config file
            string xml;
            xml = XMLGenerator.GenerateConfig(channelId, FilePath.ITUM3uPath.GetPath() + channelId + ".m3u");
            _logger.AddEntry("Config file generated for channel with id: " + channelId);
            //Create the xmlFilePath
            string xmlFilePath;
            xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
            //Write the config file to the system
            FileSystemDao.GetInstance().WriteFile(xml, xmlFilePath);
        }

        private void GenerateM3U(int channelId, int playTime)
        {
            throw new NotImplementedException();
            //create m3u file which lasts around as long as the playtime, one track over, doesnt matter anyway
        }

        private EzProcess StartEzstreamProcess(int channelId)
        {
            string xmlFilePath;
            xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";

            //Start set up the process
            //Path to ezstream executable
            string ezPath = FilePath.ITUEzStreamPath.GetPath();
            //Create the arguments
            string arguments = "-c " + xmlFilePath;
            //Start set up process info
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c " + ezPath + " " + arguments);
            startInfo.RedirectStandardInput = true; // MAYBE NEEDED FOR WHEN WE TEST CHANGE SONG VIA COMMAND LINE INPUT
            //In order to redirect the standard input for ezstream into this program
            startInfo.RedirectStandardOutput = true;
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
            //runningChannelIds.Add(channelId, p); SKAL DET GØRES HER?!?!?!?!?!?!?!?!?!?!?!?!?!?
            return p;
        }
    }

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
}