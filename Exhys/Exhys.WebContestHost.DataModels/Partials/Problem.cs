using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    partial class Problem : ExhysContestEntities.ICascadeable
    {
        public void CascadeFrom (ExhysContestEntities db)
        {
            this.Tests.ToList().ForEach(db.CascadeFunc);
            this.ProblemStatements.ToList().ForEach(db.CascadeFunc);
            this.ProblemSolutions.ToList().ForEach(db.CascadeFunc);
            db.Problems.Remove(this);
            //db.SaveChanges();
        }
    }
}
