using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.SMU;
using System.IO;
using RentItServer;

namespace RentItServer_UnitTests
{
    [TestClass]
    public class SMUController_Test
    {
        /// <summary>
        /// Deletes all tuples in SMU database.
        /// </summary>
        [TestInitialize]
        public void CleanDataBase()
        {
            //SMUController.GetInstance().DeleteSMUDatabaseData();
        }

        /// <summary>
        /// Deletes all tuples in SMU database.
        /// </summary>
        [ClassCleanup]
        public static void CleanDataBaseFinish()
        {
            //SMUController.GetInstance().DeleteSMUDatabaseData();
        }

        [TestMethod]
        public void TestSignUp()
        {
            SMUController controller = SMUController.GetInstance();
            string email1 = "gogogo1@yo.dk";
            string email2 = "gogogo2@yo.dk";
            string email3 = "gogogo3@yo.dk";

            string password1 = "1Fisk";
            string password2 = "12Fisk";
            string password3 = "123Fisk";

            controller.SignUp(email1, "Jens", password1, false);
            controller.SignUp(email2, "Jens", password2, true);
            controller.SignUp(email3, "Jens", password3, false);

            controller.LogIn(email1, password1);
            controller.LogIn(email2, password2);
            controller.LogIn(email3, password3);
        }

        [TestMethod]
        public void TestGetUser()
        {
            SMUController controller = SMUController.GetInstance();
            int u1 = controller.SignUp("Peter Parker1", "1Fisk", "gogogo1@yo.dk", false);
            int u2 = controller.SignUp("Peter Parker2", "12Fisk", "gogogo2@yo.dk", false);
            int u3 = controller.SignUp("Peter Parker3", "123Fisk", "gogogo3@yo.dk", false);

            Assert.AreEqual(u1, controller.GetUserInfo(u1).Id);
            Assert.AreEqual(u2, controller.GetUserInfo(u2).Id);
            Assert.AreEqual(u3, controller.GetUserInfo(u3).Id);
            Assert.AreNotEqual(u1, controller.GetUserInfo(u3).Id);
        }

        [TestMethod]
        public void TestUpdateUserInfo()
        {
            SMUController controller = SMUController.GetInstance();
            int u1 = controller.SignUp("Bruce Wayne1", "1Fisk", "gogogo1@yo.dk", false);

            string name = "Albert Einstein";
            string password = "Hesteboef";
            string email = "hest@yoyo.dk";

            controller.UpdateUserInfo(u1, email, name, password, false);
            RentItServer.SMU.User user = controller.GetUserInfo(u1);

            Assert.AreEqual(name, user.Username);
            Assert.AreEqual(password, user.Password);
            Assert.AreEqual(email, user.Email);
            Assert.AreNotEqual("test", user.Username);
        }

