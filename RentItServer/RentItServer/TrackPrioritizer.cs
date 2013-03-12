using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public class TrackPrioritizer
    {
        //Singleton instance of the class
        public static TrackPrioritizer _instance;
        public static const double _maxFrequency = 0.3;
        public static const int _ratioConstant = 10;
        public static const int _minimumRepeatDistance = 3;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private TrackPrioritizer()
        {
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static TrackPrioritizer GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TrackPrioritizer();
            }
            return _instance;
        }

        public int GetNextTrackId(List<Track> trackList, List<TrackPlay> plays)
        {
            //Initializing data structure for track prioritizing.
            Dictionary<int, TrackData> trackData = new Dictionary<int, TrackData>();

            //Adding a key for each track and a TrackData object.
            foreach (Track t in trackList)
            {
                trackData.Add(t.Id, new TrackData());
            }
            
            //Counting trackPlay occurences and adding it to TrackData.
            foreach (TrackPlay tp in plays)
            {
                TrackData currentTrackData = trackData[tp.TrackId];
                currentTrackData.Plays++;
                trackData.Add(tp.TrackId, currentTrackData);
            }

            //The total amount of recorded plays.
            int totalPlays = plays.Count;

            //Updating candidate boolean for TrackData based on percentage of plays.
            foreach (KeyValuePair<int, TrackData> kvp in trackData)
            {
                double percentageOfPlays = kvp.Value.Plays/totalPlays;
                if (percentageOfPlays > _maxFrequency)
                {
                    kvp.Value.NextTrackCandidate = false;
                }
            }

            //Setting candidate boolean to false for recently played tracks.
            List<int> MostRecentlyPlayedTrackIds = GetMostRecentlyPlayedTrackIds(_minimumRepeatDistance, plays);
            foreach(int i in MostRecentlyPlayedTrackIds)
            {
                trackData[i].NextTrackCandidate = false;
            }

            //Calculating TrackData ratio for tracks that are candidate to the next track.
            //Counts the total amount of ratio.
            foreach (KeyValuePair<int, TrackData> kvp in trackData)
            {
                if (kvp.Value.NextTrackCandidate)
                {
                    kvp.Value.Ratio = 
                }
            }
        }

        private double GetRatio(int upvotes, int downvotes)
        {
            return (_ratioConstant+upvotes)/(_ratioConstant+downvotes)
        }

        private List<int> GetMostRecentlyPlayedTrackIds(int numberOfTracks, List<TrackPlay> plays)
        {
            List<TrackPlay> recentlyPlayedTracks = new List<TrackPlay>(numberOfTracks);
            foreach (TrackPlay tp in plays)
            {
                if (ContainsOlderTrackPlay(tp, recentlyPlayedTracks))
                {
                    RemoveOldestTrackPlay(recentlyPlayedTracks);
                    recentlyPlayedTracks.Add(tp);
                }
            }

            List<int> ids = new List<int>(numberOfTracks);
            foreach (TrackPlay tp in recentlyPlayedTracks)
            {
                ids.Add(tp.TrackId);
            }

            return ids;
        }

        private Boolean ContainsOlderTrackPlay(TrackPlay targetTrack, List<TrackPlay> plays)
        {
            foreach (TrackPlay tp in plays)
            {
                if (tp.PlayTime < targetTrack.PlayTime) { return true; }
            }
            return false;
        }

        private void RemoveOldestTrackPlay(List<TrackPlay> plays)
        {
            DateTime oldestDate = DateTime.Now;
            TrackPlay playToBeRemoved = null;
            foreach (TrackPlay tp in plays)
            {
                if (tp.PlayTime < oldestDate)
                {
                    oldestDate = tp.PlayTime;
                    playToBeRemoved = tp;
                }
            }
            plays.Remove(playToBeRemoved);
        }
    }

    public class TrackData
    {
        public TrackData()
        {
            NextTrackCandidate = true;
            Plays = 0;
        }

        public double Ratio
        {
            get;
            set;
        }

        public int Plays
        {
            get;
            set;
        }

        public Boolean NextTrackCandidate
        {
            get;
            set;
        }
    }

    public class TrackPlay
    {
        public int TrackId
        {
            get;
            set;
        }

        public DateTime PlayTime
        {
            get;
            set;
        }

        public TrackPlay(DateTime date, int id)
        {

        }
    }

    public class Track
    {
        public int Upvotes
        {
            get;
            set;
        }

        public int Downvotes
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public Track(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}