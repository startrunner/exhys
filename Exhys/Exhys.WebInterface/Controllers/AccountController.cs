using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;

namespace Exhys.WebInterface.Controllers
{
    public class AccountController : Controller
    {
        // GET: 
        public ActionResult Index()
        {
            return Redirect("~/Account/Signin");
        }

        public ActionResult SignIn()
        {
            return PartialView();
        }
    }
}