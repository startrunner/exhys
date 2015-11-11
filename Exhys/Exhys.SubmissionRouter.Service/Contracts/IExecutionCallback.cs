using Exhys.SubmissionRouter.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Contracts
{
    public interface IExecutionCallback
    {
        [OperationContract(IsOneWay = true)]
        void ExecuteSubmission(ExecutionDto execution);

        [OperationContract(IsOneWay = false)]
        bool TestConnection();
    }
}
