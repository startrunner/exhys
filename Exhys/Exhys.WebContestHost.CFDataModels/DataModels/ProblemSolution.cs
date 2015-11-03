using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class ProblemSolution
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SourceCode { get; set; }

        [Required]
        public string LanguageAlias { get; set; }

        public byte StatusCode { get; set; }

        public string Message { get; set; }

        [Required]
        public virtual Problem Problem { get; set; }

        [Required]
        public virtual Participation Participation { get; set; }
        public virtual ICollection<SolutionTestStatus> TestStatuses { get; set; }
    }
}
