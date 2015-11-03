namespace Exhys.WebContestHost.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        UserGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id)
                .Index(t => t.UserGroup_Id);
            
            CreateTable(
                "dbo.Participations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Competition_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.Competition_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserAccounts", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Competition_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ProblemSolutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SourceCode = c.String(nullable: false),
                        LanguageAlias = c.String(nullable: false),
                        StatusCode = c.Byte(nullable: false),
                        Message = c.String(),
                        Participation_Id = c.Int(nullable: false),
                        Problem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Participations", t => t.Participation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Problems", t => t.Problem_Id, cascadeDelete: true)
                .Index(t => t.Participation_Id)
                .Index(t => t.Problem_Id);
            
            CreateTable(
                "dbo.Problems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        T_TimeLimits = c.String(),
                        T_InputFeedbacks = c.String(),
                        T_OutputFeedbacks = c.String(),
                        T_SolutionFeedbacks = c.String(),
                        T_ScoreFeedbacks = c.String(),
                        T_StatusFeedbacks = c.String(),
                        CompetitionGivenAt_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionGivenAt_Id)
                .Index(t => t.CompetitionGivenAt_Id);
            
            CreateTable(
                "dbo.ProblemStatements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Bytes = c.Binary(nullable: false),
                        Filename = c.String(nullable: false),
                        Problem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Problems", t => t.Problem_Id, cascadeDelete: true)
                .Index(t => t.Problem_Id);
            
            CreateTable(
                "dbo.ProblemTests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Input = c.String(nullable: false),
                        Solution = c.String(nullable: false),
                        GroupName = c.String(nullable: false),
                        TimeLimit = c.Double(nullable: false),
                        Points = c.Double(nullable: false),
                        InputFeedbackEnabled = c.Boolean(nullable: false),
                        OutputFeedbackEnabled = c.Boolean(nullable: false),
                        SolutionFeedbackEnabled = c.Boolean(nullable: false),
                        ScoreFeedbackEnabled = c.Boolean(nullable: false),
                        StatusFeedbackEnabled = c.Boolean(nullable: false),
                        Problem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Problems", t => t.Problem_Id, cascadeDelete: true)
                .Index(t => t.Problem_Id);
            
            CreateTable(
                "dbo.SolutionTestStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Input = c.String(),
                        Output = c.String(),
                        Solution = c.String(),
                        Score = c.Double(nullable: false),
                        StatusCode = c.Byte(nullable: false),
                        InputFeedbackEnabled = c.Boolean(nullable: false),
                        OutputFeedbackEnabled = c.Boolean(nullable: false),
                        SolutionFeedbackEnabled = c.Boolean(nullable: false),
                        ScoreFeedbackEnabled = c.Boolean(nullable: false),
                        StatusFeedbackEnabled = c.Boolean(nullable: false),
                        ProblemSolution_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProblemSolutions", t => t.ProblemSolution_Id)
                .Index(t => t.ProblemSolution_Id);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 32),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Password = c.String(nullable: false),
                        UserGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id, cascadeDelete: true)
                .Index(t => t.Username, unique: true)
                .Index(t => t.UserGroup_Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        IsOpen = c.Boolean(nullable: false),
                        IsAdministrator = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserSessions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserAgentString = c.String(),
                        BrowserName = c.String(),
                        IPAdress = c.String(),
                        UserAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccount_Id, cascadeDelete: true)
                .Index(t => t.UserAccount_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Participations", "User_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.UserSessions", "UserAccount_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.UserAccounts", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Competitions", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.SolutionTestStatus", "ProblemSolution_Id", "dbo.ProblemSolutions");
            DropForeignKey("dbo.ProblemSolutions", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.ProblemTests", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.ProblemStatements", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Problems", "CompetitionGivenAt_Id", "dbo.Competitions");
            DropForeignKey("dbo.ProblemSolutions", "Participation_Id", "dbo.Participations");
            DropForeignKey("dbo.Participations", "Competition_Id", "dbo.Competitions");
            DropIndex("dbo.UserSessions", new[] { "UserAccount_Id" });
            DropIndex("dbo.UserAccounts", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserAccounts", new[] { "Username" });
            DropIndex("dbo.SolutionTestStatus", new[] { "ProblemSolution_Id" });
            DropIndex("dbo.ProblemTests", new[] { "Problem_Id" });
            DropIndex("dbo.ProblemStatements", new[] { "Problem_Id" });
            DropIndex("dbo.Problems", new[] { "CompetitionGivenAt_Id" });
            DropIndex("dbo.ProblemSolutions", new[] { "Problem_Id" });
            DropIndex("dbo.ProblemSolutions", new[] { "Participation_Id" });
            DropIndex("dbo.Participations", new[] { "User_Id" });
            DropIndex("dbo.Participations", new[] { "Competition_Id" });
            DropIndex("dbo.Competitions", new[] { "UserGroup_Id" });
            DropTable("dbo.UserSessions");
            DropTable("dbo.UserGroups");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.SolutionTestStatus");
            DropTable("dbo.ProblemTests");
            DropTable("dbo.ProblemStatements");
            DropTable("dbo.Problems");
            DropTable("dbo.ProblemSolutions");
            DropTable("dbo.Participations");
            DropTable("dbo.Competitions");
        }
    }
}
