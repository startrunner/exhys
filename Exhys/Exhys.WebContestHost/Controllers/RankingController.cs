using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Controllers
{
    public class RankingController : Controller
    {

        [HttpGet]
        public ActionResult List()
        {
            List<CompetitionViewModel> vm = null;
            using (var db = new ExhysContestEntities())
            {
                vm = db.Competitions
                    .ToList()
                    .Select(c => new CompetitionViewModel(c))
                    .ToList();
            }
            return PartialView(vm);
        }


        private static double FullScoreOf(ProblemSolution sol)
        {
            double rt = 0;
            if(sol!=null&&sol.TestStatuses!=null)foreach (var v in sol.TestStatuses)
            {
                rt += v.Score;
            }
            return rt;
        }

        private static ProblemSolution BestSolutionOf(Problem prob, Participation part)
        {
            var rt = part.Submissions
                .Where(s => s.Problem.Id == prob.Id)
                .Where(s=>s.Status == ProblemSolution.ExecutionStatus.Completed)
                .OrderByDescending(s=>FullScoreOf(s))
                .FirstOrDefault();
            return rt;
        }

        [HttpGet]
        public ActionResult View (int id)
        {
            RankingViewModel vm = new RankingViewModel();

            using (var db = new ExhysContestEntities())
            {
                var competition = db.Competitions
                    .Include(c => c.Problems)
                    .Include(c => c.Problems.Select(pr => pr.ProblemSolutions.Select(sol => sol.TestStatuses)))
                    .Include(c => c.Participations.Select(p => p.User))
                    .Include(c => c.Participations.Select(p => p.Submissions.Select(s => s.TestStatuses)))
                    .Include(c => c.Participations.Select(p => p.Submissions.Select(s => s.Problem)))
                    .Where(c => c.Id == id)
                    .FirstOrDefault();

                vm.CompetitionName = competition.Name;
                vm.ProblemNames = competition.Problems.Select(p => p.Name).ToArray();

                vm.Users = competition.Participations.Select(participation =>
                {
                    List<string> problemScores = new List<string>();
                    double fullScore = 0;
                    foreach (Problem problem in competition.Problems)
                    {
                        double probScore = FullScoreOf(BestSolutionOf(problem, participation));
                        fullScore += probScore;
                        problemScores.Add(probScore.ToString());
                    }
                    return new RankingUserViewModel()
                    {
                        Name = participation.User.FullName,
                        ProblemScores = problemScores.ToArray(), 
                        Score = fullScore
                    };   
                })
                .OrderByDescending(user=>user.Score)
                .ToArray();
            }

            ;

            return PartialView(vm);
        }

        
    }
}