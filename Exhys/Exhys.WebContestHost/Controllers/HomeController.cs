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
        [HttpGet]
        public ActionResult Index () => RedirectToAction("List", "Participation");
        [HttpGet]
        public ActionResult Footer () => PartialView();
    }
}