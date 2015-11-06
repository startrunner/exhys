using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class UserGroupViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsOpen { get; set; }
        public bool RequestDelete { get; set; }


        public UserGroupViewModel () : this(null) { }
        public UserGroupViewModel (UserGroup model)
        {
            if (model != null)
            {
                Id = model.Id;
                Name = model.Name;
                Description = model.Description;
                IsAdmin = model.IsAdministrator;
                IsOpen = model.IsOpen;
                RequestDelete = false;
            }
        }

        public bool Validate(ViewDataDictionary viewData)
        {
            if(Name==null || Name.Length < DatabaseLimits.UserGroupName_MinLength)
            {
                viewData.ModelState.AddModelError(FormErrors.UserGroupNameTooShort);
            }
            else if(Name.Length > DatabaseLimits.UserGroupName_MaxLength)
            {
                viewData.ModelState.AddModelError(FormErrors.UserGroupNameTooLong);
            }

            return viewData.ModelState.IsValid;
        }

        
    }
}