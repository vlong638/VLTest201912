using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VL.Consoling
{
    class Program
    {
        const string logPath = @"D:\log.txt";
        static void Main(string[] args)
        {
            CommandCollection cmds = new CommandCollection();
            cmds.Add(new Command("ls", () =>
            {
                foreach (var cmd in cmds)
                {
                    Console.WriteLine(cmd.Name);
                }
            }));
            cmds.Add(new Command("pSimple", () =>
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
                        Console.WriteLine(" [x] Sent {message}", message);
                        File.AppendAllText(logPath, $" [{DateTime.Now.ToString()}] Received {message}");
                    }
                }
            }));
            cmds.Add(new Command("rSimple", () =>
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "hello",//队列名称
                                      durable: false,//是否持久化保存,重启后保留
                                      exclusive: false,//是否排他,只有当前Channel才能监听这个Queue
                                      autoDelete: false,//不再使用的时候自动删除
                                      arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            try
                            {
                                var body = ea.Body;
                                var message = System.Text.Encoding.UTF8.GetString(body);
                                Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                                File.AppendAllText(logPath, $" [{DateTime.Now.ToString()}] Received {message}");
                            }
                            catch (Exception ex)
                            {
                                File.AppendAllText(logPath, ex.ToString());
                            }
                        };
                        channel.BasicConsume(queue: "hello",
                                             autoAck: true,
                                             consumer: consumer);
                    }
                }
            }));
            cmds.Add(new Command("rWorkQueue", () =>
            {
                var name = "Worker" + DateTime.Now.Second.ToString();
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "hello",//队列名称
                                      durable: false,//是否持久化保存,重启后保留
                                      exclusive: false,//是否排他,只有当前Channel才能监听这个Queue
                                      autoDelete: false,//不再使用的时候自动删除
                                      arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = System.Text.Encoding.UTF8.GetString(body);
                            Console.WriteLine($" [{DateTime.Now.ToString()}][{name}] Received {message}");
                            File.AppendAllText(logPath, $" [{DateTime.Now.ToString()}][{name}] Received {message}");
                            System.Threading.Thread.Sleep(3 * 1000);
                        };
                        channel.BasicConsume(queue: "hello",
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" [{DateTime.Now.ToString()}][{name}] started!");
                        File.AppendAllText(logPath, $" [{DateTime.Now.ToString()}][{name}] started!");
                    }
                }
            }));
            cmds.Add(new Command("multiThread", () =>
            {
                for (int i = 0; i < 3; i++)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        var name = "Worker" + DateTime.Now.Second.ToString();
                        Console.WriteLine($" [{name}] started! ");
                        while (true)
                        {
                            Console.WriteLine($" [{name}] says hello ");
                            System.Threading.Thread.Sleep(2000);
                        }
                    });
                    System.Threading.Thread.Sleep(2000);
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

    #region CommandMode
    public class CommandCollection : List<Command>
    {
        public void Start()
        {
            Console.WriteLine("wait for a command,enter `q` to close");
            string s = "ls";
            while ((s = Console.ReadLine()) != "q")
            {
                var command = this.FirstOrDefault(c => c.Name.StartsWith(s));
                if (command == null)
                {
                    Console.WriteLine("wait for a command,enter `q` to close");
                    continue;
                }
                try
                {
                    command.Execute();
                }
                catch (Exception e)
                {
                    var error = e;
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
    #endregion
}