using System;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;
using System.Data.Entity;
using System.Collections.Generic;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class HttpRequestBase_Extensions
    {
        public static IQueryable<UserAccount> GetSignedInUserQuery(this HttpRequestBase request, ExhysContestEntities db)
        {
            IQueryable<UserAccount> empty = new List<UserAccount>().AsQueryable();
            Guid? sessionId = request.GetSessionCookie();
            if (sessionId == null) return empty;
            UserSession session = db
                .UserSessions
                .Where(s =>
                    s.Id == sessionId &&
                    s.BrowserName == request.Browser.Browser &&
                    s.UserAgentString == request.UserAgent)
                    .Include(x => x.UserAccount.UserGroup)
                    .FirstOrDefault();
            if (session == null) return empty;
            else
            {
                return db.UserAccounts.Where(a => a.Id == session.UserAccount.Id);
            }
        }

        [Obsolete]
        public static IQueryable<UserAccount> GetSignedInUserQuery(this HttpRequestBase that)
        {
            using (var db = new ExhysContestEntities())
            {
                return that.GetSignedInUserQuery(db);
            }
        }

        [Obsolete]
        public static UserAccount GetSignedInUser(this HttpRequestBase that)
        {
            using (var db = new ExhysContestEntities())
            {
                return that.GetSignedInUser(db);
            }
        }

        [Obsolete]
        public static UserAccount GetSignedInUser (this HttpRequestBase request, ExhysContestEntities db)
        {
            Guid? sessionId = request.GetSessionCookie();
            if (sessionId == null) return null;
            var session = db
                .UserSessions
                .Where(s =>
                    s.Id == sessionId &&
                    s.BrowserName == request.Browser.Browser &&
                    s.UserAgentString == request.UserAgent)
                    .Include(x => x.UserAccount.UserGroup)
                    .FirstOrDefault();
            if (session == null) return null;
            else
            {
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