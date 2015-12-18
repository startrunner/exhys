using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Exhys.SubmissionRouter.Dtos;
using Exhys.WebContestHost.Communication.SpamService.RouterReference;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Communication.SpamService
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
            tests.Add(new ProblemTest() { Input = "5", Solution = "3", TimeLimit = 2 });//Wrong Answer
            tests.Add(new ProblemTest() { Input = "5", Solution = "6", TimeLimit = 2.0 });//Correct Answer
            tests.Add(new ProblemTest() { Input = "5", Solution = "6", TimeLimit = 0.2 });//Timeout
            tests.Add(new ProblemTest() { Input = "5", Solution = "1", TimeLimit = 0.2 });//Timeout
            problemSolution = new ProblemSolution
            {
                LanguageAlias = "c++",
                SourceCode = add1Src,
                Problem = new Problem
                {
                    Name = "Add1",
                    Tests = tests
                }
            };
        }
        static ProblemSolution problemSolution;

        [CallbackBehavior(UseSynchronizationContext = true)]
        class NotThis : ISubmissionServiceCallback
        {
            public void Pong ()
            {
                
            }

            public void SubmissionProcessed (SubmissionResultDto result)
            {
                pongCount++;
                Debug.WriteLine($"Pong () #{pongCount}");
            }
        }
        static int pongCount = 0;

        public static SubmissionServiceClient CreateClientInstance ()
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            EndpointAddress endpointAddress = new EndpointAddress("ws://localhost:9080/SubmissionRouter");
            InstanceContext instanceContext = new InstanceContext(new NotThis());
            SubmissionServiceClient submissionServiceClient = new SubmissionServiceClient(instanceContext, binding, endpointAddress);
            return submissionServiceClient;
        }

        static void Main (string[] args)
        {
            Initialize();
            var client = CreateClientInstance();
            for(int i=0;i<1000;i++)
            {
                client.Submit(new SubmissionDto()
                {
                    LanguageAlias = problemSolution.LanguageAlias,
                    SourceCode = problemSolution.SourceCode,
                    Tests = problemSolution.Problem.Tests.Select(x => new TestDto()
                    {
                        Input = x.Input,
                        Solution = x.Solution,
                        TimeLimit = x.TimeLimit
                    }).ToList()
                });
                if(i==999) Debug.WriteLine("no more ping()");
            }
            Console.Read();
        }
    }
}
