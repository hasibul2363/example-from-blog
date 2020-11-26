using Messages.Commands;
using Shared;
using System;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter to send message");
            Console.Read();
            await new ServiceBus().Send(new CreateUser
            {
                Email = "yahoo@test.com",
                UserName = "yahoo"
            });
            Console.WriteLine("Sent 😊");
        }
    }
}
