using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Model
{
    public class ExecutionFailedException : Exception
    {
        public ExecutionFailedException(Guid executionId)
        {
            ExecutionId = executionId;
        }
        public ExecutionFailedException(Guid executionId, string message)
        {
            ExecutionId = executionId;
        }
        public Guid ExecutionId { get; private set; }
    }
}
