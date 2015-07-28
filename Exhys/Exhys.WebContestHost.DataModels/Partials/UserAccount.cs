using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.WebContestHost.DataModels;
using Exhys.WebContestHost.DataModels.Partials;

namespace Exhys.WebContestHost.DataModels
{ 
    public partial class UserAccount:IClearable
    {

        public bool IsAdmin ()
        {
            using (var db = new ExhysContestEntities())
            {
                try
                {
                    //The account needs to be attached in order to access its UserGroups property via lazy loading
                    db.UserAccounts.Attach(this);
                }
                catch (InvalidOperationException) {/*Already attached, nothing to do*/ }

                foreach (var g in this.UserGroups)
                {
                    if (g.IsAdministrator) return true;
                }
                return false;
            }
        }

        public void ClearForDeletion()
        {
            if (this.UserGroups != null) this.UserGroups.Clear();
            if (this.UserSessions != null) this.UserSessions.Clear();
            if (this.AuthoredSolutions != null) this.AuthoredSolutions.Clear();
        }
    }
}
