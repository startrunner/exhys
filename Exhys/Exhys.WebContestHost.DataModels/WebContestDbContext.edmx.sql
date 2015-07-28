
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/25/2015 22:56:37
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

IF OBJECT_ID(N'[dbo].[FK_UserAccounts_UserSessions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSessions] DROP CONSTRAINT [FK_UserAccounts_UserSessions];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccounts_UserGroups_UserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccounts_UserGroups] DROP CONSTRAINT [FK_UserAccounts_UserGroups_UserGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccounts_UserGroups_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccounts_UserGroups] DROP CONSTRAINT [FK_UserAccounts_UserGroups_UserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_Competitions_Problems]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Problems] DROP CONSTRAINT [FK_Competitions_Problems];
GO
IF OBJECT_ID(N'[dbo].[FK_Competitions_UserGroups_Competition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Competitions_UserGroups] DROP CONSTRAINT [FK_Competitions_UserGroups_Competition];
GO
IF OBJECT_ID(N'[dbo].[FK_Competitions_UserGroups_UserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Competitions_UserGroups] DROP CONSTRAINT [FK_Competitions_UserGroups_UserGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_Problems_Solutions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Problems] DROP CONSTRAINT [FK_Problems_Solutions];
GO
IF OBJECT_ID(N'[dbo].[FK_ProblemSolutions_UserAccounts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemSolutions] DROP CONSTRAINT [FK_ProblemSolutions_UserAccounts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProblemProblemStatement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemStatements] DROP CONSTRAINT [FK_ProblemProblemStatement];
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
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
GO
IF OBJECT_ID(N'[dbo].[Competitions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Competitions];
GO
IF OBJECT_ID(N'[dbo].[Problems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Problems];
GO
IF OBJECT_ID(N'[dbo].[ProblemSolutions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProblemSolutions];
GO
IF OBJECT_ID(N'[dbo].[ProblemStatements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProblemStatements];
GO
IF OBJECT_ID(N'[dbo].[UserAccounts_UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccounts_UserGroups];
GO
IF OBJECT_ID(N'[dbo].[Competitions_UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Competitions_UserGroups];
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
    [Name] nvarchar(32)  NOT NULL,
    [Description] nvarchar(256)  NULL,
    [IsOpen] bit  NOT NULL,
    [IsAdministrator] bit  NOT NULL
);
GO

-- Creating table 'Competitions'
CREATE TABLE [dbo].[Competitions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Description] nvarchar(256)  NULL
);
GO

-- Creating table 'Problems'
CREATE TABLE [dbo].[Problems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CompetitionGivenAt_Id] int  NOT NULL
);
GO

-- Creating table 'ProblemSolutions'
CREATE TABLE [dbo].[ProblemSolutions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SourceCode] nvarchar(max)  NOT NULL,
    [LanguageAlias] nvarchar(8)  NOT NULL,
    [Problem_Id] int  NOT NULL,
    [Author_Id] int  NOT NULL,
    [Author_Username] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'ProblemStatements'
CREATE TABLE [dbo].[ProblemStatements] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Bytes] varbinary(max)  NOT NULL,
    [Filename] nvarchar(max)  NOT NULL,
    [ProblemProblemStatement_ProblemStatement_Id] int  NOT NULL
);
GO

-- Creating table 'UserAccounts_UserGroups'
CREATE TABLE [dbo].[UserAccounts_UserGroups] (
    [UserGroups_Id] int  NOT NULL,
    [GroupMembers_Id] int  NOT NULL,
    [GroupMembers_Username] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'Competitions_UserGroups'
CREATE TABLE [dbo].[Competitions_UserGroups] (
    [AvaiableCompetitions_Id] int  NOT NULL,
    [GroupsAllowed_Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'Competitions'
ALTER TABLE [dbo].[Competitions]
ADD CONSTRAINT [PK_Competitions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Problems'
ALTER TABLE [dbo].[Problems]
ADD CONSTRAINT [PK_Problems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProblemSolutions'
ALTER TABLE [dbo].[ProblemSolutions]
ADD CONSTRAINT [PK_ProblemSolutions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProblemStatements'
ALTER TABLE [dbo].[ProblemStatements]
ADD CONSTRAINT [PK_ProblemStatements]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserGroups_Id], [GroupMembers_Id], [GroupMembers_Username] in table 'UserAccounts_UserGroups'
ALTER TABLE [dbo].[UserAccounts_UserGroups]
ADD CONSTRAINT [PK_UserAccounts_UserGroups]
    PRIMARY KEY CLUSTERED ([UserGroups_Id], [GroupMembers_Id], [GroupMembers_Username] ASC);
GO

-- Creating primary key on [AvaiableCompetitions_Id], [GroupsAllowed_Id] in table 'Competitions_UserGroups'
ALTER TABLE [dbo].[Competitions_UserGroups]
ADD CONSTRAINT [PK_Competitions_UserGroups]
    PRIMARY KEY CLUSTERED ([AvaiableCompetitions_Id], [GroupsAllowed_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserAccount_Id], [UserAccount_Username] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [FK_UserAccounts_UserSessions]
    FOREIGN KEY ([UserAccount_Id], [UserAccount_Username])
    REFERENCES [dbo].[UserAccounts]
        ([Id], [Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccounts_UserSessions'
CREATE INDEX [IX_FK_UserAccounts_UserSessions]
ON [dbo].[UserSessions]
    ([UserAccount_Id], [UserAccount_Username]);
GO

-- Creating foreign key on [UserGroups_Id] in table 'UserAccounts_UserGroups'
ALTER TABLE [dbo].[UserAccounts_UserGroups]
ADD CONSTRAINT [FK_UserAccounts_UserGroups_UserGroup]
    FOREIGN KEY ([UserGroups_Id])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GroupMembers_Id], [GroupMembers_Username] in table 'UserAccounts_UserGroups'
ALTER TABLE [dbo].[UserAccounts_UserGroups]
ADD CONSTRAINT [FK_UserAccounts_UserGroups_UserAccount]
    FOREIGN KEY ([GroupMembers_Id], [GroupMembers_Username])
    REFERENCES [dbo].[UserAccounts]
        ([Id], [Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccounts_UserGroups_UserAccount'
CREATE INDEX [IX_FK_UserAccounts_UserGroups_UserAccount]
ON [dbo].[UserAccounts_UserGroups]
    ([GroupMembers_Id], [GroupMembers_Username]);
GO

-- Creating foreign key on [CompetitionGivenAt_Id] in table 'Problems'
ALTER TABLE [dbo].[Problems]
ADD CONSTRAINT [FK_Competitions_Problems]
    FOREIGN KEY ([CompetitionGivenAt_Id])
    REFERENCES [dbo].[Competitions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Competitions_Problems'
CREATE INDEX [IX_FK_Competitions_Problems]
ON [dbo].[Problems]
    ([CompetitionGivenAt_Id]);
GO

-- Creating foreign key on [AvaiableCompetitions_Id] in table 'Competitions_UserGroups'
ALTER TABLE [dbo].[Competitions_UserGroups]
ADD CONSTRAINT [FK_Competitions_UserGroups_Competition]
    FOREIGN KEY ([AvaiableCompetitions_Id])
    REFERENCES [dbo].[Competitions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GroupsAllowed_Id] in table 'Competitions_UserGroups'
ALTER TABLE [dbo].[Competitions_UserGroups]
ADD CONSTRAINT [FK_Competitions_UserGroups_UserGroup]
    FOREIGN KEY ([GroupsAllowed_Id])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Competitions_UserGroups_UserGroup'
CREATE INDEX [IX_FK_Competitions_UserGroups_UserGroup]
ON [dbo].[Competitions_UserGroups]
    ([GroupsAllowed_Id]);
GO

-- Creating foreign key on [Problem_Id] in table 'ProblemSolutions'
ALTER TABLE [dbo].[ProblemSolutions]
ADD CONSTRAINT [FK_Problems_Solutions]
    FOREIGN KEY ([Problem_Id])
    REFERENCES [dbo].[Problems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Problems_Solutions'
CREATE INDEX [IX_FK_Problems_Solutions]
ON [dbo].[ProblemSolutions]
    ([Problem_Id]);
GO

-- Creating foreign key on [Author_Id], [Author_Username] in table 'ProblemSolutions'
ALTER TABLE [dbo].[ProblemSolutions]
ADD CONSTRAINT [FK_ProblemSolutions_UserAccounts]
    FOREIGN KEY ([Author_Id], [Author_Username])
    REFERENCES [dbo].[UserAccounts]
        ([Id], [Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProblemSolutions_UserAccounts'
CREATE INDEX [IX_FK_ProblemSolutions_UserAccounts]
ON [dbo].[ProblemSolutions]
    ([Author_Id], [Author_Username]);
GO

-- Creating foreign key on [ProblemProblemStatement_ProblemStatement_Id] in table 'ProblemStatements'
ALTER TABLE [dbo].[ProblemStatements]
ADD CONSTRAINT [FK_ProblemProblemStatement]
    FOREIGN KEY ([ProblemProblemStatement_ProblemStatement_Id])
    REFERENCES [dbo].[Problems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProblemProblemStatement'
CREATE INDEX [IX_FK_ProblemProblemStatement]
ON [dbo].[ProblemStatements]
    ([ProblemProblemStatement_ProblemStatement_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------