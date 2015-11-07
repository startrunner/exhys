using Exhys.SubmissionRouter.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Exhys.SubmissionRouter.Dtos;
using Exhys.SubmissionRouter.Service.Model;
using System.Timers;
using Exhys.ExecutionCore.Contracts;

namespace Exhys.SubmissionRouter.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ExecutionService : IExecutionService, ISubmissionService
    {
        private event EventHandler ExecutionAdded;

        private Dictionary<Guid, Executioner> executioners;
        private Dictionary<Guid, ExecutionProcess> executionProcesses;
        private Dictionary<Guid, ISubmissionCallback> submissions;
        private Queue<ExecutionDto> requestedExecutions;
        private IExecutionCallback localExecutionCallback;
        private Guid localExecutionerId;
        private Timer executionTimer;
        private object _lock;

        public ExecutionService()
        {
            _lock = new object();
            executioners = new Dictionary<Guid, Executioner>();
            executionProcesses = new Dictionary<Guid, ExecutionProcess>();
            submissions = new Dictionary<Guid, ISubmissionCallback>();
            requestedExecutions = new Queue<ExecutionDto>();
            localExecutionCallback = new LocalExecutionCallback(GetExecutionCore(), this);
            executionTimer = new Timer(1000);
            executionTimer.Elapsed += (s, e) => ExecuteNextRequest();

            RegisterLocalExecutioner();
        }

        private IExecutionCore GetExecutionCore()
        {
            return null;
        }

        #region IExecutionService implementation

        private void RegisterLocalExecutioner()
        {
            localExecutionerId = Register(localExecutionCallback);
        }

        private void UnregisterLocalExecutioner()
        {
            Unregister(localExecutionerId);
        }

        public Guid Register()
        {
            return Register(OperationContext.Current.GetCallbackChannel<IExecutionCallback>());
        }

        public Guid Register(IExecutionCallback executionCallback)
        {
            IExecutionCallback callback = executionCallback;
            Executioner executioner = new Executioner(callback);
            Guid id = Guid.NewGuid();
            executioners.Add(id, executioner);
            if (executionCallback != localExecutionCallback)
            {
                UnregisterLocalExecutioner();
            }
            return id;
        }

        public void Unregister(Guid id)
        {
            executioners.Remove(id);
            if (localExecutionerId != id)
            {
                RegisterLocalExecutioner();
            }
        }

        public void SubmitResult(Guid executionProcessId, ExecutionResultDto executionResult)
        {
            executionProcesses[executionProcessId].Executioner.OnExecutionFinished();
            executionProcesses.Remove(executionProcessId);
            ExecutionCompleted(executionResult);
        }

        private void RequestExecution(ExecutionDto execution)
        {
            requestedExecutions.Enqueue(execution);
            executionTimer.Start();
        }

        private void ExecuteNextRequest()
        {
            lock (_lock)
            {
                ExecutionDto execution = requestedExecutions.Dequeue();
                if (requestedExecutions.Count == 0)
                {
                    executionTimer.Stop();
                }
                Executioner executioner = GetFreeExecutioner();

                if (executioner != null)
                {
                    ExecuteRequest(execution, executioner);
                }
            }
        }

        private void ExecuteRequest(ExecutionDto execution, Executioner executioner)
        {
            Guid executionProcessId = Guid.NewGuid();
            ExecutionProcess executionProcess = executioner.Execute(executionProcessId, execution);
            executionProcesses.Add(executionProcessId, executionProcess);
        }

        private Executioner GetFreeExecutioner()
        {
            return executioners.FirstOrDefault(x => !x.Value.IsBusy).Value;
        }

        #endregion

        #region ISubmissionService

        public Guid Submit(SubmissionDto submission)
        {
            return Submit(submission, OperationContext.Current.GetCallbackChannel<ISubmissionCallback>());
        }

        public Guid Submit(SubmissionDto submission, ISubmissionCallback callback)
        {
            ExecutionDto execution = new ExecutionDto()
            {
                Id = Guid.NewGuid(),
                Submission = submission
            };
            RequestExecution(execution);
            submissions.Add(execution.Id, callback);
            return execution.Id;
        }

        private void ExecutionCompleted(ExecutionResultDto executionResult)
        {
            if (submissions.ContainsKey(executionResult.ExecutionId))
            {
                SubmissionResultDto submissionResult = new SubmissionResultDto()
                {
                    ExecutionId = executionResult.ExecutionId,
                    TestResults = executionResult.TestResults
                };

                ISubmissionCallback callback = submissions[submissionResult.ExecutionId];
                submissions.Remove(submissionResult.ExecutionId);
                callback.SubmissionProcessed(submissionResult);
            }
        }

        #endregion
    }
}
