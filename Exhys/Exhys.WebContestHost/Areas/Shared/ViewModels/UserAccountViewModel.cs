﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class UserAccountViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool RequestDelete { get; set; }
        public int UserId { get;  set; }
        public int GroupId { get; set; }

        public UserAccountViewModel () : this(null) { }
        public UserAccountViewModel (UserAccount model)
        {
            if (model != null)
            {
                Username = model.Username;
                Password = model.Password;
                FirstName = model.FirstName;
                LastName = model.LastName;
                this.IsAdmin = model.IsAdmin();
                this.UserId = model.Id;
                if(model.UserGroup!=null)this.GroupId = model.UserGroup.Id;
            }
            else
            {
                this.IsAdmin = false;
            }
            this.RequestDelete = false;
        }

        public IEnumerable<string> GetRegistrationErrors ()
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

        public IEnumerable<string> GetSignInErrors ()
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