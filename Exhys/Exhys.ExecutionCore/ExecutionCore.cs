using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.ExecutionCore.Contracts;
using SubmissionRouterService.Dtos;

namespace Exhys.ExecutionCore
{
    public class ExecutionCore : IExecutionCore
    {
        public ExecutionResultDto Execute (ExecutionDto execution)
        {
            return ExecuteAsync(execution).GetAwaiter().GetResult();
        }

        public async Task<ExecutionResultDto> ExecuteAsync (ExecutionDto execution)
        {
            return new ExecutionResultDto() { ExecutionId = execution.Id, TestResults = new List<TestResultDto>() { new TestResultDto() { ExecutionTime = 0, Output = "na maika ti putkata" } } };
        }
    }
}
