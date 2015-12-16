using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;
using System.Web.Mvc;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class UserAccountViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public bool RequestDelete { get; set; }
        public int UserId { get;  set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }

        public UserAccountViewModel () : this(null) { }
        public UserAccountViewModel (UserAccount model)
        {
            if (model != null)
            {
                Username = model.Username;
                FullName = model.FullName;
                Password = model.Password;
                if (model.UserGroup != null)
                {
                    this.IsAdmin = model.UserGroup.IsAdministrator;
                    this.GroupId = model.UserGroup.Id;
                    this.GroupName = model.UserGroup.Name;
                }
                this.UserId = model.Id;
            }
            else
            {
                this.IsAdmin = false;
            }
            this.RequestDelete = false;
        }

        private void ValidateUsername(ViewDataDictionary viewData)
        {
            if (Username == null || Username.Length < DatabaseLimits.Username_MinLength)
            {
                viewData.ModelState.AddModelError(FormErrors.UsernameTooShort);
            }
            else if (Username.Length > DatabaseLimits.Username_MaxLength)
            {
                viewData.ModelState.AddModelError(FormErrors.UsernameTooLong);
            }
        }

        private void ValidateNameAndGroup(ViewDataDictionary viewData)
        {
            if ((FullName != null && FullName.Length > DatabaseLimits.HumanName_MaxLength))
            {
                viewData.ModelState.AddModelError(FormErrors.HumanNameTooLong);
            }

            if (GroupId == null)
            {
                viewData.ModelState.AddModelError(FormErrors.NoGroupSelected);
            }
        }

        public bool ValidateForRegistration (ViewDataDictionary viewData)
        {
            ValidateUsername(viewData);

            if (Password == null || Password.Length < DatabaseLimits.Password_MinLength)
            {
                viewData.ModelState.AddModelError(FormErrors.PasswordTooShort);
            }
            else if (Password.Length > DatabaseLimits.Password_MaxLength)
            {
                viewData.ModelState.AddModelError(FormErrors.PasswordTooLong);
            }

            if (Password != ConfirmPassword)
            {
                viewData.ModelState.AddModelError(FormErrors.PasswordMismatch);
            }

            ValidateNameAndGroup(viewData);

            return viewData.ModelState.IsValid;
        }

        public bool ValidateForEdit (ViewDataDictionary viewData)
        {
            if (!string.IsNullOrEmpty(Password))
            {
                if (Password.Length < DatabaseLimits.Password_MinLength)
                {
                    viewData.ModelState.AddModelError(FormErrors.PasswordTooShort);
                }
                else if (Password.Length > DatabaseLimits.Password_MaxLength)
                {
                    viewData.ModelState.AddModelError(FormErrors.PasswordTooLong);
                }
                if (Password != ConfirmPassword)
                {
                    viewData.ModelState.AddModelError(FormErrors.PasswordMismatch);
                }
            }

            ValidateNameAndGroup(viewData);

            return viewData.ModelState.IsValid;
        }

        public bool ValidateForSignIn(ViewDataDictionary viewData)
        {
            ValidateUsername(viewData);

            if (Password == null || Password.Length < DatabaseLimits.Password_MinLength)
            {
                viewData.ModelState.AddModelError(FormErrors.PasswordTooShort);
            }
            else if (Password.Length > DatabaseLimits.Password_MaxLength)
            {
                viewData.ModelState.AddModelError(FormErrors.PasswordTooLong);
            }

            return viewData.ModelState.IsValid;
        }
    }
}