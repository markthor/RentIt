using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.ITU;
using RentItServer;

namespace RentItServer_UnitTests.ItuTests
{
    /// <summary>
    /// Summary description for ItuDao_Test
    /// </summary>
    [TestClass]
    public class ItuDao_Test
    {
        DatabaseDao _dao = DatabaseDao.GetInstance();

        public ItuDao_Test()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
        }

        [TestInitialize]
        public void CleanupStart()
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAddTrackPlay()
        {
            User u = _dao.SignUp("Prechtig", "andreas.p.poulsen@gmail.com", "test");
            _dao.CreateGenre("Electro");
            Channel c = _dao.CreateChannel("heh", u.Id, "hehe", new List<string>() {"Electro"});
            Track t = _dao.CreateTrackEntry(c.Id, "C:\\RentItServices\\RentIt21Files\\ITU\\Tracks\\test.mp3", "test", "test", 50, 0, 0);
            _dao.AddTrackPlay(t);
        }

        [TestMethod]
        public void TestGetTrackPlays()
        {
            User u = _dao.SignUp("Prechtig", "andreas.p.poulsen@gmail.com", "test");
            _dao.CreateGenre("Electro");
            Channel c = _dao.CreateChannel("heh", u.Id, "hehe", new List<string>() { "Electro" });
            Track t = _dao.CreateTrackEntry(c.Id, "C:\\RentItServices\\RentIt21Files\\ITU\\Tracks\\test.mp3", "test", "test", 50, 0, 0);
            _dao.AddTrackPlay(t);
            List<TrackPlay> tps = _dao.GetTrackPlays(c.Id);

        }
    }
}
