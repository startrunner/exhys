using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubmissionRouterService.Contracts;
using SubmissionRouterService.Dtos;
using SubmissionRouterService.Services;
using System;
using System.Threading;

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

        [TestMethod]
        public void TestSubmissionAndCompletion()
        {
            bool isSubmissionCompleted = false;
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            SubmissionDto submission = new SubmissionDto();

            MockSubmissionCallback callback = new MockSubmissionCallback(() =>
            {
                isSubmissionCompleted = true;
                manualEvent.Set();
            });

            submissionService.Submit(submission, callback);
            manualEvent.WaitOne(2000, false);
            Assert.IsTrue(isSubmissionCompleted);
        }
    }
}
