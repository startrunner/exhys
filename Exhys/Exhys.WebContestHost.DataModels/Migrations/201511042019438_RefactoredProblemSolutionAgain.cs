namespace Exhys.WebContestHost.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoredProblemSolutionAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProblemSolutions", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.SolutionTestStatus", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.ProblemSolutions", "StatusCode");
            DropColumn("dbo.SolutionTestStatus", "ExecutionStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolutionTestStatus", "ExecutionStatus", c => c.Int(nullable: false));
            AddColumn("dbo.ProblemSolutions", "StatusCode", c => c.Byte(nullable: false));
            DropColumn("dbo.SolutionTestStatus", "Status");
            DropColumn("dbo.ProblemSolutions", "Status");
        }
    }
}
