using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.ITU;
using RentItServer.ITU.DatabaseWrapperObjects;
using RentItServer.Utilities;

namespace RentItServer_UnitTests.ItuTests
{
    [TestClass]
    public class StreamHandlerTest
    {
        [ClassInitialize]
        public static void Populate(TestContext tc)
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
            TestExtensions.PopulateDatabase();
        }
        
        [TestMethod]
        public void TestStartStream()
        {
            using (ITUServiceReference.RentItServiceClient proxy = new ITUServiceReference.RentItServiceClient())
            {
                proxy.startChannel(TestExtensions._testChannelId);
            }
        }
    }
}
