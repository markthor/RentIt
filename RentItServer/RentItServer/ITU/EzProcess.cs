using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace RentItServer.ITU
{
    public class EzProcess : Process
    {
        public EzProcess(int channelId) : base()
        {
            ChannelId = channelId;
        }

        public int ChannelId
        {
            get;
            private set;
        }

        public int RealId
        {
            get;
            set;
        }
    }
}