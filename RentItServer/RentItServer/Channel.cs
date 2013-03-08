using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentItServer
{
    [Serializable]
    public partial class Channel
    {
        public Channel() { }

        public string Name
        {
            get;
            set;
        }

        public int OwnerId
        {
            get;
            set;
        }

        public int ChannelId
        {
            get;
            set;
        }
    }
}
