using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exhys.WebContestHost.DataModels;
using Newtonsoft.Json;

namespace Exhys.WebContestHost.Communication.ParallelRunningTest
{
    class Program
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

        public static void Initialize ()
        {
            tests = new List<ProblemTest>();
            tests.Add(new ProblemTest() { Input = "-1", Solution = "0", TimeLimit = 2 });//Segm. fault
            tests.Add(new ProblemTest() { Input = "5", Solution = "3", TimeLimit = 2.0 });//Wrong Answer
            tests.Add(new ProblemTest() { Input = "5", Solution = "6", TimeLimit = 2.0 });//Correct Answer
            tests.Add(new ProblemTest() { Input = "5", Solution = "6", TimeLimit = 0.2 });//Timeout
            tests.Add(new ProblemTest() { Input = "5", Solution = "1", TimeLimit = 0.2 });//Timeout
        }

        static int run = 0, completed = 0;
        public static void Main (string[] args)
        {
            Initialize();
            var mainThread = Thread.CurrentThread;
            for (;;)
            {
                if(run<100)Task.Run(() =>
                {
                    run++;
                    var submissionClient = new SubmissionClient();
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
                    List<SolutionTestStatus> result = null;
                    submissionClient.SubmitRequestAsync(problemSolution).ContinueWith((x) =>
                    {
                        Console.WriteLine("one done");
                        result = x.Result;
                        completed++;
                    }, TaskContinuationOptions.NotOnFaulted)
                    .ContinueWith(x =>
                    {
                        ;
                    }, TaskContinuationOptions.OnlyOnFaulted);
                });
                Console.WriteLine($"{run} {completed}");
                Thread.Sleep(200);
            }
        }
    }
}
