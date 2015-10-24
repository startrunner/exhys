﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.DataModels
{ 
    public partial class UserAccount:ExhysContestEntities.ICascadeable
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
    
        public string GetFullName ()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }

        public void CascadeFrom (ExhysContestEntities db)
        {
            this.UserSessions.ToList().ForEach(db.CascadeFunc);
            db.UserAccounts.Remove(this);
            //db.SaveChanges();
        }

        public ICollection<Competition> GetAvaiableCompetitions()
        {
            return this.UserGroup.AvaiableCompetitions;
        }
    }
}
