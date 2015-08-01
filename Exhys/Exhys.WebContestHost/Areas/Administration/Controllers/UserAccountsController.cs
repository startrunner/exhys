using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Exhys.WebContestHost.Areas.Shared;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;
using CodeBits;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    public class UserAccountsController : ExhysMvcController
    {
		[HttpGet]
        public ActionResult List()
        {
            AddUserGroupOptions();

			using (var db=new ExhysContestEntities())
            {
                var vm = new List<UserAccountViewModel>();
                db.UserAccounts
                    .OrderBy(a=>a.Username)
                    .ToList()
                    .ForEach((acc) =>
                    {
                        vm.Add(new UserAccountViewModel(acc));
                    });
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult List (List<UserAccountViewModel> vm)
        {
            using (var db = new ExhysContestEntities())
            {
                foreach (var v in vm)
                {

                    var user = db.UserAccounts
                        .Where(u => u.Id == v.UserId)
                        .Take(1)
                        .ToList()[0];
                    if (v.RequestDelete == false)
                    {
                        user.FirstName = v.FirstName;
                        user.LastName = v.LastName;
                        user.Password = v.Password;
                        user.UserSessions.Clear();
                        var gr = db.UserGroups.Where(g => g.Id == v.GroupId).Take(1).ToList()[0];
                        user.UserGroup = gr;
                    }
                    else
                    {
                        user.DeleteFrom(db);
                    }
                    db.SaveChanges();
                }
                //db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult AddUsers()
        {
            AddUserGroupOptions();

            return View();
        }

        [HttpPost]
        public ActionResult AddUsers (string prefix, int? count, string firstNames, string lastNames, int groupId)
        { 
            if (prefix == null || count == null) return RedirectToAction("AddUsers");
            else
            {
                string[] fNames = firstNames.Split('\n');
                string[] lNames = lastNames.Split('\n');
                for (int i = 0; i < fNames.Length; i++) fNames[i] = fNames[i].Trim();
                for (int i = 0; i < lNames.Length; i++) lNames[i] = lNames[i].Trim();

                using (var db = new ExhysContestEntities())
                {
                    //var gr = db.GetDefaultUserGroup();
                    var gr=db.UserGroups.Where(g=>g.Id==groupId).Take(1).ToList()[0];

                    int addedCount = 0;
                    for (int currentNumber = 0; addedCount != count; currentNumber++)
                    {
                        string currentUsername = string.Format("{0}{1:000}", prefix, currentNumber);

                        var existing = db.UserAccounts.Where(u => u.Username == currentUsername).ToList();
                        if (existing != null && existing.Count != 0)
                        {
                            //currentUsername is taken
                            continue;
                        }

                        var user = new UserAccount()
                        {
                            Username = currentUsername,
                            Password = PasswordGenerator.Generate(6, PasswordCharacters.AlphaNumeric).ToLower(),
                            FirstName = addedCount < fNames.Length ? fNames[addedCount] : "",
                            LastName = addedCount < lNames.Length ? lNames[addedCount] : "",
                            UserGroup = gr
                        };
                        db.UserAccounts.Add(user);
                        addedCount++;
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
        }
    }
}