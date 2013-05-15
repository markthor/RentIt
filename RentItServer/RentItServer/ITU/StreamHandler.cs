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
        /// <summary>
        /// Starts the stream of the specified channel. Runs ezstream and starts a countinous operation. Requires icecast to be running.
        /// </summary>
        /// <param name="channelId">Channel id of the channel to be started</param>
        public void StartStream(int channelId)
        {
            if (!IsChannelRunning(channelId)) // Check if stream is already running
            {
                _logger.AddEntry("Start Stream - ChannelId: " + channelId);

                //Get the first track which should be played on the channel
                Track track = GetNextTrack(channelId);
                if (track == null) // no tracks associated with the channel
                {
                    _logger.AddEntry("Channel with id: " + channelId + " has no associated tracks");
                    throw new NoTracksOnChannelException("Channel with id: " + channelId + " has no associated tracks");
                }
                _logger.AddEntry("Next track name: " + track.Name + " and id: " + track.Id + " for channel with id: " + channelId);

                //Create the filename for the track
                string trackFileName;
                //trackFileName = track.Id.ToString() + ".mp3"; // DET RIGTIGE KODE!!!!!!!!
                trackFileName = "b.mp3"; // TIL TESTING!!!!!!!!!!!
                _logger.AddEntry("Next track filename: " + trackFileName + " for channel with id: " + channelId);

                //Write the m3u file to the filesystem
                GenerateM3uWithOneTrack(channelId, trackFileName);
                //Create the filename for the m3u file
                string m3uFileName;
                m3uFileName = channelId + ".m3u";

                //Generate the xml for the config file
                string xml;
                xml = XMLGenerator.GenerateConfig(channelId, FilePath.ITUM3uPath.GetPath() + m3uFileName);
                _logger.AddEntry("Config file generated for channel with id: " + channelId);
                //Create the xmlFilePath
                string xmlFilePath;
                xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + channelId.ToString() + ".xml";
                //xmlFilePath = FilePath.ITUChannelConfigPath.GetPath() + "configtest.xml"; // TIL TEST!!!!!!
                //Write the config file to the system
                FileSystemDao.GetInstance().WriteFile(xml, xmlFilePath);



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
                EzProcess p = new EzProcess(channelId);
                p.StartInfo = startInfo;
                _logger.AddEntry("Process created for channel with id: " + channelId);
                p.Start();
                _logger.AddEntry("Process started for channel with id: " + channelId);

                //Listen for when a new song starts
                //p.OutputDataReceived += p_OutputDataReceived;

                //Add this process to the dictionary with running channels
                runningChannelIds.Add(channelId, p);
                AddTrackPlay(track); // should this call be here??????????


                //_logger.AddEntry(p.StandardOutput.ReadToEnd()); //thread that shiat

                //SetNextTrack(p); // FIND ANOTHER WAY OF DOING THIS, PROBLEM IS THAT IT CALLS GenerateM3uWithOneTrack


                Thread t = new Thread(new ParameterizedThreadStart(EzProcessThread));
                t.Start(p);
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

            GenerateM3uWithOneTrack(p.ChannelId, fileName);

            /* 
             * 
             * TEST OF LONG TIME IT WOULD TAKE TO CHANGE SONG WHEN A SONG HAS JUST FINISHED, IF IT IS EVEN POSSIBLE
             * 
             */
            string command = "killall -HUP ezstream";
            p.StandardInput.WriteLine(command);
            p.StandardInput.Flush();

            AddTrackPlay(track);
        }

        private void EzProcessThread(object o)
        {
            EzProcess p = (EzProcess)o;
            _logger.AddEntry("EzProcessThread for channel with id: " + p.ChannelId + " has started");
            while (true)//while channel running
            {
                //_logger.AddEntry("EzProcessThread waiting for standardoutput for channel with id: " + p.ChannelId);
                //string stdOutput = p.StandardOutput.ReadToEnd();
                _logger.AddEntry("standardoutput for channel with id: " + p.ChannelId + " has given: " + stdOutput);
                Thread.Sleep(1000);
                SetNextTrack(p);
            }
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