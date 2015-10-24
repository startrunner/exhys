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
using Exhys.WebContestHost.Areas.Shared.Mvc;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    public class ProblemsController : ExhysController
    {

        [HttpGet]
        public ActionResult Add()
        {
            AddSignedInUserInformation();
            AddCompetitionOptions();

            return View();
        }

        [HttpPost]
        public ActionResult Add(ProblemViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                AddCompetitionOptions();
                AddSignedInUserInformation();
                return View(vm);
            }

            List<HttpPostedFileBase> inputFiles, solutionFiles, statementFiles;
            #region FilesInit();
            do
            {
                inputFiles = new List<HttpPostedFileBase>();
                solutionFiles = new List<HttpPostedFileBase>();
                statementFiles = new List<HttpPostedFileBase>();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    switch (Request.Files.GetKey(i))
                    {
                        case ProblemViewModel.InputFilesInputName:
                            inputFiles.Add(Request.Files.Get(i));
                            break;
                        case ProblemViewModel.SolutionFilesInputName:
                            solutionFiles.Add(Request.Files.Get(i));
                            break;
                        case ProblemViewModel.StatementFilesInputName:
                            statementFiles.Add(Request.Files.Get(i));
                            break;
                    }
                }
                inputFiles = inputFiles.OrderBy(f => f.FileName).ToList();
                solutionFiles = solutionFiles.OrderBy(f => f.FileName).ToList();
            } while (false);
            #endregion

            List<bool> inputFeedbacks, outputFeedbacks, solutionFeedbacks, scoreFeedbacks, statusFeedbacks;
            #region FeedbacksInit();
            do
            {
                char[] sep = new char[] { ';' };
                Func<string, bool> cFunc = (s => s.Contains("1") ? true : false);

                inputFeedbacks = vm.T_InputFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);
                outputFeedbacks = vm.T_OutputFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);
                solutionFeedbacks = vm.T_SolutionFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);
                scoreFeedbacks = vm.T_ScoreFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);
                statusFeedbacks = vm.T_StatusFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);

                int min = inputFiles.Count;
                while (inputFeedbacks.Count < min) inputFeedbacks.Add(inputFeedbacks[inputFeedbacks.Count - 1]);
                while (outputFeedbacks.Count < min) outputFeedbacks.Add(outputFeedbacks[outputFeedbacks.Count - 1]);
                while (solutionFeedbacks.Count < min) solutionFeedbacks.Add(solutionFeedbacks[solutionFeedbacks.Count - 1]);
                while (scoreFeedbacks.Count < min) scoreFeedbacks.Add(scoreFeedbacks[scoreFeedbacks.Count - 1]);
                while (statusFeedbacks.Count < min) statusFeedbacks.Add(statusFeedbacks[statusFeedbacks.Count - 1]);
            } while (false);
            #endregion

            List<double> timeLimits;
            #region TimeLimitsInit();
            do
            {
                char[] sep = new char[] { ';' };
                timeLimits = vm.T_TimeLimits.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(s => double.Parse(s));
                int min = inputFiles.Count;
                while (timeLimits.Count < min) timeLimits.Add(timeLimits[timeLimits.Count - 1]);

            } while (false);
            #endregion

            if (inputFiles.Count!=solutionFiles.Count)
            {
                return RedirectToAction("Add");
            }

            using (var db = new ExhysContestEntities())
            {
                var competition = db.Competitions.Where(c => c.Id == vm.CompetitionId).FirstOrDefault();

                Problem problem = new Problem()
                {
                    CompetitionGivenAt = competition,
                    Name = vm.Name,
                    T_InputFeedbacks = vm.T_InputFeedbacks,
                    T_OutputFeedbacks = vm.T_OutputFeedbacks,
                    T_ScoreFeedbacks = vm.T_ScoreFeedbacks,
                    T_SolutionFeedbacks = vm.T_SolutionFeedbacks,
                    T_StatusFeedbacks = vm.T_StatusFeedbacks,
                    T_TimeLimits = vm.T_TimeLimits
                };

                for(int i=0;i<inputFiles.Count;i++)
                {
                    var test = new ProblemTest()
                    {
                        Input = inputFiles[i].ReadContents(),
                        Solution = solutionFiles[i].ReadContents(),
                        GroupName = "DEFAULT",
                        Points = 10,
                        InputFeedbackEnabled = inputFeedbacks[i],
                        SolutionFeedbackEnabled = solutionFeedbacks[i],
                        OutputFeedbackEnabled = outputFeedbacks[i],
                        ScoreFeedbackEnabled = scoreFeedbacks[i],
                        StatusFeedbackEnabled = statusFeedbacks[i],
                        TimeLimit = timeLimits[i]
                    };
                    problem.Tests.Add(test);
                }

                foreach(var sFile in statementFiles)
                {
                    var statement = new ProblemStatement()
                    {
                        Bytes = sFile.InputStream.ToArray(),
                        Filename = sFile.FileName
                    };
                    problem.ProblemStatements.Add(statement);
                }

                db.Problems.Add(problem);
                db.SaveChanges();

            }
            
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            using (var db = new ExhysContestEntities())
            {
                AddSignedInUserInformation();
                AddCompetitionOptions();

                var vm = new List<ProblemViewModel>();

                var problems = db.Problems.ToList();
                foreach (var p in problems) vm.Add(new ProblemViewModel(p));

                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult List (List<ProblemViewModel> vm)
        {
            using (var db = new ExhysContestEntities())
            {
                foreach (var pvm in vm)
                {
                    Problem problem = db.Problems
                        .Where(p => p.Id == pvm.Id)
                        .FirstOrDefault();
                    if (problem == null) continue;

                    if (!pvm.RequestDelete)
                    {
                        Competition competition = db.Competitions
                            .Where(c => c.Id == pvm.CompetitionId)
                            .FirstOrDefault();
                        if (competition == null) continue;

                        problem.Name = pvm.Name;
                        problem.CompetitionGivenAt = competition;
                    }
                    else problem.CascadeFrom(db);
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
        }
    }
}