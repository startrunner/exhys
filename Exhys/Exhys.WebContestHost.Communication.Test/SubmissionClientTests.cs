using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exhys.WebContestHost.DataModels;
using Exhys.SubmissionRouter.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Exhys.WebContestHost.Communication.Test
{
    [TestClass]
    public class SubmissionClientTests
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
        static List<ProblemTest> tests;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            tests = new List<ProblemTest>();
            tests.Add(new ProblemTest() { Input = "-1", Solution = "0", TimeLimit = 2 });//Segm. fault
            tests.Add(new ProblemTest() { Input = "5", Solution = "3", TimeLimit = 2.0 });//Wrong Answer
            tests.Add(new ProblemTest() { Input = "5", Solution = "6", TimeLimit = 2.0 });//Correct Answer
            tests.Add(new ProblemTest() { Input = "5", Solution = "6", TimeLimit = 0.2 });//Timeout
            tests.Add(new ProblemTest() { Input = "5", Solution = "1", TimeLimit = 0.2 });//Timeout
        }

        [TestMethod]
        public void TestSubmit()
        {
            SubmissionClient submissionClient = new SubmissionClient("");
            ProblemSolution problemSolution = new ProblemSolution
            {
                LanguageAlias = "c++",
                SourceCode = add1Src,
                Problem = new Problem
                {
                    Name = "Add1",
                    Tests = tests
                }
            };
            List<SolutionTestStatus> result = submissionClient.SubmitRequestAsync(problemSolution).Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 5);
        }
    }
}
