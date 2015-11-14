using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class CompetitionViewModel
    {
        private Competition _model;

        public List<ProblemViewModel> Problems { get; private set; }
        public CompetitionViewModel IncludeProblems()
        {
            if (_model != null)
            {
                Problems = new List<ProblemViewModel>();
                if (_model.Problems != null)
                {
                    Problems.AddRange(_model.Problems.Select(x => new ProblemViewModel(x)));
                }
            }
            return this;
        }
        public CompetitionViewModel IncludeProblemStatements()
        {
            if (Problems == null) throw new InvalidOperationException("The problems must be included before theor statements.");

            foreach (var p in Problems) p.IncludeStatements();

            return this;
        }

        private string _description;

        public int Id { get;  set; }
        public int? GroupId { get; set; }

        public string Name { get; set; }

        public string Description
        {
            get { return _description; }
            set { _description = value!=null?value.Trim():null; }
        }

        public bool RequestDelete { get; set; }
        public bool IsUserParticipating { get; set; }

        public CompetitionViewModel () : this(null) { }

        public CompetitionViewModel (Competition model)
        {
            this.RequestDelete = false;

            if (model != null)
            {
                this._model = model;
                this.Name = model.Name;
                this.Description = model.Description;
                this.Id = model.Id;
                if (model.UserGroup != null) this.GroupId = model.UserGroup.Id;
            }
        }

        public bool Validate(ViewDataDictionary viewData)
        {
            if(Name==null  || Name.Length< DatabaseLimits.CompetitionName_MinLength)
            {
                viewData.ModelState.AddModelError(FormErrors.CompetitionNameTooShort);
            }
            else if(Name.Length>DatabaseLimits.CompetitionName_MaxLength)
            {
                viewData.ModelState.AddModelError(FormErrors.CompetitionNameTooLong);
            }

            return viewData.ModelState.IsValid;
        }

        //public bool ValidateForCreation()
    }
}