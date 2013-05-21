using System;

namespace RentItServer.ITU.Exceptions
{
    /// <summary>
    /// Exception thrown when an action requireing a channel's stream to be running and the channel's stream is not running
    /// </summary>
    class ChannelNotRunningException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelNotRunningException"/> class.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        public ChannelNotRunningException(string message)
            : base(message)
        { }
    }
}