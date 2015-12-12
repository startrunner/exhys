using System;
using Exhys.WebContestHost.DataModels;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    public class SolutionTestStatusViewModel
    {
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Solution { get; set; }
        public double Score { get; private set; }
        public TestStatusVm Status { get; set; }

        public SolutionTestStatusViewModel (SolutionTestStatus model)
        {
            if (model != null)
            {
                this.Id = model.Id;
                if (model.ProblemTest != null)
                {
                    var test = model.ProblemTest;
                    if (test.InputFeedbackEnabled)
                    {
                        this.Input = test.Input;
                    }
                    if (test.SolutionFeedbackEnabled)
                    {
                        this.Solution = test.Solution;
                    }
                    if (test.OutputFeedbackEnabled)
                    {
                        this.Output = model.Output;
                    }
                    if (test.StatusFeedbackEnabled)
                    {
                        this.Status = ConvertStatus(model.Status);
                    }
                    if(test.ScoreFeedbackEnabled)
                    {
                        this.Score = model.Score;
                    }
                }
            }
        }

        private static TestStatusVm ConvertStatus (SolutionTestStatus.TestStatus status)
        {
            switch (status)
            {
                case SolutionTestStatus.TestStatus.CorrectAnswer:
                    return TestStatusVm.CorrectAnswer;
                case SolutionTestStatus.TestStatus.MemoryLimitExceeded:
                    return TestStatusVm.MemoryLimitExceeded;
                case SolutionTestStatus.TestStatus.SegmentationFault:
                    return TestStatusVm.SegmentationFault;
                case SolutionTestStatus.TestStatus.TimeOut:
                    return TestStatusVm.SegmentationFault;
                case SolutionTestStatus.TestStatus.WrongAnswer:
                    return TestStatusVm.WrongAnswer;
            }
            return 0;
        }


    }

    public enum TestStatusVm
    {
        SegmentationFault,
        WrongAnswer,
        CorrectAnswer,
        TimeOut,
        MemoryLimitExceeded
    }
}