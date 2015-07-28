using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class ProblemViewModel
    {
        public int Id { get; set; }

        public int CompetitionId { get; private set; }
        public string CompetitionName { get; private set; }

        public string Name { get; set; }
        public bool RequestDelete { get; set; }

        public ProblemViewModel () : this(null) { }
        public ProblemViewModel(Problem model)
        {
            this.RequestDelete = false;
            if (model != null)
            {
                this.Id = model.Id;
                this.Name = model.Name;
                if (model.CompetitionGivenAt != null)
                {
                    this.CompetitionId = model.CompetitionGivenAt.Id;
                    this.CompetitionName = model.CompetitionGivenAt.Name;
                }
            }
            else
            {

            }
        }
    }
}