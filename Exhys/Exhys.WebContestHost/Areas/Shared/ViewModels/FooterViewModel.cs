using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{ 
    public class FooterViewModel
    {
        public bool IsAdmin { get; set; }
		public bool IsSignedIn { get; set; }
		public string UserName { get; set; }
		public bool WithUsernameInDropdown { get; set; }
		public bool WithUsernameInFront { get; set; }
		public FooterViewModel()
        {

        }
    }
}