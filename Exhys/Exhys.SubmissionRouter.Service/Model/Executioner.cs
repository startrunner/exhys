using Exhys.SubmissionRouter.Dtos;
using Exhys.SubmissionRouter.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Exhys.SubmissionRouter.Service.Model
{
    public class Executioner
    {
        private IExecutionCallback executionCallback;

        public Executioner(IExecutionCallback executionCallback)
        {
            this.executionCallback = executionCallback;
        }

        public void Execute(ExecutionDto execution)
        {
            TestConnection(execution.Id);
            Task.Run(() =>
            {
                executionCallback.ExecuteSubmission(execution);
            }).ContinueWith(t=>
            {
                throw new ExecutionFailedException(execution.Id);
            },TaskContinuationOptions.OnlyOnFaulted);
            CurrentExecutionId = execution.Id;
        }

        private void TestConnection(Guid executionId)
        {
            bool result=false;
            try
            {
                result = executionCallback.TestConnection();
            }
            catch
            {

            }
            if (!result)
            {
                throw new ConnectionFailedException(executionId);
            }
        }

        public bool IsBusy
        {
            get
            {
                return CurrentExecutionId != null;
            }
        }

        public Guid? CurrentExecutionId { get; private set; }

        public Guid Guid { get; set; }

        public void OnExecutionFinished()
        {
            CurrentExecutionId = null;
        }
    }
}