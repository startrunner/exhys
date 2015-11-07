using Exhys.SubmissionRouter.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Contracts
{
    [ServiceContract(SessionMode = SessionMode.Allowed,
                 CallbackContract = typeof(ISubmissionCallback))]
    public interface IExecutionService
    {
        [OperationContract(IsOneWay = false)]
        Guid Register();

        Guid Register(IExecutionCallback executionCallback);

        [OperationContract(IsOneWay = true)]
        void Unregister(Guid id);

        [OperationContract(IsOneWay = true)]
        void SubmitResult(Guid executionProcessId, ExecutionResultDto executionResult);
    }
}
