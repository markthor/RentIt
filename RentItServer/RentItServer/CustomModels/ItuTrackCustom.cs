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
            return new ITU.DatabaseWrapperObjects.Track(Id, Path, Name, Artist, Length, UpVotes, DownVotes, GetVotes(), GetTrackPlays());
        }

        private List<ITU.DatabaseWrapperObjects.Vote> GetVotes()
        {
            List<ITU.DatabaseWrapperObjects.Vote> votes = new List<ITU.DatabaseWrapperObjects.Vote>(Votes.Count);
            foreach(Vote v in Votes)
            {
                votes.Add(v.GetVote());
            }
            return votes;
        }

        private List<ITU.DatabaseWrapperObjects.TrackPlay> GetTrackPlays()
        {
            List<ITU.DatabaseWrapperObjects.TrackPlay> trackPlays = new List<ITU.DatabaseWrapperObjects.TrackPlay>(TrackPlays.Count);
            foreach (TrackPlay t in TrackPlays)
            {
                trackPlays.Add(t.GetTrackPlay());
            }
            return trackPlays;
        }
    }
}