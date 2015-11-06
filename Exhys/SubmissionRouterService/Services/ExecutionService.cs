using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SubmissionRouterService.Dtos;
using SubmissionRouterService.Contracts;
using SubmissionRouterService.Model;
using System.Timers;
using Exhys.ExecutionCore;

namespace SubmissionRouterService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ExecutionService : IExecutionService
    {
        private Dictionary<Guid,Executioner> executioners;
        private Dictionary<Guid,ExecutionProcess> executionProcesses;
        private Queue<ExecutionDto> requestedExecutions;
        private IExecutionScheduler executionScheduler;
        private IExecutionCallback localExecutionCallback;
        private Guid localExecutionerId;
        IExecutionCore executionCore;
        private Timer timer;
        private object _lock;

        public ExecutionService()
            :this(ExecutionScheduler.Instance)
        {
            _lock = new object();
            executionCore = new ExecutionCore();
            localExecutionCallback = new LocalExecutionCallback(executionCore, this);
            RegisterLocalExecutioner();
        }

        private void RegisterLocalExecutioner()
        {
            localExecutionerId = Register(localExecutionCallback);
        }

        private void UnregisterLocalExecutioner()
        {
            Unregister(localExecutionerId);
        }

        public ExecutionService(IExecutionScheduler executionScheduler)
        {
            this.executionScheduler = executionScheduler;
            executioners = new Dictionary<Guid, Executioner>();
            executionProcesses = new Dictionary<Guid, ExecutionProcess>();
            requestedExecutions = new Queue<ExecutionDto>();

            executionScheduler.ExecutionRequested += (s, e) => RequestExecution(e.Execution);

            timer = new Timer(1000);
            timer.Elapsed += (s,e) => ExecuteNextRequest();

            ExecutionAdded += (s,e) => StartTimer();
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
            UnregisterLocalExecutioner();
            return id;
        }

        public void Unregister(Guid id)
        {
            executioners.Remove(id);
            RegisterLocalExecutioner();
        }

        public void SubmitResult(Guid executionProcessId, ExecutionResultDto executionResult)
        {
            executionProcesses[executionProcessId].Executioner.OnExecutionFinished();
            executionProcesses.Remove(executionProcessId);
            PublishExecutionResult(executionResult);
        }

        private void PublishExecutionResult(ExecutionResultDto executionResult)
        {
            executionScheduler.CompleteExecution(executionResult);
        }

        private void RequestExecution(ExecutionDto execution)
        {
            requestedExecutions.Enqueue(execution);
            OnExecutionAdded();
        }

        private void ExecuteNextRequest()
        {
            lock(_lock)
            {
                ExecutionDto execution = requestedExecutions.Dequeue();
                Executioner executioner = GetFreeExecutioner();

                if (executioner != null)
                {
                    ExecuteRequest(execution, executioner);
                }

                if (requestedExecutions.Count == 0)
                {
                    StopTimer();
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

        private void StartTimer()
        {
            timer.Start();
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        private void OnExecutionAdded()
        {
            if(ExecutionAdded!=null)
            {
                ExecutionAdded(this, EventArgs.Empty);
            }
        }

        private event EventHandler ExecutionAdded;
    }
}
