using Exhys.WebContestHost.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.Communication
{
    public class SubmissionResult
    {
        public bool IsSuccessful { get; set; }
        public List<SolutionTestStatus> TestResults { get; set; }
    }
}
