using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.WebContestHost.DataModels.Partials;

namespace Exhys.WebContestHost.DataModels
{
    public partial class Problem:IClearable
    {
        public void ClearForDeletion()
        {
            if (this.ProblemStatements != null) this.ProblemStatements.Clear();
        }
    }
}
