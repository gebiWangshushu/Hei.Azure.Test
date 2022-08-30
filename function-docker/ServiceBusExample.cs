using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace function_docker
{
    public class ServiceBusExample
    {
        [FunctionName("ServiceBusExample")]
        public async Task Run([ServiceBusTrigger("azure-funtion-docker", Connection = "ServiceBusSammDev")]
            string myQueueItem,
            Int32 deliveryCount,
            DateTime enqueuedTimeUtc,
            string messageId,
            ILogger log)
        {
            log.LogInformation($"{Environment.MachineName} C# ServiceBus queue trigger function processed message: {myQueueItem} EnqueuedTimeUtc={enqueuedTimeUtc} DeliveryCount={deliveryCount} MessageId={messageId}");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var combineName = DateTimeOffset.Now.ToString("yyyy-MM-dd@HH_mm_ss_fff");
            await new VideoCombine().Execute(combineName);

            sw.Stop();
            log.LogInformation($"{Environment.MachineName} -> {combineName}:{myQueueItem} VideoCombine().Executed(): { sw.Elapsed}");
        }
    }
}