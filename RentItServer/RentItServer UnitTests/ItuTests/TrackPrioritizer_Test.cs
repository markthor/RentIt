using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using RentItServer;
using RentItServer.ITU;

namespace RentItServer_UnitTests
{
    [TestClass]
    public class TrackPrioritizer_Test
    {
        /// <summary>
        /// Tests GetNextTrack with multiple tracks and multiple trackplays.
        /// </summary>
        [TestMethod]
        public void TrackPrioritizer_GetNextTrack_MultipleTracks_MultiplePlays()
        {
            List<Track> testTracks = new List<Track>();
            testTracks.Add(new Track(1, 10, 2));
            testTracks.Add(new Track(2, 0, 1));
            testTracks.Add(new Track(3, 3, 30));
            testTracks.Add(new Track(4, 20, 4));
            testTracks.Add(new Track(5, 39, 33));
            testTracks.Add(new Track(6, 8, 9));
            testTracks.Add(new Track(7, 1, 49));
            testTracks.Add(new Track(8, 15, 18));

            List<TrackPlay> testPlays = new List<TrackPlay>();
            testPlays.Add(new TrackPlay(1, new DateTime(2013, 1, 4)));
            testPlays.Add(new TrackPlay(2, new DateTime(2013, 1, 19)));
            testPlays.Add(new TrackPlay(3, new DateTime(2013, 1, 16)));
            testPlays.Add(new TrackPlay(4, new DateTime(2013, 1, 14)));
            testPlays.Add(new TrackPlay(8, new DateTime(2013, 1, 13)));
            testPlays.Add(new TrackPlay(2, new DateTime(2013, 1, 12)));
            testPlays.Add(new TrackPlay(2, new DateTime(2013, 1, 5)));
            testPlays.Add(new TrackPlay(1, new DateTime(2013, 1, 9)));
            Track result = TrackPrioritizer.GetInstance().GetNextTrack(testTracks, testPlays);
            Assert.AreNotEqual(0, result.Id);
            Assert.AreNotEqual(2, result.Id);
            Assert.AreNotEqual(3, result.Id);
            Assert.AreNotEqual(4, result.Id);
        }

        /// <summary>
        /// Tests GetNextTrack with multiple tracks and no trackplays.
        /// </summary>
        [TestMethod]
        public void TrackPrioritizer_GetNextTrack_MultipleTracks_EmptyPlays()
        {
            List<Track> testTracks = new List<Track>();
            testTracks.Add(new Track(1, 10, 2));
            testTracks.Add(new Track(2, 0, 1));
            testTracks.Add(new Track(3, 3, 30));
            testTracks.Add(new Track(4, 20, 4));
            testTracks.Add(new Track(5, 39, 33));

            List<TrackPlay> testPlays = new List<TrackPlay>();
            Track result = TrackPrioritizer.GetInstance().GetNextTrack(testTracks, testPlays);
            Assert.AreNotEqual(0, result.Id);
        }

        /// <summary>
        /// Tests GetNextTrack with one track and no trackplays.
        /// </summary>
        [TestMethod]
        public void TrackPrioritizer_GetNextTrackEmptyPlaysList_OneTrack()
        {
            List<Track> testTracks = new List<Track>();
            testTracks.Add(new Track(1, 10, 2));
            List<TrackPlay> testPlays = new List<TrackPlay>();
            Track result = TrackPrioritizer.GetInstance().GetNextTrack(testTracks, testPlays);
            Assert.AreNotEqual(0, result.Id);
        }

