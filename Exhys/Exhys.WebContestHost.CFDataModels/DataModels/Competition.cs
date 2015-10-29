using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class Competition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public virtual UserGroup UserGroup { get; set; }

        public ICollection<Problem> Problems { get; set; } = new HashSet<Problem>();
        public ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();
    }
}
