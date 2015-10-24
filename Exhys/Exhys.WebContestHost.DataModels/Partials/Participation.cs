using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    partial class Participation : ExhysContestEntities.ICascadeable
    {
        public void CascadeFrom (ExhysContestEntities db)
        {
            this.Submissions.ToList().ForEach(db.CascadeFunc);
            db.Participations.Remove(this);
        }
    }
}
