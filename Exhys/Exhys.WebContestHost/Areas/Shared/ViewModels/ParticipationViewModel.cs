using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class ParticipationViewModel
    {
        public const string SubmissionSourceCodeFileInputName = "submission-src";

        public CompetitionViewModel Competition { get; set; }

        public string SubmissionSourceCode { get; set; }

        public ParticipationViewModel () : this(null) { }
        public ParticipationViewModel (DataModels.Participation model)
        {
            if (model != null)
            {
                this.Competition = new CompetitionViewModel(model.Competition);
            }
        }
    }
}