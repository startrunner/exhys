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
        private static ProblemSolutionExecutionStatusVm ConvertStatus(ProblemSolution.ExecutionStatus status)
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

        public const string FileInputName = "input-src-file";

        public string LanguageAlias { get; set; }
        public string ProblemName { get; set; }
        public int? ProblemId { get; set; }
        public string SourceCode { get; set; }
        public ProblemSolutionExecutionStatusVm Status { get; set; }

        public ProblemSolutionViewModel () : this(null) { }

        public ProblemSolutionViewModel(ProblemSolution model)
        {
            if (model != null)
            {
                this.ProblemName = model.Problem.Name;
                this.ProblemId = model.Problem.Id;
                this.SourceCode = model.SourceCode;
                this.Status = ConvertStatus(model.Status);
            }
            else
            {
                this.SourceCode = "";
            }
        }
    }
}