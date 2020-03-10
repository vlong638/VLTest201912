using System;

namespace VL.Consoling
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = Configuration.ConfigurationHelper.Build(@"Configuration/appsettings.json");
            Console.WriteLine(config["MessageQueue:Name"]);
            var messageQueue = config.GetSection("MessageQueue");
            Console.WriteLine(messageQueue["Name"]);



            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
