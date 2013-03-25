using System;

namespace RentItServer
{
    public partial class Track
    {
        public Track(int id, int upvotes, int downvotes)
        {
            this.id = id;
            this.upvotes = new Nullable<int>(upvotes);
            this.downvotes = new Nullable<int>(downvotes);
        }
    }
}
