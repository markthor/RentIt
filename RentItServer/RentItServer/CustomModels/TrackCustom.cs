using System;

namespace RentItServer
{
    public partial class Track
    {
        public Track(int id, int upvotes, int downvotes)
        {
            Id = id;
            UpVotes = upvotes;
            DownVotes = downvotes;
        }
    }
}
