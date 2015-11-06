using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionRouterService.Dtos
{
    public class ExecutionResultDto
    {
        public Guid ExecutionId { get; set; }

        public SubmissionResultDto SubmissionResult { get; set; }
    }
}
