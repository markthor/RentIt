using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.ITU;
using RentItServer.ITU.DatabaseWrapperObjects;
using RentItServer_UnitTests.ItuTestUtilities;

namespace RentItServer_UnitTests.ItuTests
{
    [TestClass]
    public class StreamHandler_Test
    {
        [ClassInitialize]
        public static void Populate(TestContext tc)
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
            TestExtensions.PopulateDatabase();
        }
        
        [TestMethod]
        public void StartStream_Test()
        {
            using (ITUServiceReference.RentItServiceClient proxy = new ITUServiceReference.RentItServiceClient())
            {
                //proxy.Login("Prechtig", "tesat");
                proxy.startChannel(TestExtensions._testChannelId1);
            }
        }
    }
}