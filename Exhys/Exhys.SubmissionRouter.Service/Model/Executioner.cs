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
            ExceptionOccured += (s, e) =>
            {
                Debug.WriteLine("Failed to communicate with client!");
                OnExecutionFinished();
            };
        }

        public void Execute(ExecutionDto execution)
        {
                
            Task.Run(() =>
            {
                try
                {
                    executionCallback.ExecuteSubmission(execution);
                }
                catch
                {
                    OnExceptionOccured(execution.Id);
                }
            });
            CurrentExecutionId = execution.Id;
        }

        public bool IsBusy
        {
            get
            {
                return CurrentExecutionId != null;
            }
        }

        public Guid? CurrentExecutionId { get; private set; }

        private void OnExceptionOccured(Guid executionProcessId)
        {
            if(ExceptionOccured!=null)
            {
                ExceptionOccured(this, new ExceptionOccuredEventArgs { ExecutionProcessId = executionProcessId });
            }
        }

        public void OnExecutionFinished()
        {
            CurrentExecutionId = null;
        }
    }
}