using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Mvc;

namespace Exhys.WebContestHost.Controllers
{
    public class HomeController : ExhysController
    {
        public ActionResult Index ()
        { 
            return View();
        }

        public ActionResult About ()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact ()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}