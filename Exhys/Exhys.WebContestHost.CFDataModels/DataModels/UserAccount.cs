using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public class UserAccount
    {
        [Key][Column(Order =1)]
        public int Id { get; set; }

        [Key][Column(Order =2)]
        public string Username { get; set; }

        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<UserSession> UserSessions { get; set; } = new HashSet<UserSession>();
        public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();
        [Required]
        public virtual UserGroup UserGroup { get; set; }

        public ICollection<Competition> GetAvaiableCompetitions ()
        {
            return this.UserGroup.AvaiableCompetition.ToList();
        }

        public bool IsAdmin()
        {
            return this.UserGroup.IsAdministrator;
        }

        public bool IsAdmin (ExhysContestEntities db)
        {

            try
            {
                db.UserAccounts.Attach(this);
            }
            catch (InvalidOperationException) {/*Already attached, nothing to do*/ }
            if (this.UserGroup == null) return false;
            if (this.UserGroup.IsAdministrator) return true;
            return false;
        }

        public string GetFullName ()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}
