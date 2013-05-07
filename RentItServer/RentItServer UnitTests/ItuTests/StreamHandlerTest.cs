using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.ITU;
using RentItServer.ITU.DatabaseWrapperObjects;
using RentItServer;
using RentItServer.ITU;

namespace RentItServer_UnitTests.ItuTests
{
    [TestClass]
    public class StreamHandlerTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
        }

        /// <summary>
        /// Deletes all tuples in SMU database.
        /// </summary>
        [TestInitialize]
        public void CleanDataBaseFinish()
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
        }

        [TestMethod]
        public void TestStartStream()
        {
            string genreName1 = "Electro";
            string genreName2 = "Heavy Metal";
            RentItServer.ITU.DatabaseWrapperObjects.User u = Controller.GetInstance().SignUp("Prechtig", "andreas.p.poulsen@gmail.com", "test");
            Controller.GetInstance().CreateGenre(genreName1);
            Controller.GetInstance().CreateGenre(genreName2);
            int channelId1 = Controller.GetInstance().CreateChannel("Nightly Psychoactive Electro Hits", u.Id, "Sick channel with groovy beats", new List<string>() { genreName1 });
            int channelId2 = Controller.GetInstance().CreateChannel("Hard Hitting Iron Bass", u.Id, "Metal with a density over 9000.", new List<string>() { genreName2 });
            DatabaseDao.GetInstance().CreateTrackEntry(channelId1, "C:\\RentItServices\\RentIt21Files\\ITU\\Tracks\\test.mp3", "test", "temp0", 50, 0, 0);

            using (ITUServiceReference.RentItServiceClient proxy = new ITUServiceReference.RentItServiceClient())
            {
                proxy.Login("andreas.p.poulsen@gmail.com", "test");
            }

        }
    }
}
