using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer;
using RentItServer.ITU;

namespace RentItServer.Utilities
{
    public class TestExtensions
    {
        //Id of the first channel created the last time the database was populated.
        public static int _testChannelId;
        /// <summary>
        /// Adds instances of different entities to test.
        /// </summary>
        public static void PopulateDatabase()
        {
            string genreName1 = "Electro";
            string genreName2 = "Heavy Metal";
            string genreName3 = "Jazz";
            RentItServer.ITU.DatabaseWrapperObjects.User u1 = Controller.GetInstance().SignUp("Prechtig", "andreas.p.poulsen@gmail.com", "test");
            RentItServer.ITU.DatabaseWrapperObjects.User u2 = Controller.GetInstance().SignUp("TestMan Jr", "test", "test");
            Controller.GetInstance().CreateGenre(genreName1);
            Controller.GetInstance().CreateGenre(genreName2);
            Controller.GetInstance().CreateGenre(genreName3);
            int channelId1 = Controller.GetInstance().CreateChannel("Nightly Psychoactive Electro Hits", u1.Id, "Sick channel with groovy beats.", new List<string>() { genreName1 });
            int channelId2 = Controller.GetInstance().CreateChannel("Hard Hitting Iron Bass", u1.Id, "Metal with a density over 9000.", new List<string>() { genreName2 });
            int channelId3 = Controller.GetInstance().CreateChannel("Nine Inch Nails", u1.Id, "Soft rock for your soul.", new List<string>() { genreName2 });
            int channelId4 = Controller.GetInstance().CreateChannel("Wrecking Balls", u2.Id, "Not for kids.", new List<string>() { genreName3 });
            int channelId5 = Controller.GetInstance().CreateChannel("Sick Drops", u2.Id, "No description for you.", new List<string>() { genreName2 });
            Track t = new Track();
            t.Artist = "Kiss";
            t.Name = "Heaven's On Fire";
            t.Length = 0;
            t.UpVotes = 0;
            t.DownVotes = 0;
            t.ChannelId = channelId1;
            t.Path = "C:\\RentItServices\\RentIt21Files\\ITU\\Tracks\\test.mp3";
            Controller.GetInstance().AddTrack(u1.Id, channelId1, new System.IO.MemoryStream(), t);
            _testChannelId = channelId1;
        }
    }
}