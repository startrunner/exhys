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
using System.Data.Entity;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    [AuthorizeExhysAdmin]
    public class ProblemsController : ExhysController
    {

        [HttpGet]
        public ActionResult Add()
        {
            AddSignedInUserInformation();
            AddCompetitionOptions();

            return View();
        }

        #region Add_UtilityFunctions
        private void Add_FetchPostFiles(ref List<HttpPostedFileBase> inputFiles, ref List<HttpPostedFileBase> solutionFiles, ref List<HttpPostedFileBase> statementFiles)
        {
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
        }

        private void Add_ParseFeedbackArguments (ProblemViewModel vm, int testCount, ref List<bool> inputFeedbacks, ref List<bool> outputFeedbacks, ref List<bool> solutionFeedbacks, ref List<bool> scoreFeedbacks, ref List<bool> statusFeedbacks)
        {
            char[] sep = new char[] { ';' };
            Func<string, bool> cFunc = (s => s.Contains("1") ? true : false);

            inputFeedbacks = vm.T_InputFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);
            outputFeedbacks = vm.T_OutputFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);
            solutionFeedbacks = vm.T_SolutionFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);
            scoreFeedbacks = vm.T_ScoreFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);
            statusFeedbacks = vm.T_StatusFeedbacks.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(cFunc);

            inputFeedbacks.Resize(testCount, inputFeedbacks.LastByIndex());
            outputFeedbacks.Resize(testCount, outputFeedbacks.LastByIndex());
            solutionFeedbacks.Resize(testCount, solutionFeedbacks.LastByIndex());
            scoreFeedbacks.Resize(testCount, scoreFeedbacks.LastByIndex());
            statusFeedbacks.Resize(testCount, statusFeedbacks.LastByIndex());
        }
        
        private void Add_ParseTimeLimitArguments(ProblemViewModel vm, int testCount, ref List<double> timeLimits)
        {
            char[] sep = new char[] { ';' };
            timeLimits = vm.T_TimeLimits.Split(sep, StringSplitOptions.RemoveEmptyEntries).Convert(s => double.Parse(s));
            timeLimits.Resize(testCount, timeLimits.LastByIndex());
        }
        #endregion

        [HttpPost]
        public ActionResult Add(ProblemViewModel vm)
        {//bobi  e kurva
            if(!vm.Validate(ViewData))
            {
                AddCompetitionOptions();
                AddSignedInUserInformation();
                return View(vm);
            }

            List<HttpPostedFileBase>
                inputFiles = new List<HttpPostedFileBase>(),
                solutionFiles = new List<HttpPostedFileBase>(),
                statementFiles = new List<HttpPostedFileBase>();
            Add_FetchPostFiles(ref inputFiles, ref solutionFiles, ref statementFiles);

            List<bool>
                inputFeedbacks = new List<bool>(),
                outputFeedbacks = new List<bool>(),
                solutionFeedbacks = new List<bool>(),
                scoreFeedbacks = new List<bool>(),
                statusFeedbacks = new List<bool>();
            Add_ParseFeedbackArguments(vm, inputFiles.Count, ref inputFeedbacks, ref outputFeedbacks, ref solutionFeedbacks, ref scoreFeedbacks, ref statusFeedbacks);

            List<double> timeLimits=new List<double>();
            Add_ParseTimeLimitArguments(vm, inputFiles.Count, ref timeLimits);

            if (inputFiles.Count!=solutionFiles.Count)
            {
                ModelState.AddModelError(FormErrors.FileCountMismatch);
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
                problem = db.Problems.Add(problem);
                db.SaveChanges();
                db.Entry(problem).Collection(p => p.Tests).Load();
                db.Entry(problem).Collection(p => p.ProblemStatements).Load();
                

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

                db.SaveChanges();

            }
            
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            AddSignedInUserInformation();
            AddCompetitionOptions();

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
                        .Include(p=>p.CompetitionGivenAt)
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
                    else db.Problems.Remove(problem);
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
        }
    }
}