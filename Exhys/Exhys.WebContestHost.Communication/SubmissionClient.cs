using Exhys.WebContestHost.DataModels;
using Exhys.SubmissionRouter.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Exhys.WebContestHost.Communication.SubmissionRouter;
using System.Threading;
using System.ServiceModel.Channels;

namespace Exhys.WebContestHost.Communication
{
    [CallbackBehavior(UseSynchronizationContext = true)]
    public class SubmissionClient:ISubmissionServiceCallback
    {
        TaskCompletionSource<SubmissionResultDto> submissionCompletionSource;
        public SubmissionClient()
        {
        }

        public void SubmissionProcessed(SubmissionResultDto result)
        {
            submissionCompletionSource.TrySetResult(result);
        }

        async public Task<List<SolutionTestStatus>> SubmitRequestAsync (ProblemSolution problemSolution)
        {
            if(submissionCompletionSource!=null)
            {
                throw new Exception("Another submission is already being processed!");
            }
            double totalTestsTime = 0;
            if(problemSolution.Problem.Tests!=null)
            {
                totalTestsTime = problemSolution.Problem.Tests
                .Select(x => x.TimeLimit * 1000)
                .Sum();
            }
            double timeoutMs =  totalTestsTime + 10000;

            submissionCompletionSource = new TaskCompletionSource<SubmissionResultDto>();
            CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeoutMs));
            CancellationTokenRegistration token = tokenSource.Token.Register(() => 
            {
                //submissionCompletionSource.TrySetCanceled();
            }, false);

            SubmissionDto submission = CreateSubmission(problemSolution);
            try
            {
                SubmissionServiceClient client = CreateClientInstance();
                client.Open();
                Guid executionId = await client.SubmitAsync(submission);
                await submissionCompletionSource.Task;
                client.Close();
            }
            catch(Exception ex)
            {
                throw new Exception($"Unable to communicate with server! {ex.Message}");
            }

            SubmissionResultDto result = null;
            if (submissionCompletionSource.Task.Status == TaskStatus.RanToCompletion)
            {
                result = submissionCompletionSource.Task.Result;
            }
            submissionCompletionSource = null;
            token.Dispose();
            return CreateSolutionTestStatuses(result, problemSolution.Problem);
        }

        private List<SolutionTestStatus> CreateSolutionTestStatuses(SubmissionResultDto submissionResult, Problem problem)
        {
            return submissionResult.TestResults
                .ToList()
                .Select((x, i) => CreateSolutionTestStatus(x, problem.Tests.ElementAt(i)))
                .ToList();
        }

        private SolutionTestStatus CreateSolutionTestStatus(TestResultDto testResult, ProblemTest test)
        {
            return new SolutionTestStatus
            {
                Output = testResult.Output,
                Status = CreateTestStatus(testResult.Status),
                ProblemTest = test                
            };
        }

        private SolutionTestStatus.TestStatus CreateTestStatus(TestResultDto.ResultStatus resultStatus)
        {
            switch(resultStatus)
            {
                case TestResultDto.ResultStatus.CorrectAnswer:
                    return SolutionTestStatus.TestStatus.CorrectAnswer;
                case TestResultDto.ResultStatus.MemoryLimitExceeded:
                    return SolutionTestStatus.TestStatus.MemoryLimitExceeded;
                case TestResultDto.ResultStatus.SegmentationFault:
                    return SolutionTestStatus.TestStatus.SegmentationFault;
                case TestResultDto.ResultStatus.TimeOut:
                    return SolutionTestStatus.TestStatus.TimeOut;
                case TestResultDto.ResultStatus.WrongAnswer:
                    return SolutionTestStatus.TestStatus.WrongAnswer;
                default: return SolutionTestStatus.TestStatus.WrongAnswer;
            }
        }

        public SubmissionServiceClient CreateClientInstance()
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            EndpointAddress endpointAddress = new EndpointAddress("ws://localhost:9080/SubmissionRouter");
            InstanceContext instanceContext = new InstanceContext(this);
            SubmissionServiceClient submissionServiceClient = new SubmissionServiceClient(instanceContext,binding,endpointAddress);
            return submissionServiceClient;
        }

        private SubmissionDto CreateSubmission(ProblemSolution problemSolution)
        {
            return new SubmissionDto
            {
                LanguageAlias = problemSolution.LanguageAlias,
                SourceCode = problemSolution.SourceCode,
                Tests = problemSolution.Problem.Tests!=null?
                problemSolution.Problem.Tests
                .Select(x => new TestDto
                {
                    Input = x.Input,
                    Solution = x.Solution,
                    TimeLimit = x.TimeLimit
                }).ToList():null
            };
        }

        public void Pong()
        {
            
        }
    }
}
