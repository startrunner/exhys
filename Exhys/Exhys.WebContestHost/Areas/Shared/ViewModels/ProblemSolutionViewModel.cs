using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public enum ProblemSolutionExecutionStatusVm
    {
        Pending,
        Completed,
        InProgress
    }
    public class ProblemSolutionViewModel
    {
        public const string FileInputName = "input-src-file";

        public string LanguageAlias { get; set; }
        public string ProblemName { get; set; }
        public int? ProblemId { get; set; }
        public string SourceCode { get; set; }
        public List<SolutionTestStatusViewModel> TestStatuses { get; set; }
        public ProblemSolutionExecutionStatusVm Status { get; set; }

        public ProblemSolutionViewModel () : this(null) { }

        public ProblemSolutionViewModel(ProblemSolution model)
        {
            this.TestStatuses = new List<SolutionTestStatusViewModel>();

            if (model != null)
            {
                if (model.Problem != null)
                {
                    this.ProblemName = model.Problem.Name;
                    this.ProblemId = model.Problem.Id;
                }

                if(model.TestStatuses!=null)
                {
                    foreach(var v in model.TestStatuses)
                    {
                        TestStatuses.Add(new SolutionTestStatusViewModel(v));
                    }
                }
                this.SourceCode = model.SourceCode;
                this.Status = ConvertStatus(model.Status);
            }
            else
            {
                this.SourceCode = "";
            }
        }


        private static ProblemSolutionExecutionStatusVm ConvertStatus (ProblemSolution.ExecutionStatus status)
        {
            switch (status)
            {
                case ProblemSolution.ExecutionStatus.Pending:
                    return ProblemSolutionExecutionStatusVm.Pending;
                case ProblemSolution.ExecutionStatus.InProgress:
                    return ProblemSolutionExecutionStatusVm.InProgress;
                case ProblemSolution.ExecutionStatus.Completed:
                    return ProblemSolutionExecutionStatusVm.Completed;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}