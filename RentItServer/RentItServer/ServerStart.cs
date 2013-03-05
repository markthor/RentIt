using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace RentItServer
{
    public class ServerStart
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(RentItService)))
            {
                host.Open();

                Console.WriteLine("ReintIt service: Ready for connections...");
                Console.ReadLine();
            }
        }
    }
}
