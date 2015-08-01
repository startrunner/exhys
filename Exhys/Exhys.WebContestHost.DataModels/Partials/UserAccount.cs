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
    public partial class UserAccount:IDeletable
    {
        public bool IsAdmin ()
        {
            bool rt;
            using (var db = new ExhysContestEntities())
            {
                rt = this.IsAdmin(db);
            }
            return rt;
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
        

        public void DeleteFrom (ExhysContestEntities db)
        {
            db.UserAccounts.Remove(this);
        }
    }
}
