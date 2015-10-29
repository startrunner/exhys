using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public bool IsOpen { get; set; }
        public bool IsAdministrator { get; set; }

        public virtual ICollection<UserAccount> GroupMembers { get; set; } = new HashSet<UserAccount>();
        public virtual ICollection<Competition> AvaiableCompetition { get; set; } = new HashSet<Competition>();
    }
}
