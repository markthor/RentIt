using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.SMU;

namespace RentItServer_UnitTests
{
    [TestClass]
    public class SMUService_test
    {
        [TestMethod]
        public void TestSignUp_service()
        {
            ServiceReference1.SMURentItServiceClient service = new ServiceReference1.SMURentItServiceClient();
            int userId = service.SignUp("TBone@gmail.com", "TBone", "Fish", false);
            int id = service.LogIn("TBone@gmail.com", "Fish");

            Assert.AreEqual(userId, id);
        }
    }
}
