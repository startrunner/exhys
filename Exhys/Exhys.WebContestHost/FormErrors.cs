using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared
{
    public static class FormErrors
    {
        public static class Actions
        {
            public const string ParticipateInACompetition = "participate in a competition";
        }
        public class FormError
        {
            public string Key { get; set; }
            public string ErrorMessage { get; set; }
            public FormError (string key, string errorMessage)
            {
                this.Key = key;
                this.ErrorMessage = errorMessage;
            }
        }

        public static void AddModelError (this ModelStateDictionary that, FormError error)
        {
            that.AddModelError(error.Key, error.ErrorMessage);
        }

        public static void AddModelErrorRange(this ModelStateDictionary that, IEnumerable<FormError> errors)
        {
            if(errors!=null)
            {
                foreach (var v in errors) that.AddModelError(v);
            }
        }

        public static readonly FormError UsernameTooLong = new FormError("username-too-long", string.Format("A username must be at most {0} characters long.", DatabaseLimits.UserGroupName_MaxLength));
        public static readonly FormError UsernameTooShort = new FormError("username-too-short", string.Format("A username must be at least {0} characters long.", DatabaseLimits.UserGroupName_MaxLength));

        public static readonly FormError PasswordTooLong = new FormError("password-too-long", string.Format("A password must be at most {0} characters long.", DatabaseLimits.Password_MaxLength));
        public static readonly FormError PasswordTooShort = new FormError("password-too-short", string.Format("A password must be at least {0} characters long.", DatabaseLimits.Password_MinLength));

        public static readonly FormError UserGroupNameTooLong = new FormError("group-name-too-long", string.Format("A group name must be at most {0} characters long.", DatabaseLimits.UserGroupName_MaxLength));
        public static readonly FormError UserGroupNameTooShort = new FormError("group-name-too-short", string.Format("A group name must be at least {0} characters long.", DatabaseLimits.UserGroupName_MinLength));

        public static readonly FormError ProblemNameTooLong = new FormError("problem-name-too-long", string.Format("A problem name must be at most {0} characters long.", DatabaseLimits.ProblemName_MaxLength));
        public static readonly FormError ProblemNameTooShort = new FormError("problem-name-too-short", string.Format("A problem name must be at least {0} characters long.", DatabaseLimits.ProblemName_MinLength));

        public static readonly FormError HumanNameTooLong = new FormError("human-name-too-long", string.Format("A name must be at mst {0} characters long.", DatabaseLimits.HumanName_MaxLength));
        //public static readonly FormError HumanNameTooShort = new FormError("human-name-too-short", string.Format("A name must be at least {0} characters long.", DatabaseLimits.HumanName_MinLength));

        public static readonly FormError CompetitionNameTooLong = new FormError("competiton-name-too-long", string.Format("A competition name must be at most {0} characters long.", DatabaseLimits.CompetitionName_MaxLength));
        public static readonly FormError CompetitionNameTooShort = new FormError("competition-name-too-short", string.Format("A competition name must be at least {0} character long.", DatabaseLimits.CompetitionName_MinLength));

        public static readonly FormError NoGroupSelected = new FormError("null-group", "You must select a user group.");

        public static readonly FormError PasswordMismatch = new FormError("password-mismatch", "The two passwords must match.");

        public static readonly FormError FileCountMismatch = new FormError("file-count-mismatch", "The number of files for each input must be equal.");

        public static readonly FormError InvalidCredentials = new FormError("invalid-credentials", "These credentials are invalid. Please check your username and password.");

        //public static readonly FormError LanguageAliasTooShort = new FormError("language-alias-too-short", string.Format("A language alias must be at least {0} characters long.", DatabaseLimits.LanguageAlias_MinLength));
        public static readonly FormError LanguageAliasTaken = new FormError("language-alias-taken", "A language alias must be unique. this one is already taken.");
        public static readonly FormError LanguageAliasRequired = new FormError("language-alias-required", "A programming language must have a unique alias.");
        public static readonly FormError LanguageAliasTooLong = new FormError("language-alias-too-long", string.Format("A language alias can be at most {0} characters long.", DatabaseLimits.LanguageAlias_MaxLength));

        public static readonly FormError LanguageNameRequired = new FormError("language-name-required", "A language must have a name.");
        public static readonly FormError LanguageNameTooLong = new FormError("language-name-too-long", string.Format("A language name can be at most {0} characters long.", DatabaseLimits.LanguageName_MaxLength));

        public static FormError SignInRequired(string forWhatAction)
        {
            return new FormError("sign-in-required", string.Format("You need to be signed in in order to {0}", forWhatAction));
        }
    }
}