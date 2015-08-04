using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Accounts.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        { 
            if (Request.GetSignedInUser() != null)
            {
                return RedirectToAction("Profile");
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult Register (UserAccountViewModel vm)
        {
            if (Request.GetSignedInUser()!=null)
            {
                return RedirectToAction("Profile");
            }

            var errors = vm.GetRegistrationErrors();
            if (errors != null)
            {
                ViewBag.Errors = errors;
                return PartialView(vm);
            }


            using (var db = new ExhysContestEntities())
            {
                var users = db.UserAccounts.Where(u => u.Username == vm.Username).Take(1).ToList();
                if(users!=null&&users.Count!=0)
                {
                    ViewBag.Errors = new[] { "That username is already taken." };
                    return PartialView(vm);
                }
                else
                {
                    var group = db.GetDefaultUserGroup();
                    if(group==null)
                    {
                        ViewBag.Errors = new[] { "Registrations are closed." };
                        return PartialView(vm);
                    }
                    var user = new UserAccount()
                    {
                        Username = vm.Username,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Password = vm.Password
                    };
                    user.UserGroup = group;
                    db.UserAccounts.Add(user);
                    db.SaveChanges();
                    return SignIn(vm);
                }
            }
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            if (Request.GetSignedInUser()!=null)
            {
                return RedirectToAction("Profile");
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult SignIn(UserAccountViewModel vm)
        {
            if (Request.GetSignedInUser()!=null)
            {
                return RedirectToAction("Profile");
            }
            var errors = vm.GetSignInErrors();
            if(errors!=null)
            {
                ViewBag.Errors = errors;
                return PartialView(vm);
            }

            using (var db = new ExhysContestEntities())
            {
                var users = db.UserAccounts.Where(u => u.Username == vm.Username).Take(1).ToList();

                if (users == null || users.Count == 0) goto invalid_credentials;//Incorrect username
                else if (users[0].Password != vm.Password) goto invalid_credentials;//Incorrect password

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

                Response.SetSessionCookie(session.Id);
            }

            return RedirectToAction("Profile");

            invalid_credentials:
            ViewBag.Errors = new[] { "Invalid Credentials. Please try again" };
            return PartialView(vm);
        }

        [HttpGet]
        public new ActionResult Profile()
        {
            var user = Request.GetSignedInUser();
            if (user == null) return RedirectToAction("SignIn");
            return PartialView(new UserAccountViewModel(user));
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            Response.DeleteSessionCookie();
            return RedirectToAction("SignIn");
        }
    }
}