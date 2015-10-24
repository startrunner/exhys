using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    partial class Competition : ExhysContestEntities.ICascadeable
    {
        public void CascadeFrom (ExhysContestEntities db)
        {
            this.Problems.ToList().ForEach(db.CascadeFunc);
            db.Competitions.Remove(this);
            //db.SaveChanges();
        }
    }
}
