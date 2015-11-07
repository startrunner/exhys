using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class ProblemStatementViewModel
    {
        [Required]
        public string Filename { get; set; }
        public int Id { get; set; }

        public ProblemStatementViewModel () : this(null) { }
        public ProblemStatementViewModel(ProblemStatement model)
        {
            if(model!=null)
            {
                this.Filename = model.Filename;
                this.Id = model.Id;
            }
        }
    }
}