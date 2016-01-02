CREATE TABLE [dbo].[Animal]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] [NVARCHAR](256) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[TypeId] [INT] NOT NULL,
	[ColorId] [INT] NOT NULL,
	[LocationId] [INT] NOT NULL
	)
GO
ALTER TABLE [dbo].[Animal]  WITH CHECK ADD  CONSTRAINT [fk_Animal_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[AnimalType] ([Id])
GO

ALTER TABLE [dbo].[Animal]  WITH CHECK ADD  CONSTRAINT [fk_Animal_ColorId] FOREIGN KEY([ColorId])
REFERENCES [dbo].[FellColor] ([Id])
GO

ALTER TABLE [dbo].[Animal]  WITH CHECK ADD  CONSTRAINT [fk_Animal_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO