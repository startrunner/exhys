using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{

    public class RankViewModel
    {
        public int From { get; set; }
        public int To { get; set; }

        public override string ToString ()
        {
            if (From != To) return $"{From} - {To}";
            else return From.ToString();
        }
    }

    public class RankingUserViewModel
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public string[] ProblemScores { get; set; }
        public RankViewModel Rank;
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