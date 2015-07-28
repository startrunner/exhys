using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhys.WebContestHost.DataModels.Partials;

namespace Exhys.WebContestHost.DataModels
{
    public partial class Competition:IClearable
    {
        public void ClearForDeletion()
        {
            if (this.Problems != null)
            { 
                this.Problems.Clear();
            }
            if (this.GroupsAllowed != null) this.GroupsAllowed.Clear();
        }
    }
}
