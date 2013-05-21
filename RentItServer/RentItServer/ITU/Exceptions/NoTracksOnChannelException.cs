using System;

namespace RentItServer.ITU.Exceptions
{
    /// <summary>
    /// Exception thrown when an action requireing tracks is called upon a channel with no associated tracks
    /// </summary>
    class NoTracksOnChannelException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoTracksOnChannelException"/> class.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        public NoTracksOnChannelException(string message)
            : base(message)
        { }
    }
}