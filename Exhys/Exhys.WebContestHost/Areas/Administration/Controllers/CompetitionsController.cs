using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;
using System.Data.Entity;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    [AuthorizeExhysAdmin]
    public class CompetitionsController : ExhysController
    {

        [HttpGet]
        public ActionResult List()
        {
            AddSignedInUserInformation();
            AddUserGroupOptions();

            var vm = new List<CompetitionViewModel>();
            using (var db=new ExhysContestEntities())
            {
                db.Competitions
                    .Include(c=>c.UserGroup)
                    .ToList()
                    .ForEach((c) =>
                    {
                        vm.Add(new CompetitionViewModel(c));
                    });
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult List(IList<CompetitionViewModel> vm)
        {
            using (var db = new ExhysContestEntities())
            {
                foreach(CompetitionViewModel compVm in vm)
                {
                    var comp = db.Competitions
                        .Where(c => c.Id == compVm.Id)
                        .Include((c)=>c.UserGroup)
                        .FirstOrDefault();
                    if (!compVm.RequestDelete&&compVm.Validate(ViewData))
                    {

                        var gr = db.UserGroups.Where(g => g.Id == compVm.GroupId).FirstOrDefault();

                        comp.UserGroup = gr;

                        comp.Name = compVm.Name;
                        comp.Description = compVm.Description;
                    }
                    else
                    {
                        db.Competitions.Remove(comp);
                    }

                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Add ()
        {
            AddSignedInUserInformation();
            AddUserGroupOptions();

            return View();
        }

        [HttpPost]
        public ActionResult Add(CompetitionViewModel vm)
        {
            if(!vm.Validate(ViewData))
            {
                AddSignedInUserInformation();
                AddUserGroupOptions();

                return View(vm);
            }
            using (var db = new ExhysContestEntities())
            {
                var competition = new Competition()
                {
                    Name = vm.Name,
                    Description = vm.Description
                };

                if (vm.GroupId != null)
                {
                    var gr = db.UserGroups
                        .Where(g => g.Id == vm.GroupId)
                        .FirstOrDefault();
                    competition.UserGroup = gr;
                }

                db.Competitions.Add(competition);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            AddSignedInUserInformation();
            AddUserGroupOptions();

            if (id == null) return RedirectToAction("List");
            using (var db = new ExhysContestEntities())
            {
                Competition comp = db.Competitions.Where(c => c.Id == id).FirstOrDefault();
                if (comp == null) return RedirectToAction("List");
                return View(new CompetitionViewModel(comp));
            }
        }

        [HttpPost]
        public ActionResult Edit(CompetitionViewModel vm)
        {
            using (var db = new ExhysContestEntities())
            {
                var competition = db.Competitions.Where(c => c.Id == vm.Id).FirstOrDefault();
                competition.Name = vm.Name;
                competition.Description = vm.Description;
                competition.UserGroup = db.UserGroups.Where(g => g.Id == vm.GroupId).Take(1).ToList()[0];
                db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = vm.Id });
        }
    }
}