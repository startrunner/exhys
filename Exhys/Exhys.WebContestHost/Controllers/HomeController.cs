using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.DataModels;

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
            using (var db = new ExhysContestEntities())
            {
                RequireSignedInAdministrator(db);
            }
            
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