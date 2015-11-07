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
        Action callback;
        public MockSubmissionCallback(Action callback)
        {
            this.callback = callback;
        }
        public void SubmissionProcessed(SubmissionResultDto result)
        {
            callback();
        }
    }
}
