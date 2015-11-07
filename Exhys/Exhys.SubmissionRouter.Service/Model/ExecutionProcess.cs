using Exhys.SubmissionRouter.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Service.Model
{
    public class ExecutionProcess
    {
        public ExecutionProcess(Executioner executioner, ExecutionDto execution)
        {
            Executioner = executioner;
            Execution = execution;
        }

        public Executioner Executioner { get; private set; }

        public ExecutionDto Execution { get; private set; }
    }
}
