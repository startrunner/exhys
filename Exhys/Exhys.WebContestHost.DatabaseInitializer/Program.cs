﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.DatabaseInitializer
{
    class Program
    {
        static void Main (string[] args)
        {
            using (var db = new ExhysContestEntities())
            {
                var oAdmin = db.UserAccounts.Where(a => a.Username == "admin").ToList();
                var oGroups = db.UserGroups.Where(g => g.Name == "Admins").ToList();

                foreach (var v in oAdmin)
                {
                    v.UserGroups.Clear();
                    v.UserSessions.Clear();
                }
                db.UserAccounts.RemoveRange(oAdmin);
                db.UserGroups.RemoveRange(oGroups);
                

                var admins = new UserGroup()
                {
                    IsOpen = false,
                    Name = "Admins",
                    Description = "Administrators",
                    IsAdministrator = true
                };

                var admin = new UserAccount()
                {
                    FirstName = "Admin",
                    LastName = "Administers",
                    Password = "megasecret",
                    Username = "admin"
                };

                admin.UserGroups.Add(admins);

                db.UserGroups.Add(admins);
                db.UserAccounts.Add(admin);
                db.SaveChanges();
            }
            Process.GetCurrentProcess().Kill();
        }
    }
}
