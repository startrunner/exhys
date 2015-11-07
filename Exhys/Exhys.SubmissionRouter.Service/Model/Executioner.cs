using Exhys.SubmissionRouter.Dtos;
using Exhys.SubmissionRouter.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Model
{
    public class Executioner
    {
        private IExecutionCallback executionCallback;

        public Executioner(IExecutionCallback executionCallback)
        {
            this.executionCallback = executionCallback;
        }

        public ExecutionProcess Execute(Guid executionId, ExecutionDto execution)
        {
            ExecutionProcess executionProcess = new ExecutionProcess(this, execution);
            Task.Run(()=>executionCallback.ExecuteSubmission(executionId, execution));
            IsBusy = true;
            return executionProcess;
        }

        public bool IsBusy { get; private set; }

        public void OnExecutionFinished()
        {
            IsBusy = false;
        }
    }
}