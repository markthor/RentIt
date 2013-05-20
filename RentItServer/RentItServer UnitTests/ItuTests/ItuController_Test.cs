using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer;
using RentItServer.ITU;
using RentItServer_UnitTests.ItuTestUtilities;
using RentItServer_UnitTests.ItuTests;

namespace RentItServer_UnitTests.ItuTests
{
    [TestClass]
    public class ItuController_Test
    {
        private DatabaseDao _dao = DatabaseDao.GetInstance();

        private const string testname = TestExtensions._user2name;
        private const string testmail = TestExtensions._user2email;
        private const string testpw = TestExtensions._userpassword;
        private const string testGenre = TestExtensions.genreName1;
        private const string testNameSignup = "testname";
        private const string testEmailSignup = "testemail";
        private const string testPasswordSignup = "testpw";
        private static RentItServer.ITU.DatabaseWrapperObjects.User _testUser = TestExtensions._testUser1;
        private int testId = int.MaxValue;

        private const string testchannelname = "TestDummyChannel9000";
        private const string testchanneldescr = "TestDummyChannel9000Description";
        private static readonly List<string> testchannelnames = new List<string>();
        private static readonly List<string> testchanneldescrs = new List<string>();
        private static readonly List<Channel> testchannels = new List<Channel>();

        private const int interval = 3;

        private static Controller controller;


