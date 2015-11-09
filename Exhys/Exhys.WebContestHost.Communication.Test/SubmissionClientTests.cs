using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exhys.WebContestHost.DataModels;
using Exhys.SubmissionRouter.Dtos;

namespace Exhys.WebContestHost.Communication.Test
{
    [TestClass]
    public class SubmissionClientTests
    {
        [TestMethod]
        public void TestSubmit()
        {
            SubmissionClient submissionClient = new SubmissionClient();
            ProblemSolution problemSolution = new ProblemSolution();
            problemSolution.LanguageAlias = "c++";
            problemSolution.Problem = new Problem();
            SubmissionResultDto result = submissionClient.SubmitRequestAsync(problemSolution).Result;
            Assert.IsNotNull(result);
        }
    }
}
