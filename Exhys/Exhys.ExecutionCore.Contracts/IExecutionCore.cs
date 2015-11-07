using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubmissionRouterService.Dtos;

namespace Exhys.ExecutionCore.Contracts
{
    public interface IExecutionCore
    {
        ExecutionResultDto Execute (ExecutionDto execution);
        Task<ExecutionResultDto> ExecuteAsync (ExecutionDto execution);
    }
}
