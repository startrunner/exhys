using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Accounts.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Accounts.Controllers
{
    public class AccountController : Controller
    {
        public const string SessionCookieName = "exhys-session-id";
        
        public UserAccount GetSigneddInUser(HttpRequestBase request)
        {
            HttpCookie cookie = null;
            try
            {
                cookie = request.Cookies.Get(SessionCookieName);
                if (cookie == null) return null;
            }
            catch { return null; }

            Guid sessionId;
            try
            {
                sessionId = Guid.Parse(cookie.Value);
            }
            catch { return null; }

            using (var db = new ExhysContestEntities())
            {
                var sessions = db
                    .UserSessions
                    .Where(s =>
                        s.Id == sessionId &&
                        s.BrowserName == request.Browser.Browser &&
                        s.UserAgentString == request.UserAgent)
                    .Take(1)
                    .ToList();
                if (sessions == null || sessions.Count == 0) return null;
                else return sessions[0].UserAccount;
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Register (UserAccountViewModel vm)
        {
            var errors = vm.GetRegistrationErrors();
            if (errors != null)
            {
                ViewBag.Errors = errors;
                return PartialView(vm);
            }


            using (var db = new DataModels.ExhysContestEntities())
            {
                var users = db.UserAccounts.Where(u => u.Username == vm.Username).Take(1).ToList();
                if(users!=null&&users.Count!=0)
                {
                    ViewBag.Errors = new[] { "That username is already taken." };
                    return PartialView(vm);
                }
                else
                {
                    var user = new UserAccount()
                    {
                        Username = vm.Username,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Password = vm.Password
                    };
                    db.UserAccounts.Add(user);
                    db.SaveChanges();
                }
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult SignIn(UserAccountViewModel vm)
        {
            var errors = vm.GetSignInErrors();
            if(errors!=null)
            {
                ViewBag.Errors = errors;
                return PartialView(vm);
            }

            using (var db = new DataModels.ExhysContestEntities())
            {
                var users = db.UserAccounts.Where(u => u.Username == vm.Username).Take(1).ToList();

                if (users == null || users.Count == 0) goto invalid_credentials;
                else if (users[0].Password != vm.Password) goto invalid_credentials;

                //Credentials are valid, create a new session
                var session = new UserSession()
                {
                    UserAccount = users[0],
                    UserAgentString = Request.UserAgent,
                    BrowserName = Request.Browser.Browser,
                    IPAdress = Request.UserHostAddress,
                    Id=Guid.NewGuid()
                };
                db.UserSessions.Add(session);
                db.SaveChanges();
                Response.SetCookie(new HttpCookie(SessionCookieName, session.Id.ToString()) { Expires=DateTime.Now + TimeSpan.FromDays(30) });
            }

            return PartialView(vm);

            invalid_credentials:
            //Sorry boot goto.
            ViewBag.Errors = new[] { "Invalid Credentials. Please try again" };
            return PartialView(vm);
        }

        [HttpGet]
        public new ActionResult Profile()
        {
            var user = GetSigneddInUser(Request);
            return PartialView(new UserAccountViewModel(user));
        }
    }
}