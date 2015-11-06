using SubmissionRouterService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubmissionRouterService.Dtos;

namespace SubmissionRouterService.Services
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
