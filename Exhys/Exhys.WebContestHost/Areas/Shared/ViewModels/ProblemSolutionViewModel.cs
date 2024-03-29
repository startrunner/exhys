﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class ProblemSolutionViewModel
    {
        public string ProblemName { get; set; }
        public int? ProblemId { get; set; }
        public string SourceCode { get; set; }

        public ProblemSolutionViewModel () : this(null) { }

        public ProblemSolutionViewModel(ProblemSolution model)
        {
            if (model != null)
            {
                this.ProblemName = model.Problem.Name;
                this.ProblemId = model.Problem.Id;
                this.SourceCode = model.SourceCode;
            }
            else
            {
                this.SourceCode = "";
            }
        }
    }
}