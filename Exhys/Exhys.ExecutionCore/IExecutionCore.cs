using SubmissionRouterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.ExecutionCore
{
    public interface IExecutionCore
    {
        ExecutionResultDto Execute (ExecutionDto execution);
        Task<ExecutionResultDto> ExecuteAsync (ExecutionDto execution);
    }
}
