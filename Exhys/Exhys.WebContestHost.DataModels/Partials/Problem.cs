using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.WebContestHost.DataModels.Partials;

namespace Exhys.WebContestHost.DataModels
{
    public partial class Problem:IDeletable
    {
        public void DeleteFrom (ExhysContestEntities db)
        {
            this.ProblemTests.Clear();
            this.ProblemSolutions.Clear();
            this.ProblemStatements.Clear();
            db.Problems.Remove(this);
        }
    }
}
