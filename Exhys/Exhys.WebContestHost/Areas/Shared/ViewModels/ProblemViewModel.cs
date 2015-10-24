using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class ProblemViewModel
    {
        #region Constants
        public const string InputFilesInputName = "input-input-files";
        public const string SolutionFilesInputName = "input-solution-files";
        public const string StatementFilesInputName = "input-statement-files";
        #endregion

        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to enter time limits for tests.")]
        public string T_TimeLimits { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to set input feedbacks.")]
        public string T_InputFeedbacks { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to set solution feedbacks.")]
        public string T_SolutionFeedbacks { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to set output feedbacks.")]
        public string T_OutputFeedbacks { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to set score feedbacks.")]
        public string T_ScoreFeedbacks { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="You need to set status feedbacks.")]
        public string T_StatusFeedbacks { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to enter a name for the problem.")]
        public string Name { get; set; }

        private Problem _model;

        public int Id { get;  set; }
        public int CompetitionId { get;  set; }
        public string CompetitionName { get; set; }
        public bool RequestDelete { get; set; }

        public List<ProblemStatementViewModel> ProblemStatements { get; set; }
        public ProblemViewModel IncludeStatements()
        {
            ProblemStatements = new List<ProblemStatementViewModel>();
            if (_model != null && _model.ProblemStatements != null)
            {
                foreach (var statement in _model.ProblemStatements)
                {
                    ProblemStatements.Add(new ProblemStatementViewModel(statement));
                }
            }
            return this;
        }

        public ProblemViewModel () : this(null)
        {
        }
        public ProblemViewModel(Problem model)
        {
               
            this.RequestDelete = false;
            this._model = model;

            if (model != null)
            {
                this.Id = model.Id;
                this.Name = model.Name;
                if (model.CompetitionGivenAt != null)
                {
                    this.CompetitionId = model.CompetitionGivenAt.Id;
                    this.CompetitionName = model.CompetitionGivenAt.Name;
                    this.T_TimeLimits = model.T_TimeLimits;
                    this.T_SolutionFeedbacks = model.T_SolutionFeedbacks;
                    this.T_ScoreFeedbacks = model.T_ScoreFeedbacks;
                    this.T_OutputFeedbacks = model.T_OutputFeedbacks;
                    this.T_InputFeedbacks = model.T_InputFeedbacks;
                }
            }
        }
    }
}