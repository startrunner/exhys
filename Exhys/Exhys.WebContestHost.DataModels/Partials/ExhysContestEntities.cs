using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    public partial class ExhysContestEntities
    {
        public UserGroup GetDefaultUserGroup()
        {
            var groups = this.UserGroups.Where(g => g.IsOpen).Take(1).ToList();
            if (groups == null || groups.Count == 0) return null;
            return groups[0];
        }
    }
}
