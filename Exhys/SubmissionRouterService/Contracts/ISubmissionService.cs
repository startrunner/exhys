using SubmissionRouterService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SubmissionRouterService.Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required,
                 CallbackContract = typeof(ISubmissionCallback))]
    public interface ISubmissionService
    {
        [OperationContract(IsOneWay = true)]
        void Submit(SubmissionDto submission);
    }
}
