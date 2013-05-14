using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Track
    {
        public ITU.DatabaseWrapperObjects.Track GetTrack()
        {
            return new ITU.DatabaseWrapperObjects.Track(Id, Path, Name, Artist, Length, UpVotes, DownVotes, ChannelId);
        }
    }
}