using System;
using System.Collections.Generic;
using RentItServer;

namespace RentItServer.ITU
{
    /// <summary>
    /// Singleton class which has the single functionality of choosing a track from a collection of tracks and trackplays. The class also contains the constants that are involved in the selection of a track.
    /// </summary>
    public class TrackPrioritizer
    {
        //Singleton instance of the class
        private static TrackPrioritizer _instance;
        private static Random rng = new Random();
        //The maximal play percentage that a track can have to be considered for the next track.
        private double _maxFrequency = 0;
        //Determines how much upvotes and downvotes should influence the propability of a track being selected.
        private int _ratioConstant = 10;
        //The number of latest played tracks that will not be considered for the next track.
        private int _minimumRepeatDistance = 3;

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
        /// Gets a list of tracks determined from the GetNextTrack method. The minimum length of the playlist is specified and the playlist and the resulting trackplays are returned.
        /// </summary>
        /// <param name="trackList">The tracks to compose the list of</param>
        /// <param name="plays">The plays of the tracks</param>
        /// <param name="minMillisDuration">The minimum duration of the playlist in milliseconds</param>
        /// <param name="playsForPlaylist">The resulting trackPlays from playing the playlist</param>
        /// <returns>The playlist</returns>
        public List<Track> GetNextPlayList(List<Track> trackList, List<TrackPlay> plays, int minMillisDuration, out List<TrackPlay> playsForPlaylist)
        {
            int timeOfPlaylist = 0;
            List<Track> playlist = new List<Track>();
            playsForPlaylist = new List<TrackPlay>();

            for (int i = 0; i < minMillisDuration; )
            {
                Track nextTrack = GetNextTrack(trackList, plays);
                if (nextTrack.Length <= 0) throw new ArgumentException("Track has length equal to or below zero.");
                TrackPlay play = new TrackPlay(nextTrack.Id, DateTime.Now.AddMilliseconds(timeOfPlaylist));
                timeOfPlaylist = timeOfPlaylist + nextTrack.Length;
                playlist.Add(nextTrack);
                plays.Add(play);
                playsForPlaylist.Add(play);
                i = i + nextTrack.Length;
            }
            return playlist;
        }

        /// <summary>
        /// Gets the id of the next track to be played from predefined selection criteria.
        /// These includes the ratio between the upvotes and downvotes, the percentage of plays and whether the track has been played recently.
        /// </summary>
        /// <param name="trackList">The tracks on the channels playlist</param>
        /// <param name="plays">The record of tracks played on the channel</param>
        /// <returns>The id of the next track to be played</returns>
        public Track GetNextTrack(List<Track> trackList, List<TrackPlay> plays)
        {
            //Set max frequency
            _maxFrequency = (1.0 / Convert.ToDouble(trackList.Count)) * 2.0;

            if (trackList.Count == 0) throw new ArgumentException("No tracks in list");

            //Initializing data structure for track prioritizing.
            Dictionary<int, TrackData> trackData = new Dictionary<int, TrackData>();

            //Adding a key for each track and a TrackData object.
            foreach (Track t in trackList)
            {
                trackData.Add(t.Id, new TrackData(t));
            }
            
            //Counting trackPlay occurences and adding it to TrackData.
            foreach (TrackPlay tp in plays)
            {
                TrackData currentTrackData = trackData[tp.TrackId];
                currentTrackData.Plays++;
            }

            //The total amount of recorded plays.
            int totalPlays = plays.Count;

            //Updating candidate boolean for TrackData based on percentage of plays and counting the number of disqualifications.
            int disqualifications = 0;
            foreach (KeyValuePair<int, TrackData> kvp in trackData)
            {
                double percentageOfPlays = Convert.ToDouble(kvp.Value.Plays) / Convert.ToDouble(totalPlays);
                if (percentageOfPlays > _maxFrequency)
                {
                    kvp.Value.NextTrackCandidate = false;
                    disqualifications++;
                }
            }

            //Set minimum repeat distance in special cases.
            int effectiveMinimumRepeatDistance = _minimumRepeatDistance;
            if (trackList.Count - disqualifications <= _minimumRepeatDistance) effectiveMinimumRepeatDistance = trackList.Count - disqualifications - 1;
            if (effectiveMinimumRepeatDistance < 0) effectiveMinimumRepeatDistance = 0;

            //Setting candidate boolean to false for recently played tracks.
            List<int> MostRecentlyPlayedTrackIds = GetMostRecentlyPlayedTrackIds(effectiveMinimumRepeatDistance, plays);
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
                    kvp.Value.Ratio = GetRatio(kvp.Value.Track.UpVotes, kvp.Value.Track.DownVotes);
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
                        return kvp.Value.Track;
                    }
                    else
                    {
                        ratioAccumulator += kvp.Value.Ratio;
                    }

                }
            }
            throw new ArgumentException("This implementaion does not support the arguments because of an error in this method.");
        }
        
        private double GetRatio(int upvotes, int downvotes)
        {
            return Convert.ToDouble(_ratioConstant + upvotes) / Convert.ToDouble(_ratioConstant + downvotes);
        }

        public List<int> GetMostRecentlyPlayedTrackIds(int numberOfTracks, List<TrackPlay> plays)
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
                ids.Add(tp.TrackId);
            }

            return ids;
        }

        private Boolean ContainsOlderTrackPlay(TrackPlay targetTrack, List<TrackPlay> plays)
        {
            foreach (TrackPlay tp in plays)
            {
                if (tp.TimePlayed < targetTrack.TimePlayed) { return true; }
            }
            return false;
        }

        private void RemoveOldestTrackPlay(List<TrackPlay> plays)
        {
            DateTime oldestDate = DateTime.MaxValue;
            TrackPlay playToBeRemoved = null;
            foreach (TrackPlay tp in plays)
            {
                if (tp.TimePlayed <= oldestDate)
                {
                    oldestDate = tp.TimePlayed;
                    playToBeRemoved = tp;
                }
            }

            plays.Remove(playToBeRemoved);
        }
    }

    public class TrackData
    {
        public TrackData(Track t)
        {
            NextTrackCandidate = true;
            Plays = 0;
            Track = t;
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

        public Track Track
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