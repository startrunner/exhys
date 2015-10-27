using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public enum ProblemSolutionStatus : sbyte
    {
        AwaitingGrading = 0,
        Compiling = 1,
        Grading = 2,
        Graded = 3,
        CompileError = 4,
        GradingError = 5,
        GradingCancelled = 6
    }

    public partial class ProblemSolution
    {
        public ProblemSolutionStatus Status
        {
            get { return (ProblemSolutionStatus)this.StatusCode; }
            set { this.StatusCode = (byte)value; }
        }


    }
}