        [TestMethod]
        public void TestDeleteAccount()
        {
            SMUController controller = SMUController.GetInstance();
            int u1 = controller.SignUp("Don Draper", "1Fisk", "gogogo1@yo.dk", false);
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
            try
            {
                controller.AddBook("the bible", "God", "Great Book", "religion", 100.0, new MemoryStream());
                controller.AddBook("Koran", "Allah", "Great Book", "religion", 100000000.0, new MemoryStream());
                controller.AddBook("Book of the dead", "Dalai Lama", "Great Book", "religion", 0.0, new MemoryStream());
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestRentBook()
        {
            SMUController controller = SMUController.GetInstance();
            int user = controller.SignUp("Slyh Dunbar", "1Fisk", "gogogo1@yo.dk", false);
            int bookId = controller.AddBook("The Torah", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
            controller.UploadPDF(bookId, new MemoryStream());
            controller.RentBook(user, bookId, 0);

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
        public void TestHasRental()
        {
            SMUController controller = SMUController.GetInstance();
            int userId1 = controller.SignUp("Uhbah Dunbar", "1Fisk", "gogogo1@yo.dk", false);
            int userId2 = controller.SignUp("Bumbas", "TorskT", "gogogo2@yo.dk", false);
            controller.SignUp("Hippo", "HajH", "gogogo3@yo.dk", false);
            int bookId1 = controller.AddBook("The Torah", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
            int bookId2 = controller.AddBook("Blooms book", "Salla", "Great Book", "religion", 100.0, new MemoryStream());

            controller.UploadPDF(bookId1, new MemoryStream());
            controller.UploadPDF(bookId2, new MemoryStream());
            controller.UploadAudio(bookId1, new MemoryStream(), "narrator Geo");
            controller.UploadAudio(bookId2, new MemoryStream(), "narrator John");

            int mediaTypeBook = 0;
            int mediaTypeAudio = 1;
            int mediaTypeBoth = 2;
            RentAndVerify(userId1, bookId1, mediaTypeBook, mediaTypeBook);
            RentAndVerify(userId1, bookId1, mediaTypeAudio, mediaTypeBoth);
            RentAndVerify(userId2, bookId2, mediaTypeBoth, mediaTypeBoth);
        }

        public void RentAndVerify(int userId, int bookId, int mediaTypeRent, int mediaTypeAssert)
        {
            SMUController controller = SMUController.GetInstance();
            int result = -1;
            try
            {
                controller.RentBook(userId, bookId, mediaTypeRent);
                result = controller.HasRental(userId, bookId);
            }
            catch (Exception)
            {
                Assert.Fail(); 
            }
            Assert.AreEqual(result, mediaTypeAssert);
        }

        [TestMethod]
        public void TestGetBookInfo()
        {
            SMUController controller = SMUController.GetInstance();
            string title = "Ender's Game";
            string author = "Orson Scott Card";
            string genre = "Science Fiction";
            string description = "SciFi adventure far out in outer space";
            double price = 100;
            Book book = null;
            int bookId = controller.AddBook(title, author, description, genre, price, new MemoryStream());
            try
            {
                book = controller.GetBookInfo(bookId);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.AreEqual(title, book.Title);
            Assert.AreEqual(author, book.Author);
            Assert.AreEqual(genre, book.Genre);
            Assert.AreEqual(description, book.Description);
            Assert.AreEqual(price, book.Price);
        }

        [TestMethod]
        public void TestGetActiveUserRentals()
        {
            SMUController controller = SMUController.GetInstance();

            int userId = controller.SignUp("Hippo", "HajH", "gogogo3@yo.dk", false);
            int bookId1 = controller.AddBook("The Torah", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
            int bookId2 = controller.AddBook("Blooms book", "Salla", "Great Book", "religion", 100.0, new MemoryStream());
            controller.UploadPDF(bookId1, new MemoryStream());
            controller.UploadPDF(bookId2, new MemoryStream());
            controller.UploadAudio(bookId1, new MemoryStream(), "narrator Geo");
            controller.UploadAudio(bookId2, new MemoryStream(), "narrator John");

            int rentId1 = controller.RentBook(userId, bookId1, 0);
            int rentId2 = controller.RentBook(userId, bookId1, 1);
            int rentId3 = controller.RentBook(userId, bookId2, 2);

            Rental[] rentals = controller.GetActiveUserRentals(userId);
            Assert.IsTrue(ContainsRental(rentId1, rentals));
            Assert.IsTrue(ContainsRental(rentId2, rentals));
            Assert.IsTrue(ContainsRental(rentId3, rentals));
        }

        [TestMethod]
        public void TestGetAllUserRentals()
        {
            SMUController controller = SMUController.GetInstance();

            int userId = controller.SignUp("Hippo", "HajH", "gogogo3@yo.dk", false);
            int bookId1 = controller.AddBook("The Torah", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
            int bookId2 = controller.AddBook("Blooms book", "Salla", "Great Book", "religion", 100.0, new MemoryStream());
            controller.UploadPDF(bookId1, new MemoryStream());
            controller.UploadPDF(bookId2, new MemoryStream());
            controller.UploadAudio(bookId1, new MemoryStream(), "narrator Geo");
            controller.UploadAudio(bookId2, new MemoryStream(), "narrator John");

            int rentId1 = controller.RentBook(userId, bookId1, 0);
            int rentId2 = controller.RentBook(userId, bookId1, 1);
            int rentId3 = controller.RentBook(userId, bookId2, 2);

            Rental[] rentals = controller.GetAllUserRentals(userId);
            Assert.IsTrue(ContainsRental(rentId1, rentals));
            Assert.IsTrue(ContainsRental(rentId2, rentals));
            Assert.IsTrue(ContainsRental(rentId3, rentals));
        }

        public Boolean ContainsRental(int rentalId, Rental[] rentals)
        {
            foreach (Rental r in rentals)
            {
                if (r.Id == rentalId) return true;
            }
            return false;
        }



        [TestMethod]
        public void TestDeleteBook()
        {
            SMUController controller = SMUController.GetInstance();
            int bookId1 = controller.AddBook("Book of the dead", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
            int bookId2 = controller.AddBook("Blooms book", "Salla", "Great Book", "religion", 100.0, new MemoryStream());
            controller.UploadPDF(bookId1, new MemoryStream());
            controller.DeleteBook(bookId1);
            controller.DeleteBook(bookId2);
            try
            {
                // negative tests
                controller.DeleteBook(bookId1);
                controller.DeleteBook(Int32.MaxValue);
                Assert.Fail();
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void TestGetAllBooks()
        {
            SMUController controller = SMUController.GetInstance();
            try
            {
                controller.AddBook("the bible", "God", "Great Book", "religion", 100.0, new MemoryStream());
                controller.AddBook("Book of the dead", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
                controller.AddBook("Fall Of The Giants", "Ken Folett", "In the twentieth century, man must fight for survival... ", "Faction", 400.0, new MemoryStream());
                controller.AddBook("The Art Of War", "Sin Zu", "Fight or die trying", "Battle Manual", 150.0, new MemoryStream());
                Book[] result = controller.GetAllBooks();
                Assert.AreEqual(4, result.Length);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestUploadDownloadPdf()
        {
            SMUController controller = SMUController.GetInstance();
            int bookId = controller.AddBook("asd", "asd", "asd", "asd", 0, new MemoryStream());
            
            MemoryStream uploadedPdf = new MemoryStream(TestFiles.testpdf);
            uploadedPdf.Position = 0L;

            long uploadedPdfLength = uploadedPdf.Length;

            controller.UploadPDF(bookId, uploadedPdf);

            MemoryStream downloadedPdf = controller.DownloadPDF(bookId);

            Assert.IsTrue(uploadedPdfLength > 0 && uploadedPdfLength == downloadedPdf.Length);
        }

        //Bad test, does not test for exclution of books with low hit count.
        [TestMethod]
        public void TestGetPopularBooks()
        {
            SMUController controller = SMUController.GetInstance();
            Book[] result = null;
            try
            {
                controller.AddBook("the bible", "God", "Great Book", "religion", 100.0, new MemoryStream());
                controller.AddBook("Book of the dead", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
                controller.AddBook("Fall Of The Giants", "Ken Folett", "In the twentieth century, man must fight for survival... ", "Faction", 400.0, new MemoryStream());
                controller.AddBook("The Art Of War", "Sin Zu", "Fight or die trying", "Battle Manual", 150.0, new MemoryStream());
                result = controller.GetPopularBooks();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.AreEqual(4, result.Length);
        }

        [TestMethod]
        public void TestSearchBooks()
        {
            SMUController controller = SMUController.GetInstance();
            Book[] result1 = null;
            Book[] result2 = null;
            Book[] result3 = null;
            Book[] result4 = null;
            try
            {
                controller.AddBook("the bible", "God", "Great Book", "religion", 100.0, new MemoryStream());
                controller.AddBook("Book of the dead", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
                controller.AddBook("Fall Of The Giants", "Ken Folett", "In the twentieth century, man must fight for survival... ", "Faction", 400.0, new MemoryStream());
                controller.AddBook("The Art Of War", "Sin Zu", "Fight or die trying", "Battle Manual", 150.0, new MemoryStream());
                result1 = controller.SearchBooks("Fall Of The Giants");
                result2 = controller.SearchBooks("Sin");
                result3 = controller.SearchBooks("lett");
                result4 = controller.SearchBooks("God");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.AreEqual(1, result1.Length);
            Assert.AreEqual(1, result2.Length);
            Assert.AreEqual(1, result3.Length);
            Assert.AreEqual(1, result4.Length);
            Assert.AreEqual("Ken Folett", result1[0].Author);
            Assert.AreEqual("Sin Zu", result2[0].Author);
            Assert.AreEqual("Ken Folett", result3[0].Author);
            Assert.AreEqual("God", result4[0].Author);
        }

        [TestMethod]
        public void TestGetBooksByGenre()
        {
            SMUController controller = SMUController.GetInstance();
            controller.AddBook("the bible", "God", "Great Book", "religion", 100.0, new MemoryStream());
            controller.AddBook("Book of the dead", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
            controller.AddBook("Fall Of The Giants", "Ken Folett", "In the twentieth century, man must fight for survival... ", "Faction", 400.0, new MemoryStream());
            controller.AddBook("The Art Of War", "Sin Zu", "Fight or die trying", "Battle Manual", 150.0, new MemoryStream());
        }

        [TestMethod]
        public void TestUploadDownloadImage()
        {
            SMUController controller = SMUController.GetInstance();

            //Convert image to MemoryStream
            MemoryStream uploadedImage = new MemoryStream();
            TestFiles.testimage.Save(uploadedImage, System.Drawing.Imaging.ImageFormat.Jpeg);
            uploadedImage.Position = 0L;
            long uploadStreamLength = uploadedImage.Length;

            //Upload the book with the image
            int bookId = controller.AddBook("asd", "asd", "asd", "asd", 0, uploadedImage);

            //Download the image
            long downloadStreamLength = controller.DownloadImage(bookId).Length;

            //Check that the two image streams are equal
            Assert.AreEqual(uploadStreamLength, downloadStreamLength);
        }

        [TestMethod]
        public void TestGetRental()
        {
            SMUController controller = SMUController.GetInstance();
            int user = controller.SignUp("Robbie Williams", "1Fisk", "gogogo1@yo.dk", false);
            int bookId = controller.AddBook("YOYO", "Jah", "Great Book", "religion", 100.0, new MemoryStream());
            controller.UploadPDF(bookId, new MemoryStream());
            controller.UploadAudio(bookId, new MemoryStream(), "Sean John");
            int rentalId = controller.RentBook(user, bookId, 0);
            int rentalId2 = controller.RentBook(user, bookId, 1);
            Rental[] rental = controller.GetRental(user, bookId);
            Assert.AreEqual(1, rental[0].MediaType);
            Assert.AreEqual(0, rental[1].MediaType);
            Assert.AreEqual(bookId, rental[0].BookId);
            Assert.AreEqual(user, rental[0].UserId);
            Assert.AreNotEqual(Int32.MaxValue, rental[0].BookId);      
        }
    }
}
