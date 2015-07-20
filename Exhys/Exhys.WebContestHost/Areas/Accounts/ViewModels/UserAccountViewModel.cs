using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Accounts.ViewModels
{
    public class UserAccountViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsGuest { get; private set; }

        public UserAccountViewModel () : this(null) { }

        public UserAccountViewModel(UserAccount model)
        {
            if (model != null)
            {
                Username = model.Username;
                Password = model.Password;
                FirstName = model.FirstName;
                LastName = model.LastName;
                IsGuest = false;
            }
            else
            {
                IsGuest = true;
            }
        }

        public IEnumerable<string> GetRegistrationErrors()
        {
            var rt=new LinkedList<string>();

            if(Username==null||Username.Length<4)
            {
                rt.AddLast("A username is required and must be at least 4 characters long.");
            }
            else if(Username.Length>32)
            {
                rt.AddLast("A username cannot be longer than 32 characters");
            }

            if (Password == null || Password.Length < 6)
            {
                rt.AddLast("A password is required and it must be at least 6 characters long.");
            }
            else if (Password.Length > 32)
            {
                rt.AddLast("A password cannot be longer than 32 characters.");
            }
            else if (Password != ConfirmPassword)
            {
                rt.AddLast("The two passwords must match.");
            }

            else if ((FirstName != null && FirstName.Length > 32) | (LastName != null && LastName.Length > 32))
            {
                rt.AddLast("A name cannot be longer than 32 characters.");
            }

            if (rt.Count == 0)
            {
                rt.Clear();
                return null;
            }
            else return rt;
        }

        public IEnumerable<string> GetSignInErrors()
        {
            var rt = new LinkedList<string>();

            if (Username == null || Username.Length < 4)
            {
                rt.AddLast("A username is required and must be at least 4 characters long.");
            }
            else if (Username.Length > 32)
            {
                rt.AddLast("A username cannot be longer than 32 characters");
            }

            if (Password == null || Password.Length < 6)
            {
                rt.AddLast("A password is required and it must be at least 6 characters long.");
            }
            else if (Password.Length > 32)
            {
                rt.AddLast("A password cannot be longer than 32 characters.");
            }

            if (rt.Count == 0)
            {
                rt.Clear();
                return null;
            }
            else return rt;
        }
    }
}