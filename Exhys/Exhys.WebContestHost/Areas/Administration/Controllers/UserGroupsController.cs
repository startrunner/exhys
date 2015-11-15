using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    [AuthorizeExhysAdmin]
    public class UserGroupsController : ExhysController
    {
        [HttpGet]
        public ActionResult List()
        {
            AddSignedInUserInformation();
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
                foreach(UserGroupViewModel grvm in vm)
                {
                    UserGroup group = db.UserGroups
                        .Where(g => g.Id == grvm.Id)
                        .Include((g)=>g.AvaiableCompetition)
                        .FirstOrDefault();

                    if (grvm.RequestDelete == false)
                    {
                        group.Name = grvm.Name;
                        group.Description = grvm.Description;
                        group.IsAdministrator = grvm.IsAdmin;
                        group.IsOpen = grvm.IsOpen;
                    }
                    else
                    {
                        db.UserGroups.Remove(group);
                    }
                    db.SaveChanges();
                }
                //db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(UserGroupViewModel vm)
        {
            if(!vm.Validate(ViewData))
            {
                return View(vm);

            }
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