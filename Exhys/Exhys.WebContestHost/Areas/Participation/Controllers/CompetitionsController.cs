using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;
using System.Data.Entity;
using Exhys.WebContestHost.Areas.Shared;
using System.Threading.Tasks;
using Exhys.WebContestHost.Communication;

namespace Exhys.WebContestHost.Areas.Participation.Controllers
{
    [AuthorizeExhysUser]
    public class CompetitionsController : ExhysController
    {
        /// <summary>
        /// A user should not participate in any competition if they are not signed in
        /// </summary>
        /// <returns></returns>
        private ActionResult RedirectToSignIn ()
        {
            TempData.SetFormErrors(
                new List<FormErrors.FormError>
                {
                    FormErrors.SignInRequired("participate in a competition")
                });
            return RedirectToAction(controllerName: "../Accounts", actionName: "SignIn", routeValues: new { });
        }

        private ActionResult RedirectToList ()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List ()
        {
            var vm = new List<CompetitionViewModel>();

            using (var db = new ExhysContestEntities())
            {
                var user = Request.GetSignedInUserQuery(db).FirstOrDefault();
                if (user == null) return RedirectToSignIn();

                db.Entry(user)
                    .Reference(u => u.UserGroup)
                    .Query()
                    .Include(u => u.AvaiableCompetition).Load();

                var participations = db.Participations
                    .Where(p => p.User.Id == user.Id)
                    .Include(p => p.Competition)
                    .ToList();

                var competitions = user.UserGroup.AvaiableCompetition.ToList();
                foreach (var comp in competitions)
                {
                    var compVm = new CompetitionViewModel(comp);

                    if (participations.Where(p => p.Competition.Id == comp.Id).Count() != 0)
                    {
                        compVm.IsUserParticipating = true;
                    }

                    vm.Add(compVm);
                }
            }

            return View(vm);
        }

        [HttpGet]
        public ActionResult Join (int id)
        {
            using (var db = new ExhysContestEntities())
            {
                var competition = db.Competitions.Where(c => c.Id == id).FirstOrDefault();
                if (competition == null)
                {
                    return RedirectToList();
                }
                else
                {
                    return View(new CompetitionViewModel(competition));
                }
            }
        }

        [HttpPost]
        public ActionResult Join (CompetitionViewModel vm)
        {
            using (var db = new ExhysContestEntities())
            {
                var competition = db.Competitions.Where(c => c.Id == vm.Id).FirstOrDefault();
                if (competition == null) return RedirectToAction("List");

                var user = Request.GetSignedInUserQuery(db).FirstOrDefault();
                if (user == null) return RedirectToSignIn();

                var participation = db.Participations
                    .Where(p => p.User.Id == user.Id && p.Competition.Id == competition.Id)
                    .FirstOrDefault();
                if (participation != null) return RedirectToAction("Participate", new { id = vm.Id });

                participation = new DataModels.Participation();
                db.Entry(participation).State = EntityState.Added;

                participation.Competition = competition;
                participation.User = user;

                db.SaveChanges();

                return RedirectToAction("Participate", new { id = competition.Id });

            }
        }

