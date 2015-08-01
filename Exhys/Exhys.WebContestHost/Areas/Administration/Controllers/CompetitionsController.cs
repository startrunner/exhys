using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    public class CompetitionsController : ExhysMvcController
    {
        [HttpGet]
        public ActionResult List()
        {
            AddUserGroupOptions();

            var vm = new List<CompetitionViewModel>();
            using (var db=new ExhysContestEntities())
            {
                db.Competitions.ToList()
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
                foreach(var v in vm)
                {
                    var comp = db.Competitions.Where(c => c.Id == v.Id).Take(1).ToList()[0];
                    if (v.RequestDelete == false)
                    {
                        var gr = db.UserGroups.Where(g => g.Id == v.GroupId).Take(1).ToList()[0];
                        comp.UserGroup = gr;
                        comp.Name = v.Name;
                        comp.Description = v.Description;
                    }
                    else
                    {
                        comp.DeleteFrom(db);
                    }

                    db.SaveChanges();
                }
                //db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult AddCompetition ()
        {
            AddUserGroupOptions();

            return View();
        }

        [HttpPost]
        public ActionResult AddCompetition(CompetitionViewModel vm)
        {
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
                        .Take(1)
                        .ToList()[0];
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
            AddUserGroupOptions();
            if (id == null) return RedirectToAction("List");
            using (var db = new ExhysContestEntities())
            {
                Competition comp;
                try
                {
                    comp = db.Competitions
                        .Where(c => c.Id == id)
                        .Take(1)
                        .ToList()[0];
                }
                catch (ArgumentOutOfRangeException) { return RedirectToAction("List"); }

                var vm = new CompetitionViewModel(comp);
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult Edit(CompetitionViewModel vm)
        {
            using (var db = new ExhysContestEntities())
            {
                var competition = db.Competitions.Where(c => c.Id == vm.Id).Take(1).ToList()[0];
                competition.Name = vm.Name;
                competition.Description = vm.Description;
                competition.UserGroup = db.UserGroups.Where(g => g.Id == vm.GroupId).Take(1).ToList()[0];
                db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = vm.Id });
        }
    }
}