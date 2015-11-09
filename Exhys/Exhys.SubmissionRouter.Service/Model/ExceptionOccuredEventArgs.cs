using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Model
{
    public class ExceptionOccuredEventArgs : EventArgs
    {
        public Guid ExecutionProcessId { get; set; }
    }
}
