namespace Exhys.WebContestHost.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoredProblemSolution : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SolutionTestStatus", "ExecutionStatus", c => c.Int(nullable: false));
            AddColumn("dbo.SolutionTestStatus", "ProblemTest_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.SolutionTestStatus", "ProblemTest_Id");
            AddForeignKey("dbo.SolutionTestStatus", "ProblemTest_Id", "dbo.ProblemTests", "Id", cascadeDelete: true);
            DropColumn("dbo.SolutionTestStatus", "Input");
            DropColumn("dbo.SolutionTestStatus", "Solution");
            DropColumn("dbo.SolutionTestStatus", "StatusCode");
            DropColumn("dbo.SolutionTestStatus", "InputFeedbackEnabled");
            DropColumn("dbo.SolutionTestStatus", "OutputFeedbackEnabled");
            DropColumn("dbo.SolutionTestStatus", "SolutionFeedbackEnabled");
            DropColumn("dbo.SolutionTestStatus", "ScoreFeedbackEnabled");
            DropColumn("dbo.SolutionTestStatus", "StatusFeedbackEnabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolutionTestStatus", "StatusFeedbackEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolutionTestStatus", "ScoreFeedbackEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolutionTestStatus", "SolutionFeedbackEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolutionTestStatus", "OutputFeedbackEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolutionTestStatus", "InputFeedbackEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.SolutionTestStatus", "StatusCode", c => c.Byte(nullable: false));
            AddColumn("dbo.SolutionTestStatus", "Solution", c => c.String());
            AddColumn("dbo.SolutionTestStatus", "Input", c => c.String());
            DropForeignKey("dbo.SolutionTestStatus", "ProblemTest_Id", "dbo.ProblemTests");
            DropIndex("dbo.SolutionTestStatus", new[] { "ProblemTest_Id" });
            DropColumn("dbo.SolutionTestStatus", "ProblemTest_Id");
            DropColumn("dbo.SolutionTestStatus", "ExecutionStatus");
        }
    }
}
