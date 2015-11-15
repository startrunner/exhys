using System;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class HttpResponseBase_Extensions
    {
        public static void DeleteSessionCookie(this HttpResponseBase response)
        {
            response.Cookies.Set(new HttpCookie(CookieNames.SessionCookie, "")
            {
                Expires = DateTime.Now + TimeSpan.FromDays(-1)
            });
        }

        public static void SetSessionCookie (this HttpResponseBase response, Guid sessionId)
        {
            response.SetCookie(
                new HttpCookie(CookieNames.SessionCookie, sessionId.ToString())
                {
                    Expires = DateTime.Now + TimeSpan.FromDays(30)
                });
        }

        public static void SetCurrentCompetitionCookie(this HttpResponseBase that, int competitionId)
        {
            that.SetCookie(
                new HttpCookie(CookieNames.CurrentCompetitionCookie, competitionId.ToString())
                { Expires = DateTime.Now + TimeSpan.FromDays(30) });
        }
    }
}