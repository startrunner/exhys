using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Model
{
    public class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(Guid executionId)
        {
            ExecutionId = executionId;
        }
        public ConnectionFailedException(Guid executionId, string message)
        {
            ExecutionId = executionId;
        }
        public Guid ExecutionId { get; private set; }
    }
}
