using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using ExhaustedEntertainment.Hasher;

namespace Exhys.WebContestHost.Areas.Accounts.Controllers
{
    public class AccountController : Controller
    {
        // GET: Accounts/Account
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult CreateNew ()
        {
            if (Request.Form.HasKeys() && Request.Form["isSubmit"] == "1")
            {
                using (var db = new Exhys.WebContestHost.WebContestHostDataModelContainer())
                {
                    /*if (db.UserAccounts.Where(ua => ua.Login == Request.Form["username"]).ToList().Count != 0)
                    {
                        return View();
                    }*/
                    //else
                    {
                        UserAccount acc = new UserAccount()
                        {
                            Login = Request.Form["username"],
                            PasswordHash = ExhaustedHasher.CalculateMD5(Request.Form["password"]),
                            FirstName = Request.Form["firstName"],
                            LastName = Request.Form["lastName"]
                        };
                        db.UserAccounts.Add(acc);
                        db.SaveChanges();
                    }
                }

                return View();
            }
            
            return PartialView();
        }
    }
}