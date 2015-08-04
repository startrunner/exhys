using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class CompetitionViewModel
    {
        private string _description;

        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string GroupIdStr
        {
            get { return GroupId != null ? GroupId.ToString() : ""; }
            set { GroupId = int.Parse(value); }
        }

        public string Name { get; set; }
        public string Description
        {
            get { return _description; }
            set { _description = value!=null?value.Trim():null; }
        }
        public bool RequestDelete { get; set; }
        public List<ProblemViewModel> Problems { get; set; }

        public bool RequestAddNewProblem { get; set; }
        public ProblemViewModel ProblemToBeAdded { get; set; }


        public CompetitionViewModel () : this(null) { }
        public CompetitionViewModel (Competition model)
        {
            this.RequestDelete = false;

            this.Problems = new List<ProblemViewModel>();

            this.RequestAddNewProblem = false;
            this.ProblemToBeAdded = new ProblemViewModel();


            if (model != null)
            {
                this.Name = model.Name;
                this.Description = model.Description;
                this.Id = model.Id;
                if (model.UserGroup != null) this.GroupId = model.UserGroup.Id;
               

                foreach(var v in model.Problems)
                {
                    this.Problems.Add(new ProblemViewModel(v));
                }
            }
        }
    }
}