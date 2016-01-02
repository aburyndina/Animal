CREATE TABLE [dbo].[Location]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] [NVARCHAR](256) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[RegionId] INT NOT NULL	
)
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [fk_Location_RegionId] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO