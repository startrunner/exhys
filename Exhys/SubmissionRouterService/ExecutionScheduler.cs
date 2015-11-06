using SubmissionRouterService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService
{
    public class ExecutionScheduler
    {
        private static ExecutionScheduler instance;

        private ExecutionScheduler()
        {

        }
        
        public Guid RequestExecution(SubmissionDto submission)
        {
            ExecutionDto execution = CreateExecution(submission);
            OnExecutionRequested(execution);
            return execution.Id;
        }

        private ExecutionDto CreateExecution(SubmissionDto submission)
        {
            return new ExecutionDto() { Id = Guid.NewGuid(), Submission = submission };
        }

        public void CompleteExecution(ExecutionResultDto executionResult)
        {
            OnExecutionCompleted(executionResult);
        }

        private void OnExecutionRequested(ExecutionDto execution)
        {
            if(ExecutionRequested!=null)
            {
                ExecutionRequested(this, new ExecutionRequestedEventHandler { Execution = execution });
            }
        }

        private void OnExecutionCompleted(ExecutionResultDto executionResult)
        {
            if (ExecutionCompleted != null)
            {
                ExecutionCompleted(this, new ExecutionCompletedEventHandler {ExecutionResult = executionResult });
            }
        }

        public event EventHandler<ExecutionRequestedEventHandler> ExecutionRequested;

        public event EventHandler<ExecutionCompletedEventHandler> ExecutionCompleted;

        public class ExecutionRequestedEventHandler
        {
            public ExecutionDto Execution { get; set; }
        }

        public class ExecutionCompletedEventHandler
        {
            public ExecutionResultDto ExecutionResult { get; set; }
        }

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
