using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class ExhysContestEntities:DbContext
    {
        private class Initializer:DropCreateDatabaseAlways<ExhysContestEntities>
        {
            protected override void Seed (ExhysContestEntities db)
            {
                var adminGroup = new UserGroup()
                {
                    Name = "Administrators",
                    IsAdministrator = true,
                    IsOpen = false
                };
                db.UserGroups.Add(adminGroup);

                var adminUser = new UserAccount()
                {
                    FullName = "Admin Administers",
                    Password = "123456",
                    Username = "admin",
                    UserGroup = adminGroup
                };

                db.UserAccounts.Add(adminUser);

                base.Seed(db);
            }
        }

        public const string ConnectionString = "Name=ExhysContestEntities";
        public ExhysContestEntities ()//:base(ConnectionString)
        {
            Database.SetInitializer(new Initializer());
        }

        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemSolution> ProblemSolutions { get; set; }
        public DbSet<SolutionTestStatus> SolutionTestStatuses { get; set; }
        public DbSet<ProblemStatement> ProblemStatements { get; set; }
        public DbSet<ProblemTest> ProblemTests { get; set; }
    }
}
