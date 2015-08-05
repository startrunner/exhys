using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared.Extensions;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
//using Exhys.WebContestHost.Areas.Shared.ViewModels.Embedded;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.Mvc
{
    public class ExhysController:Controller
    {

        #region SignedInUser
        public void AddSignedInUserInformation ()
        {
            using (var db = new ExhysContestEntities())
            {
                AddSignedInUserInformation(db);
            }
        }

        public void AddSignedInUserInformation (ExhysContestEntities db)
        {
            var user = Request.GetSignedInUser(db);
            var vm = new SignedInUserViewModel(user);
            ViewData.SetSignedInUser(vm);
        }
        #endregion

        #region DropdownOptions
        public void AddProblemOptions(int competitionId)
        {
            using (var db = new ExhysContestEntities())
            {
                AddProblemOptions(db, competitionId);
            }
        }

        public void AddProblemOptions(ExhysContestEntities db, int competitionId)
        {
            var competition = db.Competitions.Where(c => c.Id == competitionId).ToList()[0];
            var problems = competition.Problems.ToList();
            List<SelectListItem> options = new List<SelectListItem>();
            foreach(var p in problems)
            {
                options.Add(new SelectListItem() { Text = p.Name, Value = p.Id.ToString() });
            }
            ViewData.SetProblemOptions(options);
        }

        public void AddUserGroupOptions(bool allowNull=false)
        {
            using (var db = new ExhysContestEntities())
            {
                AddUserGroupOptions(db, allowNull);
            }
        }
        public void AddUserGroupOptions (ExhysContestEntities db, bool allowNull=false)
        {
            List<SelectListItem> options = new List<SelectListItem>();
            if(allowNull)
            {
                options.Add(new SelectListItem() { Text = "[NULL]", Value = null });
            }
            db.UserGroups.ToList().ForEach((g) =>
            {
                options.Add(new SelectListItem()
                {
                    Text = g.Name,
                    Value = g.Id.ToString(),
                });
            });
            ViewData.SetUserGroupOptions(options);
            //ViewBag.UserGroupOptions = options;
        }

        public void AddCompetitionOptions()
        {
            using (var db = new ExhysContestEntities())
            {
                AddCompetitionOptions(db);
            }
        }

        public void AddCompetitionOptions(ExhysContestEntities db)
        {
            var options = new List<SelectListItem>();
            db.Competitions.ToList().ForEach((c) =>
            {
                options.Add(new SelectListItem() { Value = c.Id.ToString(), Text = c.Name });
            });

            //ViewBag.CompetitionOptions = options;
            ViewData.SetCompetitionOptions(options);
        }
        #endregion

        #region UserRequirements
        public void RequireSignedInUser()
        {
            using (var db = new ExhysContestEntities())
            {
                RequireSignedInUser(db);
            }
        }
        public void RequireSignedInUser(ExhysContestEntities db)
        {
            var user = Request.GetSignedInUser(db);
            if(user==null)
            {
                Response.StatusCode = 401;
                Response.Redirect(@"~/WebRoot/ErrorPages/401.html");
                throw new UnauthorizedAccessException();
            }
        }

        public void RequireSignedInAdministrator (ExhysContestEntities db)
        {
            var user = Request.GetSignedInUser(db);
            if (user == null || user.IsAdmin(db) == false)
            {
                Response.StatusCode = 401;
                Response.Redirect(@"~/WebRoot/ErrorPages/401.html");
                throw new UnauthorizedAccessException();
            }
        }

        
        #endregion
    }
}