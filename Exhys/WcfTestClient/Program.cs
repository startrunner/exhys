using Exhys.SubmissionRouter.Dtos;
using Exhys.WebContestHost.Communication;
using Exhys.WebContestHost.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using WcfTestClient.SubmissionService;

namespace WcfTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            NetHttpBinding binding = new NetHttpBinding();
            binding.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
            EndpointAddress endpointAddress = new EndpointAddress("ws://localhost:9080/SubmissionRouter");
            InstanceContext instanceContext = new InstanceContext(new Callback());
            SubmissionServiceClient submissionServiceClient = new SubmissionServiceClient(instanceContext, binding, endpointAddress);

            submissionServiceClient.Ping();

            Console.Read();
        }

        public void SubmissionProcessed(SubmissionResultDto result)
        {
            throw new NotImplementedException();
        }
        class Callback : ISubmissionServiceCallback
        {
            public void Pong()
            {
                Console.WriteLine("Pong!");
            }

            public void SubmissionProcessed(SubmissionResultDto result)
            {
                
            }
        }
    }
}
