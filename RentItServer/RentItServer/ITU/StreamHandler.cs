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
            xml = XMLGenerator.GenerateConfig(channelId, FilePath.ITUTrackPath + trackName);
            FileSystemDao.GetInstance().WriteEzConfig(channelId, xml);

            //get ezstream path
            string ezStreamPath = "";
            //get config path
            string configPath = "";
            string arguments = "-c " + configPath;
            EzProcess p = new EzProcess(channelId, ezStreamPath, arguments);
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
            _fileSystemHandler.GenerateM3U(new List<string>() { trackPath });

            //
            string command = "killall -HUP ezstream";
            p.StandardInput.WriteLine(command);
            p.StandardInput.Flush();
        }
    }
}