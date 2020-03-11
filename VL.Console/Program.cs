using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VL.Consoling
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandCollection cmds = new CommandCollection();
            cmds.Add(new Command("push1", () =>
            {
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "hello",
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                        string message = "Hello World!";
                        var body = System.Text.Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "hello",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                }
            }));
            cmds.Add(new Command("receive1", () =>
            {
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "hello",
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                        string message = "Hello World!";
                        var body = System.Text.Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "hello",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                }
            }));
            cmds.Add(new Command("config", () =>
            {
                var config = Configuration.ConfigurationHelper.Build(@"Configuration/appsettings.json");
                Console.WriteLine(config["MessageQueue:Name"]);
                var messageQueue = config.GetSection("MessageQueue");
                Console.WriteLine(messageQueue["Name"]);
            }));
            cmds.Start();
        }
    }

    public class CommandCollection:List<Command>
    {
        public void Start()
        {
            Console.WriteLine("wait for a command,enter `q` to close");
            string s;
            while ((s = Console.ReadLine()) != "q")
            {
                var command = this.FirstOrDefault(c => c.Name.StartsWith(s));
                try
                {
                    command.Execute();
                }
                catch (Exception)
                {

                    throw;
                }
                Console.WriteLine("wait for a command,enter `q` to close");
            }
        }
    }
    public class Command
    {
        public Command(string name, Action exe)
        {
            Name = name;
            Execute = exe;
        }

        public string Name { set; get; }

        public Action Execute { set; get; }
    }
}
