
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/16/2013 16:08:35
-- Generated from EDMX file: C:\Users\mark\Documents\Editor\GitHub\RentIt\RentItServer\RentItServer\RentItModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [RENTIT21];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ChannelComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_ChannelComment];
GO
IF OBJECT_ID(N'[dbo].[FK_CommentUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_CommentUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ChannelUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Channels] DROP CONSTRAINT [FK_ChannelUser];
GO
IF OBJECT_ID(N'[dbo].[FK_Subscription_Channel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscription] DROP CONSTRAINT [FK_Subscription_Channel];
GO
IF OBJECT_ID(N'[dbo].[FK_ChannelUser1_Channel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChannelUser1] DROP CONSTRAINT [FK_ChannelUser1_Channel];
GO
IF OBJECT_ID(N'[dbo].[FK_ChannelUser1_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChannelUser1] DROP CONSTRAINT [FK_ChannelUser1_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Subscription_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscription] DROP CONSTRAINT [FK_Subscription_User];
GO
IF OBJECT_ID(N'[dbo].[FK_ChannelGenre_Channel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChannelGenre] DROP CONSTRAINT [FK_ChannelGenre_Channel];
GO
IF OBJECT_ID(N'[dbo].[FK_ChannelGenre_Genre]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChannelGenre] DROP CONSTRAINT [FK_ChannelGenre_Genre];
GO
IF OBJECT_ID(N'[dbo].[FK_UserVote]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Votes] DROP CONSTRAINT [FK_UserVote];
GO
IF OBJECT_ID(N'[dbo].[FK_TrackVote]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Votes] DROP CONSTRAINT [FK_TrackVote];
GO
IF OBJECT_ID(N'[dbo].[FK_TrackTrackPlay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TrackPlays] DROP CONSTRAINT [FK_TrackTrackPlay];
GO
IF OBJECT_ID(N'[dbo].[FK_ChannelTrack]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tracks] DROP CONSTRAINT [FK_ChannelTrack];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[ChannelUser1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChannelUser1];
GO
IF OBJECT_ID(N'[dbo].[Channels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Channels];
GO
IF OBJECT_ID(N'[dbo].[Genres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genres];
GO
IF OBJECT_ID(N'[dbo].[Tracks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tracks];
GO
IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Votes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Votes];
GO
IF OBJECT_ID(N'[dbo].[TrackPlays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TrackPlays];
GO
IF OBJECT_ID(N'[dbo].[Subscription]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subscription];
GO
IF OBJECT_ID(N'[dbo].[ChannelGenre]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChannelGenre];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------


-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Channels'
CREATE TABLE [dbo].[Channels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Description] nvarchar(1000)  NOT NULL,
    [Rating] float  NULL,
    [Hits] int  NULL,
    [StreamUri] nvarchar(200)  NULL
);
GO

-- Creating table 'Genres'
CREATE TABLE [dbo].[Genres] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Tracks'
CREATE TABLE [dbo].[Tracks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ChannelId] int  NOT NULL,
    [Path] nvarchar(230)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Artist] nvarchar(100)  NOT NULL,
    [Length] int  NOT NULL,
    [UpVotes] int  NOT NULL,
    [DownVotes] int  NOT NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [ChannelId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Content] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [Password] nvarchar(25)  NOT NULL,
    [Email] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'Votes'
CREATE TABLE [dbo].[Votes] (
    [UserId] int  NOT NULL,
    [TrackId] int  NOT NULL,
    [Value] int  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'TrackPlays'
CREATE TABLE [dbo].[TrackPlays] (
    [TrackId] int  NOT NULL,
    [TimePlayed] datetime  NOT NULL
);
GO

-- Creating table 'Subscription'
CREATE TABLE [dbo].[Subscription] (
    [SubscribedChannels_Id] int  NOT NULL,
    [Subscribers_Id] int  NOT NULL
);
GO

-- Creating table 'ChannelGenre'
CREATE TABLE [dbo].[ChannelGenre] (
    [Channels_Id] int  NOT NULL,
    [Genres_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [Id] in table 'Channels'
ALTER TABLE [dbo].[Channels]
ADD CONSTRAINT [PK_Channels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Genres'
ALTER TABLE [dbo].[Genres]
ADD CONSTRAINT [PK_Genres]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tracks'
ALTER TABLE [dbo].[Tracks]
ADD CONSTRAINT [PK_Tracks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ChannelId], [UserId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([ChannelId], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId], [TrackId] in table 'Votes'
ALTER TABLE [dbo].[Votes]
ADD CONSTRAINT [PK_Votes]
    PRIMARY KEY CLUSTERED ([UserId], [TrackId] ASC);
GO

-- Creating primary key on [TrackId], [TimePlayed] in table 'TrackPlays'
ALTER TABLE [dbo].[TrackPlays]
ADD CONSTRAINT [PK_TrackPlays]
    PRIMARY KEY CLUSTERED ([TrackId], [TimePlayed] ASC);
GO

-- Creating primary key on [SubscribedChannels_Id], [Subscribers_Id] in table 'Subscription'
ALTER TABLE [dbo].[Subscription]
ADD CONSTRAINT [PK_Subscription]
    PRIMARY KEY NONCLUSTERED ([SubscribedChannels_Id], [Subscribers_Id] ASC);
GO

-- Creating primary key on [Channels_Id], [Genres_Id] in table 'ChannelGenre'
ALTER TABLE [dbo].[ChannelGenre]
ADD CONSTRAINT [PK_ChannelGenre]
    PRIMARY KEY NONCLUSTERED ([Channels_Id], [Genres_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------


-- Creating foreign key on [ChannelId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_ChannelComment]
    FOREIGN KEY ([ChannelId])
    REFERENCES [dbo].[Channels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_CommentUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CommentUser'
CREATE INDEX [IX_FK_CommentUser]
ON [dbo].[Comments]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Channels'
ALTER TABLE [dbo].[Channels]
ADD CONSTRAINT [FK_ChannelUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChannelUser'
CREATE INDEX [IX_FK_ChannelUser]
ON [dbo].[Channels]
    ([UserId]);
GO

-- Creating foreign key on [SubscribedChannels_Id] in table 'Subscription'
ALTER TABLE [dbo].[Subscription]
ADD CONSTRAINT [FK_Subscription_Channel]
    FOREIGN KEY ([SubscribedChannels_Id])
    REFERENCES [dbo].[Channels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Subscribers_Id] in table 'Subscription'
ALTER TABLE [dbo].[Subscription]
ADD CONSTRAINT [FK_Subscription_User]
    FOREIGN KEY ([Subscribers_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Subscription_User'
CREATE INDEX [IX_FK_Subscription_User]
ON [dbo].[Subscription]
    ([Subscribers_Id]);
GO

-- Creating foreign key on [Channels_Id] in table 'ChannelGenre'
ALTER TABLE [dbo].[ChannelGenre]
ADD CONSTRAINT [FK_ChannelGenre_Channel]
    FOREIGN KEY ([Channels_Id])
    REFERENCES [dbo].[Channels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Genres_Id] in table 'ChannelGenre'
ALTER TABLE [dbo].[ChannelGenre]
ADD CONSTRAINT [FK_ChannelGenre_Genre]
    FOREIGN KEY ([Genres_Id])
    REFERENCES [dbo].[Genres]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChannelGenre_Genre'
CREATE INDEX [IX_FK_ChannelGenre_Genre]
ON [dbo].[ChannelGenre]
    ([Genres_Id]);
GO

-- Creating foreign key on [UserId] in table 'Votes'
ALTER TABLE [dbo].[Votes]
ADD CONSTRAINT [FK_UserVote]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TrackId] in table 'Votes'
ALTER TABLE [dbo].[Votes]
ADD CONSTRAINT [FK_TrackVote]
    FOREIGN KEY ([TrackId])
    REFERENCES [dbo].[Tracks]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TrackVote'
CREATE INDEX [IX_FK_TrackVote]
ON [dbo].[Votes]
    ([TrackId]);
GO

-- Creating foreign key on [TrackId] in table 'TrackPlays'
ALTER TABLE [dbo].[TrackPlays]
ADD CONSTRAINT [FK_TrackTrackPlay]
    FOREIGN KEY ([TrackId])
    REFERENCES [dbo].[Tracks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ChannelId] in table 'Tracks'
ALTER TABLE [dbo].[Tracks]
ADD CONSTRAINT [FK_ChannelTrack]
    FOREIGN KEY ([ChannelId])
    REFERENCES [dbo].[Channels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ChannelTrack'
CREATE INDEX [IX_FK_ChannelTrack]
ON [dbo].[Tracks]
    ([ChannelId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------