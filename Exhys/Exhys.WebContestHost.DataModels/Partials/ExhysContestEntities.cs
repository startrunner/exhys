using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    

    public partial class ExhysContestEntities
    {
        public interface ICascadeable
        {
            void CascadeFrom (ExhysContestEntities db);
        }

        public void CascadeFunc(ICascadeable item)
        {
            item.CascadeFrom(this);
        }

        public UserGroup GetDefaultUserGroup()
        {
            return this.UserGroups.Where(g => g.IsOpen).FirstOrDefault();
        }
    }
}
