using System.Diagnostics;

namespace RentItServer.ITU
{
    //In order to make visual studio handle the class as a coding class rather than a component
    [System.ComponentModel.DesignerCategory("Code")]
    public class EzProcess : Process
    {
        /// <summary>
        /// Constructor taking the channel id corresponding to the channel this is a stream for
        /// </summary>
        /// <param name="channelId">channel id corresponding to the channel this is a stream for</param>
        public EzProcess(int channelId) : base()
        {
            ChannelId = channelId;
        }

        /// <summary>
        /// The channelid which this is a process for
        /// </summary>
        public int ChannelId
        {
            get;
            private set;
        }

        /// <summary>
        /// The ezstream windows process id
        /// In order to kill a processes later
        /// </summary>
        public int RealProcessId
        {
            get;
            set;
        }
    }
}