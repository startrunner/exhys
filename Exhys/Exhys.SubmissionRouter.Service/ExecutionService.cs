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
using System.Diagnostics;
using Exhys.ExecutionCore;

namespace Exhys.SubmissionRouter.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ExecutionService : IExecutionService, ISubmissionService
    {
        private Dictionary<Guid, Executioner> executioners;
        private Dictionary<Guid, ISubmissionCallback> callbacks;
        private Queue<ExecutionDto> requestedExecutions;
        private IExecutionCallback localExecutionCallback;
        private Guid localExecutionerId;
        private Timer executionTimer;
        private object _lock;

        public ExecutionService()
        {
            _lock = new object();
            executioners = new Dictionary<Guid, Executioner>();
            callbacks = new Dictionary<Guid, ISubmissionCallback>();
            requestedExecutions = new Queue<ExecutionDto>();
            localExecutionCallback = new LocalExecutionCallback(GetExecutionCore(), this);
            executionTimer = new Timer(1000);
            executionTimer.Elapsed += (s, e) => ExecuteNextRequest();

            RegisterLocalExecutioner();
        }

        private IExecutionCore GetExecutionCore()
        {
            return ExecutionCoreFactory.Generate();
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
            Executioner executioner = new Executioner(executionCallback);
            executioner.ExceptionOccured += (s, e) => OnExecutionerExceptionOccured(e.ExecutionProcessId);
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
            if (localExecutionerId != id && executioners.Count==0)
            {
                RegisterLocalExecutioner();
            }
        }

        public void OnExecutionerExceptionOccured(Guid executionId)
        {
            ExecutionResultDto executionResult = new ExecutionResultDto();
            executionResult.ExecutionId = executionId;
            executionResult.IsExecutionSuccessful = false;
            SubmitResult(executionResult);
        }

        public void SubmitResult(ExecutionResultDto executionResult)
        {
            Executioner executioner = executioners.FirstOrDefault(x => x.Value.CurrentExecutionId == executionResult.ExecutionId).Value;
            if (executioner != null)
            {
                executioner.OnExecutionFinished();
            }
            OnExecutionCompleted(executionResult);
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
            executioner.Execute(execution);
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
            callbacks.Add(execution.Id, callback);
            
            return execution.Id;
        }

        private void OnExecutionCompleted(ExecutionResultDto executionResult)
        {
            SubmissionResultDto submissionResult = new SubmissionResultDto()
            {
                ExecutionId = executionResult.ExecutionId,
                TestResults = executionResult.TestResults
            };

            ISubmissionCallback callback = callbacks[submissionResult.ExecutionId];
            try
            {
                callback.SubmissionProcessed(submissionResult);
            }
            catch
            {
                Debug.WriteLine("Unable to notify client of completed execution!");
            }
        }

        public void Ping()
        {
            var callback = OperationContext.Current.GetCallbackChannel<ISubmissionCallback>();
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);
                callback.Pong();
            });
        }

        #endregion
    }
}
