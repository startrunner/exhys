using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class SolutionTestStatus
    {
        [Key]
        public int Id { get; set; }

        public string Input { get; set; }
        public string Output { get; set; }
        public string Solution { get; set; }
        public double Score { get; set; }
        public byte StatusCode { get; set; }

        public bool InputFeedbackEnabled { get; set; }
        public bool OutputFeedbackEnabled { get; set; }
        public bool SolutionFeedbackEnabled { get; set; }
        public bool ScoreFeedbackEnabled { get; set; }
        public bool StatusFeedbackEnabled { get; set; }
    }
}
