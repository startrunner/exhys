using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels.Embedded
{
    public class UserSessionViewModel
    {
        public bool IsGuest { get; private set; }
        public string FullName { get; private set; }

        public UserSessionViewModel(UserSession model)
        {
            if (model != null)
            {
                try
                {
                    var user = model.UserAccount;
                    this.FullName = user.GetFullName();
                    this.IsGuest = false;
                }
                catch { this.IsGuest = true; }
            }
            else { this.IsGuest = true; }
        }

        public UserSessionViewModel (UserAccount model)
        {
            if (model != null)
            {
                this.FullName = model.GetFullName();
                this.IsGuest = false;
            }
            else { this.IsGuest = true; }
        }

        public UserSessionViewModel()
        {
            this.IsGuest = true;
        }
    }
}