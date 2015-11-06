using SubmissionRouterService.Contracts;
using SubmissionRouterService.Dtos;
using SubmissionRouterService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService.Services
{
    public class ExecutionScheduler : IExecutionScheduler
    {
        private static ExecutionScheduler instance;
        
        public Guid RequestExecution(SubmissionDto submission)
        {
            ExecutionDto execution = CreateExecution(submission);
            OnExecutionRequested(execution);
            return execution.Id;
        }
   
        public void CompleteExecution(ExecutionResultDto executionResult)
        {
            SubmissionResultDto submissionResult = CreateSubmissionResult(executionResult);
            OnSubmissionCompleted(submissionResult);
        }

        private ExecutionDto CreateExecution(SubmissionDto submission)
        {
            return new ExecutionDto() { Id = Guid.NewGuid(), Submission = submission };
        }

        private SubmissionResultDto CreateSubmissionResult(ExecutionResultDto executionResult)
        {
            return new SubmissionResultDto() { ExecutionId=executionResult.ExecutionId, TestResults = executionResult.TestResults };
        }

        private void OnExecutionRequested(ExecutionDto execution)
        {
            if(ExecutionRequested!=null)
            {
                ExecutionRequested(this, new ExecutionRequestedEventArgs { Execution = execution });
            }
        }

        private void OnSubmissionCompleted(SubmissionResultDto submissionResult)
        {
            if (SubmissionCompleted != null)
            {
                SubmissionCompleted(this, new SubmissionCompletedEventArgs { SubmissionResult = submissionResult });
            }
        }

        public event EventHandler<ExecutionRequestedEventArgs> ExecutionRequested;

        public event EventHandler<SubmissionCompletedEventArgs> SubmissionCompleted;

        public static ExecutionScheduler Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new ExecutionScheduler();
                }
                return instance;
            }
        }
    }
}
