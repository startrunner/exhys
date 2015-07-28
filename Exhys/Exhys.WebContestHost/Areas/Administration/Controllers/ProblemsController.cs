using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    public class ProblemsController : Controller
    {
        [HttpGet]
        public ActionResult Edit (int? id)
        {
            if (id == null) return RedirectToAction(controllerName: "Competitions", actionName: "List");

            using(var db=new ExhysContestEntities())
            {
                var problem = db.Problems.Where(p => p.Id == id)
                    .Take(1)
                    .ToList()[0];
                var vm = new ProblemViewModel(problem);
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProblemViewModel vm)
        {
            using (var db = new ExhysContestEntities())
            {
                var problem = db.Problems
                    .Where(p => p.Id == vm.Id)
                    .Take(1)
                    .ToList()[0];
                problem.Name = vm.Name;
                db.SaveChanges();
            }

            return RedirectToAction(actionName: "Edit", routeValues: new { id = vm.Id });
        }

        [HttpGet]
        public ActionResult List()
        {
            using (var db = new ExhysContestEntities())
            {
                var vm = new List<ProblemViewModel>();
                db.Problems.ToList()
                    .ForEach((p) => { vm.Add(new ProblemViewModel(p)); });
                return View(vm);
            }
            //return View();
        }

        [HttpPost]
        public ActionResult List(IList<ProblemViewModel> vm)
        {
            return View();
        }
    }
}