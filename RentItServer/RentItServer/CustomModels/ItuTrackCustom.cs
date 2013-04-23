using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Track
    {
        public ITU.DataObjects.Track GetTrack()
        {
            return new ITU.DataObjects.Track(Id, Path, Name, Artist, Length, UpVotes, DownVotes, GetVotes(), GetTrackPlays());
        }

        private List<ITU.DataObjects.Vote> GetVotes()
        {
            List<ITU.DataObjects.Vote> votes = new List<ITU.DataObjects.Vote>(Votes.Count);
            foreach(Vote v in Votes)
            {
                votes.Add(v.GetVote());
            }
            return votes;
        }

        private List<ITU.DataObjects.TrackPlay> GetTrackPlays()
        {
            List<ITU.DataObjects.TrackPlay> trackPlays = new List<ITU.DataObjects.TrackPlay>(TrackPlays.Count);
            foreach (TrackPlay t in TrackPlays)
            {
                trackPlays.Add(t.GetTrackPlay());
            }
            return trackPlays;
        }
    }
}