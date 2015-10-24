using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    partial class SolutionTestStatus : ExhysContestEntities.ICascadeable
    {
        public void CascadeFrom (ExhysContestEntities db)
        {
            db.SolutionTestStatuses.Remove(this);
            //db.SaveChanges();
        }
    }
}
