using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Administration.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Administration.Models
{
    public class UserAccountsController : Controller
    {
		[HttpGet]
        public ActionResult List()
        {
			using (var db=new ExhysContestEntities())
            {
                var vm = new List<UserAccountViewModel>();
                db.UserAccounts.ToList().ForEach((acc) => { vm.Add(new UserAccountViewModel(acc)); });
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult List (List<UserAccountViewModel> vm)
        {
            return View(vm);
        }
    }
}