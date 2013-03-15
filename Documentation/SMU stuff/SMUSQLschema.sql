CREATE TABLE [dbo].[SMUusers] (
	[id] int IDENTITY(1,1) NOT NULL,
	[email] VARCHAR(255) NOT NULL,
	[username] VARCHAR(50) NOT NULL,
	[password] VARCHAR(20) NOT NULL,
	[isAdmin] int NOT NULL
);
	
CREATE TABLE [dbo].[SMUbooks] (
	[id] int IDENTITY(1,1) NOT NULL,
	[title] VARCHAR(50) NOT NULL,
	[author] VARCHAR(50) NOT NULL,
	[description] VARCHAR(255) NULL,
	[genre] VARCHAR(50) NULL,
	[price] FLOAT NOT NULL,
	[dateAdded] DATE NOT NULL,
	[hasAudio] int NOT NULL,
	[audioId] int NULL,
	[PDFFilePath] VARCHAR(255),
	[imageFilePath] VARCHAR(255),
	[hit] int NOT NULL,
	FOREIGN KEY(`audioId`) REFERENCES `audio`(`id`),
	PRIMARY KEY(`id`));

CREATE TABLE [dbo].[SMUaudio] (
	[id] int IDENTITY(1,1) NOT NULL,
	[narrator] VARCHAR(50) NULL,
	[filePath] VARCHAR(255) NOT NULL,
	PRIMARY KEY(`id`));
	
CREATE TABLE [dbo].[SMUrentals] (
	[id] int IDENTITY(1,1) NOT NULL,
	[userId] int NOT NULL,
	[bookId] int NULL,
	[audioId] int NULL,
	[startDate] datetime NOT NULL,
	FOREIGN KEY(`bookId`) REFERENCES `books`(`id`),
	FOREIGN KEY(`audioId`) REFERENCES `audio`(`id`),
	PRIMARY KEY(`id`));
	
ALTER TABLE [dbo].[SMUusers]
ADD CONSTRAINT [PK_SMUusers]
 PRIMARY KEY CLUSTERED ([id] ASC);
 
ALTER TABLE [dbo].[SMUbooks]
ADD CONSTRAINT [PK_SMUbooks]
 PRIMARY KEY CLUSTERED ([id] ASC);

ALTER TABLE [dbo].[SMUaudio]
ADD CONSTRAINT [PK_SMUaudio]
 PRIMARY KEY CLUSTERED ([id] ASC);

ALTER TABLE [dbo].[SMUrentals]
ADD CONSTRAINT [PK_SMUrentals]
 PRIMARY KEY CLUSTERED ([id] ASC);

ALTER TABLE [dbo].[SMUrentals]
	ADD CONSTRAINT [FK_SMUrentals_userId]
	FOREIGN KEY ([userId])
	REFERENCES [dbo].[SMUusers]
		([id])
	ON DELETE CASCADE;

ALTER TABLE [dbo].[SMUrentals]
	ADD CONSTRAINT [FK_SMUrentals_bookId]
	FOREIGN KEY ([bookId])
	REFERENCES [dbo].[SMUbooks]
		([id])
	ON DELETE NO ACTION;
	
ALTER TABLE [dbo].[SMUrentals]
	ADD CONSTRAINT [FK_SMUrentals_audioId]
	FOREIGN KEY ([audioId])
	REFERENCES [dbo].[SMUaudio]
		([id])
	ON DELETE NO ACTION;

ALTER TABLE [dbo].[SMUbooks]
	ADD CONSTRAINT [FK_SMUrentals_audioId]
	FOREIGN KEY ([audioId])
	REFERENCES [dbo].[SMUaudio]
		([id])
	ON DELETE NO ACTION;