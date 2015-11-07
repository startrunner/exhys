using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubmissionRouterService.Services;
using SubmissionRouterService.Contracts;
using System.Threading;
using SubmissionRouterDTOs;

namespace SubmissionRouterService.Test
{
    [TestClass]
    public class ExecutionServiceTest
    {
        private static IExecutionService executionService;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            executionService = new ExecutionService();
        }

        [TestMethod]
        public void TestRegisterUnregister()
        {
            Guid id = RegisterMockExecutioner();
            UnregisterMockExecutioner(id);
        }

        private void UnregisterMockExecutioner(Guid id)
        {
            executionService.Unregister(id);
        }

        private Guid RegisterMockExecutioner()
        {
            MockExecutionCallback mockExecutioner = new MockExecutionCallback(executionService);
            Guid id = executionService.Register(mockExecutioner);
            return id;
        }

        [TestMethod]
        public void TestRequestAndCompleteExecution()
        {
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            bool isSubmissionCompleted = false;
            ExecutionScheduler.Instance.SubmissionCompleted += (s,e)=>
            {
                isSubmissionCompleted = true;
                manualEvent.Set();
            };
            ExecutionScheduler.Instance.RequestExecution(new SubmissionDto());
            manualEvent.WaitOne(3000, false);
            Assert.IsTrue(isSubmissionCompleted);
        }
    }
}
