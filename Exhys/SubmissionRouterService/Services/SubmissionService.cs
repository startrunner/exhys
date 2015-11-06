using SubmissionRouterService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SubmissionRouterService.Dtos;

namespace SubmissionRouterService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SubmissionService : ISubmissionService
    {
        public void Submit(SubmissionDto submission)
        {
            throw new NotImplementedException();
        }
    }
}
