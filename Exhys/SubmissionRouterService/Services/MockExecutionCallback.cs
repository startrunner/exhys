using SubmissionRouterService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubmissionRouterService.Dtos;

namespace SubmissionRouterService.Services
{
    public class MockExecutionCallback : IExecutionCallback
    {
        IExecutionService executionService;

        public MockExecutionCallback(IExecutionService executionService)
        {
            this.executionService = executionService;
        }

        public void ExecuteSubmission(Guid submissionProcessId, ExecutionDto execution)
        {
            ExecutionResultDto executionResult = new ExecutionResultDto()
            {
                ExecutionId = execution.Id,
                TestResults = new List<TestResultDto>
                {
                    new TestResultDto
                    {
                        ExecutionTime=15165165,
                        Output="Maika ti e kurva!"
                    },
                    new TestResultDto
                    {
                        ExecutionTime=15165165,
                        Output="Bashta ti lapa pishki!"
                    },
                    new TestResultDto
                    {
                        ExecutionTime=15165165,
                        Output="Ebah sestra ti!"
                    },
                }
            };
            executionService.SubmitResult(submissionProcessId, executionResult);
        }
    }
}
