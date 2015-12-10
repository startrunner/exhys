namespace Exhys.WebContestHost.DataModels.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Exhys.WebContestHost.DataModels.ExhysContestEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed (Exhys.WebContestHost.DataModels.ExhysContestEntities context)
        {
            var adminGroup = new UserGroup()
            {
                Name = "Administrators",
                IsAdministrator = true,
                IsOpen = false
            };
            try
            {
                context.UserGroups.AddOrUpdate(x => x.Name, adminGroup);
            }
            catch { }

            var adminUser = new UserAccount()
            {
                FullName = "Admin Administers",
                Password = "123456",
                Username = "admin",
                UserGroup = adminGroup
            };
            try
            {
                context.UserAccounts.AddOrUpdate(x => x.Username, adminUser);
            }
            catch { }

            context.SaveChanges();
        }
    }
}
