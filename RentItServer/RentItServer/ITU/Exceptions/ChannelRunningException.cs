using System;

namespace RentItServer.ITU.Exceptions
{
    /// <summary>
    /// Exception thrown when an action requireing a channel's stream to not be running and the channel's stream is running
    /// </summary>
    class ChannelRunningException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelRunningException"/> class.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        public ChannelRunningException(string message)
            : base(message)
        { }
    }
}