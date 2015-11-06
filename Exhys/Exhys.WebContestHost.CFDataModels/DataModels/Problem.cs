using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class Problem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DatabaseLimits.ProblemName_MaxLength)]
        [MinLength(DatabaseLimits.ProblemName_MinLength)]
        public string Name { get; set; }

        public string T_TimeLimits { get; set; }
        public string T_InputFeedbacks { get; set; }
        public string T_OutputFeedbacks { get; set; }
        public string T_SolutionFeedbacks { get; set; }
        public string T_ScoreFeedbacks { get; set; }
        public string T_StatusFeedbacks { get; set; }

        //[Required]
        public virtual Competition CompetitionGivenAt { get; set; }

        public virtual ICollection<ProblemSolution> ProblemSolutions { get; set; }

        public virtual ICollection<ProblemStatement> ProblemStatements { get; set; }

        public virtual ICollection<ProblemTest> Tests { get; set; }
    }
}
