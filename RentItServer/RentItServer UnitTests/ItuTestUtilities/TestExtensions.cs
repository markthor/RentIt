using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer;
using RentItServer.ITU;

namespace RentItServer_UnitTests.ItuTestUtilities
{
    /// <summary>
    /// Class containing static methods for generating test data to the database.
    /// </summary>
    public class TestExtensions
    {
        //Id of the first channel created the last time the database was populated.
        public static int _testChannelId1;
        public static int _testChannelId2;
        public static int _testChannelId3;
        public static int _testChannelId4;
        public static int _testChannelId5;
        public const string _testChannel1Name = "Nightly Psychoactive Electro Hits";
        public const string _testChannel1Description = "Sick channel with groovy beats.";
        public static double? _testChannel1Rating = 3;
        public static double? _testChannel1Hits = 15;
        public const string _user1name = "Prechtig";
        public const string _user2name = "TestMan Jr";
        public const string _user1email = "andreas.p.poulsen@gmail.com";
        public const string _user2email = "test@test.dk";
        public const string _userpassword = "test";
        public const string genreName1 = "Electro";
        public const string genreName2 = "Heavy Metal";
        public const string genreName3 = "Jazz";
        public static int genreId1;
        public static int genreId2;
        public static int genreId3;
        //The first user created the last time the database was populated.
        public static RentItServer.ITU.DatabaseWrapperObjects.User _testUser1;
        //The second user created the last time the database was populated.
        public static RentItServer.ITU.DatabaseWrapperObjects.User _testUser2;
        public static RentItServer.ITU.DatabaseWrapperObjects.Track _testTrack;
        public static RentItServer.ITU.DatabaseWrapperObjects.Channel _testChannel1;
        public static RentItServer.ITU.DatabaseWrapperObjects.Channel _testChannel2;
        public static RentItServer.ITU.DatabaseWrapperObjects.Channel _testChannel3;
        public static RentItServer.ITU.DatabaseWrapperObjects.Channel _testChannel4;
        public static RentItServer.ITU.DatabaseWrapperObjects.Channel _testChannel5;


        /// <summary>
        /// Adds instances of different entities to test.
        /// </summary>
        public static void PopulateDatabase()
        {
            _testUser1 = Controller.GetInstance().SignUp(_user1name, _user1email, _userpassword);
            _testUser2 = Controller.GetInstance().SignUp(_user2name, _user2email, _userpassword);
            genreId1 = Controller.GetInstance().CreateGenre(genreName1);
            genreId2 = Controller.GetInstance().CreateGenre(genreName2);
            genreId3 = Controller.GetInstance().CreateGenre(genreName3);
            int channelId1 = Controller.GetInstance().CreateChannel(_testChannel1Name, _testUser1.Id, _testChannel1Description, new int[] { genreId1 });
            Controller.GetInstance().UpdateChannel(channelId1, null, null, null, _testChannel1Hits, _testChannel1Rating, null);
            int channelId2 = Controller.GetInstance().CreateChannel("Hard Hitting Iron Bass", _testUser1.Id, "Metal with a density over 9000.", new int[] { genreId2 });
            Controller.GetInstance().UpdateChannel(channelId2, null, null, null, 4, 2, null);
            int channelId3 = Controller.GetInstance().CreateChannel("Nine Inch Nails", _testUser1.Id, "Soft rock for your soul.", new int[] { genreId2 });
            Controller.GetInstance().UpdateChannel(channelId3, null, null, null, 75, 7, null);
            int channelId4 = Controller.GetInstance().CreateChannel("Pegasus Pop", _testUser1.Id, "Not for kids.", new int[] { genreId3 });
            Controller.GetInstance().UpdateChannel(channelId4, null, null, null, 30, 9, null);
            int channelId5 = Controller.GetInstance().CreateChannel("Sick Drops", _testUser1.Id, "No description for you.", new int[] { genreId3 });
            Controller.GetInstance().UpdateChannel(channelId5, null, null, null, 102, 1, null);
            Controller.GetInstance().Subscribe(_testUser1.Id, channelId1);
            RentItServer.ITU.DatabaseWrapperObjects.Track t1 = new RentItServer.ITU.DatabaseWrapperObjects.Track();
            t1.Artist = "Kiss";
            t1.Name = "Heaven's On Fire";
            t1.Length = 0;
            t1.UpVotes = 1;
            t1.DownVotes = 7;
            t1.Path = "C:\\RentItServices\\RentIt21Files\\ITU\\Tracks\\test.mp3";
            RentItServer.ITU.DatabaseWrapperObjects.Track t2 = new RentItServer.ITU.DatabaseWrapperObjects.Track();
            t2.Artist = "Kryptic MindsKryptic MindsKryptic MindsKryptic MindsKryptic MindsKryptic MindsKryptic Minds";
            t2.Name = "Wasteland";
            t2.Length = 0;
            t2.UpVotes = 3;
            t2.DownVotes = 1;
            t2.Path = "C:\\RentItServices\\RentIt21Files\\ITU\\Tracks\\test.mp3";
            RentItServer.ITU.DatabaseWrapperObjects.Track t3 = new RentItServer.ITU.DatabaseWrapperObjects.Track();
            t3.Artist = "Temp0";
            t3.Name = "When Im Grandmaster In North American Region I Am Not A Good Player Because My APM is Below 15";
            t3.Length = 0;
            t3.UpVotes = 1;
            t3.DownVotes = 1;
            t3.Path = "C:\\RentItServices\\RentIt21Files\\ITU\\Tracks\\test.mp3";
            //Controller.GetInstance().AddTrack(_testUser1.Id, channelId1, new System.IO.MemoryStream());
            //Controller.GetInstance().AddTrack(_testUser1.Id, channelId1, new System.IO.MemoryStream());
            //Controller.GetInstance().AddTrack(_testUser1.Id, channelId1, new System.IO.MemoryStream()); 
            _testTrack = t1;
            _testChannelId1 = channelId1;
            _testChannelId2 = channelId2;
            _testChannelId3 = channelId3;
            _testChannelId4 = channelId4;
            _testChannelId5 = channelId5;
            DatabaseDao.GetInstance().CreateComment("testcomment", _testUser1.Id, _testChannelId1);
            _testChannel1 = DatabaseDao.GetInstance().GetChannel(_testChannelId1).GetChannel();
            _testChannel2 = DatabaseDao.GetInstance().GetChannel(_testChannelId2).GetChannel();
            _testChannel3 = DatabaseDao.GetInstance().GetChannel(_testChannelId3).GetChannel();
            _testChannel4 = DatabaseDao.GetInstance().GetChannel(_testChannelId4).GetChannel();
            _testChannel5 = DatabaseDao.GetInstance().GetChannel(_testChannelId5).GetChannel();
        }

