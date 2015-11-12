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
        const string add1Src = @"
        //add1.c
                #include <iostream>
                #include <unistd.h>
                #include <windows.h>
                using namespace std;
                int main()
                {
                    int a;
                    cin>>a;
                    if(a==-1)a/=0;
                    Sleep (500);
                    cout<<a+1<<endl;
            }
        ";
        static List<TestDto> tests;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            tests = new List<TestDto>();
            tests.Add(new TestDto() { Input = "-1", Solution = "0", TimeLimit = 2 });//Segm. fault
            tests.Add(new TestDto() { Input = "5", Solution = "3", TimeLimit = 2.0 });//Wrong Answer
            tests.Add(new TestDto() { Input = "5", Solution = "6", TimeLimit = 2.0 });//Correct Answer
            tests.Add(new TestDto() { Input = "5", Solution = "6", TimeLimit = 0.2 });//Timeout
            tests.Add(new TestDto() { Input = "5", Solution = "1", TimeLimit = 0.2 });//Timeout
        }

        [TestMethod]
        public void TestRequestAndCompleteSubmission()
        {
            ISubmissionService service = new ExecutionService();
            SubmissionResultDto result = null;
            ManualResetEvent manualEvent = new ManualResetEvent(false);
            bool isSubmissionCompleted = false;
            SubmissionDto submission = new SubmissionDto
            {
                LanguageAlias = "c++",
                SourceCode = add1Src,
                Tests = tests
            };
            service.Submit(submission, new MockSubmissionCallback((r) =>
            {
                result = r;
                isSubmissionCompleted = true;
                manualEvent.Set();
            }));
            manualEvent.WaitOne(3000, false);

            Assert.IsTrue(isSubmissionCompleted);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TestResults.Count, 5);
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
