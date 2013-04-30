using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GuiTest.ServiceReference1;
namespace GuiTest
{
    [TestClass]
    public class ChannelTest
    {
        [TestMethod]
        public void AddChannelsTest()
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                RentItMvc.RentItService.User user = new RentItMvc.RentItService.User();
                user = proxy.Login("andreas.p.poulsen@gmail.com", "test");
                RentItMvc.RentItService.User user2 = new RentItMvc.RentItService.User();
                //user2 = proxy.SignUp("testest2", "hello2@hello.dk", "333333");
                string[] genres = { "rock", "pop", "jazz" };
                genres = null;
                int channelId = proxy.CreateChannel("Super test service channel1", user.Id, "Nice Channel, Hear It", genres);
                int channelId2 = proxy.CreateChannel("Super test service channel2", user.Id, "Nice Channel, Hear It", genres);
                int channelId3 = proxy.CreateChannel("Super test service channel3", user.Id, "Nice Channel, Hear It", genres);            
            }
        }
    }
}
