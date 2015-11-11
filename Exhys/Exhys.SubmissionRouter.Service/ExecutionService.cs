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
        private List<Executioner> executioners;
        private Dictionary<Guid, ISubmissionCallback> callbacks;
        private Queue<ExecutionDto> requestedExecutions;
        private IExecutionCallback localExecutionCallback;
        private Guid localExecutionerId;
        private object _lock;

        public ExecutionService()
        {
            _lock = new object();
            executioners = new List<Executioner>();
            callbacks = new Dictionary<Guid, ISubmissionCallback>();
            requestedExecutions = new Queue<ExecutionDto>();
            localExecutionCallback = new LocalExecutionCallback(GetExecutionCore(), this);

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
            executioner.Guid = Guid.NewGuid();
            executioners.Add(executioner);
            if (executionCallback != localExecutionCallback)
            {
                UnregisterLocalExecutioner();
            }
            return executioner.Guid;
        }

        public void Unregister(Guid id)
        {
            executioners.RemoveAll(x=>x.Guid==id);
            if (localExecutionerId != id && executioners.Count==0)
            {
                RegisterLocalExecutioner();
            }
        }

        public void OnExecutionerExceptionOccured(Guid executionId)
        {
            
        }

        public void SubmitResult(ExecutionResultDto executionResult)
        {
            Executioner executioner = executioners.FirstOrDefault(x => x.CurrentExecutionId == executionResult.ExecutionId);
            if (executioner != null)
            {
                executioner.OnExecutionFinished();
            }
            OnExecutionCompleted(executionResult);
        }

        private void ExecuteRequest(ExecutionDto execution, int retriesCount = 0)
        {
            lock (_lock)
            {
                Executioner executioner = GetFreeExecutioner();

                if (executioner != null)
                {
                    try
                    {
                        executioner.Execute(execution);
                    }
                    catch(ExecutionFailedException)
                    {
                        ExecutionResultDto executionResult = new ExecutionResultDto();
                        executionResult.ExecutionId = execution.Id;
                        executionResult.IsExecutionSuccessful = false;
                        SubmitResult(executionResult);
                    }
                    catch(ConnectionFailedException)
                    {
                        Unregister(executioner.Guid);
                        if (retriesCount < 2)
                        {
                            ExecuteRequest(execution, retriesCount++);
                        }
                    }
                }
            }
        }

        private Executioner GetFreeExecutioner()
        {
            return executioners.FirstOrDefault(x => !x.IsBusy);
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
            ExecuteRequest(execution);
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
