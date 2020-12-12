using Messages.Commands;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.AzureFunction
{
    public class CreateUserHandler
    {
        [FunctionName("CreateUserHandler")]
        public async Task Run([ServiceBusTrigger("helloworld", Connection = "BusConnectionString")]Message message, ILogger log, MessageReceiver messageReceiver)
        {
            try
            {
                var command = JsonConvert.DeserializeObject<CreateUser>(Encoding.UTF8.GetString(message.Body));
                log.LogInformation($"\nReceived {message.MessageId}:   CreateUser command with details {JsonConvert.SerializeObject(command)}");
                log.LogInformation($"Processing {message.MessageId}..");
                await Task.Delay(5000);
                log.LogInformation($"Done {message.MessageId} :)\n");

            }
            catch (Exception ex)
            {
                message.UserProperties.Add("Exception_detail", ex.ToString());
                await messageReceiver.AbandonAsync(message.SystemProperties.LockToken, message.UserProperties);
                throw;
            }
        }
    }
}