        [TestInitialize]
        public void Initialize()
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
            TestExtensions.PopulateDatabase();
        }

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
        }

        /// <summary>
        /// Deletes all tuples in database.
        /// </summary>
        [ClassCleanup]
        public static void CleanDataBaseFinish()
        {
            DatabaseDao.GetInstance().DeleteDatabaseData();
            TestExtensions.PopulateDatabase();
        }

        [ClassInitialize]
        public static void CleanDataBaseStart(TestContext tc)
        {
            //DatabaseDao.GetInstance().DeleteDatabaseData();
            //TestExtensions.PopulateDatabase();
            controller = Controller.GetInstance();
            for (int i = 0; i < interval; i++)
            {
                testchannelnames.Add(testchannelname + i);
                testchanneldescrs.Add(testchanneldescr + i);
            }
        }

        #region Controller_Login
        [TestMethod]
        public void Controller_Login_Parameter_NullNull()
        {
            controller = Controller.GetInstance();
            try
            {
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login(null, null);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login("", null);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login(null, "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login("", "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login(testname, "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login(testmail, "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login("", testpw);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login(testname, testpw);
            }
            catch
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.Login(testmail, testpw);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion
        #region Controller_IsCorrectPassword
        [TestMethod]
        public void Controller_IsCorrectPassword_Parameter_InvalidNull()
        {
            try
            {
                controller.IsCorrectPassword(-1, null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsCorrectPassword_Parameter_InvalidEmpty()
        {
            try
            {
                controller.IsCorrectPassword(-1, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsCorrectPassword_Parameter_InvalidValid()
        {
            try
            {
                controller.IsCorrectPassword(-1, TestExtensions._userpassword);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsCorrectPassword_Parameter_ValidNull()
        {
            try
            {
                controller.IsCorrectPassword(TestExtensions._testUser2.Id, null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsCorrectPassword_Parameter_ValidEmpty()
        {
            try
            {
                controller.IsCorrectPassword(TestExtensions._testUser2.Id, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsCorrectPassword_Parameter_ValidValid()
        {
            Assert.IsTrue(controller.IsCorrectPassword(TestExtensions._testUser2.Id, TestExtensions._userpassword));
        }
        #endregion
        #region Controller_IsEmailAvailable
        [TestMethod]
        public void Controller_IsEmailAvailable_Parameter_Null()
        {
            try
            {
                controller.IsEmailAvailable(null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsEmailAvailable_Parameter_Empty()
        {
            try
            {
                controller.IsEmailAvailable("");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsEmailAvailable_Parameter_Invalid()
        {
            try
            {
                controller.IsEmailAvailable(testname);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsEmailAvailable_Parameter_Valid()
        {
            try
            {
                controller.IsEmailAvailable(TestExtensions._user1email);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_IsEmailAvailable_State_Available()
        {
            Assert.IsTrue(controller.IsEmailAvailable("thisisatest@thisisatest.org"));
        }
        [TestMethod]
        public void Controller_IsEmailAvailable_State_Unavailable()
        {
            Assert.IsFalse(controller.IsEmailAvailable(TestExtensions._user1email));
        }
        #endregion
        #region Controller_IsUsernameAvailable
        [TestMethod]
        public void Controller_IsUsernameAvailable_Parameter_Null()
        {
            try
            {
                controller.IsUsernameAvailable(null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsUsernameAvailable_Parameter_Empty()
        {
            try
            {
                controller.IsUsernameAvailable("");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_IsUsernameAvailable_Parameter_Valid()
        {
            try
            {
                controller.IsUsernameAvailable(TestExtensions._testUser1.Username);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_IsUsernameAvailable_State_Available()
        {
            Assert.IsTrue(controller.IsUsernameAvailable("iamatestuserthatnoonewilleverduplicate"));
        }
        [TestMethod]
        public void Controller_IsUsernameAvailable_State_Unavailable()
        {
            Assert.IsFalse(controller.IsUsernameAvailable(TestExtensions._testUser1.Username));
        }
        #endregion
        #region Controller_IsChannelNameAvailable

        [TestMethod]
        public void Controller_IsChannelNameAvailable_Parameter_InvalidNull()
        {
            try
            {
                controller.IsChannelNameAvailable(-1, null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }

        [TestMethod]
        public void Controller_IsChannelNameAvailable_Parameter_InvalidEmpty()
        {
            try
            {
                controller.IsChannelNameAvailable(-1, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_IsChannelNameAvailable_Parameter_InvalidValid()
        {
            try
            {
                controller.IsChannelNameAvailable(-1, TestExtensions._testChannel1.Name);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_IsChannelNameAvailable_Parameter_ValidNull()
        {
            try
            {
                controller.IsChannelNameAvailable(TestExtensions._testChannelId1, null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_IsChannelNameAvailable_Parameter_ValidEmpty()
        {
            try
            {
                controller.IsChannelNameAvailable(TestExtensions._testChannelId1, "");
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_IsChannelNameAvailable_Parameter_ValidValid()
        {
            try
            {
                controller.IsChannelNameAvailable(TestExtensions._testChannelId1, TestExtensions._testChannel1.Name);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_IsChannelNameAvailable_State_Available()
        {
            Assert.IsTrue(controller.IsChannelNameAvailable(TestExtensions._testChannelId1, "thischannelnamewillnotexistithink"));
        }
        [TestMethod]
        public void Controller_IsChannelNameAvailable_State_Unavailable()
        {
            Assert.IsFalse(controller.IsChannelNameAvailable(TestExtensions._testChannelId1, TestExtensions._testChannel2.Name));
        }
        #endregion
        #region Controller_GetSubscriberCount
        [TestMethod]
        public void Controller_GetSubscriberCount_Parameter_Invalid()
        {
            try
            {
                controller.GetSubscriberCount(-1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_GetSubscriberCount_Parameter_Valid()
        {
            try
            {
                controller.GetSubscriberCount(TestExtensions._testChannelId1);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetSubscriberCount_State_NoSubscribers()
        {
            Assert.IsTrue(controller.GetSubscriberCount(TestExtensions._testChannelId1) > 0);
        }
        [TestMethod]
        public void Controller_GetSubscriberCount_State_Subscribers()
        {
            Assert.IsTrue(controller.GetSubscriberCount(TestExtensions._testChannelId2) == 0);
        }
        #endregion
        #region Controller_GetChannelComments
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_InvalidNullNull()
        {
            try
            {
                controller.GetChannelComments(-1, null, null);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidNullNull()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, null, null);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidInvalidNull()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, -1, null);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidValidNull()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, 0, null);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidNullInvalid()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, null, -1);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidNullValid()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, null, 1);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidInvalidInvalid()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, -1, -1);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidInvalidValid()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, -1, 1);
            }
            catch 
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidValidInvalid()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, 0, -1);
            }
            catch 
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_Parameter_ValidValidValid()
        {
            try
            {
                controller.GetChannelComments(TestExtensions._testChannelId1, 0, 1);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_GetChannelComments_State_NoComments()
        {
            RentItServer.ITU.DatabaseWrapperObjects.Comment[] comments = controller.GetChannelComments(TestExtensions._testChannelId2, 0, 1);
            Assert.IsTrue(comments.Length == 0);
        }
        [TestMethod]
        public void Controller_GetChannelComments_State_Comments()
        {
            RentItServer.ITU.DatabaseWrapperObjects.Comment[] comments = controller.GetChannelComments(TestExtensions._testChannelId1, 0, 1);
            Assert.IsTrue(comments.Length > 0);
        }
        #endregion

        /* R0 */
        #region Controller_CreateChannel
        [TestMethod]
        public void Controller_CreateChannel_State_ChannelCreated()
        {
            try
            {
                string channelName = "thischannelisatestchannel";
                string channelDescr = "a description";
                int channelId = controller.CreateChannel(channelName, TestExtensions._testUser1.Id,
                                                         channelDescr, new string[] { TestExtensions.genreName1 });
                RentItServer.ITU.DatabaseWrapperObjects.Channel channel = _dao.GetChannel(channelId).GetChannel();
                Assert.IsTrue(channel.OwnerId == TestExtensions._testUser1.Id);
                Assert.IsTrue(channel.Description.Equals(channelDescr));
                Assert.IsTrue(channel.Name.Equals(channelName));
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion
        #region Controller_DeleteChannel
        [TestMethod]
        public void Controller_DeleteChannel_State_ChannelDeleted()
        {
            try
            {
                controller.DeleteChannel(TestExtensions._testChannelId1);
                Channel channel = _dao.GetChannel(TestExtensions._testChannelId1);
                Assert.IsNull(channel);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion
        #region Controller_UpdateChannel
        [TestMethod]
        public void Controller_UpdateChannel_State_UpdatedAllAttributes()
        {
            try
            {
                int? updatedOwnerId = TestExtensions._testUser2.Id;
                string updatedChannelName = "thisisanewnameforatestchannel";
                string updatedDescription = "thisisanewandupdateddescriptionofatestchannel";
                double? updatedHits = 1000;
                double? updatedRating = 10000;
                controller.UpdateChannel(TestExtensions._testChannelId1, updatedOwnerId, updatedChannelName, updatedDescription, updatedHits, updatedRating);
                Channel channel = _dao.GetChannel(TestExtensions._testChannelId1);
                Assert.IsTrue(channel.UserId == updatedOwnerId);
                Assert.IsTrue(channel.Description.Equals(updatedDescription));
                Assert.IsTrue(channel.Hits == updatedHits);
                Assert.IsTrue(channel.Rating == updatedRating);
                Assert.IsTrue(channel.Name.Equals(updatedChannelName));
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_UpdateChannel_State_UpdatedNoAttributes()
        {
            try
            {
                controller.UpdateChannel(TestExtensions._testChannelId1, null, null, null, null, null);
                Channel channel = _dao.GetChannel(TestExtensions._testChannelId1);
                Assert.IsTrue(channel.UserId == TestExtensions._testUser1.Id);
                Assert.IsTrue(channel.Description.Equals(TestExtensions._testChannel1Description));
                Assert.IsTrue(channel.Hits == TestExtensions._testChannel1Hits);
                Assert.IsTrue(channel.Rating == TestExtensions._testChannel1Rating);
                Assert.IsTrue(channel.Name.Equals(TestExtensions._testChannel1Name));
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion

        /* R2 */
        #region Controller_CreateVote
        [TestMethod]
        public void Controller_CreateVote_Parameter_InvalidInvalidInvalid()
        {
            try
            {
                controller.CreateVote(0, -1, -1);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_CreateVote_Parameter_InvalidValidInvalid()
        {
            try
            {
                controller.CreateVote(0, TestExtensions._testUser2.Id, -1);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_CreateVote_Parameter_ValidInvalidInvalid()
        {
            try
            {
                controller.CreateVote(1, -1, -1);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_CreateVote_Parameter_ValidValidInvalid()
        {
            try
            {
                controller.CreateVote(1, TestExtensions._testUser2.Id, -1);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_CreateVote_Parameter_InvalidInvalidValid()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.CreateVote(0, -1, track.Id);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_CreateVote_Parameter_InvalidValidValid()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.CreateVote(0, TestExtensions._testUser2.Id, track.Id);
            }
            catch (ArgumentException e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        [TestMethod]
        public void Controller_CreateVote_Parameter_ValidInvalidValid()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.CreateVote(1, -1, track.Id);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_CreateVote_Parameter_ValidValidValid()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.CreateVote(1, TestExtensions._testUser2.Id, track.Id);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        [TestMethod]
        public void Controller_CreateVote_State_IgnoreVoteZero()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.CreateVote(0, TestExtensions._testUser2.Id, track.Id);
                Assert.IsNull(_dao.GetVote(TestExtensions._testUser2.Id, track.Id));
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        [TestMethod]
        public void Controller_CreateVote_State_IgnoreVoteMinInt()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.CreateVote(int.MinValue, TestExtensions._testUser2.Id, track.Id);
                Assert.IsNull(_dao.GetVote(TestExtensions._testUser2.Id, track.Id));
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        [TestMethod]
        public void Controller_CreateVote_State_IgnoreVoteMaxInt()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.CreateVote(int.MaxValue, TestExtensions._testUser2.Id, track.Id);
                Assert.IsNull(_dao.GetVote(TestExtensions._testUser2.Id, track.Id));
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        [TestMethod]
        public void Controller_CreateVote_State_Upvoting()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                    {
                        Artist = "testartist",
                        Name = "testname",
                        Path = ""
                    });
                controller.CreateVote(1, TestExtensions._testUser2.Id, track.Id);
                RentItServer.ITU.DatabaseWrapperObjects.Track aTrack = _dao.GetTrack(track.Id).GetTrack();
                Assert.IsTrue(aTrack.UpVotes == 1);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        [TestMethod]
        public void Controller_CreateVote_State_Downvoting()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.CreateVote(-1, TestExtensions._testUser2.Id, track.Id);
                RentItServer.ITU.DatabaseWrapperObjects.Track aTrack = _dao.GetTrack(track.Id).GetTrack();
                Assert.IsTrue(aTrack.DownVotes == 1);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        #endregion
        #region Controller_DeleteVote
        [TestMethod]
        public void Controller_DeleteVote_Parameter_InvalidInvalid()
        {
            try
            {
                controller.DeleteVote(-1, -1);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteVote_Parameter_ValidInvalid()
        {
            try
            {
                controller.DeleteVote(TestExtensions._testUser2.Id, -1);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteVote_Parameter_InvalidValid()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                controller.DeleteVote(-1, track.Id);
                Assert.Fail("No exception was thrown ");
            }
            catch (ArgumentException)
            {
                // This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteVote_Parameter_ValidValid()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                _dao.CreateVote(1, TestExtensions._testUser2.Id, track.Id);
                RentItServer.ITU.DatabaseWrapperObjects.Track aTrack = _dao.GetTrack(track.Id).GetTrack();
                if (aTrack.UpVotes != 1) Assert.Fail("Only added 1 vote, but the track has more or less than 1");
                controller.DeleteVote(TestExtensions._testUser2.Id, track.Id);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        [TestMethod]
        public void Controller_DeleteVote_State_DeleteUpvote()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                _dao.CreateVote(1, TestExtensions._testUser2.Id, track.Id);
                RentItServer.ITU.DatabaseWrapperObjects.Track aTrack = _dao.GetTrack(track.Id).GetTrack();
                if (aTrack.UpVotes != 1) Assert.Fail("Only added 1 vote, but the track has more or less than 1");
                controller.DeleteVote(TestExtensions._testUser2.Id, track.Id);
                RentItServer.ITU.DatabaseWrapperObjects.Track anotherTrack = _dao.GetTrack(track.Id).GetTrack();
                Assert.IsTrue(anotherTrack.DownVotes == 0);
                Assert.IsTrue(anotherTrack.UpVotes == 0);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        [TestMethod]
        public void Controller_DeleteVote_State_DeleteDownvote()
        {
            try
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                {
                    Artist = "testartist",
                    Name = "testname",
                    Path = ""
                });
                _dao.CreateVote(-1, TestExtensions._testUser2.Id, track.Id);
                RentItServer.ITU.DatabaseWrapperObjects.Track aTrack = _dao.GetTrack(track.Id).GetTrack();
                if (aTrack.DownVotes != 1) Assert.Fail("Only added 1 vote, but the track has more or less than 1");
                controller.DeleteVote(TestExtensions._testUser2.Id, track.Id);
                RentItServer.ITU.DatabaseWrapperObjects.Track anotherTrack = _dao.GetTrack(track.Id).GetTrack();
                Assert.IsTrue(anotherTrack.DownVotes == 0);
                Assert.IsTrue(anotherTrack.UpVotes == 0);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was thrown " + e);
            }
        }
        #endregion

        /* R4 */
        #region Controller_UnSubscribe
        [TestMethod]
        public void Controller_UnSubscribe_Parameter_InvalidInvalid()
        {
            try
            {
                controller.UnSubscribe(-1, -1);
                Assert.Fail("An exception was raised");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UnSubscribe_Parameter_InvalidValid()
        {
            try
            {
                controller.UnSubscribe(-1, TestExtensions._testChannelId1);
                Assert.Fail("An exception was raised");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UnSubscribe_Parameter_ValidInvalid()
        {
            try
            {
                controller.UnSubscribe(TestExtensions._testUser2.Id, -1);
                Assert.Fail("An exception was raised");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UnSubscribe_Parameter_ValidValid()
        {
            try
            {
                controller.UnSubscribe(TestExtensions._testUser1.Id, TestExtensions._testChannelId1);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_UnSubscribe_State_SubscribeCount()
        {
            int subsBefore = _dao.GetSubscriberCount(TestExtensions._testChannelId1);
            try
            {
                controller.UnSubscribe(TestExtensions._testUser1.Id, TestExtensions._testChannelId1);
                int subsAfter = _dao.GetSubscriberCount(TestExtensions._testChannelId1);
                Assert.IsTrue(subsBefore > subsAfter);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        [TestMethod]
        public void Controller_UnSubscribe_State_RemovedVotes()
        {
            for (int i = 0; i < interval; i++)
            {
                Track track = _dao.CreateTrackEntry(TestExtensions._testChannelId1, new Track()
                    {
                        Artist = "testartist"+i,
                        Name = "testname"+i,
                        Path = ""
                    });
                controller.CreateVote(1, TestExtensions._testUser1.Id, track.Id);
            }
            int votesBefore = _dao.GetUserVotes(TestExtensions._testUser1.Id).Count;
            // precondition check
            Assert.IsTrue(votesBefore == interval);
            try
            {
                controller.UnSubscribe(TestExtensions._testUser1.Id, TestExtensions._testChannelId1);
                int votesAfter = _dao.GetUserVotes(TestExtensions._testUser1.Id).Count;
                Assert.IsTrue(votesAfter == 0);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion
        #region Controller_Subscribe
        [TestMethod]
        public void Controller_Subscribe_Parameter_InvalidInvalid()
        {
            try
            {
                controller.Subscribe(-1, -1);
                Assert.Fail("An exception was raised");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }

        [TestMethod]
        public void Controller_Subscribe_Parameter_ValidInvalid()
        {
            try
            {
                RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
                controller.Subscribe(user.Id, -1);
                Assert.Fail("An exception was raised");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }

        [TestMethod]
        public void Controller_Subscribe_Parameter_InvalidValid()
        {
            try
            {
                RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
                controller.Subscribe(-1, TestExtensions._testChannelId1);
                Assert.Fail("An exception was raised");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }

        [TestMethod]
        public void Controller_Subscribe_Parameter_ValidValid()
        {
            try
            {
                RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
                controller.Subscribe(user.Id, TestExtensions._testChannelId1);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was raised. " + e);
            }
        }

        [TestMethod]
        public void Controller_SubscribeGetSubscriptions_State_OneChannel()
        {
            try
            {
                int channelId = TestExtensions._testChannelId1;
                RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
                List<Channel> channels = controller.GetSubscribedChannels(user.Id);
                Assert.IsTrue(channels[0].Id == channelId);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was raised. " + e);
            }
        }

        [TestMethod]
        public void Controller_SubscribeGetSubscriptions_State_MultipleChannels()
        {
            try
            {
                RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
                controller.Subscribe(user.Id, TestExtensions._testChannelId2);
                controller.Subscribe(user.Id, TestExtensions._testChannelId3);
                controller.Subscribe(user.Id, TestExtensions._testChannelId4);
                controller.Subscribe(user.Id, TestExtensions._testChannelId5);
                List<Channel> channels = controller.GetSubscribedChannels(user.Id);
                foreach (Channel c in channels)
                {
                    Assert.IsTrue(ChannelsContainId(channels, c.Id));
                }
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was raised. " + e);
            }
        }

        private Boolean ChannelsContainId(List<Channel> channels, int id)
        {
            foreach (Channel c in channels)
            {
                if (c.Id == id) return true;
            }
            return false;
        }
        #endregion

        /* R5 */
        #region Controller_CreateComment
        [TestMethod]
        public void Controller_CreateComment_Parameter_NullInvalidInvalid()
        {
            try
            {
                controller.CreateComment(null, -1, -1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_EmptyInvalidInvalid()
        {
            try
            {
                controller.CreateComment("", -1, -1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_WhitespaceInvalidInvalid()
        {
            try
            {
                controller.CreateComment("  ", -1, -1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_ValidInvalidInvalid()
        {
            try
            {
                controller.CreateComment("testcomment", -1, -1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_NullValidInvalid()
        {
            try
            {
                controller.CreateComment(null, TestExtensions._testUser1.Id, -1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_EmptyValidInvalid()
        {
            try
            {
                controller.CreateComment("", TestExtensions._testUser1.Id, -1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_ValidValidInvalid()
        {
            try
            {
                controller.CreateComment("testcomment", TestExtensions._testUser1.Id, -1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_NullInvalidValid()
        {
            try
            {
                controller.CreateComment(null, -1, TestExtensions._testChannelId1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_EmptyInvalidValid()
        {
            try
            {
                controller.CreateComment("", -1, TestExtensions._testChannelId1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_ValidInvalidValid()
        {
            try
            {
                controller.CreateComment("testcomment", -1, TestExtensions._testChannelId1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_NullValidValid()
        {
            try
            {
                controller.CreateComment(null, TestExtensions._testUser1.Id, TestExtensions._testChannelId1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentNullException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_EmptyValidValid()
        {
            try
            {
                controller.CreateComment("", TestExtensions._testUser1.Id, TestExtensions._testChannelId1);
                Assert.Fail("No exception was raised");
            }
            catch (ArgumentException)
            {
                //This is Expected
            }
        }
        [TestMethod]
        public void Controller_CreateComment_Parameter_ValidValidValid()
        {
            try
            {
                controller.CreateComment("", TestExtensions._testUser1.Id, TestExtensions._testChannelId1);
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was raised " + e);
            }
        }
        [TestMethod]
        public void Controller_CreateComment_State_CommentCreated()
        {
            try
            {
                controller.CreateComment("", TestExtensions._testUser1.Id, TestExtensions._testChannelId1);
                RentItServer.ITU.DatabaseWrapperObjects.Comment[] comments = controller.GetChannelComments(TestExtensions._testChannelId1, 0, int.MaxValue);

            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion

        /* R6 */
        #region Controller_Signup
        [TestMethod]
        public void Controller_SignUp_Parameter_NullNullNull()
        {
            controller = Controller.GetInstance();
            try
            {
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(null, null, null);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp("", null, null);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(null, "", null);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(null, null, "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp("", "", null);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(null, "", "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(null, "", "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp("", "", "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(testname, "", "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp("", testmail, "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp("", "", testpw);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(testname, testmail, "");
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp("", testmail, testpw);
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
                RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(testname, "", testpw);
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
            RentItServer.ITU.DatabaseWrapperObjects.User user = controller.SignUp(testNameSignup, testEmailSignup, testPasswordSignup);
            testId = user.Id;
            Assert.IsNotNull(user);
            Assert.AreEqual(testNameSignup, user.Username);
            Assert.AreEqual(testEmailSignup, user.Email);
        }
        #endregion
        #region Controller_DeleteUser
        [TestMethod]
        public void Controller_DeleteUser_Parameter_Neg()
        {
            controller = Controller.GetInstance();
            testId = TestExtensions._testUser1.Id;
            try
            {
                controller.DeleteUser(-1);
                Assert.Fail("No exception was thrown");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteUser_Parameter_MaxInt()
        {
            controller = Controller.GetInstance();
            testId = TestExtensions._testUser1.Id;
            try
            {
                controller.DeleteUser(int.MaxValue);
                Assert.Fail("No exception was thrown");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteUser_Parameter_MinInt()
        {
            controller = Controller.GetInstance();
            testId = TestExtensions._testUser1.Id;
            try
            {
                controller.DeleteUser(int.MinValue);
                Assert.Fail("No exception was thrown");
            }
            catch (ArgumentException)
            {
                //This is good
            }
        }
        [TestMethod]
        public void Controller_DeleteUser_Parameter_Valid()
        {
            controller = Controller.GetInstance();
            testId = TestExtensions._testUser1.Id;
            try
            {
                controller.DeleteUser(testId);
                User user = _dao.GetUser(testId);
                Assert.Fail("No exception was thrown. ");
            }
            catch
            {
                //This is expected
                testId = int.MaxValue;
            }
        }
        [TestMethod]
        public void Controller_DeleteUser_State_ProperCascading()
        {
            controller = Controller.GetInstance();
            testId = TestExtensions._testUser1.Id;
            try
            {
                controller.DeleteUser(testId);
                User user = _dao.GetUser(testId);
                Assert.Fail("No exception was thrown. ");
            }
            catch
            {
                List<Channel> userCreatedChannels = _dao.GetCreatedChannels(testId);
                Assert.IsTrue(userCreatedChannels.Count == 0);
                List<Comment> userCreatedComments = _dao.GetUserComments(testId, 0, int.MaxValue);
                Assert.IsTrue(userCreatedComments.Count == 0);
                List<Vote> userVotes = _dao.GetUserVotes(testId);
                Assert.IsTrue(userVotes.Count == 0);
                testId = int.MaxValue;
            }
        }
        #endregion
        #region Controller_UpdateUser
        [TestMethod]
        public void Controller_UpdateUser_Parameter_NegNullNullNull()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            try
            {
                controller.UpdateUser(-1, null, null, null);
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosNullNullNull()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            try
            {
                controller.UpdateUser(user.Id, null, null, null);
                User updatedUser = _dao.GetUser(user.Id);
                Assert.IsTrue(user.Username.Equals(updatedUser.Username));
                Assert.IsTrue(user.Email.Equals(updatedUser.Email));
            }
            catch (ArgumentNullException e)
            {
                Assert.Fail("An exception was thrown. " + e);
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosEmptyNullNull()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            testId = user.Id;
            try
            {
                controller.UpdateUser(testId, "", null, null);
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosNullEmptyNull()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            testId = user.Id;
            try
            {
                controller.UpdateUser(testId, null, "", null);
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentNullException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosNullNullEmpty()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            testId = user.Id;
            try
            {
                controller.UpdateUser(testId, null, null, "");
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentNullException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosEmptyEmptyNull()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            testId = user.Id;
            try
            {
                controller.UpdateUser(testId, "", "", null);
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosNullEmptyEmpty()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            testId = user.Id;
            try
            {
                controller.UpdateUser(testId, null, "", "");
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentNullException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosEmptyNullEmpty()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            testId = user.Id;
            try
            {
                controller.UpdateUser(testId, null, "", "");
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosEmptyEmptyEmpty()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            testId = user.Id;
            try
            {
                controller.UpdateUser(testId, "", "", "");
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentException)
            {
                //This is expected
            }
        }
        [TestMethod]
        public void Controller_UpdateUser_Parameter_PosNullNullValid()
        {
            controller = Controller.GetInstance();
            RentItServer.ITU.DatabaseWrapperObjects.User user = TestExtensions._testUser1;
            testId = user.Id;
            try
            {
                controller.UpdateUser(testId, null, null, testpw);
                Assert.Fail("An exception was thrown");
            }
            catch (ArgumentNullException)
            {
                //This is expected
            }
        }
        #endregion

        /* R7 */
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
            catch (ArgumentNullException)
            {
                //This is expected
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_Default()
        {
            controller = Controller.GetInstance();

            ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();

            try
            {
                controller.GetChannels(csa);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        public void CreateChannelsForTests()
        {
            Channel channel = null;
            for (int i = 0; i < interval; i++)
            {
                channel = _dao.CreateChannel(testchannelnames[i], TestExtensions._testUser1.Id, testchanneldescrs[i], new string[] { testGenre });
                _dao.UpdateChannel(channel.Id, null, null, null, i, i, null);
                testchannels.Add(channel);
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_State_ProperOwner()
        {
            try
            {
                testId = TestExtensions._testUser1.Id;
                CreateChannelsForTests();
                ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
                csa.SearchString = testchannelname;
                IEnumerable<RentItServer.ITU.DatabaseWrapperObjects.Channel> channels = controller.GetChannels(csa);

                List<RentItServer.ITU.DatabaseWrapperObjects.Channel> theChannels = new List<RentItServer.ITU.DatabaseWrapperObjects.Channel>(channels);
                foreach (RentItServer.ITU.DatabaseWrapperObjects.Channel aChannel in theChannels)
                {
                    Assert.IsTrue(aChannel.OwnerId == testId);
                }
            }
            catch (Exception e)
            {
                //Cleanup();
                Assert.Fail("An exception was raised. " + e);
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_State_PositiveInterval()
        {
            try
            {
                CreateChannelsForTests();
                ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
                csa.StartIndex = 0;
                csa.EndIndex = interval;
                RentItServer.ITU.DatabaseWrapperObjects.Channel[] channels = controller.GetChannels(csa);

                Assert.IsTrue(channels.Length >= interval);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_Parameter_NegativeInterval()
        {
            try
            {
                CreateChannelsForTests();
                ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
                csa.StartIndex = -interval;
                csa.EndIndex = interval;
                IEnumerable<RentItServer.ITU.DatabaseWrapperObjects.Channel> channels = controller.GetChannels(csa);

                List<RentItServer.ITU.DatabaseWrapperObjects.Channel> theChannels = new List<RentItServer.ITU.DatabaseWrapperObjects.Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_State_SortHitsDescending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
            csa.SortOption = csa.HitsDesc;
            try
            {
                CreateChannelsForTests();
                IEnumerable<RentItServer.ITU.DatabaseWrapperObjects.Channel> channels = controller.GetChannels(csa);
                List<RentItServer.ITU.DatabaseWrapperObjects.Channel> theChannels = new List<RentItServer.ITU.DatabaseWrapperObjects.Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be larger then current
                    Assert.IsTrue(theChannels[i - 1].Hits >= theChannels[i].Hits);
                }
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was raised. " + e);
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_State_SortHitsAscending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
            csa.SortOption = csa.HitsAsc;
            try
            {
                CreateChannelsForTests();
                IEnumerable<RentItServer.ITU.DatabaseWrapperObjects.Channel> channels = controller.GetChannels(csa);
                List<RentItServer.ITU.DatabaseWrapperObjects.Channel> theChannels = new List<RentItServer.ITU.DatabaseWrapperObjects.Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be less then current
                    Assert.IsTrue(theChannels[i - 1].Hits <= theChannels[i].Hits);
                }
            }
            catch (Exception e)
            {
                Assert.Fail("An exception was raised. " + e);
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_State_SortNumberOfCommentsDescending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
            csa.SortOption = csa.NumberOfCommentsDesc;
            try
            {
                CreateChannelsForTests();
                IEnumerable<RentItServer.ITU.DatabaseWrapperObjects.Channel> channels = controller.GetChannels(csa);
                List<RentItServer.ITU.DatabaseWrapperObjects.Channel> theChannels = new List<RentItServer.ITU.DatabaseWrapperObjects.Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be larger then current
                    Assert.IsTrue(_dao.GetChannelComments(theChannels[i - 1].Id, 0, int.MaxValue).Count >= _dao.GetChannelComments(theChannels[i].Id, 0, int.MaxValue).Count);
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_State_SortNumberOfCommentsAscending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
            csa.SortOption = csa.NumberOfCommentsAsc;
            try
            {
                CreateChannelsForTests();
                IEnumerable<RentItServer.ITU.DatabaseWrapperObjects.Channel> channels = controller.GetChannels(csa);
                List<RentItServer.ITU.DatabaseWrapperObjects.Channel> theChannels = new List<RentItServer.ITU.DatabaseWrapperObjects.Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be less then current
                    Assert.IsTrue(_dao.GetChannelComments(theChannels[i - 1].Id, 0, int.MaxValue).Count < _dao.GetChannelComments(theChannels[i].Id, 0, int.MaxValue).Count);
                }
            }
            catch (Exception)
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_State_SortSubscriptionsDescending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
            csa.SortOption = csa.SubscriptionsDesc;
            try
            {
                CreateChannelsForTests();
                IEnumerable<RentItServer.ITU.DatabaseWrapperObjects.Channel> channels = controller.GetChannels(csa);
                List<RentItServer.ITU.DatabaseWrapperObjects.Channel> theChannels = new List<RentItServer.ITU.DatabaseWrapperObjects.Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be larger then current
                    Assert.IsTrue(_dao.GetSubscriberCount(theChannels[i - 1].Id) > _dao.GetSubscriberCount(theChannels[i].Id));
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }

        [TestMethod]
        public void Controller_GetChannelsWithFilter_State_SortSubscriptionsAscending()
        {
            controller = Controller.GetInstance();
            ChannelSearchArgs csa = controller.GetDefaultChannelSearchArgs();
            csa.SortOption = csa.SubscriptionsAsc;
            try
            {
                CreateChannelsForTests();
                IEnumerable<RentItServer.ITU.DatabaseWrapperObjects.Channel> channels = controller.GetChannels(csa);
                List<RentItServer.ITU.DatabaseWrapperObjects.Channel> theChannels = new List<RentItServer.ITU.DatabaseWrapperObjects.Channel>(channels);
                Assert.IsTrue(theChannels.Count >= interval);
                for (int i = 1; i < theChannels.Count; i++)
                {   // The previous should be less then current
                    Assert.IsTrue(_dao.GetSubscriberCount(theChannels[i - 1].Id) < _dao.GetSubscriberCount(theChannels[i].Id));
                }
            }
            catch
            {
                Assert.Fail("An exception was raised");
            }
        }
        #endregion

        //TODO:
        #region Controller_GetCreatedChannels
        //public void Controller_GetCreatedChannels_Parameter_Invalid()
        //{
        //    controller.GetCreatedChannels()
        //}
        //public void Controller_GetCreatedChannels_Parameter_Valid()
        //{
        //    controller.GetCreatedChannels()
        //}
        //public void Controller_GetCreatedChannels_State_Invalid()
        //{
        //    controller.GetCreatedChannels()
        //}
        #endregion
        #region Controller_GetSubscribedChannels
        #endregion
        #region Controller_GetUser
        #endregion
        #region Controller_GetChannel
        #endregion

        #region Controller_GetTracksByChannelId
        #endregion
        #region Controller_IncrementChannelPlays
        #endregion

        #region Controller_GetTracks
        #endregion
        #region Controller_RemoveTrack
        #endregion
    }
}
