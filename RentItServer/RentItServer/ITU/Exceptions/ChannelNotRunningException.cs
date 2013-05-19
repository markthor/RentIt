using System;

/// <summary>
/// Exception thrown when an action requireing a channel's stream to be running and the channel's stream is not running
/// </summary>
class ChannelNotRunningException : Exception
{
    public ChannelNotRunningException(string message)
        : base(message)
    { }
}