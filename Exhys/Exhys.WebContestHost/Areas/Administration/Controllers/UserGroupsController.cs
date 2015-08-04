using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    public class UserGroupsController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            using (var db = new ExhysContestEntities())
            {
                var groups = new List<UserGroupViewModel>();
                db.UserGroups.ToList().ForEach((g) => { groups.Add(new UserGroupViewModel(g)); });
                return View(groups);
            }
        }

        [HttpPost]
        public ActionResult List(IList<UserGroupViewModel> vm)
        {
            using (var db = new ExhysContestEntities())
            {
                foreach(var grvm in vm)
                {
                    var group = db.UserGroups.Where(g => g.Id == grvm.Id).ToList()[0];

                    if (grvm.RequestDelete == false)
                    {
                        group.Name = grvm.Name;
                        group.Description = grvm.Description;
                        group.IsAdministrator = grvm.IsAdmin;
                        group.IsOpen = grvm.IsOpen;
                    }
                    else
                    {
                        group.GroupMembers.Clear();
                        db.UserGroups.Remove(group);
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult AddGroup(UserGroupViewModel vm)
        {
            using (var db = new ExhysContestEntities())
            {
                var gr = new UserGroup()
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    IsAdministrator = vm.IsAdmin,
                    IsOpen = vm.IsOpen
                };
                db.UserGroups.Add(gr);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}