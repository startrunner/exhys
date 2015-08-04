using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    public class UserAccountsController : Controller
    {
		[HttpGet]
        public ActionResult List()
        {
			using (var db=new ExhysContestEntities())
            {
                var userGroups = db.UserGroups.ToList();

                string[] groupNames = new string[userGroups.Count];
                int[] groupIds = new int[userGroups.Count];
                for(int i=0;i<userGroups.Count;i++)
                {
                    groupNames[i] = userGroups[i].Name;
                    groupIds[i] = userGroups[i].Id;
                }
                ViewBag.GroupNames = groupNames;
                ViewBag.GroupIds = groupIds;
                
                var vm = new List<UserAccountViewModel>();
                db.UserAccounts
                    .OrderBy(a=>a.Username)
                    .ToList()
                    .ForEach((acc) =>
                    {
                        var cVm = new UserAccountViewModel(acc);
                        cVm.UserGroups = new bool[userGroups.Count];
                        vm.Add(cVm);
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
                    }
                    else
                    {
                        user.DeleteFrom(db);
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult AddUsers(string prefix, int? count, string firstNames, string lastNames)
        {
            if (prefix == null || count == null) return View();
            else
            {
                string[] fNames = firstNames.Split('\n');
                string[] lNames = lastNames.Split('\n');
                for (int i = 0; i < fNames.Length; i++) fNames[i] = fNames[i].Trim();
                for (int i = 0; i < lNames.Length; i++) lNames[i] = lNames[i].Trim();

                using (var db = new ExhysContestEntities())
                {
                    var gr = db.GetDefaultUserGroup();

                    int added = 0;
                    for(int i = 0; added != count; i++)
                    {
                        string current = string.Format("{0}{1:000}", prefix, i);
                        var existing = db.UserAccounts.Where(u => u.Username == current).ToList();
                        if (existing != null && existing.Count != 0) continue;
                        var rnd = new Random();
                        var user = new UserAccount()
                        {
                            Username = current,
                            Password = Guid.NewGuid().ToString("n").Substring(rnd.Next(6), 6),
                            FirstName = added < fNames.Length ? fNames[added] : "",
                            LastName = added < lNames.Length ? lNames[added] : "",
                            UserGroup = gr
                        };
                        db.UserAccounts.Add(user);
                        added++;
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
        }
    }
}