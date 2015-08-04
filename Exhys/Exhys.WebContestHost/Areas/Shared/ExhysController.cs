using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared
{
    public class ExhysController:Controller
    {
        public void AddUserGroupOptions(bool allowNull=true)
        {
            using (var db = new ExhysContestEntities())
            {
                AddUserGroupOptions(db, allowNull);
            }
        }
        public void AddUserGroupOptions (ExhysContestEntities db, bool allowNull=true)
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
                    Value = g.Id.ToString()
                });
            });
            ViewBag.UserGroupOptions = options;
        }
    }
}