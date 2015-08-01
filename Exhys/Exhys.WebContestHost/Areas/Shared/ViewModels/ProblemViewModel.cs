using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class ProblemViewModel
    {
        public int Id { get;  set; }
        public int CompetitionId { get;  set; }
        public string CompetitionName { get; set; }
        public string Name { get; set; }
        public bool RequestDelete { get; set; }
        public string InputFileExtension { get; set; }
        public string OutputFileExtension { get; set; }
        public bool IgnoreTestBlankSpace { get; set; }
        public double PointsPerTest { get; set; }
        public bool RequiresChecker { get; set; }
        public int DummyTestCount { get; set; }

        public ProblemViewModel () : this(null) { }
        public ProblemViewModel(Problem model)
        {
               
            this.RequestDelete = false;
            if (model != null)
            {
                this.Id = model.Id;
                this.Name = model.Name;
                this.IgnoreTestBlankSpace = model.IgnoreTestBlankSpaces;
                this.PointsPerTest = model.PointsPerTest;
                this.RequiresChecker = model.RequiresChecker;
                this.DummyTestCount = model.DummyTestCount;
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