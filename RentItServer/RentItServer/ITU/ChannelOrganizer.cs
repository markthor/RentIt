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
        //channelId, socket
        Dictionary<int,Socket> channelSocketDic;

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
            channelSocketDic = new Dictionary<int, Socket>();
            currentPort = 22000;
            Console.WriteLine("Setup: setup done");
        }

        public void StartListener(int channelId)
        {
            Console.WriteLine("StartListener: Listener started - chID: " + channelId);


            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ChannelStreamer channel = null;
            if (channelsDic.TryGetValue(channelId, out channel))
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, channel.PortNumber);
                s.Bind(endpoint);

                s.Listen(1);

                AsyncCallback aCallback = new AsyncCallback(OnClientConnect);
                s.BeginAccept(aCallback, s);

                channelSocketDic.Add(channelId, s);
            }
            else
            {
                Console.WriteLine("StartListener: Channel eksisterer ikke - chID: " + channelId);
                //channel eksisterer ikke
                //er det muligt på det her tidspunkt?
            }
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            Console.WriteLine("Client connected");
            Socket listener = (Socket)ar.AsyncState;
            Socket client = listener.EndAccept(ar);

            int port = ((IPEndPoint)client.LocalEndPoint).Port;
            int cId = -1;
            if (channelsPortDic.TryGetValue(port, out cId))
            {
                channelsDic[cId].AddClient(client);
            }
            else
            {
                Console.WriteLine("OnClientConnect: Channel eksisterer ikke - portNumber: " + port);
                //Is it ever possible that the channel is not running when someone connects? if it is not, then this should be an error
                //start channel, then add
            }

            AsyncCallback aCallback = new AsyncCallback(OnClientConnect);
            listener.BeginAccept(aCallback, listener);
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
                    //error, channel should exsist
                }
            }
            else
            {
                //error channel should be running now
            }

            return -1;
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

                StartListener(channelId);

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
                    return false;
                }
            }
        }
    }
}