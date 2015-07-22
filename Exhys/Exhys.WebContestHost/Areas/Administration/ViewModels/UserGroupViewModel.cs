using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Administration.ViewModels
{
    public class UserGroupViewModel
    {
        public UserGroupViewModel () : this(null) { }
        public UserGroupViewModel(UserGroup model)
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

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsOpen { get; set; }
        public bool RequestDelete { get; set; }
    }
}