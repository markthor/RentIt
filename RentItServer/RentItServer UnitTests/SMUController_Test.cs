using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.SMU;
using RentItServer;
using System.Data.Entity;

namespace RentItServer_UnitTests
{
    [TestClass]
    public class SMUController_Test
    {
        [TestMethod]
        public void TestSignUp()
        {
            SMUController controller = SMUController.GetInstance();

            int u1 = controller.SignUp("John Doe1", "1Fisk", "gogogo1@yo.dk");
            int u2 = controller.SignUp("John Doe2", "12Fisk", "gogogo2@yo.dk");
            int u3 = controller.SignUp("John Doe3", "123Fisk", "gogogo3@yo.dk");

            controller.LogIn("John Doe1", "1Fisk");
            controller.LogIn("John Doe2", "12Fisk");
            controller.LogIn("John Doe3", "123Fisk");     
       }

        [TestMethod]
        public void TestGetUser()
        {
            SMUController controller = SMUController.GetInstance(); 
            int u1 = controller.SignUp("Peter Parker1", "1Fisk", "gogogo1@yo.dk");
            int u2 = controller.SignUp("Peter Parker2", "12Fisk", "gogogo2@yo.dk");
            int u3 = controller.SignUp("Peter Parker3", "123Fisk", "gogogo3@yo.dk");

            Assert.AreEqual(u1, controller.GetUser(u1).id);
            Assert.AreEqual(u2, controller.GetUser(u2).id);
            Assert.AreEqual(u3, controller.GetUser(u3).id);
            Assert.AreNotEqual(u1, controller.GetUser(u3).id);
        }

        [TestMethod]
        public void TestUpdateUserInfo()
        {
            SMUController controller = SMUController.GetInstance(); 
            int u1 = controller.SignUp("Bruce Wayne1", "1Fisk", "gogogo1@yo.dk");

            string name = "Albert Einstein";
            string password = "Hesteboef";
            string email = "hest@yoyo.dk";

            bool b = controller.UpdateUserInfo(u1, email, name, password);
            SMUuser user = controller.GetUser(u1);

            Assert.AreEqual(name, user.username);
            Assert.AreEqual(password, user.password);
            Assert.AreEqual(email, user.email);
            Assert.AreNotEqual("test", user.username);
        }

        [TestMethod]
        public void TestDeleteAccount()
        {
            SMUController controller = SMUController.GetInstance(); 
            int u1 = controller.SignUp("Don Draper", "1Fisk", "gogogo1@yo.dk");
            Assert.AreEqual(true, controller.DeleteAccount(u1));
            try
            {
                controller.DeleteAccount(u1);
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }    
        }

        [TestMethod]
        public void TestAddBook()
        {
            SMUController controller = SMUController.GetInstance(); 
            int user = controller.SignUp("Lee Perry", "1Fisk", "gogogo1@yo.dk");
            try
            {
                int bookId = controller.AddBook(user, "the bible", "God", "Great Book", "religion", 100.0, "/testpathpdf", "testpathimg");
            }
            catch (Exception e) {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestRentBook()
        {
            SMUController controller = SMUController.GetInstance(); 
            int user = controller.SignUp("Sly Dunbar", "1Fisk", "gogogo1@yo.dk");
            int bookId = controller.AddBook(user, "The Torah", "Jah", "Great Book", "religion", 100.0, "/testpathpdf", "testpathimg");
            int rental = controller.RentBook(user, bookId, 0);
            try
            {
                controller.RentBook(Int32.MaxValue, Int32.MaxValue, 0);
                Assert.Fail();
            }
            catch (Exception)
            { 
            
            }
        }

        [TestMethod]
        public void TestDeleteBook()
        {
            SMUController controller = SMUController.GetInstance(); 
            int user = controller.SignUp("Anton Knopper", "1Fisk", "gogogo1@yo.dk");
            int bookId = controller.AddBook(user, "Book of the dead", "Jah", "Great Book", "religion", 100.0, "/testpathpdf", "testpathimg");

            Assert.AreEqual(true,controller.DeleteBook(user, bookId));
            try
            {
                controller.DeleteBook(user, Int32.MaxValue);
                Assert.Fail();
            }
            catch (Exception) { }
        }
    }
}
