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

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private StreamHandler() { _fileSystemHandler = FileSystemDao.GetInstance(); }

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

        public void StartStream(int channelId, string trackName)
        {
            throw new NotImplementedException();
            string xml;
            string xmlFilePath;
            xml = XMLGenerator.GenerateConfig(channelId, FilePath.ITUTrackPath.GetPath() + trackName);
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
            ProcessOutputData(sender);
        }

        public event Action<object> ProcessOutputData;

        public void NextTrack(EzProcess p, string trackPath)
        {
            //generate m3u
            List<string> list = new List<string>();
            FileSystemDao.GetInstance().WriteM3u(new List<string>() {trackPath}, FilePath.ITUM3uPath.GetPath() + p.ChannelId.ToString());
            //_fileSystemHandler.GenerateM3U(new List<string>() { trackPath });

            //
            string command = "killall -HUP ezstream";
            p.StandardInput.WriteLine(command);
            p.StandardInput.Flush();
        }
    }
}