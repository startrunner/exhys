using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.SubmissionRouter.Dtos
{
    public class TestResultDto
    {
        public double ExecutionTime { get; set; }
        public string Output { get; set; }
        public ResultStatus Status { get; set; }
        public enum ResultStatus
        {
            CorrectAnswer,
            WrongAnswer,
            TimeOut,
            MemoryLimitExceeded,
            SegmentationFault
        }
    }
}
