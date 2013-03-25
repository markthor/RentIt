using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace RentItServer.ITU
{
    public class ChannelOrganizer
    {
        //Singleton instance of the class
        private static ChannelOrganizer _instance;

        //ChannelId, channelPlayer
        Dictionary<int, ChannelStreamer> channelsDic;
        //port number, channelId
        Dictionary<int, int> channelsPortDic;

        //The next port number to be assignd
        private int currentPort;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private ChannelOrganizer()
        {
            Setup();
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static ChannelOrganizer GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ChannelOrganizer();
            }
            return _instance;
        }

        private void Setup()
        {
            Console.WriteLine("Setup: setup started");

            channelsDic = new Dictionary<int, ChannelStreamer>();
            channelsPortDic = new Dictionary<int, int>();
            currentPort = 22000;
            Console.WriteLine("Setup: setup done");
        }

        public int GetChannelPortNumber(int channelId)
        {
            if (IsChannelRunnig(channelId))
            {
                ChannelStreamer c = null;

                if (channelsDic.TryGetValue(channelId, out c))
                {
                    return c.PortNumber;
                }
                else
                {
                    throw new ChannelNotFound("ChannelId: " + channelId + " is not found in \"channelsDic\"-dictionary");
                    //error, channel should exsist
                }
            }
            else
            {
                throw new ChannelNotRunningException("ChannelId: " + channelId + " is not running");
            }
        }

        //Den her metode er public for the sake of testing lige nu. Channels skal startes herinde fra på en måde regner jeg med
        public void StartChannel(int channelId)
        {
            if (!IsChannelRunnig(channelId))
            {
                Console.WriteLine("StartChannel: Starting channel - chID: " + channelId);
                ChannelStreamer cs = new ChannelStreamer(channelId, currentPort);

                cs.Start();

                channelsDic.Add(channelId, cs);
                channelsPortDic.Add(currentPort, channelId);
                currentPort++;

                Console.WriteLine("StartChannel: Channel started - chID: " + channelId);
            }
        }

        public bool IsChannelRunnig(int channelId)
        {
            ChannelStreamer cs = null;
            {
                if (channelsDic.TryGetValue(channelId, out cs))
                {
                    return cs.IsRunning;
                }
                else
                {
                    throw new ChannelNotFound("ChannelId: " + channelId + " is not found in \"channelsDic\"-dictionary");
                }
            }
        }
    }

    class ChannelNotRunningException : Exception
    {
        string ErrorMessage
        {
            get;
            set;
        }

        public ChannelNotRunningException(string message)
        {
            ErrorMessage = message;
        }
    }

    class ChannelNotFound : Exception
    {
        string ErrorMessage
        {
            get;
            set;
        }

        public ChannelNotFound(string message)
        {
            ErrorMessage = message;
        }
    }
}