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
using System.Runtime.Serialization;
using Newtonsoft.Json;

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
        #endregion

        [DataContract]
        class Add_FeedbackParameters
        {
            [DataMember(Name = "input-feedback")]
            public bool[] InputFeedback { get; set; }

            [DataMember(Name = "output-feedback")]
            public bool[] OutputFeedback { get; set; }

            [DataMember(Name = "solution-feedback")]
            public bool[] SolutionFeedback { get; set; }

            [DataMember(Name = "status-feedback")]
            public bool[] StatusFeedback { get; set; }

            [DataMember(Name = "score-feedback")]
            public bool[] ScoreFeedback { get; set; }
            public Add_FeedbackParameters () { }

            [DataMember(Name ="time-limits")]
            public double[] TimeLimits { get; set; }

            [DataMember(Name ="test-scores")]
            public double[] TestScores { get; set; }
        }

        [HttpPost]
        public ActionResult Add(ProblemViewModel vm)
        {
            string parametersJson = Request.Form["parameters"];
            Add_FeedbackParameters feedbackParameters = JsonConvert.DeserializeObject<Add_FeedbackParameters>(Request.Form["parameters"]);
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

            //Add_ParseTimeLimitArguments(vm, inputFiles.Count, ref timeLimits);

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
                    Name = vm.Name
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
                        Points = feedbackParameters.TestScores[i],
                        InputFeedbackEnabled = feedbackParameters.InputFeedback[i],
                        SolutionFeedbackEnabled = feedbackParameters.SolutionFeedback[i],
                        OutputFeedbackEnabled = feedbackParameters.OutputFeedback[i],
                        ScoreFeedbackEnabled = feedbackParameters.ScoreFeedback[i],
                        StatusFeedbackEnabled = feedbackParameters.StatusFeedback[i],
                        TimeLimit = feedbackParameters.TimeLimits[i]
                    };
                    problem.Tests.Add(test);
                }

                foreach(var sFile in statementFiles)
                {
                    if (sFile.ContentLength == 0 || sFile.FileName == null || sFile.FileName == "") continue;
                    var statement = new ProblemStatement()
                    {
                        Bytes = sFile.InputStream.ToArray(),
                        Filename = sFile.FileName
                    };
                    problem.ProblemStatements.Add(statement);
                }

                ;

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

                var problems = db.Problems
                    .Include(prob=>prob.CompetitionGivenAt)
                    .ToList();
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

                    ;

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

                    ;
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
        }
    }
}