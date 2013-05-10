using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using RentItServer.Utilities;
using System.Threading;

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

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private StreamHandler()
        {
            _fileSystemHandler = FileSystemDao.GetInstance();
            _dao = DatabaseDao.GetInstance();
            runningChannelIds = new Dictionary<int, EzProcess>();
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

        public void StartStream(int channelId)
        {
            _logger.AddEntry("Start Stream start");

            if (!IsChannelRunning(channelId))
            {
                _logger.AddEntry("Channel with id: " + channelId + " is not running");
                Track track = GetNextTrack(channelId);
                if (track == null) // no tracks on channel
                {
                    _logger.AddEntry("Channel with id: " + channelId + " has no tracks"); 
                    return; //notracks on channel exception
                }

                _logger.AddEntry("Next track name " + track.Name + " and id " + track.Id);

                string fileName = track.Id.ToString() + ".mp3";
                _logger.AddEntry("Track filename: " + fileName);

                GenerateM3u(channelId, fileName); // generate m3u
                string m3uFileName;
                m3uFileName = channelId + ".m3u";


                string xml;
                string xmlFilePath;
                xml = XMLGenerator.GenerateConfig(channelId, FilePath.ITUM3uPath.GetPath() + m3uFileName);
                _logger.AddEntry("channel config xml: " + xml);
                xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
                _logger.AddEntry("xml file path: " + xmlFilePath);
                FileSystemDao.GetInstance().WriteFile(xml, xmlFilePath);

                //get config path
                string configPath = FilePath.ITUChannelConfigPath.GetPath();
                string arguments = "-c " + xmlFilePath;
                _logger.AddEntry("Arguments: " + arguments);
                EzProcess p = new EzProcess(channelId, FilePath.ITUEzStreamPath.GetPath(), arguments);
                p.Start();
                _logger.AddEntry("Process start");

                //Listen for when a new song starts
                p.OutputDataReceived += p_OutputDataReceived;

                runningChannelIds.Add(channelId, p);
                AddTrackPlay(track); // should this call be here

                SetNextTrack(p);
            }
            else //channel is already running
            {
                throw new ChannelRunningException("The channel is already running");
            }
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _logger.AddEntry("Process has given output data");
            EzProcess p = (EzProcess)sender;

            SetNextTrack(p);
        }

        private void SetNextTrack(EzProcess p)
        {
            Track track = GetNextTrack(p.ChannelId);
            string fileName = track.Id.ToString() + ".mp3";

            GenerateM3u(p.ChannelId, fileName);

            string command = "killall -HUP ezstream";
            p.StandardInput.WriteLine(command);
            p.StandardInput.Flush();

            AddTrackPlay(track);
        }

        private Track GetNextTrack(int channelId)
        {
            Track track;

            List<Track> tracks = _dao.GetTrackList(channelId); // check that there are tracks on the channel!
            if (!tracks.Any())//no tracks on channel
            {
                throw new NoTracksOnChannelException("There are no tracks associated with the channel");
            }

            List<TrackPlay> plays = _dao.GetTrackPlays(channelId);
            int tId = TrackPrioritizer.GetInstance().GetNextTrackId(tracks, plays);

            track = _dao.GetTrack(tId);

            return track;
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
            catch (KeyNotFoundException) { }
            return false;
        }

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

        private void GenerateM3u(int channelId, string fileName)
        {
            string trackPath = FilePath.ITUTrackPath.GetPath() + fileName;
            FileSystemDao.GetInstance().WriteM3u(new List<string>() { trackPath }, FilePath.ITUM3uPath.GetPath() + channelId.ToString() + ".m3u");
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