        /// <summary>
        /// Creates all genres in the database.
        /// </summary>
        public void AddAllGenres()
        {
            ///Initialize all genres
            Controller.GetInstance().CreateGenre("Pop");
            Controller.GetInstance().CreateGenre("Country");
            Controller.GetInstance().CreateGenre("Rock");
            Controller.GetInstance().CreateGenre("Electro");
            Controller.GetInstance().CreateGenre("Hip Hop");
            Controller.GetInstance().CreateGenre("Classical");
            Controller.GetInstance().CreateGenre("Jazz");
            Controller.GetInstance().CreateGenre("Heavy Metal");
            Controller.GetInstance().CreateGenre("Reggae");
            Controller.GetInstance().CreateGenre("Folk");
            Controller.GetInstance().CreateGenre("Dance");
            Controller.GetInstance().CreateGenre("Soul");
            Controller.GetInstance().CreateGenre("Disco");
            Controller.GetInstance().CreateGenre("Funk");
            Controller.GetInstance().CreateGenre("Dupstep");
            Controller.GetInstance().CreateGenre("Soft Rock");
            Controller.GetInstance().CreateGenre("Nu Metal");
            Controller.GetInstance().CreateGenre("Indie Rock");
            Controller.GetInstance().CreateGenre("Spoken word");
            Controller.GetInstance().CreateGenre("Talk");
            Controller.GetInstance().CreateGenre("Grunge");
            Controller.GetInstance().CreateGenre("Rockabilly");
            Controller.GetInstance().CreateGenre("Death Metal");
            Controller.GetInstance().CreateGenre("World music");
            Controller.GetInstance().CreateGenre("Blues");
            Controller.GetInstance().CreateGenre("R&B");
            Controller.GetInstance().CreateGenre("Gangsta Rap");
            Controller.GetInstance().CreateGenre("House");
            Controller.GetInstance().CreateGenre("Techno");
            Controller.GetInstance().CreateGenre("Hardstyle");
            Controller.GetInstance().CreateGenre("Drum n Bass");
            Controller.GetInstance().CreateGenre("Hard Rock");
            Controller.GetInstance().CreateGenre("Latin");
            Controller.GetInstance().CreateGenre("Punk");
            Controller.GetInstance().CreateGenre("Industrial");
            Controller.GetInstance().CreateGenre("K-Pop");
            Controller.GetInstance().CreateGenre("Dancehall");
            Controller.GetInstance().CreateGenre("Grime");
            Controller.GetInstance().CreateGenre("Post-punk");
            Controller.GetInstance().CreateGenre("Opera");
            Controller.GetInstance().CreateGenre("Trance");
            Controller.GetInstance().CreateGenre("Noise");
            Controller.GetInstance().CreateGenre("Experimental");
            Controller.GetInstance().CreateGenre("Soundtrack");
            Controller.GetInstance().CreateGenre("Musical");
            Controller.GetInstance().CreateGenre("Minimal");
            Controller.GetInstance().CreateGenre("Chill out");
        }
    }
}