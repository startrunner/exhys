
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/24/2015 21:06:04
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
IF OBJECT_ID(N'[dbo].[FK_UserAccounts_UserGroups]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccounts] DROP CONSTRAINT [FK_UserAccounts_UserGroups];
GO
IF OBJECT_ID(N'[dbo].[FK_Competitions_Problems]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Problems] DROP CONSTRAINT [FK_Competitions_Problems];
GO
IF OBJECT_ID(N'[dbo].[FK_Competitions_UserGroups]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Competitions] DROP CONSTRAINT [FK_Competitions_UserGroups];
GO
IF OBJECT_ID(N'[dbo].[FK_Problems_Solutions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemSolutions] DROP CONSTRAINT [FK_Problems_Solutions];
GO
IF OBJECT_ID(N'[dbo].[FK_ProblemProblemStatement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemStatements] DROP CONSTRAINT [FK_ProblemProblemStatement];
GO
IF OBJECT_ID(N'[dbo].[FK_Problems_ProblemTests]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemTests] DROP CONSTRAINT [FK_Problems_ProblemTests];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccounts_ProblemSolutions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemSolutions] DROP CONSTRAINT [FK_UserAccounts_ProblemSolutions];
GO
IF OBJECT_ID(N'[dbo].[FK_ActiveCheckerProgrammingLanguage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProgrammingLanguages] DROP CONSTRAINT [FK_ActiveCheckerProgrammingLanguage];
GO
IF OBJECT_ID(N'[dbo].[FK_ActiveCheckerPerformanceMetric]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PerformanceMetrics] DROP CONSTRAINT [FK_ActiveCheckerPerformanceMetric];
GO
IF OBJECT_ID(N'[dbo].[FK_ProblemSolutionSolutionTestStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SolutionTestStatuses] DROP CONSTRAINT [FK_ProblemSolutionSolutionTestStatus];
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
IF OBJECT_ID(N'[dbo].[ProblemTests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProblemTests];
GO
IF OBJECT_ID(N'[dbo].[ActiveCheckers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActiveCheckers];
GO
IF OBJECT_ID(N'[dbo].[ProgrammingLanguages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProgrammingLanguages];
GO
IF OBJECT_ID(N'[dbo].[PerformanceMetrics]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PerformanceMetrics];
GO
IF OBJECT_ID(N'[dbo].[SolutionTestStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SolutionTestStatuses];
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
    [Password] nvarchar(32)  NOT NULL,
    [UserGroup_Id] int  NOT NULL
);
GO

-- Creating table 'UserSessions'
CREATE TABLE [dbo].[UserSessions] (
    [Id] uniqueidentifier  NOT NULL,
    [UserAgentString] nvarchar(400)  NOT NULL,
    [BrowserName] nvarchar(400)  NOT NULL,
    [IPAdress] nvarchar(64)  NOT NULL,
    [UserAccount_Id] int  NOT NULL,
    [UserAccount_Username] nvarchar(32)  NOT NULL
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
    [Description] nvarchar(256)  NULL,
    [UserGroup_Id] int  NOT NULL
);
GO

-- Creating table 'Problems'
CREATE TABLE [dbo].[Problems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [T_TimeLimits] nvarchar(max)  NOT NULL,
    [T_InputFeedbacks] nvarchar(max)  NOT NULL,
    [T_OutputFeedbacks] nvarchar(max)  NOT NULL,
    [T_SolutionFeedbacks] nvarchar(max)  NOT NULL,
    [T_ScoreFeedbacks] nvarchar(max)  NOT NULL,
    [T_StatusFeedbacks] nvarchar(max)  NOT NULL,
    [CompetitionGivenAt_Id] int  NOT NULL
);
GO

-- Creating table 'ProblemSolutions'
CREATE TABLE [dbo].[ProblemSolutions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SourceCode] nvarchar(max)  NOT NULL,
    [LanguageAlias] nvarchar(8)  NOT NULL,
    [StatusCode] tinyint  NOT NULL,
    [Message] nvarchar(max)  NULL,
    [ParticipationId] int  NOT NULL,
    [Problem_Id] int  NOT NULL
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

-- Creating table 'ProblemTests'
CREATE TABLE [dbo].[ProblemTests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Input] nvarchar(max)  NULL,
    [Solution] nvarchar(max)  NULL,
    [GroupName] nvarchar(max)  NOT NULL,
    [TimeLimit] float  NOT NULL,
    [Points] float  NOT NULL,
    [InputFeedbackEnabled] bit  NOT NULL,
    [OutputFeedbackEnabled] bit  NOT NULL,
    [SolutionFeedbackEnabled] bit  NOT NULL,
    [ScoreFeedbackEnabled] bit  NOT NULL,
    [StatusFeedbackEnabled] bit  NOT NULL,
    [Problem_Id] int  NOT NULL
);
GO

-- Creating table 'ActiveCheckers'
CREATE TABLE [dbo].[ActiveCheckers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IPAdress] nvarchar(max)  NOT NULL,
    [LastActive] datetime  NOT NULL
);
GO

-- Creating table 'ProgrammingLanguages'
CREATE TABLE [dbo].[ProgrammingLanguages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Alias] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Checker_Id] int  NOT NULL
);
GO

-- Creating table 'PerformanceMetrics'
CREATE TABLE [dbo].[PerformanceMetrics] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [CPULoad] float  NOT NULL,
    [MaximumMemory] bigint  NOT NULL,
    [UtilizedMemory] bigint  NOT NULL,
    [Checker_Id] int  NOT NULL
);
GO

-- Creating table 'SolutionTestStatuses'
CREATE TABLE [dbo].[SolutionTestStatuses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Input] nvarchar(max)  NULL,
    [Output] nvarchar(max)  NULL,
    [Solution] nvarchar(max)  NULL,
    [Score] float  NOT NULL,
    [StatusCode] tinyint  NOT NULL,
    [InputFeedbackEnabled] bit  NOT NULL,
    [OutputFeedbackEnabled] bit  NOT NULL,
    [SolutionFeedbackEnabled] bit  NOT NULL,
    [ScoreFeedbackEnabled] bit  NOT NULL,
    [StatusFeedbackEnabled] bit  NOT NULL,
    [ProblemSolutionSolutionTestStatus_SolutionTestStatus_Id] int  NOT NULL
);
GO

-- Creating table 'Participations'
CREATE TABLE [dbo].[Participations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [User_Id] int  NOT NULL,
    [User_Username] nvarchar(32)  NOT NULL,
    [Competition_Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'ProblemTests'
ALTER TABLE [dbo].[ProblemTests]
ADD CONSTRAINT [PK_ProblemTests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ActiveCheckers'
ALTER TABLE [dbo].[ActiveCheckers]
ADD CONSTRAINT [PK_ActiveCheckers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProgrammingLanguages'
ALTER TABLE [dbo].[ProgrammingLanguages]
ADD CONSTRAINT [PK_ProgrammingLanguages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PerformanceMetrics'
ALTER TABLE [dbo].[PerformanceMetrics]
ADD CONSTRAINT [PK_PerformanceMetrics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SolutionTestStatuses'
ALTER TABLE [dbo].[SolutionTestStatuses]
ADD CONSTRAINT [PK_SolutionTestStatuses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Participations'
ALTER TABLE [dbo].[Participations]
ADD CONSTRAINT [PK_Participations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating foreign key on [UserGroup_Id] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [FK_UserAccounts_UserGroups]
    FOREIGN KEY ([UserGroup_Id])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccounts_UserGroups'
CREATE INDEX [IX_FK_UserAccounts_UserGroups]
ON [dbo].[UserAccounts]
    ([UserGroup_Id]);
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

-- Creating foreign key on [UserGroup_Id] in table 'Competitions'
ALTER TABLE [dbo].[Competitions]
ADD CONSTRAINT [FK_Competitions_UserGroups]
    FOREIGN KEY ([UserGroup_Id])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Competitions_UserGroups'
CREATE INDEX [IX_FK_Competitions_UserGroups]
ON [dbo].[Competitions]
    ([UserGroup_Id]);
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

-- Creating foreign key on [Problem_Id] in table 'ProblemTests'
ALTER TABLE [dbo].[ProblemTests]
ADD CONSTRAINT [FK_Problems_ProblemTests]
    FOREIGN KEY ([Problem_Id])
    REFERENCES [dbo].[Problems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Problems_ProblemTests'
CREATE INDEX [IX_FK_Problems_ProblemTests]
ON [dbo].[ProblemTests]
    ([Problem_Id]);
GO

-- Creating foreign key on [Checker_Id] in table 'ProgrammingLanguages'
ALTER TABLE [dbo].[ProgrammingLanguages]
ADD CONSTRAINT [FK_ActiveCheckerProgrammingLanguage]
    FOREIGN KEY ([Checker_Id])
    REFERENCES [dbo].[ActiveCheckers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActiveCheckerProgrammingLanguage'
CREATE INDEX [IX_FK_ActiveCheckerProgrammingLanguage]
ON [dbo].[ProgrammingLanguages]
    ([Checker_Id]);
GO

-- Creating foreign key on [Checker_Id] in table 'PerformanceMetrics'
ALTER TABLE [dbo].[PerformanceMetrics]
ADD CONSTRAINT [FK_ActiveCheckerPerformanceMetric]
    FOREIGN KEY ([Checker_Id])
    REFERENCES [dbo].[ActiveCheckers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActiveCheckerPerformanceMetric'
CREATE INDEX [IX_FK_ActiveCheckerPerformanceMetric]
ON [dbo].[PerformanceMetrics]
    ([Checker_Id]);
GO

-- Creating foreign key on [ProblemSolutionSolutionTestStatus_SolutionTestStatus_Id] in table 'SolutionTestStatuses'
ALTER TABLE [dbo].[SolutionTestStatuses]
ADD CONSTRAINT [FK_ProblemSolutionSolutionTestStatus]
    FOREIGN KEY ([ProblemSolutionSolutionTestStatus_SolutionTestStatus_Id])
    REFERENCES [dbo].[ProblemSolutions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProblemSolutionSolutionTestStatus'
CREATE INDEX [IX_FK_ProblemSolutionSolutionTestStatus]
ON [dbo].[SolutionTestStatuses]
    ([ProblemSolutionSolutionTestStatus_SolutionTestStatus_Id]);
GO

-- Creating foreign key on [User_Id], [User_Username] in table 'Participations'
ALTER TABLE [dbo].[Participations]
ADD CONSTRAINT [FK_ParticipationUserAccount]
    FOREIGN KEY ([User_Id], [User_Username])
    REFERENCES [dbo].[UserAccounts]
        ([Id], [Username])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParticipationUserAccount'
CREATE INDEX [IX_FK_ParticipationUserAccount]
ON [dbo].[Participations]
    ([User_Id], [User_Username]);
GO

-- Creating foreign key on [Competition_Id] in table 'Participations'
ALTER TABLE [dbo].[Participations]
ADD CONSTRAINT [FK_ParticipationCompetition]
    FOREIGN KEY ([Competition_Id])
    REFERENCES [dbo].[Competitions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParticipationCompetition'
CREATE INDEX [IX_FK_ParticipationCompetition]
ON [dbo].[Participations]
    ([Competition_Id]);
GO

-- Creating foreign key on [ParticipationId] in table 'ProblemSolutions'
ALTER TABLE [dbo].[ProblemSolutions]
ADD CONSTRAINT [FK_ParticipationProblemSolution]
    FOREIGN KEY ([ParticipationId])
    REFERENCES [dbo].[Participations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParticipationProblemSolution'
CREATE INDEX [IX_FK_ParticipationProblemSolution]
ON [dbo].[ProblemSolutions]
    ([ParticipationId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------