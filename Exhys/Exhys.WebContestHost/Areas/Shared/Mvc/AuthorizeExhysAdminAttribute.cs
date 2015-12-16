using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.Mvc
{
    public class AuthorizeExhysAdminAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore (HttpContextBase httpContext)
        {
            using (var db = new ExhysContestEntities())
            {
                var user = httpContext.Request.GetSignedInUserQuery(db)
                    .Include(u=>u.UserGroup)
                    .FirstOrDefault();
                if (user!=null && user.UserGroup.IsAdministrator)
                {
#if DEBUG
                    Debug.WriteLine("Authorizing user {0} as an administrator", user.Username);
#endif
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected override void HandleUnauthorizedRequest (AuthorizationContext filterContext)
        {
            var route= filterContext.RequestContext.RouteData;
            filterContext.Controller.TempData.SetRedirectsBackTo(route);
            filterContext.Result = new RedirectResult("~/accounts/signin");
            //base.HandleUnauthorizedRequest(filterContext);
        }
    }
}