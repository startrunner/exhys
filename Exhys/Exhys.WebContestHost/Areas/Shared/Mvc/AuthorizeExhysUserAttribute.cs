using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.Mvc
{
    public class AuthorizeExhysUserAttribute:AuthorizeAttribute
    {
        ControllerBase controller = null;
        
        public override void OnAuthorization (AuthorizationContext filterContext)
        {
                this.controller = filterContext.Controller;
                base.OnAuthorization(filterContext);
                this.controller = null;
        }

        protected override bool AuthorizeCore (HttpContextBase httpContext)
        {
            using (var db = new ExhysContestEntities())
            {
                var user = httpContext.Request.GetSignedInUserQuery(db)
                    .Include(u => u.UserGroup)
                    .FirstOrDefault();
                if (user != null)
                {
#if DEBUG
                    Debug.WriteLine("Authorizing user {0} as signed in", user.Username);
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
            filterContext.Controller.TempData.SetRedirectsBackTo(filterContext.RequestContext.RouteData);
            filterContext.Result = new RedirectResult("~/accounts/signin");
        }
    }
}