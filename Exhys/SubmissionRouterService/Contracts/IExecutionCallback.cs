using SubmissionRouterService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService.Contracts
{
    public interface IExecutionCallback
    {
        [OperationContract(IsOneWay = true)]
        void ExecuteSubmission(Guid submissionProcessId, ExecutionDto execution);
    }
}
