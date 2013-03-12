
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/12/2013 15:23:48
-- Generated from EDMX file: D:\Dropbox\PRIVATE\Team programming\2års projekt\RentIt\RentItServer\RentItServer\RentItModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK__channels__userId__182C9B23]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[channels] DROP CONSTRAINT [FK__channels__userId__182C9B23];
GO
IF OBJECT_ID(N'[dbo].[FK__comments__channe__1CF15040]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[comments] DROP CONSTRAINT [FK__comments__channe__1CF15040];
GO
IF OBJECT_ID(N'[dbo].[FK__tracks__channelI__31EC6D26]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tracks] DROP CONSTRAINT [FK__tracks__channelI__31EC6D26];
GO
IF OBJECT_ID(N'[dbo].[FK__comments__userId__1DE57479]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[comments] DROP CONSTRAINT [FK__comments__userId__1DE57479];
GO
IF OBJECT_ID(N'[dbo].[FK__trackplay__track__36B12243]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[trackplays] DROP CONSTRAINT [FK__trackplay__track__36B12243];
GO
IF OBJECT_ID(N'[dbo].[FK__votes__trackId__3B75D760]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[votes] DROP CONSTRAINT [FK__votes__trackId__3B75D760];
GO
IF OBJECT_ID(N'[dbo].[FK__votes__userId__3C69FB99]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[votes] DROP CONSTRAINT [FK__votes__userId__3C69FB99];
GO
IF OBJECT_ID(N'[dbo].[FK_channelgenres_channels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[channelgenres] DROP CONSTRAINT [FK_channelgenres_channels];
GO
IF OBJECT_ID(N'[dbo].[FK_channelgenres_genres]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[channelgenres] DROP CONSTRAINT [FK_channelgenres_genres];
GO
IF OBJECT_ID(N'[dbo].[FK_subscriptions_channels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[subscriptions] DROP CONSTRAINT [FK_subscriptions_channels];
GO
IF OBJECT_ID(N'[dbo].[FK_subscriptions_users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[subscriptions] DROP CONSTRAINT [FK_subscriptions_users];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[channels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[channels];
GO
IF OBJECT_ID(N'[dbo].[comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[comments];
GO
IF OBJECT_ID(N'[dbo].[genres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[genres];
GO
IF OBJECT_ID(N'[dbo].[trackplays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[trackplays];
GO
IF OBJECT_ID(N'[dbo].[tracks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tracks];
GO
IF OBJECT_ID(N'[dbo].[users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[users];
GO
IF OBJECT_ID(N'[dbo].[votes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[votes];
GO
IF OBJECT_ID(N'[dbo].[channelgenres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[channelgenres];
GO
IF OBJECT_ID(N'[dbo].[subscriptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[subscriptions];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'channels'
CREATE TABLE [dbo].[channels] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(200)  NOT NULL,
    [rating] float  NULL,
    [plays] int  NULL,
    [description] varchar(1000)  NULL,
    [userId] int  NOT NULL
);
GO

-- Creating table 'comments'
CREATE TABLE [dbo].[comments] (
    [channelId] int  NOT NULL,
    [userId] int  NOT NULL,
    [content] varchar(500)  NOT NULL,
    [date] datetime  NOT NULL
);
GO

-- Creating table 'genres'
CREATE TABLE [dbo].[genres] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(200)  NOT NULL
);
GO

-- Creating table 'trackplays'
CREATE TABLE [dbo].[trackplays] (
    [trackId] int  NOT NULL,
    [date] datetime  NOT NULL,
    [playtime] datetime  NOT NULL
);
GO

-- Creating table 'tracks'
CREATE TABLE [dbo].[tracks] (
    [id] int IDENTITY(1,1) NOT NULL,
    [channelId] int  NOT NULL,
    [trackpath] varchar(230)  NOT NULL,
    [name] varchar(100)  NOT NULL,
    [artist] varchar(100)  NULL,
    [length] varchar(10)  NULL,
    [upvotes] int  NULL,
    [downvotes] int  NULL
);
GO

-- Creating table 'users'
CREATE TABLE [dbo].[users] (
    [id] int IDENTITY(1,1) NOT NULL,
    [username] varchar(50)  NOT NULL,
    [password] varchar(25)  NOT NULL,
    [channelsSubscriped_id] int  NOT NULL
);
GO

-- Creating table 'votes'
CREATE TABLE [dbo].[votes] (
    [trackId] int  NOT NULL,
    [userId] int  NOT NULL,
    [value] int  NOT NULL,
    [date] datetime  NULL
);
GO

-- Creating table 'channelgenres'
CREATE TABLE [dbo].[channelgenres] (
    [channels_id] int  NOT NULL,
    [genres_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'channels'
ALTER TABLE [dbo].[channels]
ADD CONSTRAINT [PK_channels]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [channelId], [userId] in table 'comments'
ALTER TABLE [dbo].[comments]
ADD CONSTRAINT [PK_comments]
    PRIMARY KEY CLUSTERED ([channelId], [userId] ASC);
GO

-- Creating primary key on [id] in table 'genres'
ALTER TABLE [dbo].[genres]
ADD CONSTRAINT [PK_genres]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [trackId], [date] in table 'trackplays'
ALTER TABLE [dbo].[trackplays]
ADD CONSTRAINT [PK_trackplays]
    PRIMARY KEY CLUSTERED ([trackId], [date] ASC);
GO

-- Creating primary key on [id] in table 'tracks'
ALTER TABLE [dbo].[tracks]
ADD CONSTRAINT [PK_tracks]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'users'
ALTER TABLE [dbo].[users]
ADD CONSTRAINT [PK_users]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [trackId], [userId] in table 'votes'
ALTER TABLE [dbo].[votes]
ADD CONSTRAINT [PK_votes]
    PRIMARY KEY CLUSTERED ([trackId], [userId] ASC);
GO

-- Creating primary key on [channels_id], [genres_id] in table 'channelgenres'
ALTER TABLE [dbo].[channelgenres]
ADD CONSTRAINT [PK_channelgenres]
    PRIMARY KEY NONCLUSTERED ([channels_id], [genres_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [userId] in table 'channels'
ALTER TABLE [dbo].[channels]
ADD CONSTRAINT [FK__channels__userId__182C9B23]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[users]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__channels__userId__182C9B23'
CREATE INDEX [IX_FK__channels__userId__182C9B23]
ON [dbo].[channels]
    ([userId]);
GO

-- Creating foreign key on [channelId] in table 'comments'
ALTER TABLE [dbo].[comments]
ADD CONSTRAINT [FK__comments__channe__1CF15040]
    FOREIGN KEY ([channelId])
    REFERENCES [dbo].[channels]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [channelId] in table 'tracks'
ALTER TABLE [dbo].[tracks]
ADD CONSTRAINT [FK__tracks__channelI__31EC6D26]
    FOREIGN KEY ([channelId])
    REFERENCES [dbo].[channels]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__tracks__channelI__31EC6D26'
CREATE INDEX [IX_FK__tracks__channelI__31EC6D26]
ON [dbo].[tracks]
    ([channelId]);
GO

-- Creating foreign key on [userId] in table 'comments'
ALTER TABLE [dbo].[comments]
ADD CONSTRAINT [FK__comments__userId__1DE57479]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[users]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__comments__userId__1DE57479'
CREATE INDEX [IX_FK__comments__userId__1DE57479]
ON [dbo].[comments]
    ([userId]);
GO

-- Creating foreign key on [trackId] in table 'trackplays'
ALTER TABLE [dbo].[trackplays]
ADD CONSTRAINT [FK__trackplay__track__36B12243]
    FOREIGN KEY ([trackId])
    REFERENCES [dbo].[tracks]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [trackId] in table 'votes'
ALTER TABLE [dbo].[votes]
ADD CONSTRAINT [FK__votes__trackId__3B75D760]
    FOREIGN KEY ([trackId])
    REFERENCES [dbo].[tracks]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [userId] in table 'votes'
ALTER TABLE [dbo].[votes]
ADD CONSTRAINT [FK__votes__userId__3C69FB99]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[users]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__votes__userId__3C69FB99'
CREATE INDEX [IX_FK__votes__userId__3C69FB99]
ON [dbo].[votes]
    ([userId]);
GO

-- Creating foreign key on [channels_id] in table 'channelgenres'
ALTER TABLE [dbo].[channelgenres]
ADD CONSTRAINT [FK_channelgenres_channels]
    FOREIGN KEY ([channels_id])
    REFERENCES [dbo].[channels]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [genres_id] in table 'channelgenres'
ALTER TABLE [dbo].[channelgenres]
ADD CONSTRAINT [FK_channelgenres_genres]
    FOREIGN KEY ([genres_id])
    REFERENCES [dbo].[genres]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_channelgenres_genres'
CREATE INDEX [IX_FK_channelgenres_genres]
ON [dbo].[channelgenres]
    ([genres_id]);
GO

-- Creating foreign key on [channelsSubscriped_id] in table 'users'
ALTER TABLE [dbo].[users]
ADD CONSTRAINT [FK_subscriptions]
    FOREIGN KEY ([channelsSubscriped_id])
    REFERENCES [dbo].[channels]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_subscriptions'
CREATE INDEX [IX_FK_subscriptions]
ON [dbo].[users]
    ([channelsSubscriped_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------