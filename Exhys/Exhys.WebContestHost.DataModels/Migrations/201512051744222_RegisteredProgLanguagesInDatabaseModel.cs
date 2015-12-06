namespace Exhys.WebContestHost.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegisteredProgLanguagesInDatabaseModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgrammingLanguages",
                c => new
                    {
                        Alias = c.String(nullable: false, maxLength: 12),
                        Name = c.String(nullable: false, maxLength: 12),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Alias);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProgrammingLanguages");
        }
    }
}
