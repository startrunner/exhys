using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class Participation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual UserAccount User { get; set; }
        [Required]
        public virtual Competition Competition { get; set; }
        public virtual ICollection<ProblemSolution> Submissions { get; set; } = new HashSet<ProblemSolution>();
    }
}
