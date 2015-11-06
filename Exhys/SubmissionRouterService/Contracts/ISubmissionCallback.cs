using SubmissionRouterService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService.Contracts
{
    public interface ISubmissionCallback
    {
        [OperationContract(IsOneWay = true)]
        void SubmissionProcessed(SubmissionResultDto result);
    }
}
