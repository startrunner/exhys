using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.SubmissionRouter.Dtos;
using Exhys.SubmissionRouter.Service.Contracts;

namespace Exhys.SubmissionRouter.Service.Test
{
    public class MockSubmissionCallback : ISubmissionCallback
    {
        Action<SubmissionResultDto> callback;
        public MockSubmissionCallback(Action<SubmissionResultDto> callback)
        {
            this.callback = callback;
        }

        public void Pong()
        {
            
        }

        public void SubmissionProcessed(SubmissionResultDto result)
        {
            callback(result);
        }
    }
}
