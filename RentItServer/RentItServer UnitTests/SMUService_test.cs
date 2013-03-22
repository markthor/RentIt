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


        [TestMethod]
        public void AddBookTest_service()
        {
            ServiceReference1.SMURentItServiceClient service = new ServiceReference1.SMURentItServiceClient();
            string title = "bible";
            string author = "God";
            int bookId = service.UploadBook(title, author, "gods book", "religious", DateTime.Now, 1000.0);
            int rentId = service.RentBook(1, bookId, DateTime.Now, 0);
            Book book = service.GetBookInfo(bookId);

            Assert.AreEqual(bookId, book.id);
            Assert.AreEqual(title, book.title);
            Assert.AreEqual(author, book.author);

        }
    }
}
