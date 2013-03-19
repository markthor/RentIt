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
        [TestMethod]
        public void TrackPrioritizer_GetNextTrackId()
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
            int result = TrackPrioritizer.GetInstance().GetNextTrackId(testTracks, testPlays);
            Assert.AreNotEqual(0, result);
        }
    }
}