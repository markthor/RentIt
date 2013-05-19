using System;

/// <summary>
/// Exception thrown when an action requireing tracks is called upon a channel with no associated tracks
/// </summary>
class NoTracksOnChannelException : Exception
{
    public NoTracksOnChannelException(string message)
        : base(message)
    { }
}