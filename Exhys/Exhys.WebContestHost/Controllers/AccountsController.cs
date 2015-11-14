using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Controllers
{
    public class AccountsController : ExhysController
    {
        [HttpGet]
        public ActionResult Register ()
        {
            AddOpenUserGroupOptions();

            if (Request.GetSignedInUser() != null)
            {
                return RedirectToAction("Profile");
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult Register (UserAccountViewModel vm)
        {
            if (Request.GetSignedInUser() != null)
            {
                return RedirectToAction("Profile");
            }

            if (!vm.ValidateForRegistration(ViewData))
            {
                AddOpenUserGroupOptions();
                return PartialView(vm);
            }


            using (var db = new ExhysContestEntities())
            {
                var users = db.UserAccounts.Where(u => u.Username == vm.Username).Take(1).ToList();
                if (users != null && users.Count != 0)
                {
                    ViewData.ModelState.AddModelError("username-taken", "That username already exists.");//
                    return PartialView(vm);
                }

                var group = db.UserGroups.Where(g => g.Id == vm.GroupId).FirstOrDefault();
                var user = new UserAccount()
                {
                    Username = vm.Username,
                    FullName = vm.FullName,
                    Password = vm.Password
                };
                user.UserGroup = group;
                db.UserAccounts.Add(user);
                db.SaveChanges();
                return SignIn(vm);
            }
            
        }

        [HttpGet]
        public ActionResult SignIn ()
        {
            if (Request.GetSignedInUser() != null)
            {
                return RedirectToAction("Profile");
            }
            else
            {
                IEnumerable<FormErrors.FormError> errors = TempData[FormErrors.DictionaryKey] as IEnumerable<FormErrors.FormError>;
                ViewData.ModelState.AddModelErrorRange(errors);
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult SignIn (UserAccountViewModel vm)
        {
            if (Request.GetSignedInUser() != null)
            {
                return RedirectToAction("Profile");
            }
            if (!vm.ValidateForSignIn(ViewData))
            {
                AddOpenUserGroupOptions();
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
                    Id = Guid.NewGuid()
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
        public new ActionResult Profile ()
        {
            var user = Request.GetSignedInUser();
            if (user == null) return RedirectToAction("SignIn");
            return PartialView(new UserAccountViewModel(user));
        }

        [HttpGet]
        public ActionResult SignOut ()
        {
            Response.DeleteSessionCookie();
            return RedirectToAction("SignIn");
        }
    }
}