using Exhys.WebContestHost.DataModels;
using Exhys.SubmissionRouter.Dtos;
using Exhys.SubmissionRouter.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Exhys.WebContestHost.Communication
{
    public class SubmissionClient
    {
        public SubmissionClient()
        {

        }

        public void SubmitRequest(ProblemSolution problemSolution)
        {
            
        }

        private SubmissionDto CreateSubmission(ProblemSolution problemSolution)
        {
            return new SubmissionDto
            {
                LanguageAlias=problemSolution.LanguageAlias,
                SourceCode = problemSolution.SourceCode,
                Tests=problemSolution.Problem.Tests
                .Select(x => new TestDto
                {
                    Input = x.Input,
                    Solution = x.Solution,
                    TimeLimit = x.TimeLimit
                }).ToList()
            };
        }

        //private 
    }
}
