using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Administration.ViewModels
{
    public class UserAccountViewModel
    {
        public UserAccountViewModel () : this(null) { }
        public UserAccountViewModel (UserAccount model)
        {
            if (model != null)
            {
                this.UserId = model.Id;
                this.FirstName = model.FirstName;
                this.LastName = model.LastName;
                this.Username = model.Username;
                this.Password = model.Password;
            }
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}