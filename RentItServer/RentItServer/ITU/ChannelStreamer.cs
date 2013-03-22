using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace RentItServer
{
    public class ChannelStreamer
    {
        List<Socket> clients;
        int bytesSend;
        int packetSize;
        int songLengthMillis;

        Stopwatch stopwatch;

        public ChannelStreamer(int portNumber)
        {
            PortNumber = portNumber;
            clients = new List<Socket>();
            stopwatch = new Stopwatch();
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

        public void AddClient(Socket client)
        {
            clients.Add(client);
        }

        private void PlaySong()
        {
            List<Socket> disconnected = new List<Socket>();


            Console.WriteLine("PlaySong: Started playing");
            while (stopwatch.ElapsedMilliseconds < songLengthMillis)
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
                        }
                    }

                    //Remove disconnected clients
                    foreach (var s in disconnected)
                    {
                        clients.Remove(s);
                    }

                    bytesSend += packetSize;
                }
            }
            Console.WriteLine("PlaySong: Finished playing");
            SongFinished();
        }

        private void SongFinished()
        {
            Console.WriteLine("SongFinished: Starting next song");
            NextSong();

            //load song bytes

            stopwatch.Restart();
            PlaySong();
        }

        //Method not done
        private void NextSong()
        {
            Console.WriteLine("NextSong: Loading next song");
            SongBytes = LoadSong("a.mp3");
            songLengthMillis = 211000; //get from DB
            packetSize = (int)((double)SongBytes.Length / (double)songLengthMillis) * 1000;
        }

        private byte[] LoadSong(string fileName)
        {
            //default path to all music
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\";
            //IO-handling, ryk til file system handler
            return File.ReadAllBytes(path + fileName);
        }

        public void Start()
        {
            Console.WriteLine("Start: starting channel");
            Thread t = new Thread(new ThreadStart(SongFinished));
            t.Start();
            IsRunning = true;
        }
    }
}