using Exhys.SubmissionRouter.Dtos;
using Exhys.SubmissionRouter.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Model
{
    public class Executioner
    {
        public event EventHandler<ExceptionOccuredEventArgs> ExceptionOccured;
        private IExecutionCallback executionCallback;

        public Executioner(IExecutionCallback executionCallback)
        {
            this.executionCallback = executionCallback;
        }

        public ExecutionProcess Execute(Guid executionProcessId, ExecutionDto execution)
        {
            IsBusy = true;
            ExceptionOccured += (s, e) =>
            {
                Debug.WriteLine("Failed to communicate with client!");
            };
                
            Task.Run(() =>
            {
                try
                {
                    executionCallback.ExecuteSubmission(executionProcessId, execution);
                }
                catch
                {
                    OnExceptionOccured(executionProcessId);
                }
            });
            ExecutionProcess executionProcess = new ExecutionProcess(this, execution);
            return executionProcess;
        }

        public bool IsBusy { get; private set; }

        private void OnExceptionOccured(Guid executionProcessId)
        {
            if(ExceptionOccured!=null)
            {
                ExceptionOccured(this, new ExceptionOccuredEventArgs { ExecutionProcessId = executionProcessId });
            }
        }

        public void OnExecutionFinished()
        {
            IsBusy = false;
        }
    }
}