        /// <summary>
        /// Tests GetNextTrackPlayList with one track and no plays.
        /// </summary>
        [TestMethod]
        public void TrackPrioritizer_GetNextPlayList_OneTrack_NoPlays()
        {
            int trackLength = 180000;
            int minMilliSeconds = 3000000;
            List<Track> testTracks = new List<Track>();
            testTracks.Add(new Track(1, 10, 2, trackLength));

            List<TrackPlay> testPlays = new List<TrackPlay>();
            List<TrackPlay> playListPlays = new List<TrackPlay>();

            List<Track> result = TrackPrioritizer.GetInstance().GetNextPlayList(testTracks, testPlays, minMilliSeconds, out playListPlays);
            int roundUpDivision = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(minMilliSeconds) / Convert.ToDouble(trackLength)));
            Assert.AreEqual(roundUpDivision, result.Count);
            Assert.AreEqual(roundUpDivision, playListPlays.Count);
        }

        [TestMethod]
        public void TrackPrioritizer_GetNextPlaylist_MultipleTracks_NoPlays()
        {
            int trackLength = 180000;
            List<Track> testTracks = new List<Track>();
            testTracks.Add(new Track(1, 10, 2, trackLength));
            testTracks.Add(new Track(2, 0, 1, trackLength));
            testTracks.Add(new Track(3, 3, 30, trackLength));
            testTracks.Add(new Track(4, 20, 4, trackLength));
            testTracks.Add(new Track(5, 39, 33, trackLength));
            testTracks.Add(new Track(6, 8, 9, trackLength));
            testTracks.Add(new Track(7, 1, 49, trackLength));
            testTracks.Add(new Track(8, 15, 18, trackLength));

            List<TrackPlay> testPlays = new List<TrackPlay>();
            List<TrackPlay> playListPlays = new List<TrackPlay>();

            List<Track> result = TrackPrioritizer.GetInstance().GetNextPlayList(testTracks, testPlays, 5000000, out playListPlays);

        }

        /// <summary>
        /// Tests GetNextPlayList with multiple tracks and multiple plays.
        /// </summary>
        [TestMethod]
        public void TrackPrioritizer_GetNextPlayList_MultipleTracks_MultiplePlays()
        {
            int trackLength = 180000;
            List<Track> testTracks = new List<Track>();
            testTracks.Add(new Track(1, 10, 2, trackLength));
            testTracks.Add(new Track(2, 0, 1, trackLength));
            testTracks.Add(new Track(3, 3, 30, trackLength));
            testTracks.Add(new Track(4, 20, 4, trackLength));
            testTracks.Add(new Track(5, 39, 33, trackLength));
            testTracks.Add(new Track(6, 8, 9, trackLength));
            testTracks.Add(new Track(7, 1, 49, trackLength));
            testTracks.Add(new Track(8, 15, 18, trackLength));

            List<TrackPlay> testPlays = new List<TrackPlay>();
            testPlays.Add(new TrackPlay(1, new DateTime(2013, 1, 4)));
            testPlays.Add(new TrackPlay(2, new DateTime(2013, 1, 19)));
            testPlays.Add(new TrackPlay(3, new DateTime(2013, 1, 16)));
            testPlays.Add(new TrackPlay(4, new DateTime(2013, 1, 14)));
            testPlays.Add(new TrackPlay(8, new DateTime(2013, 1, 13)));
            testPlays.Add(new TrackPlay(2, new DateTime(2013, 1, 12)));
            testPlays.Add(new TrackPlay(2, new DateTime(2013, 1, 5)));
            testPlays.Add(new TrackPlay(1, new DateTime(2013, 1, 9)));
            List<TrackPlay> playListPlays = new List<TrackPlay>();
            int minMilliSeconds = 200000;
            for ( ; minMilliSeconds < 10000000; minMilliSeconds = minMilliSeconds + 200000)
            {
                List<Track> result = TrackPrioritizer.GetInstance().GetNextPlayList(testTracks, testPlays, minMilliSeconds, out playListPlays);
                int roundUpDivision = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(minMilliSeconds) / Convert.ToDouble(trackLength)));
                Assert.AreEqual(roundUpDivision, result.Count);
                Assert.AreEqual(roundUpDivision, playListPlays.Count);
                playListPlays.Clear();
            }
        }

        /// <summary>
        /// Tests GetNextPlayList with no tracks.
        /// </summary>
        [TestMethod]
        public void TrackPrioritizer_GetNextPlayList_NoTracks()
        {
            try
            {
                List<Track> testTracks = new List<Track>();
                List<TrackPlay> testPlays = new List<TrackPlay>();
                List<TrackPlay> playListPlays = new List<TrackPlay>();
                List<Track> result = TrackPrioritizer.GetInstance().GetNextPlayList(testTracks, testPlays, 1000, out playListPlays);
                //No exception was thrown.
                Assert.Fail();
            }
            catch (Exception e)
            {
                //This is expected.
            }
        }



    }
}