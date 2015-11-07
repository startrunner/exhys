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
        [MaxLength(DatabaseLimits.UserGroupName_MaxLength)]
        [MinLength(DatabaseLimits.UserGroupName_MinLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsOpen { get; set; }

        [Required]
        public bool IsAdministrator { get; set; }

        public virtual ICollection<UserAccount> GroupMembers { get; set; }

        public virtual ICollection<Competition> AvaiableCompetition { get; set; }
    }
}
