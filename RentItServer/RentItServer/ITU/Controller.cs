using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using RentItServer.ITU.Exceptions;
using RentItServer.Utilities;
using System.Text;

namespace RentItServer.ITU
{
    /// <summary>
    /// Controller class. Through this class goes all communication between the webservice implementation and the serverside code
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// The log file name
        /// </summary>
        private static readonly string LogFileName = "ItuLogs.txt";
        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static Controller _instance;
        /// <summary>
        /// Data access object for database IO
        /// </summary>
        private readonly DatabaseDao _dao = DatabaseDao.GetInstance();
        /// <summary>
        /// Data access object for file system IO
        /// </summary>
        private readonly FileSystemDao _fileSystemHandler = FileSystemDao.GetInstance();
        /// <summary>
        /// The logger
        /// </summary>
        private readonly Logger _logger;
        /// <summary>
        /// The streamhandler
        /// </summary>
        private readonly StreamHandler _streamHandler;
        /// <summary>
        ///The url properties of the stream
        /// </summary>
        public static int _defaultPort = 27000;
        /// <summary>
        /// The default URI
        /// </summary>
        public static string _defaultUri = "http://rentit.itu.dk";
        /// <summary>
        /// The default URL
        /// </summary>
        public static string _defaultUrl = _defaultUri + ":" + _defaultPort + "/";

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private Controller()
        {
            // Initialize the logger
            _logger = new Logger(FilePath.ITULogPath.GetPath() + LogFileName);
            
            //Initialize the streamhandler
            _streamHandler = StreamHandler.GetInstance();
            InitializeStreamHandler();
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static Controller GetInstance()
        {
            return _instance ?? (_instance = new Controller());
        }

        /// <summary>
        /// Initializes the stream handler.
        /// </summary>
        private void InitializeStreamHandler()
        {
            _streamHandler.SetLogger(_logger);
            _streamHandler.InitTimer();
        }

        /// <summary>
        /// Login the specified user.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The id of the user, or -1 if the (username,password) is not found.</returns>
        public DatabaseWrapperObjects.User Login(string usernameOrEmail, string password)
        {
            if (usernameOrEmail == null) LogAndThrowException(new ArgumentNullException("usernameOrEmail"), "Login");
            if (usernameOrEmail.Equals("")) LogAndThrowException(new ArgumentException("usernameOrEmail was empty"), "Login");
            if (password == null) LogAndThrowException(new ArgumentNullException("password"), "Login");
            if (password.Equals("")) LogAndThrowException(new ArgumentException("password was empty"), "Login");

            User user = null;
            try
            {
                user = _dao.Login(usernameOrEmail, password);
                _logger.AddEntry("Login succeeded. Local variables: usernameOrEmail = " + usernameOrEmail + ", password = " + password);
                return user.GetUser();
            }
            catch (Exception e)
            {
                _logger.AddEntry("Login failed with exception [" + e + "]. Local variables: usernameOrEmail = " + usernameOrEmail + ", password = " + password + ", user = " + user);
                throw;
            }
        }


        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="email">The email associated with user.</param>
        /// <returns>The id of the created user.</returns>
        public DatabaseWrapperObjects.User SignUp(string username, string email, string password)
        {
            if (username == null) LogAndThrowException(new ArgumentNullException("username"), "CreateUser");
            if (username.Equals("")) LogAndThrowException(new ArgumentException("username was empty"), "CreateUser");
            if (password == null) LogAndThrowException(new ArgumentException("password"), "CreateUser");
            if (password.Equals("")) LogAndThrowException(new ArgumentException("password was empty"), "CreateUser");
            if (email == null) LogAndThrowException(new ArgumentNullException("email"), "CreateUser");
            if (email.Equals("")) LogAndThrowException(new ArgumentException("email was empty"), "CreateUser");
            String theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                       + "@"
                       + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            if (Regex.IsMatch(email, theEmailPattern) == false) LogAndThrowException(new ArgumentException("email is invalid"), "IsEmailEavailable");
            try
            {
                User user = null;

                user = _dao.SignUp(username, email, password);
                _logger.AddEntry("User created with username [" + username + "] and e-mail [" + email + "].");

                return user.GetUser();
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("User creation failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("User creation failed with exception [{0}]. Local variables: username = {1}, email = {2}, password = {3}", e, username, email, password));
                throw;
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public void DeleteUser(int userId)
        {
            if (userId < 1) LogAndThrowException(new ArgumentException("userId was < 1"), "DeleteUser");
            User user = null;
            List<Channel> userCreatedChannels = GetCreatedChannels(userId);
            foreach (Channel c in userCreatedChannels)
            {
                DeleteChannel(c.Id);
            }
            try
            {
                user = _dao.GetUser(userId);
                _dao.DeleteUser(userId);
                _dao.DeleteVotesForUser(userId);
                _dao.DeleteUserComments(userId);
                _logger.AddEntry(string.Format("User successfully deleted. Local variables: userId = {0}, theUser = {1}", userId, user));
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("User deletion failed with exception [" + e + "]."));
                _logger.AddEntry(
                    string.Format(
                        "User deletion failed with exception [{0}]. Local variables: userId = {1}, theUser = {2}.", e,
                        userId, user));
                throw;
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public DatabaseWrapperObjects.User GetUser(int userId)
        {
            try
            {
                return _dao.GetUser(userId).GetUser();
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("Get user failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("GetUser failed with exception [{0}]. Local variables: userId = {1}.", e, userId));
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is correct password] [the specified user id].
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c> if [is correct password] [the specified user id]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCorrectPassword(int userId, string password)
        {
            if (userId < 1) LogAndThrowException(new ArgumentException("User id was below 1"), "IsCorrectPassword");
            if (password == null) LogAndThrowException(new ArgumentNullException("password"), "IsCorrectPassword");
            if (password.Equals("")) LogAndThrowException(new ArgumentException("Password was empty"), "IsCorrectPassword");
            return _dao.IsCorrectPassword(userId, password);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username. Can be null</param>
        /// <param name="password">The password. Can be null</param>
        /// <param name="email">The email. Can be null</param>
        public void UpdateUser(int userId, string username, string password, string email)
        {
            if (username != null && username.Equals("")) LogAndThrowException(new ArgumentException("username is empty"), "UpdateUser");
            if (password != null && password.Equals("")) LogAndThrowException(new ArgumentException("password is empty"), "UpdateUser");
            if (email != null)
            {
                String theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                       + "@"
                       + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
                if (Regex.IsMatch(email, theEmailPattern) == false) LogAndThrowException(new ArgumentException("email is invalid"), "IsEmailEavailable");
            }
            User user = null;
            User updatedUser = null;
            try
            {
                user = _dao.GetUser(userId);
                _dao.UpdateUser(userId, username, password, email);
                updatedUser = _dao.GetUser(userId);
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("UpdateUser failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("UpdateUser failed with exception [{0}]. Local variables: userId = {1}, username = {2}, password = {3}, email = {4}, user = {5}, updatedUser = {6}", e, userId, username, password, email, user, updatedUser));
                throw;
            }
        }

        /// <summary>
        /// Creates a channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="userId">The id of the user creating the channel.</param>
        /// <param name="description">The description of the channel.</param>
        /// <param name="genres">The genres associated with the channel.</param>
        /// <returns>The id of the created channel. -1 if the channel creation failed.</returns>
        public int CreateChannel(string channelName, int userId, string description, int[] genreIds)
        {
            if (channelName == null) LogAndThrowException(new ArgumentNullException("channelName"), "CreateChannel");
            if (channelName.Equals("")) LogAndThrowException(new ArgumentException("channelName was empty"), "CreateChannel");
            if (userId < 0) LogAndThrowException(new ArgumentException("userId was below 0"), "CreateChannel");
            if (description == null) LogAndThrowException(new ArgumentException("description"), "CreateChannel");
            //if (genres == null) LogAndThrowException(new ArgumentNullException("genres"), "CreateChannel");

            Channel channel = null;
            string logEntry = "User id [" + userId + "] want to create the channel [" + channelName + "] with description [" + description + "] and genreIds [" + genreIds + "]. ";
            try
            {

                channel = _dao.CreateChannel(channelName, userId, description, genreIds);
                _dao.UpdateChannel(channel.Id, null, null, null, null, null, _defaultUrl + channel.Id, genreIds);
                _logger.AddEntry(logEntry + "Channel creation succeeded.");

            }
            catch (Exception)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs(logEntry + "Channel creation failed with exception [" + e + "]."));
                _logger.AddEntry("ChannelCreation failed with exception [{0}]. logEntry = " + logEntry + ". Local variable: channel = " + channel + ".");
                throw;
            }
            return channel.Id;
        }

        /// <summary>
        /// Creates a genre with the name.
        /// </summary>
        /// <param name="genreName">The name of the genre.</param>
        public void CreateGenre(string genreName)
        {
            if (genreName == null) LogAndThrowException(new ArgumentNullException("genreName"), "CreateGenre");
            string logEntry = "Genre with name: " + " [" + genreName + "] has been created.";

            try
            {

                _dao.CreateGenre(genreName);
                _logger.AddEntry(logEntry + "Genre creation succeeded.");

            }
            catch (Exception)
            {
                _logger.AddEntry("GenreCreation failed with exception [{0}]. logEntry = " + logEntry + ".");
                throw;
            }
        }

        /// <summary>
        /// Deletes the channel.
        /// </summary>
        /// <param name="userId">The user id making the request, this must correspond to the channel owners id.</param>
        /// <param name="channelId">The channel id.</param>
        public void DeleteChannel(int channelId)
        {
            if (channelId < 0) LogAndThrowException(new ArgumentException("channelId was below 0"), "DeleteChannel");
            if (_streamHandler.IsChannelStreamRunning(channelId) == true) throw new ChannelRunningException("Cannot delete channel while its stream is running");

            List<Track> associatedTracks = GetTracksByChannelId(channelId);
            foreach (Track t in associatedTracks)
            {
                RemoveTrack(t.Id);
            }
            _dao.DeleteAllComments(channelId);

            Channel channel = null;
            try
            {

                channel = _dao.GetChannel(channelId);
                _dao.DeleteChannel(channel.GetChannel());
                _logger.AddEntry("Channel: [" + channel.Name + "] with id: [" + channelId + "] has been deleted");

            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("Channel deletion failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("DeleteChannel failed with exception [{0}]. Local variables: channelId = {1}, channel = {2}", e, channelId, channel));
                throw;
            }
        }

        /// <summary>
        /// Updates the channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="ownerId">The owner id.</param>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="description">The description.</param>
        /// <param name="hits">The hits.</param>
        /// <param name="rating">The rating.</param>
        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits,
                                  double? rating, int[] genreIds)
        {
            try
            {
                _dao.UpdateChannel(channelId, ownerId, channelName, description, hits, rating, null, genreIds);
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("Update channel failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("Update channel failed with exception [{0}]. Local variables: channelId = {1}, ownerId = {2}, channelName = {3}, description = {4}, hits = {5}, rating = {6}.", e, channelId, ownerId, channelName, description, hits, rating));
                throw;
            }
        }

        /// <summary>
        /// Gets a channel.
        /// </summary>
        /// <param name="channelId">The channel id for the channel to get.</param>
        /// <returns>The channel matching the given id.</returns>
        public DatabaseWrapperObjects.Channel GetChannel(int channelId)
        {
            if (channelId < 0) LogAndThrowException(new ArgumentException("channelId was below 0"), "GetChannel");

            Channel channel = _dao.GetChannel(channelId);
            if (channel == null)
            {
                LogAndThrowException(new ArgumentException("No channel with channelId = " + channelId + " exist."), "GetChannel");
            }

            return channel.GetChannel();
        }

        /// <summary>
        /// Gets the channels.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public DatabaseWrapperObjects.Channel[] GetChannels(ChannelSearchArgs args)
        {
            if (args == null) LogAndThrowException(new ArgumentNullException("args"), "GetChannels");
            try
            {
                IEnumerable<Channel> channels = _dao.GetChannelsWithFilter(args);
                return Channel.GetChannels(channels).ToArray();
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("GetChannels failed with exception [" + e + "]."));
                string entry = string.Format("GetChannels failed with exception [{0}]. Local variables: args = {1}", e, args);
                if (args != null)
                    entry =
                        string.Format(
                            "{0}, args.AmountPlayed = {1}, args.Genres = {2}, args.NumberOfComments = {3}, args.NumberOfSubscriptions = {4}, args.SearchString = {6}, args.SortOption = {7}, args.StartIndex = {8}, args.EndIndex = {9}",
                            entry, args.MinAmountPlayed, args.Genres, args.MinNumberOfComments, args.MinNumberOfSubscriptions, args.SearchString.Equals("") ? "\"\"" : args.SearchString, args.SortOption.Equals("") ? "\"\"" : args.SortOption, args.StartIndex, args.EndIndex);
                _logger.AddEntry(entry);
                throw;
            }
        }

        /// <summary>
        /// Counts all channels with filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public int CountAllChannelsWithFilter(ChannelSearchArgs filter)
        {
            filter.StartIndex = 0;
            filter.EndIndex = Int32.MaxValue;
            return GetChannels(filter).Length;
        }

        /// <summary>
        /// Creates a vote.
        /// </summary>
        /// <param name="rating">The rating. -1 for downvote, 1 for upvote. Any other value than -1 or 1 will be have no effect</param>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        public void CreateVote(int rating, int userId, int trackId)
        {
            if (userId < 1) LogAndThrowException(new ArgumentException("userId was < 1"), "CreateVote");
            if (trackId < 1) LogAndThrowException(new ArgumentException("trackId was < 1"), "CreateVote");
            // ignore ratings different from -1 and 1
            if (rating != -1 && rating != 1) return;
            try
            {
                _dao.CreateVote(rating, userId, trackId);
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("CreateVote failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("CreateVote failed with exception [{0}]. Local variables: rating = {1}, userId = {2}, trackId = {3}.", e, rating, userId, trackId));
                throw;
            }
        }

        /// <summary>
        /// Adds a track(mp3 file) to the filesystem
        /// </summary>
        /// <param name="userId">User id of the owner of the track</param>
        /// <param name="channelId">Id of the channel which own the track</param>
        /// <param name="audioStream">MemoryStream of the mp3 file</param>
        /// <returns>The id of the track</returns>
        public int AddTrack(int userId, int channelId, MemoryStream audioStream)
        {
            //save file
            //get track info
            //save to db

            //Create initial track
            Track track = new Track()
            {
                ChannelId = channelId,
                Path = "",
                Name = "",
                Artist = "",
                Length = 0,
                UpVotes = 0,
                DownVotes = 0,
                Channel = null,
                TrackPlays = new Collection<TrackPlay>(),
                Votes = new Collection<Vote>()
            };

            try
            {
                //Create the track enty in the database. This is in order to get the id of the track
                track = _dao.CreateTrackEntry(channelId, track);
                //Write the file to the filesystem
                string fileName = track.Id + ".mp3";
                _fileSystemHandler.WriteFile(FilePath.ITUTrackPath, fileName, audioStream);

                //Set track properties and update the track in the database
                int tId = track.Id;
                track = GetTrackInfo(FilePath.ITUTrackPath + fileName);
                track.Id = tId;
                track.ChannelId = channelId;
                _dao.UpdateTrack(track);
            }
            catch (Exception e)
            {
                _fileSystemHandler.DeleteFile(FilePath.ITUTrackPath + track.Id.ToString() + ".mp3");
                _dao.DeleteTrackEntry(track.Id);
                _logger.AddEntry("exception: " + e); //LAV ORDENTLY EXCEPTION HANDLING
            }
            _logger.AddEntry("Added track with id: [" + track.Id + "], artist: [" + track.Artist + "] and title: [" + track.Name + "] for userid: [" + userId + "] to channel with id: [" + channelId + "]");
            return track.Id;
        }

        /// <summary>
        /// Uses Taglib to retreive information about the mp3 file from the filepath
        /// </summary>
        /// <param name="filePath">Absolute filepath to mp3 file</param>
        /// <returns>A track with all properties set to the values of the mp3 file. NOTE: id and channel id is set to -1</returns>
        private Track GetTrackInfo(string filePath)
        {
            //Create a taglib file containing all the information
            TagLib.File audioFile = TagLib.File.Create(filePath);

            //Create a track entity and fill its properties
            Track track = new Track();
            track.Id = -1;
            track.ChannelId = -1;//FIX
            track.Path = filePath;
            track.UpVotes = 0;
            track.DownVotes = 0;
            track.Name = audioFile.Tag.Title;
            track.TrackPlays = new List<TrackPlay>();
            track.Votes = new List<Vote>();
            track.Length = (int)audioFile.Properties.Duration.TotalMilliseconds;

            //An mp3 file may have several artists. This loop puts them into a singles string
            track.Artist = "";
            string[] artists = audioFile.Tag.AlbumArtists;
            if (artists.Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach (string artist in artists)
                {
                    sb.Append(artist);
                    sb.Append(", ");
                }
                track.Artist = sb.ToString().Remove(sb.Length - 2);
            }

            return track;
        }

        /// <summary>
        /// Removes the track.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        public void RemoveTrack(int trackId)
        {
            if (trackId < 0) LogAndThrowException(new ArgumentException("trackId was below 0"), "RemoveTrack");
            if (_streamHandler.IsChannelStreamRunning(_dao.GetTrack(trackId).ChannelId) == true) throw new ChannelRunningException("Cannot delete track while channel stream is running");

            try
            {
                Track track = _dao.GetTrack(trackId);
                string logEntry = "[" + track.Name + "] with id [" + trackId + "] is being deleted.";
                if (_fileSystemHandler.Exists(track.Path))
                    _fileSystemHandler.DeleteFile(track.Path);
                _dao.DeleteTrackPlaysByTrackId(trackId);
                _dao.DeleteTrackEntry(track.GetTrack());
                _dao.DeleteVotesForTrack(trackId);
                //_logger.AddEntry(logEntry + "Deletion successful.");
            }
            catch (Exception e)
            {

                _logger.AddEntry("Track deletion failed with exception [" + e + "].");
                throw;
            }
        }

        /// <summary>
        /// Comments on the specified channel.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void CreateComment(string comment, int userId, int channelId)
        {
            if (comment == null) LogAndThrowException(new ArgumentNullException("comment"), "CreateComment");
            if (comment.Equals("")) LogAndThrowException(new ArgumentException("comment was empty"), "CreateComment");
            if (string.IsNullOrWhiteSpace(comment) == true) LogAndThrowException(new ArgumentException("comment was only whitespace"), "CreateComment");
            _dao.CreateComment(comment, userId, channelId);
            _logger.AddEntry("User id [" + userId + "] commented on the channel [" + channelId + "] with the comment [" + comment + "].");
        }

        /// <summary>
        /// Gets comments associated with a channel within the specified range. Ranges outside the size of the comment collection or null is interpreted as extremes.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// /// <param name="fromInclusive">The start index to retrieve comments from inclusive.</param>
        /// /// <param name="toExclusive">The end index to retieve comments from exclusive.</param>
        /// <returns>
        /// Comments from a specific channel in the specified range.
        /// </returns>
        public DatabaseWrapperObjects.Comment[] GetChannelComments(int channelId, int? fromInclusive, int? toExclusive)
        {
            if (channelId < 1) LogAndThrowException(new ArgumentException("channelId was < 1"), "GetChannelComments");
            if (fromInclusive == null) fromInclusive = 0;
            if (toExclusive == null) toExclusive = int.MaxValue;
            try
            {
                List<Comment> comments = _dao.GetChannelComments(channelId, fromInclusive.Value, toExclusive.Value);
                return Comment.GetComments(comments).ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Counts the total amount of comments for a channel
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <returns>The total amount of comments</returns>
        public int GetCountChannelComments(int channelId)
        {
            return GetChannelComments(channelId, null, null).Length;
        }

        /// <summary>
        /// Gets the specified number of recently played tracks for a channel.
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <param name="numberOfTracks">The number of tracks to be retrieved</param>
        /// <returns>The most recently played tracks</returns>
        public List<DatabaseWrapperObjects.Track> GetRecentlyPlayedTracks(int channelId, int numberOfTracks)
        {
            if (numberOfTracks < 1) throw new ArgumentException("Number of tracks is not a positive number.");
            if (channelId < 0) LogAndThrowException(new ArgumentException("channelId was below 0"), "GetChannel");
            try
            {
                List<Track> tracks = _dao.GetRecentlyPlayedTracks(channelId, numberOfTracks);
                List<DatabaseWrapperObjects.Track> result = new List<DatabaseWrapperObjects.Track>();
                foreach (Track t in tracks)
                {
                    result.Add(t.GetTrack());
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.AddEntry(string.Format("GetRecentlyPlayedTracks [{0}]. Local variables: channelId = {1}, numberOfTracks = {2}", e, channelId, numberOfTracks));
                throw;
            }
        }

        /// <summary>
        /// Subscribes the specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void Subscribe(int userId, int channelId)
        {
            if (userId < 1) LogAndThrowException(new ArgumentException("userId was < 1"), "UnSubscribe");
            if (channelId < 1) LogAndThrowException(new ArgumentException("channelId was < 1"), "UnSubscribe");
            try
            {
                _dao.Subscribe(userId, channelId);

            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("Subscribe failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("Subscribe failed with exception [{0}]. Local variables: userId = {1}, channelId = {2}", e, userId, channelId));
                throw;
            }
        }

        /// <summary>
        /// Unsubscribes the specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void UnSubscribe(int userId, int channelId)
        {
            if (userId < 1) LogAndThrowException(new ArgumentException("userId was < 1"), "UnSubscribe");
            if (channelId < 1) LogAndThrowException(new ArgumentException("channelId was < 1"), "UnSubscribe");
            try
            {
                _dao.UnSubscribe(userId, channelId);
                _dao.DeleteVotesForUser(userId, channelId);

            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("UnSubscribe failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("Unsubscribe failed with exception [{0}]. Local variables: userId = {1}, channelId = {2}", e, userId, channelId));
                throw;
            }
        }

        /// <summary>
        /// Logs and throws the exception e.
        /// </summary>
        /// <param name="e">The exception to throw.</param>
        /// <param name="operationName">Name of the operation.</param>
        private void LogAndThrowException(Exception e, String operationName)
        {
            _logger.AddEntry("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "].");
            //if (_handler != null)
            //    _handler(this, new RentItEventArgs("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "]."));
            throw e;
        }

        /// <summary>
        /// Determines whether [is email available] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        ///   <c>true</c> if [is email available] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmailAvailable(string email)
        {
            if (email == null) LogAndThrowException(new ArgumentNullException("email"), "IsEmailEavailable");
            if (email.Equals("")) LogAndThrowException(new ArgumentException("email was empty"), "IsEmailEavailable");
            String theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                       + "@"
                       + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            if (Regex.IsMatch(email, theEmailPattern) == false) LogAndThrowException(new ArgumentException("email is invalid"), "IsEmailEavailable");
            return _dao.IsEmailAvailable(email);
        }

        /// <summary>
        /// Determines whether [is username available] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        ///   <c>true</c> if [is username available] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUsernameAvailable(string username)
        {
            if (username == null) LogAndThrowException(new ArgumentNullException("username"), "IsUsernameAvailable");
            if (username.Equals("")) LogAndThrowException(new ArgumentException("username was empty"), "IsUsernameAvailable");
            return _dao.IsUsernameAvailable(username);
        }

        /// <summary>
        /// Gets the default channel search args.
        /// </summary>
        /// <returns></returns>
        public ChannelSearchArgs GetDefaultChannelSearchArgs()
        {
            return new ChannelSearchArgs();
        }

        /// <summary>
        /// Gets the created channels.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<Channel> GetCreatedChannels(int userId)
        {
            return _dao.GetCreatedChannels(userId);
        }

        /// <summary>
        /// Gets the subscribed channels.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<Channel> GetSubscribedChannels(int userId)
        {
            return _dao.GetSubscribedChannels(userId);
        }

        /// <summary>
        /// Gets the tracks by channel id.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        public List<Track> GetTracksByChannelId(int channelId)
        {
            return _dao.GetTracksByChannelId(channelId);
        }

        /// <summary>
        /// Determines whether [is channel name available] [the specified channel id].
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="channelName">Name of the channel.</param>
        /// <returns>
        ///   <c>true</c> if [is channel name available] [the specified channel id]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsChannelNameAvailable(int channelId, string channelName)
        {
            _logger.AddEntry("IsChannelNameAvailable --- channelId = " + channelId + " - channelName = " + channelName);
            if (channelId < 1 && channelId != -1) LogAndThrowException(new ArgumentException("channelId was < 1 && channelId != -1"), "IsChannelNameAvailable");
            if (channelName == null) LogAndThrowException(new ArgumentNullException("channelName"), "IsChannelNameAvailable");
            if (channelName.Equals("")) LogAndThrowException(new ArgumentException("channelName was empty"), "IsChannelNameAvailable");
            return _dao.IsChannelNameAvailable(channelId, channelName);
        }

        /// <summary>
        /// Gets the subscriber count.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        public int GetSubscriberCount(int channelId)
        {
            if (channelId < 1) LogAndThrowException(new ArgumentException("channelId was < 1"), "GetSubscriberCount ");
            return _dao.GetSubscriberCount(channelId);
        }

        /// <summary>
        /// Increments the channel plays.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        public void IncrementChannelPlays(int channelId)
        {
            _dao.IncrementChannelPlays(channelId);
        }

        /// <summary>
        /// Gets the vote.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        /// <returns></returns>
        public DatabaseWrapperObjects.Vote GetVote(int userId, int trackId)
        {
            Vote vote = _dao.GetVote(userId, trackId);
            if (vote != null)
            {
                return _dao.GetVote(userId, trackId).GetVote();
            }
            return null;
        }

        /// <summary>
        /// Deletes the vote.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        public void DeleteVote(int userId, int trackId)
        {
            if (userId < 1) LogAndThrowException(new ArgumentException("userId was < 1"), "DeleteVote");
            if (trackId < 1) LogAndThrowException(new ArgumentException("trackId was < 1"), "DeleteVote");
            _dao.DeleteVote(userId, trackId);
        }

        /// <summary>
        /// Counts the total amount of upvotes given to a track
        /// </summary>
        /// <param name="trackId">The id of the track</param>
        /// <returns>The total amount of upvotes</returns>
        public int CountAllUpvotes(int trackId)
        {
            return _dao.CountTrackVotes(trackId, 1);
        }

        /// <summary>
        /// Counts the total amount of downvotes given to a track
        /// </summary>
        /// <param name="trackId">The id of the track</param>
        /// <returns>The total amount of dwonvotes</returns>
        public int CountAllDownvotes(int trackId)
        {
            return _dao.CountTrackVotes(trackId, -1);
        }

        /// <summary>
        /// Retreives all genres
        /// </summary>
        /// <returns>A list of all genres in the database</returns>
        public List<Genre> GetAllGenres()
        {
            return _dao.GetAllGenres();
        }

        /// <summary>
        /// Retreives all genres associated with the given channel
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <returns>List of asociated genres</returns>
        public List<Genre> GetGenresForChannel(int channelId)
        {
            return _dao.GetGenresForChannel(channelId);
        }

        /// <summary>
        /// Counts the amount of channels which pass the given ChannelSearchArgs filter
        /// </summary>
        /// <param name="filter">ChannelSearchArgs filter to apply to all channels</param>
        /// <returns>The amount of channels passing the filter</returns>
        public int CountChannelsPassingFilter(ChannelSearchArgs filter)
        {
            return _dao.GetChannelsWithFilter(filter).Count;
        }
    }
}