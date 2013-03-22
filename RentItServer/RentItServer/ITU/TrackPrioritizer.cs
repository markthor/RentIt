using System;
using System.Collections.Generic;

namespace RentItServer.ITU
{
    public class TrackPrioritizer
    {
        //Singleton instance of the class
        private static TrackPrioritizer _instance;
        private static Random rng = new Random();
        private const double _maxFrequency = 0.3;
        private const int _ratioConstant = 10;
        private const int _minimumRepeatDistance = 3;

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
        
        /// <summary>
        /// Gets the id of the next track to be played from predefined selection criteria.
        /// These includes the ratio between the upvotes and downvotes, the percentage of plays and whether the track has been played recently.
        /// </summary>
        /// <param name="trackList">The tracks on the channels playlist</param>
        /// <param name="plays">The record of tracks played on the channel</param>
        /// <returns>The id of the next track to be played</returns>
        public int GetNextTrackId(List<Track> trackList, List<TrackPlay> plays)
        {
            //Initializing data structure for track prioritizing.
            Dictionary<int, TrackData> trackData = new Dictionary<int, TrackData>();

            //Adding a key for each track and a TrackData object.
            foreach (Track t in trackList)
            {
                trackData.Add(t.id, new TrackData(t.upvotes.Value, t.downvotes.Value));
            }
            
            //Counting trackPlay occurences and adding it to TrackData.
            foreach (TrackPlay tp in plays)
            {
                TrackData currentTrackData = trackData[tp.trackId];
                currentTrackData.Plays++;
                trackData.Remove(tp.trackId);
                trackData.Add(tp.trackId, currentTrackData);
            }

            //The total amount of recorded plays.
            int totalPlays = plays.Count;

            //Updating candidate boolean for TrackData based on percentage of plays.
            foreach (KeyValuePair<int, TrackData> kvp in trackData)
            {
                double percentageOfPlays = Convert.ToDouble(kvp.Value.Plays) / Convert.ToDouble(totalPlays);
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
            double sumOfRatios = 0;
            foreach (KeyValuePair<int, TrackData> kvp in trackData)
            {
                if (kvp.Value.NextTrackCandidate)
                {
                    kvp.Value.Ratio = GetRatio(kvp.Value.Upvotes, kvp.Value.Downvotes);
                    sumOfRatios += kvp.Value.Ratio;
                }
            }

            //Generates a random number from 0 to 1 that chooses the next track.
            double nextTrackRandomRatioIndex = sumOfRatios * rng.NextDouble();
            //Finds the next track from the nextTrackRandomRatioIndex.
            double ratioAccumulator = 0;
            foreach (KeyValuePair<int, TrackData> kvp in trackData)
            {
                if (kvp.Value.NextTrackCandidate)
                {
                    if ((ratioAccumulator + kvp.Value.Ratio) > nextTrackRandomRatioIndex)
                    {
                        return kvp.Key;
                    }
                    else
                    {
                        ratioAccumulator += kvp.Value.Ratio;
                    }

                }
            }

            return 0;
        }
        
        private double GetRatio(int upvotes, int downvotes)
        {
            return Convert.ToDouble(_ratioConstant + upvotes) / Convert.ToDouble(_ratioConstant + downvotes);
        }

        private List<int> GetMostRecentlyPlayedTrackIds(int numberOfTracks, List<TrackPlay> plays)
        {
            List<TrackPlay> recentlyPlayedTracks = new List<TrackPlay>(numberOfTracks);

            foreach (TrackPlay tp in plays)
            {
                if (recentlyPlayedTracks.Count < numberOfTracks)
                {
                    recentlyPlayedTracks.Add(tp);
                }
                else if (ContainsOlderTrackPlay(tp, recentlyPlayedTracks))
                {
                    RemoveOldestTrackPlay(recentlyPlayedTracks);
                    recentlyPlayedTracks.Add(tp);
                }
            }

            List<int> ids = new List<int>(numberOfTracks);
            foreach (TrackPlay tp in recentlyPlayedTracks)
            {
                ids.Add(tp.trackId);
            }

            return ids;
        }

        private Boolean ContainsOlderTrackPlay(TrackPlay targetTrack, List<TrackPlay> plays)
        {
            foreach (TrackPlay tp in plays)
            {
                if (tp.playtime < targetTrack.playtime) { return true; }
            }
            return false;
        }

        private void RemoveOldestTrackPlay(List<TrackPlay> plays)
        {
            DateTime oldestDate = DateTime.Now;
            TrackPlay playToBeRemoved = null;
            foreach (TrackPlay tp in plays)
            {
                if (tp.playtime < oldestDate)
                {
                    oldestDate = tp.playtime;
                    playToBeRemoved = tp;
                }
            }
            plays.Remove(playToBeRemoved);
        }
    }

    public class TrackData
    {
        public TrackData(int Upvotes, int Downvotes)
        {
            NextTrackCandidate = true;
            Plays = 0;
            this.Upvotes = Upvotes;
            this.Downvotes = Downvotes;
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

        public Boolean NextTrackCandidate
        {
            get;
            set;
        }
    }
}