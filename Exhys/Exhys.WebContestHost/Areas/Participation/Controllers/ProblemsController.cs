using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Participation.Controllers
{
    public class ProblemsController:Controller
    {
        [HttpPost]
        public ActionResult SubmitSolution(ProblemSolutionViewModel vm)
        {
            using (var db = new ExhysContestEntities())
            {
                var problem = db.Problems
                    .Where(p => p.Id == vm.ProblemId)
                    .Take(1)
                    .ToList()[0];

                var user = Request.GetSignedInUser(db);


                
                var solution = new ProblemSolution()
                {
                    Author=user,
                    Problem = problem,
                    LanguageAlias = "c++",
                    SourceCode = vm.SourceCode
                };

                db.ProblemSolutions.Add(solution);
                db.SaveChanges();
            }

            return View(vm);
        }
    }
}