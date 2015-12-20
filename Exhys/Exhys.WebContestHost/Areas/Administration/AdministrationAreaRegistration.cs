using System.Web.Mvc;

namespace Exhys.WebContestHost.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            } 
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}",
                new { controller="UserAccounts", action = "List"}
            );
        }
    }
}