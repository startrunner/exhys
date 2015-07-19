using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exhys.WebContestHost.Areas.Accounts.ViewModels
{
    public class UserAccountViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public IEnumerable<string> CheckValidity()
        {
            var rt = new LinkedList<string>();
            if (UserName == null || UserName == "")
            {
                rt.AddLast("A username is required.");
            }
            else if(UserName.Length>16)
            {
                rt.AddLast("A username cannot be longer than 16 characters.");
            }

            if (Password == null || Password == "")
            {
                rt.AddLast("A password is required.");
            }
            else if(Password.Length>32)
            {
                rt.AddLast("A password cannot be longer than 32 characters.");
            }

            if(ConfirmPassword!=Password)
            {
                rt.AddLast("The passwords must match");
            }

            if(FirstName!=null&&FirstName.Length>32)
            {
                rt.AddLast("A first name cannot be longer than 32 characters.");
            }
            if(LastName!=null&&LastName.Length>32)
            {
                rt.AddLast("A last name cannot be longer than 32 characters.");
            }

            if (rt.Count == 0) return null;
            else return rt;
        }
    }
}