using Messages.Commands;
using Newtonsoft.Json;
using Shared;
using System;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to listen...");
            Console.ReadLine();
            var bus = new ServiceBus();
            bus.Listen<CreateUser>(CreateUserHandler);
            Console.ReadLine();
        }

        public static Task CreateUserHandler(CreateUser command)
        {
            Console.WriteLine("I am from Handler..");
            Console.WriteLine($"Here is message {JsonConvert.SerializeObject(command)}");
            return Task.CompletedTask;
        }
    }
}
