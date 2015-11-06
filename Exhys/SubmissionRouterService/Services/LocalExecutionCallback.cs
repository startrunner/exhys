using SubmissionRouterService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubmissionRouterService.Dtos;
using Exhys.ExecutionCore;

namespace SubmissionRouterService.Services
{
    public class LocalExecutionCallback : IExecutionCallback
    {
        IExecutionCore executionCore;
        IExecutionService executionService;
        public LocalExecutionCallback(IExecutionCore executionCore, IExecutionService executionService)
        {
            this.executionCore = executionCore;
            this.executionService = executionService;
        }
        public void ExecuteSubmission(Guid submissionProcessId, ExecutionDto execution)
        {
            ExecutionResultDto result = executionCore.Execute(execution);
            executionService.SubmitResult(submissionProcessId, result);
        }
    }
}
