using SubmissionRouterService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SubmissionRouterDTOs;

namespace SubmissionRouterService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SubmissionService : ISubmissionService
    {
        public Dictionary<Guid,ISubmissionCallback> submissions;

        public SubmissionService()
        {
            submissions = new Dictionary<Guid, ISubmissionCallback>();
            ExecutionScheduler.Instance.SubmissionCompleted += (s, e) => SubmissionCompleted(e.SubmissionResult);
        }

        public Guid Submit(SubmissionDto submission)
        {
            return Submit(submission, OperationContext.Current.GetCallbackChannel<ISubmissionCallback>());
        }

        public Guid Submit(SubmissionDto submission, ISubmissionCallback submissionCallback)
        {
            Guid submissionId = ExecutionScheduler.Instance.RequestExecution(submission);
            submissions.Add(submissionId, submissionCallback);
            return submissionId;
        }

        public void SubmissionCompleted(SubmissionResultDto submissionResult)
        {
            if(submissions.ContainsKey(submissionResult.ExecutionId))
            {
                ISubmissionCallback callback = submissions[submissionResult.ExecutionId];
                submissions.Remove(submissionResult.ExecutionId);
                callback.SubmissionProcessed(submissionResult);
            }
        }
    }
}
