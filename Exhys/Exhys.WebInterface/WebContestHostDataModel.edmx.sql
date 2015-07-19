
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/19/2015 21:44:16
-- Generated from EDMX file: C:\Users\Alexander\Source\Repos\Exhys\Exhys\Exhys.WebInterface\WebContestHostDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ExhysWebContest];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserAccountTeam]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccounts] DROP CONSTRAINT [FK_UserAccountTeam];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccountUserGroup_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccountUserGroup] DROP CONSTRAINT [FK_UserAccountUserGroup_UserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccountUserGroup_UserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccountUserGroup] DROP CONSTRAINT [FK_UserAccountUserGroup_UserGroup];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccounts];
GO
IF OBJECT_ID(N'[dbo].[Teams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teams];
GO
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
GO
IF OBJECT_ID(N'[dbo].[UserAccountUserGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccountUserGroup];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserAccounts'
CREATE TABLE [dbo].[UserAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Login] nchar(16)  NOT NULL,
    [PasswordHash] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Team_Id] int  NOT NULL
);
GO

-- Creating table 'Teams'
CREATE TABLE [dbo].[Teams] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserAccountUserGroup'
CREATE TABLE [dbo].[UserAccountUserGroup] (
    [GroupMembers_Id] int  NOT NULL,
    [GroupMembers_Login] nchar(16)  NOT NULL,
    [UserGroups_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id], [Login] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [PK_UserAccounts]
    PRIMARY KEY CLUSTERED ([Id], [Login] ASC);
GO

-- Creating primary key on [Id] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [PK_Teams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [PK_UserGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [GroupMembers_Id], [GroupMembers_Login], [UserGroups_Id] in table 'UserAccountUserGroup'
ALTER TABLE [dbo].[UserAccountUserGroup]
ADD CONSTRAINT [PK_UserAccountUserGroup]
    PRIMARY KEY CLUSTERED ([GroupMembers_Id], [GroupMembers_Login], [UserGroups_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Team_Id] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [FK_UserAccountTeam]
    FOREIGN KEY ([Team_Id])
    REFERENCES [dbo].[Teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccountTeam'
CREATE INDEX [IX_FK_UserAccountTeam]
ON [dbo].[UserAccounts]
    ([Team_Id]);
GO

-- Creating foreign key on [GroupMembers_Id], [GroupMembers_Login] in table 'UserAccountUserGroup'
ALTER TABLE [dbo].[UserAccountUserGroup]
ADD CONSTRAINT [FK_UserAccountUserGroup_UserAccount]
    FOREIGN KEY ([GroupMembers_Id], [GroupMembers_Login])
    REFERENCES [dbo].[UserAccounts]
        ([Id], [Login])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserGroups_Id] in table 'UserAccountUserGroup'
ALTER TABLE [dbo].[UserAccountUserGroup]
ADD CONSTRAINT [FK_UserAccountUserGroup_UserGroup]
    FOREIGN KEY ([UserGroups_Id])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccountUserGroup_UserGroup'
CREATE INDEX [IX_FK_UserAccountUserGroup_UserGroup]
ON [dbo].[UserAccountUserGroup]
    ([UserGroups_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------