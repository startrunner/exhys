using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Dtos
{
    public class ExecutionDto
    {
        public Guid Id { get; set; }

        public SubmissionDto Submission { get; set; }
    }
}
