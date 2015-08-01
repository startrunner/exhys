using System.Web.Mvc;

namespace Exhys.WebContestHost.Areas.Participation
{
    public class ParticipationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Participation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Participation_default",
                "Participation/{controller}/{action}/{id}",
                new { controller="Competitions", action = "Index", id = UrlParameter.Optional }
            );

        }

        
    }
}