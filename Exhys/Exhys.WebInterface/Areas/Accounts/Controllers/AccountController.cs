using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;

namespace Exhys.WebContestHost.Areas.Accounts.Controllers
{
    public class AccountController : Controller
    {
        // GET: Accounts/Account
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult CreateNew()
        {
            Request.Form.
            return View();
        }
    }
}