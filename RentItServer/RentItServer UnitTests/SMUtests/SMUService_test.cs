using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.SMU;
using System.Collections.Generic;
using System.IO;

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
            int bookId = service.UploadBook(title, author, "gods book", "religious", 1000.0, new MemoryStream());
            Book book = service.GetBookInfo(bookId);

            Assert.AreEqual(bookId, book.Id);
            Assert.AreEqual(title, book.Title);
            Assert.AreEqual(author, book.Author);
        }

        [TestMethod]
        public void RentBookTest_service()
        {
            ServiceReference1.SMURentItServiceClient service = new ServiceReference1.SMURentItServiceClient();
            int bookId = service.UploadBook("Peter PLys", "A. A. Milne", "gbook", "religious", 1000.0, new MemoryStream());
            service.UploadPdf(bookId, new System.IO.MemoryStream());
            int userId = service.SignUp("peter@gogo.dk", "userman", "abekat1", false); 
            int rentId = service.RentBook(userId, bookId, 0);
        }


        [TestMethod]
        public void GetAllBooksTest_service()
        {
            ServiceReference1.SMURentItServiceClient service = new ServiceReference1.SMURentItServiceClient();
            int bookId1 = service.UploadBook("The Wolf", "Mr. Bean", "gods book", "religious", 1000.0, new MemoryStream());
            int bookId2 = service.UploadBook("The Bird", "Mr. Bean", "gods book", "religious", 1000.0, new MemoryStream());
            int bookId3 = service.UploadBook("The Rat", "Mr. Bean", "gods book", "religious", 1000.0, new MemoryStream());

            List<Book> list = new List<Book>(service.GetAllBooks());

            bool boo1 = false;
            bool boo2 = false;
            bool boo3 = false;
            foreach(Book b in list)
            {        
                if (b.Id == bookId1)
                    boo1 = true;
                if (b.Id == bookId2)
                    boo2 = true;
                if (b.Id == bookId3)
                    boo3 = true;
            }
            Assert.AreEqual(true, boo1 && boo2 && boo3);         
        }

        [TestMethod]
        public void DeleteBookTest_service()
        {
            ServiceReference1.SMURentItServiceClient service = new ServiceReference1.SMURentItServiceClient();
            int bookId = service.UploadBook("Captain Pepper", "Yoyooyoy", "gods book", "religious", 1000.0, new MemoryStream());
            service.DeleteBook(bookId);
            try
            {
                service.DeleteBook(bookId);
                Assert.Fail(); // must fail
            }
            catch (Exception)
            { 
            
            }
        }

        [TestMethod]
        public void UploadPDFTest_service()
        {
            ServiceReference1.SMURentItServiceClient service = new ServiceReference1.SMURentItServiceClient();
            int bookId = service.UploadBook("Sean Paul bio", "SP", "gods book", "religious", 1000.0, new MemoryStream());
            service.UploadPdf(bookId, new System.IO.MemoryStream());
            try
            {
                service.UploadPdf(Int32.MaxValue, new System.IO.MemoryStream());
                Assert.Fail(); // must fail
            }
            catch (Exception)
            { 
            
            }
        }


        [TestMethod]
        public void TestGetRental_service()
        {
            ServiceReference1.SMURentItServiceClient service = new ServiceReference1.SMURentItServiceClient();
            int bookId = service.UploadBook("Sasdasd", "SP", "gods book", "religious", 1000.0, new MemoryStream());
            service.UploadPdf(bookId, new System.IO.MemoryStream());
            int userId = service.SignUp("sdfsdfgdsg","sdgsdg","sdsdgsdgsdggsd",false);
            int rentalId = service.RentBook(userId, bookId, 0);
            Rental[] rentals = service.GetRental(userId, bookId);
            service.DeleteAccount(userId);
            Assert.AreEqual(rentalId, rentals[0].Id);
            Assert.AreEqual(0, rentals[0].MediaType);
        }
    }
}
