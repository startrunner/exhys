namespace Exhys.WebContestHost.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedT_Properties : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Problems", "T_TimeLimits");
            DropColumn("dbo.Problems", "T_InputFeedbacks");
            DropColumn("dbo.Problems", "T_OutputFeedbacks");
            DropColumn("dbo.Problems", "T_SolutionFeedbacks");
            DropColumn("dbo.Problems", "T_ScoreFeedbacks");
            DropColumn("dbo.Problems", "T_StatusFeedbacks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Problems", "T_StatusFeedbacks", c => c.String());
            AddColumn("dbo.Problems", "T_ScoreFeedbacks", c => c.String());
            AddColumn("dbo.Problems", "T_SolutionFeedbacks", c => c.String());
            AddColumn("dbo.Problems", "T_OutputFeedbacks", c => c.String());
            AddColumn("dbo.Problems", "T_InputFeedbacks", c => c.String());
            AddColumn("dbo.Problems", "T_TimeLimits", c => c.String());
        }
    }
}
