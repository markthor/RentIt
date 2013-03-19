using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.SMU;
using System.Data.Entity;

namespace RentItServer_UnitTests
{
    [TestClass]
    public class SMUController_Test
    {
        [TestMethod]
        public void TestSignUp()
        {
            SMUController controller = new SMUController();

            int u1 = controller.SignUp("John Doe1", "1Fisk", "gogogo1@yo.dk");
            int u2 = controller.SignUp("John Doe2", "12Fisk", "gogogo2@yo.dk");
            int u3 = controller.SignUp("John Doe3", "123Fisk", "gogogo3@yo.dk");

            controller.LogIn("John Doe1", "1Fisk");
            controller.LogIn("John Doe2", "12Fisk");
            controller.LogIn("John Doe3", "123Fisk");     
       }
    }
}
