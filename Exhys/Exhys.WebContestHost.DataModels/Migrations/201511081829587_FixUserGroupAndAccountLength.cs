namespace Exhys.WebContestHost.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserGroupAndAccountLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserAccounts", "FullName", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserGroups", "Name", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserGroups", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.UserAccounts", "FullName", c => c.String());
        }
    }
}
