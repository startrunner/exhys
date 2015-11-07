using SubmissionRouterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService.Model
{
    public class ExecutionRequestedEventArgs
    {
        public ExecutionDto Execution { get; set; }
    }
}
