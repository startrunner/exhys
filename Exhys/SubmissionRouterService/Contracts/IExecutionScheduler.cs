using SubmissionRouterDTOs;
using SubmissionRouterService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService.Contracts
{
    public interface IExecutionScheduler
    {
        Guid RequestExecution(SubmissionDto submission);
        void CompleteExecution(ExecutionResultDto executionResult);

        event EventHandler<ExecutionRequestedEventArgs> ExecutionRequested;

        event EventHandler<SubmissionCompletedEventArgs> SubmissionCompleted;
    }
}
