using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Dtos
{
    public class ExecutionResultDto
    {
        public Guid ExecutionId { get; set; }

        public List<TestResultDto> TestResults { get; set; }

        public bool IsExecutionSuccessful { get; set; }

        public string CompilerOutput { get; set; }
    }
}
