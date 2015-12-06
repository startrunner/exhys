using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.Areas.Shared;
using Exhys.WebContestHost.Areas.Shared.Mvc;
using Exhys.WebContestHost.Areas.Shared.ViewModels;
using Exhys.WebContestHost.DataModels;
using Exhys.WebContestHost.DataModels.DataModels;

namespace Exhys.WebContestHost.Areas.Administration.Controllers
{
    [AuthorizeExhysAdmin]
    public class ProgrammingLanguagesController : ExhysController
    {
        [HttpGet]
        public ActionResult List ()
        {
            var vm = new List<ProgrammingLanguageViewModel>();
            using (var db = new ExhysContestEntities())
            {
                db.ProgrammingLanguages
                    .ToList()
                    .ForEach(l => vm.Add(new ProgrammingLanguageViewModel(l)));
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult List (List<ProgrammingLanguageViewModel> vm)
        {
            using (var db = new ExhysContestEntities())
            {
                foreach (var cvm in vm)
                {
                    var lang = db.ProgrammingLanguages
                        .Where(l => l.Alias == cvm.Alias)
                        .FirstOrDefault();
                    if (!cvm.RequestDelete)
                    {
                        if (cvm.Validate(ViewData))
                        {
                            lang.Description = cvm.Description;
                            lang.Name = cvm.Name;
                        }
                    }
                    else
                    {
                        db.ProgrammingLanguages.Remove(lang);
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("List");//Get 
        }

        [HttpGet]
        public ActionResult Add ()
        { return View(); }

        [HttpPost]
        public ActionResult Add (ProgrammingLanguageViewModel vm)
        {
            if (!vm.Validate(ViewData))
            {
                return View();
            }
            using (var db = new ExhysContestEntities())
            {
                var existingLanguage = db.ProgrammingLanguages
                    .Where(l => l.Alias == vm.Alias)
                    .FirstOrDefault();
                if (existingLanguage != null)
                {
                    ViewData.ModelState.AddModelError(FormErrors.LanguageAliasTaken);
                    return View(vm);
                }
                else
                {
                    var language = new ProgrammingLanguage()
                    {
                        Alias = vm.Alias,
                        Description = vm.Description,
                        Name = vm.Name
                    };
                    db.ProgrammingLanguages.Add(language);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}