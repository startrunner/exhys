using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;
using Exhys.WebContestHost.Areas.Shared;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Exhys.WebContestHost.Areas.Shared.Extensions;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    public class ProblemsController : ExhysMvcController
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
            AddCompetitionOptions();

            using (var db = new ExhysContestEntities())
            {
                var vm = new List<ProblemViewModel>();
                db.Problems.ToList()
                    .ForEach((p) => { vm.Add(new ProblemViewModel(p)); });
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult List (IList<ProblemViewModel> vm)
        {
            using (var db = new ExhysContestEntities())
            {
                foreach (var v in vm)
                {
                    var model = db.Problems.Where(p => p.Id == v.Id).Take(1).ToList()[0];
                    if (v.RequestDelete == false)
                    {
                        model.Name = v.Name;
                        model.IgnoreTestBlankSpaces = v.IgnoreTestBlankSpace;
                        var comp = db.Competitions.Where(c => c.Id == v.CompetitionId).Take(1).ToList()[0];
                        model.CompetitionGivenAt = comp;
                        model.PointsPerTest = v.PointsPerTest;
                        model.DummyTestCount = v.DummyTestCount;
                        model.RequiresChecker = v.RequiresChecker;
                    }
                    else
                    {
                        model.DeleteFrom(db);
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult AddProblem()
        {
            AddCompetitionOptions();

            return View();
        }

        [HttpPost]
        public ActionResult AddProblem(ProblemViewModel vm)
        {
            using (var db = new ExhysContestEntities())
            {
                var competition = db.Competitions.Where(c => c.Id == vm.CompetitionId).Take(1).ToList()[0];

                List<HttpPostedFileBase> files=new List<HttpPostedFileBase>();
                for (int i = 0; i < Request.Files.Count; i++) files.Add(Request.Files.Get(i));

                var inputFiles = files
                    .Where(f => f.FileName.EndsWith(vm.InputFileExtension))
                    .OrderBy(f=>f.FileName)
                    .ToList();
                var outputFiles = files
                    .Where(f => f.FileName.EndsWith(vm.OutputFileExtension))
                    .OrderBy(f=>f.FileName)
                    .ToList() ;

                var tests = new LinkedList<ProblemTest>();

                if(inputFiles.Count==outputFiles.Count)
                {
                    int count = inputFiles.Count;
                    string root = Path.GetTempPath();
                    for(int i=0;i< count;i++)
                    {
                        string inp = inputFiles[i].ReadContents();
                        string outp = outputFiles[i].ReadContents();

                        var test = new ProblemTest()
                        {
                            GroupName = "",
                            Input = inp,
                            Output = outp
                        };
                        tests.AddLast(test);
                    }
                }

                var problem = new Problem()
                {
                    Name = vm.Name,
                    CompetitionGivenAt = competition,
                    IgnoreTestBlankSpaces = vm.IgnoreTestBlankSpace,
                    ProblemTests = tests,
                    DummyTestCount = vm.DummyTestCount,
                    PointsPerTest = vm.PointsPerTest,
                    RequiresChecker = vm.RequiresChecker
                };

                db.Problems.Add(problem);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}