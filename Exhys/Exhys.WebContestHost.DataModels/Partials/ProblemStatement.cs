using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    partial class ProblemStatement : ExhysContestEntities.ICascadeable
    {
        public void CascadeFrom (ExhysContestEntities db)
        {
            db.ProblemStatements.Remove(this);
            //db.SaveChanges();
        }
    }
}
