﻿/*
Deployment script for AnimalDb

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "AnimalDb"
:setvar DefaultFilePrefix "AnimalDb"
:setvar DefaultDataPath "C:\Users\aburindina\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\v11.0"
:setvar DefaultLogPath "C:\Users\aburindina\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\v11.0"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [master];


GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creating $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)]
    ON 
    PRIMARY(NAME = [$(DatabaseName)], FILENAME = N'$(DefaultDataPath)$(DefaultFilePrefix)_Primary.mdf')
    LOG ON (NAME = [$(DatabaseName)_log], FILENAME = N'$(DefaultLogPath)$(DefaultFilePrefix)_Primary.ldf') COLLATE SQL_Latin1_General_CP1_CI_AS
GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF),
                CONTAINMENT = NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)];


GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
PRINT N'Creating [dbo].[Animal]...';


GO
CREATE TABLE [dbo].[Animal] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (256) COLLATE Cyrillic_General_CI_AS NOT NULL,
    [TypeId]     INT            NOT NULL,
    [ColorId]    INT            NOT NULL,
    [LocationId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[AnimalType]...';


GO
CREATE TABLE [dbo].[AnimalType] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (256) COLLATE Cyrillic_General_CI_AS NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[FellColor]...';


GO
CREATE TABLE [dbo].[FellColor] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (256) COLLATE Cyrillic_General_CI_AS NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Location]...';


GO
CREATE TABLE [dbo].[Location] (
    [Id]       INT            NOT NULL,
    [Name]     NVARCHAR (256) COLLATE Cyrillic_General_CI_AS NOT NULL,
    [RegionId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Region]...';


GO
CREATE TABLE [dbo].[Region] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (256) COLLATE Cyrillic_General_CI_AS NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[fk_Animal_TypeId]...';


GO
ALTER TABLE [dbo].[Animal]
    ADD CONSTRAINT [fk_Animal_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[AnimalType] ([Id]);


GO
PRINT N'Creating [dbo].[fk_Animal_ColorId]...';


GO
ALTER TABLE [dbo].[Animal]
    ADD CONSTRAINT [fk_Animal_ColorId] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[FellColor] ([Id]);


GO
PRINT N'Creating [dbo].[fk_Animal_LocationId]...';


GO
ALTER TABLE [dbo].[Animal]
    ADD CONSTRAINT [fk_Animal_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]);


GO
PRINT N'Creating [dbo].[fk_Location_RegionId]...';


GO
ALTER TABLE [dbo].[Location]
    ADD CONSTRAINT [fk_Location_RegionId] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Region] ([Id]);


GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
insert into dbo.AnimalType(Id, Name) values(1, N'Земноводное')
insert into dbo.AnimalType(Id, Name) values(2, N'Птицы')
insert into dbo.AnimalType(Id, Name) values(3, N'Пресмыкающиеся')
insert into dbo.AnimalType(Id, Name) values(4, N'Млекопитаюшие')
insert into dbo.AnimalType(Id, Name) values(5, N'Рыбы')
GO
insert into dbo.FellColor(Id, Name) values(1, N'серая')
insert into dbo.FellColor(Id, Name) values(2, N'цветная')
insert into dbo.FellColor(Id, Name) values(3, N'черная')
insert into dbo.FellColor(Id, Name) values(4, N'белая')

GO
insert into dbo.Region(Id, Name) values(1, N'Россия')
insert into dbo.Region(Id, Name) values(2, N'Германия')
insert into dbo.Region(Id, Name) values(3, N'Франция')
insert into dbo.Region(Id, Name) values(4, N'Италия')
GO
insert into dbo.Location(Id, Name, RegionId) values(1, N'Москва', 1 )
insert into dbo.Location(Id, Name, RegionId) values(2, N'Екатеринбург', 1)
insert into dbo.Location(Id, Name, RegionId) values(3, N'Казань', 1)
insert into dbo.Location(Id, Name, RegionId) values(4, N'Берлин', 2 )
insert into dbo.Location(Id, Name, RegionId) values(5, N'Кельн', 2 )
insert into dbo.Location(Id, Name, RegionId) values(6, N'Париж', 3)
GO

GO
DECLARE @VarDecimalSupported AS BIT;

SELECT @VarDecimalSupported = 0;

IF ((ServerProperty(N'EngineEdition') = 3)
    AND (((@@microsoftversion / power(2, 24) = 9)
          AND (@@microsoftversion & 0xffff >= 3024))
         OR ((@@microsoftversion / power(2, 24) = 10)
             AND (@@microsoftversion & 0xffff >= 1600))))
    SELECT @VarDecimalSupported = 1;

IF (@VarDecimalSupported > 0)
    BEGIN
        EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
    END


GO
PRINT N'Update complete.';


GO
