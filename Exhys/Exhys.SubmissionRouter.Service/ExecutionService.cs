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
using System.Threading;
using System.ServiceModel.Activation;

namespace Exhys.SubmissionRouter.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExecutionService : IExecutionService, ISubmissionService
    {
        Dictionary<Guid, ISubmissionCallback> callbacks;
        IExecutionCallback localExecutionCallback;
        System.Timers.Timer executionTimer = null;
        List<Executioner> executioners;
        Queue<ExecutionDto> executionQueue;
        Guid localExecutionerId;
        object _lock;

        public ExecutionService ()
        {
            _lock = new object();
            executionQueue = new Queue<ExecutionDto>();
            executioners = new List<Executioner>();
            callbacks = new Dictionary<Guid, ISubmissionCallback>();
            localExecutionCallback = new LocalExecutionCallback(GetExecutionCore(), this);

            RegisterLocalExecutioner();
        }

        private IExecutionCore GetExecutionCore () => ExecutionCoreFactory.Generate();

        #region IExecutionService implementation

        private void RegisterLocalExecutioner () => localExecutionerId = Register(localExecutionCallback);
        private void UnregisterLocalExecutioner () => Unregister(localExecutionerId);
        public Guid Register () => Register(OperationContext.Current.GetCallbackChannel<IExecutionCallback>());

        public Guid Register (IExecutionCallback executionCallback)
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

        public void Unregister (Guid id)
        {
            executioners.RemoveAll((x) => x.Guid == id);
            if (localExecutionerId != id && executioners.Count == 0)
            {
                RegisterLocalExecutioner();
            }
        }

        public void SubmitResult (ExecutionResultDto executionResult)
        {
            Executioner executioner = executioners.FirstOrDefault(x => x.CurrentExecutionId == executionResult.ExecutionId);
            if (executioner != null)
            {
                executioner.OnExecutionFinished();
            }
            OnExecutionCompleted(executionResult);
        }

        private void ExecuteRequest (ExecutionDto execution)
        {
            lock (_lock)
            {
                executionQueue.Enqueue(execution);
                StartExecutionTimer();
            }
        }

        private Executioner GetFreeExecutioner () => executioners.FirstOrDefault(x => !x.IsBusy);

        #endregion

        #region ISubmissionService

        public Guid Submit (SubmissionDto submission) => Submit(submission, OperationContext.Current.GetCallbackChannel<ISubmissionCallback>());

        public Guid Submit (SubmissionDto submission, ISubmissionCallback callback)
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

        private void OnExecutionCompleted (ExecutionResultDto executionResult)
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

        public void Ping ()
        {
            var callback = OperationContext.Current.GetCallbackChannel<ISubmissionCallback>();
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                callback.Pong();
            });
        }

        #endregion

        #region ExecutionWork
        private void StartExecutionTimer ()
        {
            if (executionTimer == null)
            {
                executionTimer = new System.Timers.Timer()
                {
                    AutoReset = true,
                    Interval = TimeSpan.FromSeconds(.25).TotalMilliseconds,
                };
            }
            executionTimer.Elapsed += OnExecutionTimerTicked;
            executionTimer.Enabled = true;
        }

        private void StopExecutionTimer ()
        {
            if (executionTimer != null)
            {
                executionTimer.Enabled = false;
            }
        }

        private void OnExecutionTimerTicked (object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                var executioner = GetFreeExecutioner();
                while (executioner != null && executionQueue.Count != 0)
                {
                    var item = executionQueue.Dequeue();
                    try
                    {
                        executioner.Execute(item);
                    }
                    catch (ExecutionFailedException)
                    {
                        ExecutionResultDto executionResult = new ExecutionResultDto();
                        executionResult.ExecutionId = item.Id;
                        executionResult.IsExecutionSuccessful = false;
                        SubmitResult(executionResult);
                    }
                    catch (ConnectionFailedException)
                    {
                        Unregister(executioner.Guid);
                    }

                    executioner = GetFreeExecutioner();
                }
                if (executionQueue.Count == 0) StopExecutionTimer();
            }
        }
        #endregion
    }
}
