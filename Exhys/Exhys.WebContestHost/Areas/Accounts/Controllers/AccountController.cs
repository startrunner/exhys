using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Accounts.ViewModels;
using WebContestDataModels;

namespace Exhys.WebContestHost.Areas.Accounts.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index ()
        {
            return Content("Hello, World!");
        }

        [HttpGet]
        public ActionResult CreateAccount()
        {
            return PartialView(new UserAccountViewModel());
        }

        [HttpPost]
        public ActionResult CreateAccount(UserAccountViewModel vm)
        {
            var errors = vm.CheckValidity();
            if (errors != null)
            {
                ViewBag.Errors = errors;
                return PartialView(vm);
            }
            else
            {
                using (var db=new WebContestDataModels.)
                {

                }

                return PartialView();
            }
        }
    }
}
