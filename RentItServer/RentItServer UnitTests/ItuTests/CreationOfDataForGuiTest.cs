using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RentItServer.ITU;
using RentItServer;

namespace RentItServer_UnitTests.ItuTests
{
    [TestClass]
    public class CreationOfDataForGuiTest
    {
        [TestMethod]
        public void CreateUsersAndChannels()
        {
            string genreName1 = "Electro";
            string genreName2 = "Heavy Metal";
            User u = Controller.GetInstance().SignUp("Prechtig", "andreas.p.poulsen@gmail.com", "test");
            Controller.GetInstance().CreateGenre(genreName1);
            Controller.GetInstance().CreateGenre(genreName2);
            int channelId1 = Controller.GetInstance().CreateChannel("Nightly Psychoactive Electro Hits", u.Id, "Sick channel with groovy beats", new List<string>() { genreName1 });
            int channelId2 = Controller.GetInstance().CreateChannel("Hard Hitting Iron Bass", u.Id, "Metal with a density over 9000.", new List<string>() { genreName2 });
            int channelId3 = Controller.GetInstance().CreateChannel("Nine Inch Nails", u.Id, "Metal with a density over 9000.", new List<string>() { genreName2 });
            int channelId4 = Controller.GetInstance().CreateChannel("Wrecking Balls", u.Id, "Metal with a density over 9000.", new List<string>() { genreName2 });
            int channelId5 = Controller.GetInstance().CreateChannel("Sick Drops", u.Id, "Metal with a density over 9000.", new List<string>() { genreName2 });
        }
    }
}
