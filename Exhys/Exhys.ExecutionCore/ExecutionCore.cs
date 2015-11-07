using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }
    }
}
