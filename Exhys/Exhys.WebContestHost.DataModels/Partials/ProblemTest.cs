using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    partial class ProblemTest : ExhysContestEntities.ICascadeable
    {
        public void CascadeFrom (ExhysContestEntities db)
        {
            db.ProblemTests.Remove(this);
            //db.SaveChanges();
        }
    }
}
