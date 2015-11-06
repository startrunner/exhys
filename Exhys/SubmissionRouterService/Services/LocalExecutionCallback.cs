using SubmissionRouterService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubmissionRouterService.Dtos;

namespace SubmissionRouterService.Services
{
    public class LocalExecutionCallback : IExecutionCallback
    {
        public void ExecuteSubmission(Guid submissionProcessId, ExecutionDto execution)
        {
            
        }
    }
}
