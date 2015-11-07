using SubmissionRouterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService.Contracts
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed,
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
