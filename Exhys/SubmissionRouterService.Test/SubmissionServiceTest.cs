using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubmissionRouterService.Contracts;
using SubmissionRouterService.Dtos;
using SubmissionRouterService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SubmissionRouterService.Test
{
    [TestClass]
    public class SubmissionServiceTest
    {
        static ISubmissionService submissionService;
        static IExecutionService executionService;
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            submissionService = new SubmissionService();
            executionService = new ExecutionService();
        }

        private Guid RegisterMockExecutioner()
        {
            MockExecutionCallback mockExecutioner = new MockExecutionCallback(executionService);
            Guid id = executionService.Register(mockExecutioner);
            return id;
        }

        [TestMethod]
        public void TestSubmissionAndCompletion()
        {
            Guid id = RegisterMockExecutioner();
            bool isSubmissionCompleted = false;
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            SubmissionDto submission = new SubmissionDto();

            MockSubmissionCallback callback = new MockSubmissionCallback(() =>
            {
                isSubmissionCompleted = true;
                manualEvent.Set();
            });
            submissionService.Submit(submission, callback);
            manualEvent.WaitOne(1000, false);
            Assert.IsTrue(isSubmissionCompleted);
        }
    }
}
