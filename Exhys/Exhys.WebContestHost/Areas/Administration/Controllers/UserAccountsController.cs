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
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using System.Data.Entity;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    public class UserAccountsController : ExhysController
    {
		[HttpGet]
        public ActionResult List()
        {
            AddSignedInUserInformation();
            AddUserGroupOptions();

			using (var db=new ExhysContestEntities())
            {
                var vm = new List<UserAccountViewModel>();
                db.UserAccounts
                    .OrderBy(a=>a.Username)
                    .Include(u=>u.UserGroup)
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
                        .Include(u=>u.UserGroup)
                        .FirstOrDefault();
                    if (v.RequestDelete == false)
                    {
                        user.FullName = v.FullName;
                        user.Password = v.Password;
                        UserGroup gr = db.UserGroups.Where(g => g.Id == v.GroupId).FirstOrDefault();
                        user.UserGroup = gr;
                    }
                    else
                    {
                        db.UserAccounts.Remove(user);
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult AddMany()
        {
            AddSignedInUserInformation();
            AddUserGroupOptions();

            return View();
        }

        [HttpPost]
        public ActionResult AddMany (string prefix, int? count, string fullNames, int groupId)
        { 
            if (prefix == null || count == null) return RedirectToAction("AddUsers");
            else
            {
                List<string> peopleNames = fullNames.Split('\n').ToList().Resize(count.Value, "");

                using (var db = new ExhysContestEntities())
                {
                    var gr = db.UserGroups.Where(g => g.Id == groupId).FirstOrDefault();

                    int addedCount = 0;
                    for (int currentNumber = 0; addedCount != count; currentNumber++)
                    {
                        string currentUsername = string.Format("{0}{1:000}", prefix, currentNumber);

                        var existing = db.UserAccounts.Where(u => u.Username == currentUsername).FirstOrDefault();
                        if (existing != null)
                        {
                            //currentUsername is taken
                            continue;
                        }

                        var user = new UserAccount()
                        {
                            Username = currentUsername,
                            Password = PasswordGenerator.Generate(6, PasswordCharacters.AlphaNumeric).ToLower(),
                            FullName = peopleNames[addedCount],
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