using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.DataModels
{
    /// <summary>
    /// Used to store database limits such as max. and min. lengths of database keys as constants
    /// </summary>
    public static class DatabaseLimits
    {
        public const int Username_MaxLength = 32;
        public const int Username_MinLength = 3;

        public const int Password_MaxLength = 64;
        public const int Password_MinLength = 6;

        public const int CompetitionName_MaxLength = 32;
        public const int CompetitionName_MinLength = 3;

        public const int ProblemName_MaxLength = 32;
        public const int ProblemName_MinLength = 3;

        public const int UserGroupName_MaxLength = 32;
        public const int UserGroupName_MinLength = 1;

        public const int HumanName_MaxLength = 64;
        //public const int HumanName_MinLength = 2;//Some people are called Wu...
    }
}
