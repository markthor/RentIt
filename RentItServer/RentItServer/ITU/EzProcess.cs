using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace RentItServer.ITU
{
    public class EzProcess : Process
    {
        /*public EzProcess(int channelId, string ezStreamPath, string arguments)
        {
            StartInfo.FileName = ezStreamPath;
            StartInfo.Arguments = arguments;
            ChannelId = channelId;
        }*/

        public EzProcess(int channelId)
        {
            ChannelId = channelId;
        }

        public int ChannelId
        {
            get;
            private set;
        }

        public int CurrentTrackLength //LONG?!?
        {
            get;
            set;
        }
    }
}