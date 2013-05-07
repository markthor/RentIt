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
        private List<int> _runningChannelIds;


        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private StreamHandler()
        {
            _fileSystemHandler = FileSystemDao.GetInstance();
            _dao = DatabaseDao.GetInstance();
            _runningChannelIds = new List<int>();
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
            Track track = GetNextTrack(channelId);
            string fileName = track.Id.ToString() + ".mp3";

            string xml;
            string xmlFilePath;
            xml = XMLGenerator.GenerateConfig(channelId, FilePath.ITUTrackPath.GetPath() + fileName);
            xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
            FileSystemDao.GetInstance().WriteFile(xml, xmlFilePath);

            //get config path
            string configPath = FilePath.ITUChannelConfigPath.GetPath();
            string arguments = "-c " + xmlFilePath;
            EzProcess p = new EzProcess(channelId, FilePath.ITUEzStreamPath.GetPath(), arguments);
            p.Start();
            
            //Listen for when a new song starts
            p.OutputDataReceived += p_OutputDataReceived;
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            EzProcess p = (EzProcess)sender;
            Track track = GetNextTrack(p.ChannelId);
            string fileName = track.Id.ToString() + ".mp3";
            NextTrack(p, fileName);
        }

        private void NextTrack(EzProcess p, string trackPath)
        {
            FileSystemDao.GetInstance().WriteM3u(new List<string>() {trackPath}, FilePath.ITUM3uPath.GetPath() + p.ChannelId.ToString());

            string command = "killall -HUP ezstream";
            p.StandardInput.WriteLine(command);
            p.StandardInput.Flush();
        }

        private Track GetNextTrack(int channelId)
        {
            Track track;

            List<Track> tracks = _dao.GetTrackList(channelId); // check that there are tracks on the channel!
            List<TrackPlay> plays = _dao.GetTrackPlays(channelId);
            int tId = TrackPrioritizer.GetInstance().GetNextTrackId(tracks, plays);

            track = _dao.GetTrack(tId);

            return track;
        }
    }
}