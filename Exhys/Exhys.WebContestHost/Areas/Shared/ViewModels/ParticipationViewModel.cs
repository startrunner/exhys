using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class ParticipationViewModel
    {
        public CompetitionViewModel Competition { get; set; }
        public List<ProblemSolutionViewModel> ProblemSubmissions { get; set; }
        public UserAccountViewModel User { get; set; }

        public ParticipationViewModel () : this(null) { }
        public int Id { get; set; }
        public ParticipationViewModel (DataModels.Participation model)
        {
            if (model != null)
            {
                this.Competition = new CompetitionViewModel(model.Competition);
                this.Id = model.Id;
                if(model.Submissions!=null)
                {
                    ProblemSubmissions = new List<ProblemSolutionViewModel>();
                    foreach(var v in model.Submissions)
                    {
                        ProblemSubmissions.Add(new ProblemSolutionViewModel(v));
                    }
                }
                if(model.User!=null)
                {
                    this.User = new UserAccountViewModel(model.User);
                }
            }
        }
    }
}