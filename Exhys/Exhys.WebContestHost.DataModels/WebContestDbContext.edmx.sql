
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/20/2015 17:39:19
-- Generated from EDMX file: C:\Users\Alexander\Source\Repos\Exhys\Exhys\Exhys.WebContestHost.DataModels\WebContestDbContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ExhysContest];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserSessionUserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSessions] DROP CONSTRAINT [FK_UserSessionUserAccount];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccounts];
GO
IF OBJECT_ID(N'[dbo].[UserSessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSessions];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserAccounts'
CREATE TABLE [dbo].[UserAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(32)  NOT NULL,
    [FirstName] nvarchar(32)  NULL,
    [LastName] nvarchar(32)  NULL,
    [Password] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'UserSessions'
CREATE TABLE [dbo].[UserSessions] (
    [Id] uniqueidentifier  NOT NULL,
    [UserAgentString] nvarchar(400)  NOT NULL,
    [BrowserName] nvarchar(400)  NOT NULL,
    [IPAdress] nvarchar(64)  NOT NULL,
    [UserAccount_Id] int  NULL,
    [UserAccount_Username] nvarchar(32)  NULL
);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsOpen] bit  NOT NULL,
    [IsAdministrator] bit  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Description] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'UserGroupUserAccount'
CREATE TABLE [dbo].[UserGroupUserAccount] (
    [UserGroups_Id] int  NOT NULL,
    [MemberUsers_Id] int  NOT NULL,
    [MemberUsers_Username] nvarchar(32)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id], [Username] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [PK_UserAccounts]
    PRIMARY KEY CLUSTERED ([Id], [Username] ASC);
GO

-- Creating primary key on [Id] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [PK_UserSessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [PK_UserGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserGroups_Id], [MemberUsers_Id], [MemberUsers_Username] in table 'UserGroupUserAccount'
ALTER TABLE [dbo].[UserGroupUserAccount]
ADD CONSTRAINT [PK_UserGroupUserAccount]
    PRIMARY KEY CLUSTERED ([UserGroups_Id], [MemberUsers_Id], [MemberUsers_Username] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserAccount_Id], [UserAccount_Username] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [FK_UserSessionUserAccount]
    FOREIGN KEY ([UserAccount_Id], [UserAccount_Username])
    REFERENCES [dbo].[UserAccounts]
        ([Id], [Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSessionUserAccount'
CREATE INDEX [IX_FK_UserSessionUserAccount]
ON [dbo].[UserSessions]
    ([UserAccount_Id], [UserAccount_Username]);
GO

-- Creating foreign key on [UserGroups_Id] in table 'UserGroupUserAccount'
ALTER TABLE [dbo].[UserGroupUserAccount]
ADD CONSTRAINT [FK_UserGroupUserAccount_UserGroup]
    FOREIGN KEY ([UserGroups_Id])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MemberUsers_Id], [MemberUsers_Username] in table 'UserGroupUserAccount'
ALTER TABLE [dbo].[UserGroupUserAccount]
ADD CONSTRAINT [FK_UserGroupUserAccount_UserAccount]
    FOREIGN KEY ([MemberUsers_Id], [MemberUsers_Username])
    REFERENCES [dbo].[UserAccounts]
        ([Id], [Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupUserAccount_UserAccount'
CREATE INDEX [IX_FK_UserGroupUserAccount_UserAccount]
ON [dbo].[UserGroupUserAccount]
    ([MemberUsers_Id], [MemberUsers_Username]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------