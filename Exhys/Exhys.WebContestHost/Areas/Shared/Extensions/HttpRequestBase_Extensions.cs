using System;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class HttpRequestBase_Extensions
    {
        public static UserAccount GetSignedInUser(this HttpRequestBase that)
        {
            using (var db = new ExhysContestEntities())
            {
                return that.GetSignedInUser(db);
            }
        }
        public static UserAccount GetSignedInUser (this HttpRequestBase request, ExhysContestEntities db)
        {
            Guid? sessionId = request.GetSessionCookie();
            if (sessionId == null) return null;
            var session = db
                .UserSessions
                .Where(s =>
                    s.Id == sessionId &&
                    s.BrowserName == request.Browser.Browser &&
                    s.UserAgentString == request.UserAgent).FirstOrDefault();
            if (session == null) return null;
            else
            {
                db.SaveChanges();
                return session.UserAccount;
            }
            
        }

        public static Guid? GetSessionCookie(this HttpRequestBase req)
        {
            try
            {
                return Guid.Parse(req.Cookies.Get(CookieNames.SessionCookie).Value);
            }
            catch
            {
                return null;
            }
        }

        public static int? GetCurrentCompetitionCookie(this HttpRequestBase that)
        {
            try
            {
                return int.Parse(that.Cookies.Get(CookieNames.CurrentCompetitionCookie).Value);
            }
            catch
            {
                return null;
            }
        }

    }
}