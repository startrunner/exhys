using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class SignedInUserViewModel
    {
        public bool IsGuest { get; private set; }
        public string FullName { get; private set; }

        public SignedInUserViewModel(UserSession model)
        {
            if (model != null)
            {
                try
                {
                    var user = model.UserAccount;
                    this.FullName = model.UserAccount.FullName;
                    this.IsGuest = false;
                }
                catch { this.IsGuest = true; }
            }
            else { this.IsGuest = true; }
        }

        public SignedInUserViewModel (UserAccount model)
        {
            if (model != null)
            {
                this.FullName = model.FullName;
                this.IsGuest = false;
            }
            else { this.IsGuest = true; }
        }

        public SignedInUserViewModel()
        {
            this.IsGuest = true;
        }
    }
}