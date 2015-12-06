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
            public const string UserGroupOptions = "user-group-options";
            public const string OpenUserGroupOptions = "open-user-group-options";
            public const string CompetitionOptions = "competition-options";
            public const string ProgrammingLanguageOptions = "programming-language-options";
            public const string PageCount = "page-count";
            //public const string CurrentPage = "current-page";
            //public const string PageSize = "page-size";
            public const string PageTitle = "page-title";
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

        public static int? GetPageCount(this ViewDataDictionary that)
        {
            return that[PropertyNames.PageCount] as int?;
        }

        public static List<SelectListItem> GetOpenUserGroupOptions(this ViewDataDictionary that)
        {
            return that[PropertyNames.OpenUserGroupOptions] as List<SelectListItem>;
        }

        public static string GetPageTitle(this ViewDataDictionary that)
        {
            return that[PropertyNames.PageTitle] as string;
        }

        public static List<SelectListItem> GetProgrammingLanguageOptions(this ViewDataDictionary that)
        {
            return that[PropertyNames.ProgrammingLanguageOptions] as List<SelectListItem>;
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

        public static void SetPageCount(this ViewDataDictionary that, int? val)
        {
            that[PropertyNames.PageCount] = val;
        }

        public static void SetPageTitle(this ViewDataDictionary that, string val)
        {
            that[PropertyNames.PageTitle] = val;
        }

        public static void SetOpenUserGroupOptions(this ViewDataDictionary that, List<SelectListItem> options)
        {
            that[PropertyNames.OpenUserGroupOptions] = options;
        }

        public static void SetProgrammingLanguageOptions(this ViewDataDictionary that, List<SelectListItem> options)
        {
            that[PropertyNames.ProgrammingLanguageOptions] = options;
        }


    }
}