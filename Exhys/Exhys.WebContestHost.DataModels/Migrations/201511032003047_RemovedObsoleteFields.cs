namespace Exhys.WebContestHost.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedObsoleteFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "FullName", c => c.String());
            DropColumn("dbo.UserAccounts", "FirstName");
            DropColumn("dbo.UserAccounts", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserAccounts", "LastName", c => c.String());
            AddColumn("dbo.UserAccounts", "FirstName", c => c.String());
            DropColumn("dbo.UserAccounts", "FullName");
        }
    }
}
