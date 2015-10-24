using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    partial class UserGroup : ExhysContestEntities.ICascadeable
    {
        public void CascadeFrom (ExhysContestEntities db)
        {
            this.GroupMembers.ToList().ForEach(db.CascadeFunc);
            this.AvaiableCompetitions.ToList().ForEach(db.CascadeFunc);
            db.UserGroups.Remove(this);
            //db.SaveChanges();
        }
    }
}
