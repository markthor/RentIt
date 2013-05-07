﻿using System;
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
        private const int interval = 3;

        private Controller controller;
       
            
        [TestInitialize]
        public void Initialize()
        {
            controller = Controller.GetInstance();
            for (int i = 0; i < interval; i++)
            {
                testchannelnames.Add(testchannelname + i);
                testchanneldescrs.Add(testchanneldescr + i);
            }
        }

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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
            controller = Controller.GetInstance();
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
        public void Controller_GetChannelsWithFilter_Parameter_Default()
        {
            controller = Controller.GetInstance();
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
        public void Controller_GetChannelsWithFilter_Behavior_ProperOwner()
        {
            User user = _dao.SignUp(testname, testmail, testpw);
            testId = user.Id;
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(new ChannelSearchArgs()
                {
                    SearchString = testchannelname
                });

                List<Channel> theChannels = new List<Channel>(channels);
                foreach(Channel aChannel in theChannels)
                {
                    Assert.IsTrue(aChannel.ChannelOwner.Id == testId);
                }
            }
            catch
            {
                Cleanup();
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_PositiveInterval()
        {
            User user = _dao.SignUp(testname, testmail, testpw);
            testId = user.Id;
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

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_NegativeInterval()
        {
            User user = _dao.SignUp(testname, testmail, testpw);
            testId = user.Id;
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(new ChannelSearchArgs()
                {
                    StartIndex = -interval,
                    EndIndex = interval
                });

                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
            }
            catch
            {
                Cleanup();
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_SortHitsDescending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs args = new ChannelSearchArgs()
                {
                    SortOption = ChannelSearchArgs.HitsDesc
                };
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    channel.Hits = i;
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(args); 
                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be larger then current
                    Assert.IsTrue(theChannels[i-1].Hits > theChannels[i].Hits);   
                } 
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_SortHitsAscending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs args = new ChannelSearchArgs()
            {
                SortOption = ChannelSearchArgs.HitsAsc
            };
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    channel.Hits = i;
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(args);
                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be less then current
                    Assert.IsTrue(theChannels[i - 1].Hits < theChannels[i].Hits);
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_SortNumberOfCommentsDescending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs args = new ChannelSearchArgs()
            {
                SortOption = ChannelSearchArgs.NumberOfCommentsDesc
            };
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    channel.Hits = i;
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(args);
                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be larger then current
                    Assert.IsTrue(theChannels[i - 1].Comments.Count > theChannels[i].Comments.Count);
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_SortNumberOfCommentsAscending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs args = new ChannelSearchArgs()
            {
                SortOption = ChannelSearchArgs.NumberOfCommentsAsc
            };
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    channel.Hits = i;
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(args);
                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be less then current
                    Assert.IsTrue(theChannels[i - 1].Comments.Count < theChannels[i].Comments.Count);
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_SortRatingDescending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs args = new ChannelSearchArgs()
            {
                SortOption = ChannelSearchArgs.RatingDesc
            };
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    channel.Hits = i;
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(args);
                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be larger then current
                    Assert.IsTrue(theChannels[i - 1].Rating > theChannels[i].Rating);
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_SortRatingAscending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs args = new ChannelSearchArgs()
            {
                SortOption = ChannelSearchArgs.RatingAsc
            };
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    channel.Hits = i;
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(args);
                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be less then current
                    Assert.IsTrue(theChannels[i - 1].Rating < theChannels[i].Rating);
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_SortDescriptionsDescending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs args = new ChannelSearchArgs()
            {
                SortOption = ChannelSearchArgs.SubscriptionsDesc
            };
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    channel.Hits = i;
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(args);
                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be larger then current
                    Assert.IsTrue(theChannels[i - 1].Subscribers.Count > theChannels[i].Subscribers.Count);
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_SortDescriptionsAscending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs args = new ChannelSearchArgs()
            {
                SortOption = ChannelSearchArgs.SubscriptionsAsc
            };
            try
            {
                Channel channel = null;
                for (int i = 0; i < interval; i++)
                {
                    channel = _dao.CreateChannel(testchannelnames[i], testId, testchanneldescrs[i], new string[] { "jazz" });
                    channel.Hits = i;
                    testchannels.Add(channel);
                }
                IEnumerable<Channel> channels = controller.GetChannels(args);
                List<Channel> theChannels = new List<Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be less then current
                    Assert.IsTrue(theChannels[i - 1].Subscribers.Count < theChannels[i].Subscribers.Count);
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion
        #region Controller_UpdateUser
        [TestMethod]
        public void Controller_UpdateUser_Parameter_NegNullNullNull()
        {
            controller = Controller.GetInstance();
            User user = _dao.SignUp(testname, testmail, testpw);
            try
            {
                controller.UpdateUser(-1, null, null, null);
                User updatedUser = _dao.GetUser(user.Id);
                Assert.IsTrue(user.Username.Equals(updatedUser.Username));
                Assert.IsTrue(user.Email.Equals(updatedUser.Email));
                Assert.IsTrue(user.Password.Equals(updatedUser.Password));
            }
            catch
            {
                Cleanup();
                Assert.Fail("An exception was thrown");
            }
        }
        //[TestMethod]
        //public void Controller_UpdateUser_Parameter_NegEmptyNullNull()
        //{
        //    controller = Controller.GetInstance();
        //    User user = _dao.SignUp(testname, testmail, testpw);
        //    try
        //    {
        //        controller.UpdateUser(-1, "", null, null);
        //        User updatedUser = _dao.GetUser(user.Id);
        //        Assert.IsTrue(user.Username.Equals(updatedUser.Username));
        //        Assert.IsTrue(user.Email.Equals(updatedUser.Email));
        //        Assert.IsTrue(user.Password.Equals(updatedUser.Password));
        //    }
        //    catch
        //    {
        //        Cleanup();
        //        Assert.Fail("An exception was thrown");
        //    }
        //}
        #endregion
    }
}
