using Exhys.SubmissionRouter.Dtos;
using Exhys.SubmissionRouter.Service.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Test
{
    [TestClass]
    public class ExecutionServiceTest
    {
        [TestMethod]
        public void TestRequestAndCompleteSubmission()
        {
            ISubmissionService service = new ExecutionService();
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            bool isSubmissionCompleted = false;
            service.Submit(new SubmissionDto { LanguageAlias = "c++" }, new MockSubmissionCallback(() =>
            {
                isSubmissionCompleted = true;
                manualEvent.Set();
            }));
            manualEvent.WaitOne(999999999, false);
            Assert.IsTrue(isSubmissionCompleted);
        }

        [TestMethod]
        public void TestRegisterUnregister()
        {
            IExecutionService service = new ExecutionService();
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            Guid executionerId = service.Register(new MockExecutionCallback(service));
            service.Unregister(executionerId);
            Assert.IsNotNull(executionerId);
        }

    }
}
