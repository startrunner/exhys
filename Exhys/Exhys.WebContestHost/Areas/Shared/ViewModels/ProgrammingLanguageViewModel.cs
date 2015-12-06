using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.DataModels;
using Exhys.WebContestHost.DataModels.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class ProgrammingLanguageViewModel
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool RequestDelete { get; set; }

        public ProgrammingLanguageViewModel (ProgrammingLanguage model)
        {
            if (model != null)
            {
                this.Name = model.Name;
                this.Alias = model.Alias;
                this.Description = model.Description;
            }
            this.RequestDelete = false;
        }

        public ProgrammingLanguageViewModel () : this(null) { }

        public bool Validate (ViewDataDictionary viewData)
        {
            Func<FormErrors.FormError, int> add = (FormErrors.FormError e) =>
            {
                viewData.ModelState.AddModelError(e);
                return 0;
            };

            if (Name == null || Name == "")
            {
                add(FormErrors.LanguageNameRequired);
            }
            else if (Name.Length > DatabaseLimits.LanguageName_MaxLength)
            {
                add(FormErrors.LanguageNameTooLong);
            }

            if (Alias == null || Alias == "")
            {
                add(FormErrors.LanguageAliasRequired);
            }
            else if (Alias.Length > DatabaseLimits.LanguageAlias_MaxLength)
            {
                add(FormErrors.LanguageAliasTooLong);
            }

            return viewData.ModelState.IsValid;
        }
    }
}