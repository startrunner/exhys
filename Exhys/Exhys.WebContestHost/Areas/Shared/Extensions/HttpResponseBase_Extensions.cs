using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class HttpResponseBase_Extensions
    {
        public static void DeleteSessionCookie(this HttpResponseBase response)
        {
            response.Cookies.Set(new HttpCookie(CookieNames.SessionCookieName, "")
            {
                Expires = DateTime.Now + TimeSpan.FromDays(-1)
            });
        }

        public static void SetSessionCookie (this HttpResponseBase response, Guid sessionId)
        {
            response.SetCookie(
                new HttpCookie(CookieNames.SessionCookieName, sessionId.ToString())
                {
                    Expires = DateTime.Now + TimeSpan.FromDays(30)
                });
        }
    }
}