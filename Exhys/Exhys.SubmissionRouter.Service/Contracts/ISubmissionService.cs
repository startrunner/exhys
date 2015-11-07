using Exhys.SubmissionRouter.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Exhys.SubmissionRouter.Service.Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required,
                 CallbackContract = typeof(ISubmissionCallback))]
    public interface ISubmissionService
    {
        [OperationContract(IsOneWay = false)]
        Guid Submit(SubmissionDto submission);
        
        Guid Submit(SubmissionDto submission,ISubmissionCallback callback);
    }
}
