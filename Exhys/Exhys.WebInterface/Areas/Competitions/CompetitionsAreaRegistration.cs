using System.Web.Mvc;

namespace Exhys.WebContestHost.Areas.Competitions
{
    public class CompetitionsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Competitions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Competitions_default",
                "Competitions/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}