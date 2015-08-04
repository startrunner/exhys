using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Participation.ViewModels
{
    public class ParticipationViewModel
    {
        public CompetitionViewModel Competition { get; set; }
        public List<ProblemViewModel> Problems { get; set; }

        public ParticipationViewModel(Competition model)
        {
            this.Competition = new CompetitionViewModel(model);
            this.Problems = new List<ProblemViewModel>();
            foreach(Problem p in model.Problems)
            {
                this.Problems.Add(new ProblemViewModel(p));
            }
        }
    }
}
