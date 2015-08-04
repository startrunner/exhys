using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.Areas.Shared.Attributes;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class CompetitionViewModel
    {
        private string _description;

        public int Id { get;  set; }
        public int? GroupId { get; set; }

        public string Name { get; set; }

        public string Description
        {
            get { return _description; }
            set { _description = value!=null?value.Trim():null; }
        }

        public bool RequestDelete { get; set; }

        public CompetitionViewModel () : this(null) { }
        public CompetitionViewModel (Competition model)
        {
            this.RequestDelete = false;

            if (model != null)
            {
                this.Name = model.Name;
                this.Description = model.Description;
                this.Id = model.Id;
                if (model.UserGroup != null) this.GroupId = model.UserGroup.Id;
            }
        }
    }
}