using Exhys.SubmissionRouter.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore;
using Exhys.ExecutionCore.Contracts;
using Exhys.SubmissionRouter.Dtos;
using System.ServiceModel;

namespace Exhys.SubmissionRouter.Service
{
    [CallbackBehavior(UseSynchronizationContext = true)]
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