        [HttpGet]
        public ActionResult Participate (int? id)
        {
            if (id == null)
            {
                id = Request.GetCurrentCompetitionCookie();
                if (id == null)
                {
                    return RedirectToAction("List");
                }
            }
            else
            {
                Response.SetCurrentCompetitionCookie(id.Value);
            }

            using (var db = new ExhysContestEntities())
            {
                AddProgrammingLanguageOptions(db);
                AddProblemOptions(db, id.Value);

                var competition = db.Competitions.Where(c => c.Id == id)

                    .FirstOrDefault();
                if (competition == null) return RedirectToList();

                var user = Request.GetSignedInUserQuery(db).FirstOrDefault();
                if (user == null) return RedirectToSignIn();

                var participation = db.Participations
                    .Where(p => p.User.Id == user.Id && p.Competition.Id == competition.Id)
                    .Include(p => p.Competition.Problems.Select(prob => prob.ProblemStatements))
                    .Include(p => p.Submissions)
                    .FirstOrDefault();
                if (participation == null) return RedirectToList();

                var vm = new ParticipationViewModel(participation);
                vm.Competition.IncludeProblems().IncludeProblemStatements();

                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult DownloadStatement (int id)
        {
            using (var db = new ExhysContestEntities())
            {
                ProblemStatement statement = db.ProblemStatements
                    .Where(s => s.Id == id)
                    .FirstOrDefault();

                return File(fileContents: statement.Bytes, fileDownloadName: statement.Filename, contentType: "application/zip");
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SubmitSolution (ProblemSolutionViewModel vm)
        {
            using (var db = new ExhysContestEntities())
            {
                var problem = db.Problems
                    .Where(p => p.Id == vm.ProblemId)
                    .Include(p => p.ProblemSolutions)
                    .Include(p => p.CompetitionGivenAt)
                    .FirstOrDefault();
                var competition = db.Competitions
                    .Where(c => c.Id == problem.CompetitionGivenAt.Id)
                    .Include(c => c.Participations)
                    .FirstOrDefault();
                var user = Request
                    .GetSignedInUserQuery(db)
                    .Include(u => u.Participations)
                    .FirstOrDefault();
                var participation = db.Participations
                    .Where(p => p.User.Id == user.Id && p.Competition.Id == competition.Id)
                    .FirstOrDefault();

                var solution = new ProblemSolution()
                {
                    LanguageAlias = vm.LanguageAlias,
                    Participation = participation,
                    Problem = problem,
                    Status = ProblemSolution.ExecutionStatus.Pending,
                    SourceCode = vm.SourceCode,
                    Message = ""
                };

                db.ProblemSolutions.Add(solution);
                db.SaveChanges();

                int solutionId = db.Entry(solution).Entity.Id;

                Task.Run(() =>
                {
                    HandleSolution(solutionId);
                })

                .ContinueWith((x) =>
                {
                    throw new Exception();
                }, TaskContinuationOptions.OnlyOnFaulted);

                return RedirectToAction("Participate");
            }
        } 

        private void HandleSolution (int solutionId)
        {
            ProblemSolution solution = null;
            using (var db = new ExhysContestEntities())
            {
                solution = db.ProblemSolutions
                    .Where(sol => sol.Id == solutionId)
                    .Include(sol => sol.Problem)
                    .Include(sol => sol.Problem.Tests)
                    .Include(sol => sol.Participation)
                    .Include(sol => sol.Participation.Competition)
                    .Include(sol => sol.Participation.User)
                    .AsNoTracking()
                    .FirstOrDefault();
                solution.Status = ProblemSolution.ExecutionStatus.InProgress;
                db.SaveChanges();

            }

            SubmissionClient client = new SubmissionClient();
            client.SubmitRequestAsync(solution).ContinueWith((x) =>
            {
                List<SolutionTestStatus> result = null;
                result = x.Result;

                using (var db = new ExhysContestEntities())
                {
                    solution = db.ProblemSolutions
                        .Where(sol => sol.Id == solutionId)
                        .Include(sol => sol.TestStatuses)
                        .Include(sol => sol.Problem)
                        .Include(sol => sol.Participation)
                        .FirstOrDefault();

                    solution.TestStatuses.Clear();
                    foreach (var v in result)
                    {
                        SolutionTestStatus status = new SolutionTestStatus
                        {
                            Output = v.Output,
                            ProblemTest = db.ProblemTests.Where(y=>y.Id==v.ProblemTest.Id).FirstOrDefault(),
                            Score = v.Score,
                            Status = v.Status
                        };
                        solution.TestStatuses.Add(status);
                    }
                    solution.Status = ProblemSolution.ExecutionStatus.Completed;
                    db.SaveChanges();
                }
            });
            
        }
    }
}