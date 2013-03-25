using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace RentItServer.ITU
{
    public class ChannelStreamer
    {
        List<Socket> clients;
        int bytesSend;
        int packetSize;
        //int songLengthMillis;
        Track currentTrack;
        Socket socketListener;

        Stopwatch stopwatch;

        public ChannelStreamer(int channelId, int portNumber)
        {
            ChannelId = channelId;
            PortNumber = portNumber;
            clients = new List<Socket>();
            stopwatch = new Stopwatch();
        }

        public int ChannelId
        {
            get;
            set;
        }

        public int PortNumber
        {
            get;
            private set;
        }

        private byte[] SongBytes
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        private void AddClient(Socket client)
        {
            clients.Add(client);
        }

        private void PlaySong()
        {
            List<Socket> disconnected = new List<Socket>();

            Console.WriteLine("PlaySong: Started playing");
            while (stopwatch.ElapsedMilliseconds < currentTrack.length)
            {
                while (bytesSend < SongBytes.Length)
                {
                    //sleep 1 second
                    Thread.Sleep(1000);
                    foreach (var s in clients)
                    {
                        try
                        {
                            s.Send(SongBytes, bytesSend, packetSize, 0);
                        }
                        catch (SocketException e)
                        {
                            //socket har ikke forbindelse længere, klienten har lukket
                            disconnected.Add(s);
                            //Logger e
                        }
                    }

                    //Remove disconnected clients
                    foreach (var s in disconnected)
                    {
                        clients.Remove(s);
                    }
                    if (clients.Count == 0)
                    {
                        socketListener.Disconnect(false);
                        socketListener.Dispose();
                        socketListener = null;
                        Thread.CurrentThread.Abort();
                    }
                    bytesSend += packetSize;
                }
            }
            Console.WriteLine("PlaySong: Finished playing");
            //SongFinished();
        }

        private void SongFinished()
        {
            Console.WriteLine("SongFinished: Starting next song");
            //load song bytes
            NextSong();
        }

        //Method not done
        private void NextSong()
        {
            TrackPrioritizer tp = TrackPrioritizer.GetInstance();
            int trackId = tp.GetNextTrackId(DAO.GetInstance().GetTrackList(ChannelId), DAO.GetInstance().GetTrackPlays(ChannelId));
            //currentTrack = DAO.GetInstance().GetTrack(trackId);

            //SongBytes = FileSystemHandler.LoadTrackBytes(currentTrack.trackpath);

            Console.WriteLine("NextSong: Loading next song");


            currentTrack = new Track();
            SongBytes = LoadSong("a.mp3");
            currentTrack.length = 235000;


            packetSize = (int)((double)SongBytes.Length / (double)currentTrack.length) * 1000;
        }

        private void ThreadRun()
        {
            try
            {
                while (true) //while (IsRunning)
                {
                    SongFinished();
                    stopwatch.Restart();
                    PlaySong();
                }
            }
            catch (ThreadInterruptedException e)
            {
                //Logger
                /*SongBytes = null;
                stopwatch.Stop();
                currentTrack = null;
                IsRunning = false;
                clients.RemoveAll((x)=>true);
                bytesSend = 0;
                packetSize = 0;*/
                throw e;
            }
        }

        private byte[] LoadSong(string fileName)
        {
            //default path to all music
            string path = "C:\\RentItServices\\Rentit21\\Tracks\\";
            //IO-handling, ryk til file system handler
            return File.ReadAllBytes(path + fileName);
        }

        public void Start()
        {
            Console.WriteLine("Start: starting channel");
            StartListener();
            Thread t = new Thread(new ThreadStart(SongFinished));
            t.Start();
            IsRunning = true;
        }

        private void StartListener()
        {
            Console.WriteLine("StartListener: Listener started - chID: " + ChannelId);


            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, PortNumber);
            socketListener.Bind(endpoint);

            socketListener.Listen(1);

            AsyncCallback aCallback = new AsyncCallback(OnClientConnect);
            socketListener.BeginAccept(aCallback, socketListener);
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            Console.WriteLine("Client connected");
            Socket listener = (Socket)ar.AsyncState;
            Socket client = listener.EndAccept(ar);

            int port = ((IPEndPoint)client.LocalEndPoint).Port;

            AddClient(client);

            AsyncCallback aCallback = new AsyncCallback(OnClientConnect);
            listener.BeginAccept(aCallback, listener);
        }
    }
}