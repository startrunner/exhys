using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class ProblemTest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Input { get; set; }

        [Required]
        public string Solution { get; set; }

        [Required]
        public string GroupName { get; set; }

        public double TimeLimit { get; set; }
        public double Points { get; set; }

        public bool InputFeedbackEnabled { get; set; }
        public bool OutputFeedbackEnabled { get; set; }
        public bool SolutionFeedbackEnabled { get; set; }
        public bool ScoreFeedbackEnabled { get; set; }
        public bool StatusFeedbackEnabled { get; set; }

        [Required]
        public virtual Problem Problem { get; set; }
    }
}
