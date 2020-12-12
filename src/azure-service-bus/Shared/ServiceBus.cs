using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Shared
{
    public class ServiceBus
    {
        private readonly IQueueClient _queueClient;
        public ServiceBus()
        {
            _queueClient = new QueueClient("Endpoint=sb://somename.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=--your key--", "helloworld");
        }
        public Task Send<T>(T message)
        {
            var strMessage = JsonConvert.SerializeObject(message);
            return _queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(strMessage)));
        }

        public void Listen<T>(Func<T, Task> action)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = true,
                MaxConcurrentCalls = 1
            };
            _queueClient.PrefetchCount = 1;
            _queueClient.RegisterMessageHandler((message, token) =>
            {
                try
                {
                    var typedMessage = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
                    return action(typedMessage);
                }
                catch (Exception ex)
                {
                    message.UserProperties.Add("Exception_detail", ex.ToString());
                    throw;
                }
            }, messageHandlerOptions);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler encountered an exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }


    }
}
