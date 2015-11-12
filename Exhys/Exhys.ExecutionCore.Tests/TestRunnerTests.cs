using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.SubmissionRouter.Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniGWCpp11;

namespace Exhys.ExecutionCore.Tests
{
    [TestClass]
    public class TestRunnerTests
    {
        const string add1Exe = @"C:\Users\Alexander\Desktop\add1.exe";
        [TestMethod]
        public void Test1()
        {
            List<TestDto> tests = new List<TestDto>();
            tests.Add(new TestDto() { Input = "-1", Solution = "0", TimeLimit = 2 });//Segm. fault
            tests.Add(new TestDto() { Input = "5", Solution = "3", TimeLimit = 2.0 });//Wrong Answer
            tests.Add(new TestDto() { Input = "5", Solution = "6", TimeLimit = 2.0 });//Correct Answer
            tests.Add(new TestDto() { Input = "5", Solution = "6", TimeLimit = 0.2 });//Timeout
            tests.Add(new TestDto() { Input = "5", Solution = "1", TimeLimit = 0.2 });//Timeout

            TestRunner runner = new TestRunner(add1Exe, tests);
            List<TestResultDto> results = runner.Run();
        }

        [TestMethod]
        public void TestCompileAndRun()
        {
            var program = new MiniGW().Compile(add1Src);
            Assert.IsTrue(program.IsSuccessful);

            List<TestDto> tests = new List<TestDto>();
            tests.Add(new TestDto() { Input = "-1", Solution = "0", TimeLimit = 2 });//Segm. fault
            tests.Add(new TestDto() { Input = "5", Solution = "3", TimeLimit = 2.0 });//Wrong Answer
            tests.Add(new TestDto() { Input = "5", Solution = "6", TimeLimit = 2.0 });//Correct Answer
            tests.Add(new TestDto() { Input = "5", Solution = "6", TimeLimit = 0.2 });//Timeout
            tests.Add(new TestDto() { Input = "5", Solution = "1", TimeLimit = 0.2 });//Timeout

            TestRunner runner = new TestRunner(program.ExecutablePath, tests);
            List<TestResultDto> results = runner.Run();

            Assert.IsTrue(results.Count == 5);
            Assert.IsTrue(results[0].Status == TestResultDto.ResultStatus.SegmentationFault);
            Assert.IsTrue(results[1].Status == TestResultDto.ResultStatus.WrongAnswer);
            Assert.IsTrue(results[2].Status == TestResultDto.ResultStatus.CorrectAnswer);
            Assert.IsTrue(results[3].Status == TestResultDto.ResultStatus.TimeOut);
            Assert.IsTrue(results[4].Status == TestResultDto.ResultStatus.TimeOut);
        }

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

        /*
        //add1.c
        #include <stdio.h>
        #include <unistd.h>
        #include <windows.h>
        int main()
        {
            int a;
            scanf("%d", &a);
            if(a==-1)a/=0;
            Sleep(500);
            printf("%d\n", a+1);
        }
        */
    }
}
