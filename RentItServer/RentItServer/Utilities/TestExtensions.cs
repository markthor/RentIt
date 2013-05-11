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
        public const string _user1name = "Prechtig";
        public const string _user2name = "TestMan Jr";
        public const string _user1email = "andreas.p.poulsen@gmail.com";
        public const string _user2email = "test";
        public const string _userpassword = "test";
        public const string genreName1 = "Electro";
        public const string genreName2 = "Heavy Metal";
        public const string genreName3 = "Jazz";
        //The first user created the last time the database was populated.
        public static ITU.DatabaseWrapperObjects.User _testUser1;
        //The second user created the last time the database was populated.
        public static ITU.DatabaseWrapperObjects.User _testUser2;
        public static ITU.DatabaseWrapperObjects.Track _testTrack;

        /// <summary>
        /// Adds instances of different entities to test.
        /// </summary>
        public static void PopulateDatabase()
        {
            _testUser1 = Controller.GetInstance().SignUp(_user1name, _user1email, _userpassword);
            _testUser2 = Controller.GetInstance().SignUp(_user2name, _user2email, _userpassword);
            Controller.GetInstance().CreateGenre(genreName1);
            Controller.GetInstance().CreateGenre(genreName2);
            Controller.GetInstance().CreateGenre(genreName3);
            int channelId1 = Controller.GetInstance().CreateChannel("Nightly Psychoactive Electro Hits", _testUser1.Id, "Sick channel with groovy beats.", new List<string>() { genreName1 });
            int channelId2 = Controller.GetInstance().CreateChannel("Hard Hitting Iron Bass", _testUser1.Id, "Metal with a density over 9000.", new List<string>() { genreName2 });
            int channelId3 = Controller.GetInstance().CreateChannel("Nine Inch Nails", _testUser1.Id, "Soft rock for your soul.", new List<string>() { genreName2 });
            int channelId4 = Controller.GetInstance().CreateChannel("Wrecking Balls", _testUser1.Id, "Not for kids.", new List<string>() { genreName3 });
            int channelId5 = Controller.GetInstance().CreateChannel("Sick Drops", _testUser1.Id, "No description for you.", new List<string>() { genreName2 });
            //Controller.GetInstance().Subscribe(_testUser2.Id, channelId1);
            RentItServer.ITU.DatabaseWrapperObjects.Track t = new RentItServer.ITU.DatabaseWrapperObjects.Track();
            t.Artist = "Kiss";
            t.Name = "Heaven's On Fire";
            t.Length = 0;
            t.UpVotes = 0;
            t.DownVotes = 0;
            t.Path = "C:\\RentItServices\\RentIt21Files\\ITU\\Tracks\\test.mp3";
            Controller.GetInstance().AddTrack(_testUser1.Id, channelId1, new System.IO.MemoryStream(), t); 
            _testTrack = t;
            _testChannelId = channelId1;
        }
    }
}