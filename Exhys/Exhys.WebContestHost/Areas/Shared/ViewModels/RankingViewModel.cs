using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class RankingUserViewModel
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public string[] ProblemScores { get; set; }
        public RankingUserViewModel ()
        {
            ProblemScores = new string[0];
        }
    }

    public class RankingViewModel
    {
        public string CompetitionName { get; set; }
        public string[] ProblemNames { get; set; }
        public RankingUserViewModel[] Users { get; set; }

        public RankingViewModel()
        {
            ProblemNames = new string[0];
            Users = new RankingUserViewModel[0];
        }
    }
}