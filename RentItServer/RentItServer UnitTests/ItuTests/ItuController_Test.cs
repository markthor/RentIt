using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer;
using RentItServer.ITU;

namespace RentItServer_UnitTests.ItuTests
{
    [TestClass]
    public class ItuController_Test
    {
        private DatabaseDao _dao = DatabaseDao.GetInstance();

        private const string testname = "TestDummy9000";
        private const string testmail = "TestDummy@9000.gg";
        private const string testpw = "TestDummyPassword9000";
        private int testId = int.MaxValue;

        private const string testchannelname = "TestDummyChannel9000";
        private const string testchanneldescr = "TestDummyChannel9000Description";
        private readonly List<string> testchannelnames = new List<string>();
        private readonly List<string> testchanneldescrs = new List<string>();
        private readonly List<Channel> testchannels = new List<Channel>();
            
        [TestCleanup]
        public void Cleanup()
        {
            foreach (Channel channel in testchannels)
            {
                _dao.DeleteChannel(testId, channel);
            } 
            if (testId != int.MaxValue)
            {
                _dao.DeleteUser(testId);
                testId = int.MaxValue;
            }
        }

        #region Controller_Signup

        [TestMethod]
        public void Controller_SignUp_Parameter_NullNullNull()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp(null, null, null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_EmptyNullNull()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp("", null, null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_NullEmptyNull()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp(null, "", null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_NullNullEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp(null, null, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_EmptyEmptyNull()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp("", "", null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_NullEmptyEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp(null, "", "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_EmptyNullEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp(null, "", "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_EmptyEmptyEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp("", "", "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_ValidEmptyEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp(testname, "", "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_EmptyValidEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp("", testmail, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_EmptyEmptyValid()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp("", "", testpw);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_ValidValidEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp(testname, testmail, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_EmptyValidValid()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp("", testmail, testpw);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_ValidEmptyValid()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.SignUp(testname, "", testpw);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //this is good
            }
        }

        [TestMethod]
        public void Controller_SignUp_Parameter_ValidValidValid()
        {
            Controller controller = Controller.GetInstance();
            User user = controller.SignUp(testname, testmail, testpw);
            testId = user.Id;
            Assert.IsNotNull(user);
            Assert.AreEqual(testname, user.Username);
            Assert.AreEqual(testmail, user.Email);
            Assert.AreEqual(testpw, user.Password);
        }
        #endregion
        #region Controller_Login
        [TestMethod]
        public void Controller_Login_Parameter_NullNull()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login(null, null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //This is good
            }
        }

        [TestMethod]
        public void Controller_Login_Parameter_EmptyNull()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login("", null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }

        [TestMethod]
        public void Controller_Login_Parameter_NullEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login(null, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //This is good
            }
        }

        [TestMethod]
        public void Controller_Login_Parameter_EmptyEmpty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login("", "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }

        [TestMethod]
        public void Controller_Login_Parameter_ValidEmpty1()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login(testname, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }

        [TestMethod]
        public void Controller_Login_Parameter_ValidEmpty2()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login(testmail, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }

        [TestMethod]
        public void Controller_Login_Parameter_EmptyValid()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login("", testpw);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }

        [TestMethod]
        public void Controller_Login_Parameter_ValidValid1()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login(testname, testpw);
            }
            catch (ArgumentException)
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_Login_Parameter_ValidValid2()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                User user = controller.Login(testmail, testpw);
            }
            catch (ArgumentException)
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion
        #region Controller_DeleteUser
        [TestMethod]
        public void Controller_DeleteUser_Parameter_Neg()
        {
            Controller controller = Controller.GetInstance();
            User user = _dao.SignUp(testname, testmail, testpw);
            testId = user.Id;
            try
            {
                controller.DeleteUser(-1);
                Assert.Fail("No exception was thrown");
            }
            catch (Exception)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteUser_Parameter_MaxInt()
        {
            Controller controller = Controller.GetInstance();
            User user = _dao.SignUp(testname, testmail, testpw);
            testId = user.Id;
            try
            {
                controller.DeleteUser(int.MaxValue);
                Assert.Fail("No exception was thrown");
            }
            catch (Exception)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteUser_Parameter_MinInt()
        {
            Controller controller = Controller.GetInstance();
            User user = _dao.SignUp(testname, testmail, testpw);
            testId = user.Id;
            try
            {
                controller.DeleteUser(int.MinValue);
                Assert.Fail("No exception was thrown");
            }
            catch (Exception)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteUser_Parameter_Valid()
        {
            Controller controller = Controller.GetInstance();
            User user = _dao.SignUp(testname, testmail, testpw);
            testId = user.Id;
            try
            {
                controller.DeleteUser(testId);
                testId = int.MaxValue;
            }
            catch (Exception)
            {
                Assert.Fail("An exception was thrown");
            }
        }
        #endregion

        #region Controller_GetChannelsWithFilter
        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_Null()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                controller.GetChannels(null);
                Assert.Fail("No exception was raised");
            }
            catch
            {
                // This is good
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_Empty()
        {
            Controller controller = Controller.GetInstance();
            try
            {
                controller.GetChannels(new ChannelSearchArgs());
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_Interval()
        {
            Controller controller = Controller.GetInstance();
            User user = _dao.SignUp(testname, testmail, testpw);
            testId = user.Id;
            int interval = 10;
            for (int i = 0; i < interval; i++)
            {
                testchannelnames.Add(testchannelname + i);
                testchanneldescrs.Add(testchanneldescr + i);
            } 
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[]{"jazz"});
                    testchannels.Add(channel);
                } 
                RentItServer.ITU.DatabaseWrapperObjects.Channel[] channels = controller.GetChannels(new ChannelSearchArgs()
                    {
                        StartIndex = 0,
                        EndIndex = interval
                    });

                Assert.IsTrue(channels.Length >= interval);
            }
            catch
            {
                Cleanup();
                Assert.Fail("An exception was raised");
            }
        }
        #endregion
    }
}
