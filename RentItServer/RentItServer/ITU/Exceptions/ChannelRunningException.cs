using System;

/// <summary>
/// Exception thrown when an action requireing a channel's stream to not be running and the channel's stream is running
/// </summary>
class ChannelRunningException : Exception
{
    public ChannelRunningException(string message)
        : base(message)
    { }
}