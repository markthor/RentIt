﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.SMU;
using RentItServer;
using System.Data.Entity;
using System.IO;

namespace RentItServer_UnitTests
{
    [TestClass]
    public class SMUController_Test
    {
        /// <summary>
        /// Deletes all tuples in SMU database.
        /// </summary>
        //[TestInitialize()]
        public void CleanDataBase()
        {
            SMUController.GetInstance().DeleteSMUDatabaseData();
        }

        /// <summary>
        /// Deletes all tuples in SMU database.
        /// </summary>
        //[ClassCleanup()]
        public static void CleanDataBaseFinish()
        {
            SMUController.GetInstance().DeleteSMUDatabaseData();
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

            int u1 = controller.SignUp(email1, "Jens", password1, false);
            int u2 = controller.SignUp(email2, "Jens", password2, false);
            int u3 = controller.SignUp(email3, "Jens", password3, false);

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

            Assert.AreEqual(u1, controller.GetUser(u1).id);
            Assert.AreEqual(u2, controller.GetUser(u2).id);
            Assert.AreEqual(u3, controller.GetUser(u3).id);
            Assert.AreNotEqual(u1, controller.GetUser(u3).id);
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
            RentItServer.SMU.User user = controller.GetUser(u1);

            Assert.AreEqual(name, user.username);
            Assert.AreEqual(password, user.password);
            Assert.AreEqual(email, user.email);
            Assert.AreNotEqual("test", user.username);
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
                controller.AddBook("the bible", "God", "Great Book", "religion", DateTime.Now, 100.0);
            }
            catch (Exception e) {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestRentBook()
        {
            SMUController controller = SMUController.GetInstance(); 
            int user = controller.SignUp("Sly Dunbar", "1Fisk", "gogogo1@yo.dk", false);
            int bookId = controller.AddBook("The Torah", "Jah", "Great Book", "religion", DateTime.Now, 100.0);
            int rental = controller.RentBook(user, bookId, DateTime.Now, 0);
            try
            {
                controller.RentBook(Int32.MaxValue, Int32.MaxValue,DateTime.Now , 0);
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
            int userId1 = controller.SignUp("Sly Dunbar", "1Fisk", "gogogo1@yo.dk", false);
            int userId2 = controller.SignUp("Bumbas", "TorskT", "gogogo2@yo.dk", false);
            int userId3 = controller.SignUp("Hippo", "HajH", "gogogo3@yo.dk", false);
            int bookId = controller.AddBook("The Torah", "Jah", "Great Book", "religion", DateTime.Now, 100.0);
            int mediaTypeBook = 0;
            int mediaTypeAudio = 1;
            int mediaTypeBoth = 2;
            RentAndVerify(userId1, bookId, mediaTypeBook, mediaTypeBook);
            RentAndVerify(userId1, bookId, mediaTypeAudio, mediaTypeBoth);
            RentAndVerify(userId2, bookId, mediaTypeBoth, mediaTypeBoth);
            RentAndVerify(userId3, bookId, mediaTypeAudio, mediaTypeAudio);

        }

        public void RentAndVerify(int userId, int bookId, int mediaTypeRent, int mediaTypeAssert)
        {
            SMUController controller = SMUController.GetInstance();
            int result = -1;
            try
            {
                controller.RentBook(userId, bookId, DateTime.Now, mediaTypeRent);
                result = controller.HasRental(userId, bookId);
            }
            catch (Exception e)
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
            DateTime time = DateTime.Now;
            double price = 100;
            Book book = null;
            int bookId = controller.AddBook(title, author, description, genre, time, price);
            try
            {
                book = controller.GetBookInfo(bookId);
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
            Assert.AreEqual(title, book.title);
            Assert.AreEqual(author, book.author);
            Assert.AreEqual(genre, book.genre);
            Assert.AreEqual(description, book.description);
            Assert.AreEqual(price, book.price);
        }

        [TestMethod]
        public void TestDeleteBook()
        {
            SMUController controller = SMUController.GetInstance(); 
            int user = controller.SignUp("Anton Knopper", "1Fisk", "gogogo1@yo.dk", false);
            int bookId = controller.AddBook("Book of the dead", "Jah", "Great Book", "religion", DateTime.Now, 100.0);

            controller.DeleteBook(bookId);
            try
            {
                controller.DeleteBook(Int32.MaxValue);
                Assert.Fail();
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void TestGetAllBooks()
        {
            //TODO: Clean DB
            SMUController controller = SMUController.GetInstance();
            try
            {
                controller.AddBook("the bible", "God", "Great Book", "religion", DateTime.Now, 100.0);
                controller.AddBook("Book of the dead", "Jah", "Great Book", "religion", DateTime.Now, 100.0);
                controller.AddBook("Fall Of The Giants", "Ken Folett", "In the twentieth century, man must fight for survival... ", "Faction", DateTime.Now, 400.0);
                controller.AddBook("The Art Of War", "Sin Zu", "Fight or die trying", "Battle Manual", DateTime.Now, 150.0);
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
            int bookId = controller.AddBook("asd", "asd", "asd", "asd", DateTime.Now, 0);
            
            string path = Directory.GetCurrentDirectory();
            string filename = "test.pdf";
            path = string.Concat(path, "\\..\\..\\..\\RentItServer\\Test Files\\", filename);
            MemoryStream uploadedPdf = new MemoryStream();
            File.OpenRead(path).CopyTo(uploadedPdf);
            uploadedPdf.Position = 0L;

            long uploadedPdfLength = uploadedPdf.Length;

            controller.UploadPDF(bookId, uploadedPdf);

            MemoryStream downloadedPdf = controller.DownloadPDF(bookId);

            Assert.IsTrue(uploadedPdfLength > 0 && uploadedPdfLength == downloadedPdf.Length);
        }
    }
}