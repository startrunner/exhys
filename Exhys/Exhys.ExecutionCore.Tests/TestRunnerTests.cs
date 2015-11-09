using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.SubmissionRouter.Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
