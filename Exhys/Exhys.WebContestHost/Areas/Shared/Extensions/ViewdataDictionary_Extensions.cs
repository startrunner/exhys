using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class ViewdataDictionary_Extensions
    {
        private static class PropertyNames
        {
            public const string SignedInUser = "signed-in-user";
            public const string ProblemOptions = "problem-options";
            public const string UserGroupOptions = "user=group-options";
            public const string CompetitionOptions = "competition-options";
        }

        public static List<SelectListItem> GetCompetitionOptions(this ViewDataDictionary that)
        {
            return that[PropertyNames.CompetitionOptions] as List<SelectListItem>;
        }

        public static List<SelectListItem> GetProblemOptions(this ViewDataDictionary that)
        {
            return that[PropertyNames.ProblemOptions] as List<SelectListItem>;
        }

        public static List<SelectListItem> GetUserGroupOptions(this ViewDataDictionary that)
        {
            return that[PropertyNames.UserGroupOptions] as List<SelectListItem>;
        }
        
        public static SignedInUserViewModel GetSignedInUser(this ViewDataDictionary that)
        {
            return that[PropertyNames.SignedInUser] as SignedInUserViewModel;
        }

        public static void SetCompetitionOptions(this ViewDataDictionary that, List<SelectListItem> options)
        {
            that[PropertyNames.CompetitionOptions] = options;
        }

        public static void SetUserGroupOptions(this ViewDataDictionary that, List<SelectListItem> options)
        {
            that[PropertyNames.UserGroupOptions] = options;
        }

        public static void SetProblemOptions(this ViewDataDictionary that, List<SelectListItem> options)
        {
            that[PropertyNames.ProblemOptions] = options;
        }

        public static void SetSignedInUser(this ViewDataDictionary that, SignedInUserViewModel vm)
        {
            that[PropertyNames.SignedInUser] = vm;
        }
    }
}