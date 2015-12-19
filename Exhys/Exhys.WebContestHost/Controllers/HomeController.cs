using System.Linq;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Controllers
{
    public class HomeController : ExhysController
    {
        [HttpGet]
        public ActionResult Index () => RedirectToAction("List", "Participation");
        [HttpGet]
        public ActionResult Footer ()
        {
            FooterViewModel vm = new FooterViewModel();
            using (var db = new ExhysContestEntities())
            {
                var user = Request.GetSignedInUserQuery(db).FirstOrDefault();
                if (user != null && user.UserGroup.IsAdministrator) vm.IsAdmin = true;
            }
            return PartialView(vm);
        }
    }
}