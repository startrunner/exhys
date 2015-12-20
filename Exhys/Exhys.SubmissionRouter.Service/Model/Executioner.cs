using Exhys.SubmissionRouter.Dtos;
using Exhys.SubmissionRouter.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Threading;

namespace Exhys.SubmissionRouter.Service.Model
{
    public class Executioner
    {
        public event EventHandler<EventArgs> ExecutionFinished;

        private IExecutionCallback executionCallback;

        public Executioner(IExecutionCallback executionCallback)
        {
            this.executionCallback = executionCallback;
        }

        public void Execute(ExecutionDto execution)
        {
            TestConnection(execution.Id);
            double totalTestsTime = 0;
            if (execution.Submission.Tests != null)
            {
                totalTestsTime = execution.Submission.Tests
                .Select(x => x.TimeLimit * 1000)
                .Sum();
            }
            double timeoutMs = totalTestsTime + 10000;
            CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeoutMs));
            Task.Run(() =>
            {
                executionCallback.ExecuteSubmission(execution);
            },tokenSource.Token).ContinueWith(t=>
            {
                //throw new ExecutionFailedException(execution.Id);
            },TaskContinuationOptions.NotOnRanToCompletion);
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
            if(ExecutionFinished!=null)
            {
                ExecutionFinished(this, new EventArgs());
            }
        }
    }
}