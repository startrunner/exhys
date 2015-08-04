using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Participation.Controllers
{
    public class CompetitionsController : ExhysController
    {
        [HttpGet]
        public ActionResult List()
        {

            AddSignedInUserInformation();

            var vm = new List<CompetitionViewModel>();
            using (var db = new ExhysContestEntities())
            {
                var user = Request.GetSignedInUser(db);
                if (user == null) return Content("sry m8");
                else
                {
                    var competitions = user.UserGroup.AvaiableCompetitions;
                    foreach (var c in competitions)
                        vm.Add(new CompetitionViewModel(c));
                }
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Join (int? id)
        {
            AddSignedInUserInformation();

            if (id == null) return RedirectToAction("List");
            else
            {
                using (var db = new ExhysContestEntities())
                {
                    var competition = db.Competitions.Where(c => c.Id == id).Take(1).ToList()[0];
                    var vm = new CompetitionViewModel(competition);
                    return View(vm);
                }
            }
        }

        [HttpPost]
        public ActionResult Join(int id)
        {
            return RedirectToAction("Participate", new { id = id });
        }

        [HttpGet]

        public ActionResult Participate(int? id=null)
        {
            AddSignedInUserInformation();


            if(id==null)
            {
                id = Request.GetCurrentCompetitionCookie();
                if (id == null) return RedirectToAction("List");
            }
            else
            {
                Response.SetCurrentCompetitionCookie((int)id);
                using (var db = new ExhysContestEntities())
                {
                    var competition = db.Competitions.Where(c => c.Id == id).Take(1).ToList().FirstOrDefault();
                    var vm = new CompetitionViewModel(competition);
                    return View(vm);
                }
            }
            return View();
        }
    }
}