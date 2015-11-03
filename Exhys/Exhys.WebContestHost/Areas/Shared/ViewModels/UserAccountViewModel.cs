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

        public UserAccountViewModel () : this(null) { }
        public UserAccountViewModel (UserAccount model)
        {
            if (model != null)
            {
                Username = model.Username;
                Password = model.Password;
                FullName = model.FullName;
                this.IsAdmin = model.UserGroup.IsAdministrator;
                this.UserId = model.Id;
                if(model.UserGroup!=null)this.GroupId = model.UserGroup.Id;
            }
            else
            {
                this.IsAdmin = false;
            }
            this.RequestDelete = false;
        }

        public bool ValidateForRegistration(ViewDataDictionary viewData)
        {
            if (Username == null || Username.Length < 4)
            {
                viewData.ModelState.AddModelError("short-username", "A username is required and must be at least 4 characters long.");
            }
            else if(Username.Length>32)
            {
                viewData.ModelState.AddModelError("long-username", "A username cannot be longer than 32 characters.");
            }

            if(Password==null || Password.Length<6)
            {
                viewData.ModelState.AddModelError("short-password", "A password must be at least 6 characters long.");
            }
            else if(Password.Length>32)
            {
                viewData.ModelState.AddModelError("long-password", "A password cannot be longer than 32 characters.");
            }

            if(Password!=ConfirmPassword)
            {
                viewData.ModelState.AddModelError("unmatching-passwords", "The two passwords must match.");
            }

            if ((FullName != null && FullName.Length > 32))
            {
                viewData.ModelState.AddModelError("long-name", "A name cannot be longer than 32 characters");
            }

            if(GroupId==null)
            {
                viewData.ModelState.AddModelError("null-group", "You must select a group");
            }
            return viewData.ModelState.IsValid;
        }

        public bool ValidateForSignIn(ViewDataDictionary viewData)
        {
            if (Username == null || Username.Length < 4)
            {
                viewData.ModelState.AddModelError("short-username", "A username is required and must be at least 4 characters long.");
            }
            else if (Username.Length > 32)
            {
                viewData.ModelState.AddModelError("long-username", "A username cannot be longer than 32 characters.");
            }

            if (Password == null || Password.Length < 6)
            {
                viewData.ModelState.AddModelError("short-password", "A password must be at least 6 characters long.");
            }
            else if (Password.Length > 32)
            {
                viewData.ModelState.AddModelError("long-password", "A password cannot be longer than 32 characters.");
            }

            return viewData.ModelState.IsValid;
        }
    }
}