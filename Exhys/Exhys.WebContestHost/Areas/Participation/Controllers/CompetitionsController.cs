using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exhys.WebContestHost.Areas.Participation.Controllers
{
    public class CompetitionsController : Controller
    {
        // GET: Participation/Competitions
        public ActionResult Index()
        {
            return View();
        }
    }
}