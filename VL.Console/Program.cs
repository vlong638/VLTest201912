using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using VL.Consoling.RabitMQUtils;

namespace VL.Consoling
{
    class Program
    {
        static string LogPath = @"D:\log.txt";
        static bool IsFileLog = false;

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
            #region RabbitMQ Simple p1
            cmds.Add(new Command("---------------------RabbitMQ Simple-------------------", () => { }));
            cmds.Add(new Command("p1s,Push_hello", () =>
            {
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "192.168.99.100", Port = 5672 };
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
                        Console.WriteLine($" [x] Sent {message}");
                        if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}] Received {message}");
                    }
                }
            }));
            cmds.Add(new Command("r1s,Receive_hello", () =>
            {
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
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
                            Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                            if (IsFileLog) File.AppendAllLines(LogPath, new string[] { $" [{DateTime.Now.ToString()}] Received {message}" });
                        };
                        channel.BasicConsume(queue: "hello",
                                             autoAck: true,
                                             consumer: consumer);
                        Console.ReadLine();
                    }
                }
            }));
            cmds.Add(new Command($"r11s_Multiworker_Receive_hello", () =>
            {
                var name = "Worker" + DateTime.Now.Second.ToString();
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
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
                            System.Threading.Thread.Sleep(3 * 1000);
                        };
                        channel.BasicConsume(queue: "hello",
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" [{DateTime.Now.ToString()}][{name}] started!");
                        if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}][{name}] started!");
                        Console.ReadLine();
                    }
                }
            }));
            #endregion
            #region RabbitMQ Fanout p2
            cmds.Add(new Command("---------------------RabbitMQ Funout-------------------", () => { }));
            cmds.Add(new Command($"p20f,Push,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                var routingKey = "";
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        string message = Newtonsoft.Json.JsonConvert.SerializeObject(new NamedMessage1(DateTime.Now.Ticks));
                        var body = System.Text.Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: exchangeName,
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($" [x] Sent {message}");
                        if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}] Received {message}");
                    }
                }
            }));
            cmds.Add(new Command($"p21f,Push,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                var routingKey = "Insert";//证明该配置对类型Fanout无效
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        string message = Newtonsoft.Json.JsonConvert.SerializeObject(new NamedMessage1(DateTime.Now.Ticks));
                        var body = System.Text.Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: exchangeName,
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($" [x] Sent {message}");
                        if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}] Received {message}");
                    }
                }
                cmds.Add(new Command($"p21f,Push,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
                {
                    var exchangeType = ExchangeType.Fanout;
                    var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                    var routingKey = "Insert";//证明该配置对类型Fanout无效
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "192.168.99.100" };
                    using (var connection = factory.CreateConnection())
                    {
                        using (var channel = connection.CreateModel())
                        {
                            channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                            string message = Newtonsoft.Json.JsonConvert.SerializeObject(new NamedMessage1(DateTime.Now.Ticks));
                            var body = System.Text.Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish(exchange: exchangeName,
                                                 routingKey: routingKey,
                                                 basicProperties: null,
                                                 body: body);
                            Console.WriteLine($" [x] Sent {message}");
                            if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}] Received {message}");
                        }
                    }
                }));
            }));
            cmds.Add(new Command($"r20f,Receive,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                string queueName;
                var routingKey = "";
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare().QueueName;
                        channel.QueueBind(queue: queueName,
                                          exchange: exchangeName,
                                          routingKey: routingKey);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = System.Text.Encoding.UTF8.GetString(body);
                            Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                        };
                        channel.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" started for {queueName}");
                        Console.ReadLine();
                    }
                }
            }));
            cmds.Add(new Command("---------------------routingKey测试-------------------", () => { }));
            cmds.Add(new Command($"r21f,Receive,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
            {
                //证明routingKey配置对类型Fanout无效
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                string queueName;
                var routingKey = "Insert";
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare().QueueName;
                        channel.QueueBind(queue: queueName,
                                          exchange: exchangeName,
                                          routingKey: routingKey);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = System.Text.Encoding.UTF8.GetString(body);
                            Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                        };
                        channel.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" started for {queueName}");
                        Console.ReadLine();
                    }
                }
            }));
            cmds.Add(new Command($"r22f,Receive,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
            {
                //证明routingKey配置对类型Fanout无效
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                string queueName;
                var routingKey = "Update";
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare().QueueName;
                        channel.QueueBind(queue: queueName,
                                          exchange: exchangeName,
                                          routingKey: routingKey);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = System.Text.Encoding.UTF8.GetString(body);
                            Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                        };
                        channel.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" started for {queueName}");
                        Console.ReadLine();
                    }
                }
            }));
            cmds.Add(new Command("---------------------多对多测试-------------------", () => { }));
            cmds.Add(new Command($"p23f,Push,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName2}", () =>
            {
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName2;
                var routingKey = "";
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        string message = Newtonsoft.Json.JsonConvert.SerializeObject(new NamedMessage1(DateTime.Now.Ticks));
                        var body = System.Text.Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: exchangeName,
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($" [x] Sent {message}");
                        if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}] Received {message}");
                    }
                }
            }));
            cmds.Add(new Command($"r24f,Receive,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1},{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName2}", () =>
            {
                ///在p20:FunoutExchangeName1监听的基础上,增加对p23:FunoutExchangeName2的监听
                ///启动r24和r22 
                ///p20 两者都接收
                ///p23 r22接收

                var exchangeType = ExchangeType.Fanout;
                var exchangeName1 = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                var exchangeName2 = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName2;
                string queueName;
                var routingKey = "Update";
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        queueName = channel.QueueDeclare().QueueName;
                        channel.ExchangeDeclare(exchange: exchangeName1, type: exchangeType);
                        channel.QueueBind(queue: queueName,
                                          exchange: exchangeName1,
                                          routingKey: routingKey);
                        channel.ExchangeDeclare(exchange: exchangeName2, type: exchangeType);
                        channel.QueueBind(queue: queueName,
                                          exchange: exchangeName2,
                                          routingKey: routingKey);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = System.Text.Encoding.UTF8.GetString(body);
                            Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                        };
                        channel.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" started for {queueName}");
                        Console.ReadLine();
                    }
                }
            }));
            #endregion
            #region RabbitMQ Direct p3
            cmds.Add(new Command("---------------------RabbitMQ Direct-------------------", () => { }));
            cmds.Add(new Command($"p3d,Push,Direct,{RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Durable_Name1}", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1;
                var routingKey = "";
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        string message = Newtonsoft.Json.JsonConvert.SerializeObject(new NamedMessage1(DateTime.Now.Ticks));
                        var body = System.Text.Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: exchangeName,
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($" [x] Sent {message}");
                        if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}] Received {message}");
                    }
                }
            }));
            cmds.Add(new Command($"r3d,Receive,Direct,{RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1}", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Durable_Name1;
                string queueName;
                var routingKey = "";
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare().QueueName;
                        channel.QueueBind(queue: queueName,
                                          exchange: exchangeName,
                                          routingKey: routingKey);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = System.Text.Encoding.UTF8.GetString(body);
                            Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                        };
                        channel.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" started for {queueName}");
                        Console.ReadLine();
                    }
                }
            }));
            #endregion
            #region RabbitMQ Topic p4
            cmds.Add(new Command("---------------------RabbitMQ Topic-------------------", () => { }));
            cmds.Add(new Command($"p4d,Push,Topic,{RabbitMQUtils.RabbitMQHelper.TopicExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Topic;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.TopicExchangeName1;
                var routingKey = "";
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "192.168.99.100" };
                //using (var connection = factory.CreateConnection())
                //{
                //    using (var channel = connection.CreateModel())
                //    {
                //        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                //        string message = Newtonsoft.Json.JsonConvert.SerializeObject(new NamedMessage1(DateTime.Now.Ticks));
                //        var body = System.Text.Encoding.UTF8.GetBytes(message);
                //        channel.BasicPublish(exchange: exchangeName,
                //                             routingKey: routingKey,
                //                             basicProperties: null,
                //                             body: body);
                //        Console.WriteLine($" [x] Sent {message}");
                //        if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}] Received {message}");
                //    }
                //}
            }));
            cmds.Add(new Command($"r4d,Receive,Topic,{RabbitMQUtils.RabbitMQHelper.TopicExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Topic;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.TopicExchangeName1;
                string queueName;
                var routingKey = "";
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
                //using (var connection = factory.CreateConnection())
                //{
                //    using (var channel = connection.CreateModel())
                //    {
                //        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                //        queueName = channel.QueueDeclare().QueueName;
                //        channel.QueueBind(queue: queueName,
                //                          exchange: exchangeName,
                //                          routingKey: routingKey);
                //        var consumer = new EventingBasicConsumer(channel);
                //        consumer.Received += (model, ea) =>
                //        {
                //            var body = ea.Body;
                //            var message = System.Text.Encoding.UTF8.GetString(body);
                //            Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                //        };
                //        channel.BasicConsume(queue: queueName,
                //                             autoAck: true,
                //                             consumer: consumer);
                //        Console.WriteLine($" started for {queueName}");
                //        Console.ReadLine();
                //    }
                //}
            }));
            #endregion
            #region RabbitMQ Headers p5
            cmds.Add(new Command("---------------------RabbitMQ Headers-------------------", () => { }));

            #endregion
            #region RabbitMQ Serialize p6
            cmds.Add(new Command("---------------------RabbitMQ Serialize-------------------", () => { }));
            cmds.Add(new Command("p6s,Push", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Durable_Name1;
                var routingKey = "";
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: true); //Sample: Exchange的持久化
                        string message = Newtonsoft.Json.JsonConvert.SerializeObject(new NamedMessage1(DateTime.Now.Ticks));
                        IBasicProperties properties = null;
                        byte[] body = System.Text.Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: exchangeName,
                                             routingKey: routingKey,
                                             basicProperties: properties,
                                             body: body);
                        Console.WriteLine($" [x] Sent {message}");
                        if (IsFileLog) File.AppendAllText(LogPath, $" [{DateTime.Now.ToString()}] Received {message}");
                    }
                }
            }));
            cmds.Add(new Command("r6s,Receive", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Durable_Name1;
                string queueName;
                var routingKey = "";
                var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: true); //Sample: Exchange的持久化
                        queueName = channel.QueueDeclare(durable: true).QueueName;//Sample: Queue的持久化
                        channel.QueueBind(queue: queueName,
                                          exchange: exchangeName,
                                          routingKey: routingKey);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = System.Text.Encoding.UTF8.GetString(body);
                            Console.WriteLine($" [{DateTime.Now.ToString()}] Received {message}");
                        };
                        channel.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" started for {queueName}");
                        Console.ReadLine();
                    }
                }
            }));
            #endregion
            #region Authentication OAuth2.0
            cmds.Add(new Command("---------------------Authentication OAuth2.0-------------------", () => { }));
            cmds.Add(new Command("OwinAPI", () =>
            {
                //string HOST_ADDRESS = "http://localhost:81420";
                //IDisposable _webApp;
                //System.Net.Http.HttpClient _httpClient;
                //_webApp = Microsoft.Owin.Hosting.WebApp.Start<VL.API.Startup>(HOST_ADDRESS);
                //    Console.WriteLine("Web API started!");
                //    _httpClient = new System.Net.Http.HttpClient();
                //_httpClient.BaseAddress = new Uri(HOST_ADDRESS);
                Console.WriteLine("HttpClient started!");
                Console.ReadLine();
            }));
            #endregion
            #region HTTP
            cmds.Add(new Command("---------------------HTTP-------------------", () => { }));
            cmds.Add(new Command("post1 GetPregnantInfoById", () =>
            {
                var url = @"http://localhost:8020/TestAPI/api/pt/GetPregnantInfoById?id=63816";
                //var url = @"http://localhost:44347/api/pt/GetPregnantInfoById?id=63816";
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36";
                Encoding requestEncoding = Encoding.UTF8;
                var response = request.GetResponse() as HttpWebResponse;
                Console.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
            }));
            cmds.Add(new Command("post2 SavePregnantInfo", () =>
            {
                //POST /TestAPI/api/PT/SavePregnantInfo HTTP/1.1
                //Host: localhost: 8020
                //Connection: keep-alive
                //Content - Length: 15499
                //Accept: application / json, text / javascript, */*; q=0.01
                //Sec-Fetch-Dest: empty
                //User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36
                //Content-Type: application/x-www-form-urlencoded; charset=UTF-8
                //Origin: http://localhost:8020
                //Sec-Fetch-Site: same-origin
                //Sec-Fetch-Mode: cors
                //Referer: http://localhost:8020/PT/Page/Page_PregnantInfo.aspx?jobnumber=%27DBA%27&doctorname=%27%25E7%25AE%25A1%25E7%2590%2586%25E5%2591%2598%27&mechanism=%27107%27&departmentcode=%27107%27&departmentname=%27%25E4%25BA%25A7%25E7%25A7%2591%27&bingrenid=%271429738%27&shenfenzh=%27330304199012189760%27&bingrenxm=%27%25E5%25BC%25A0%25E6%2599%2593%25E7%258E%25B2%27&jiuzhenid=%271000189789%27&jigoudm=%27330302014%27&timestamp=%2720200327153716%27&initmod=edit
                //Accept-Encoding: gzip, deflate, br
                //Accept-Language: zh-CN,zh;q=0.9
                //Cookie: Idea-20191b67=d31890b0-b099-416b-9789-73711bbf4a0d

                var url = @"http://localhost:8020/TestAPI/api/pt/SavePregnantInfo";
                //var url = @"http://localhost:44347/api/pt/SavePregnantInfo";
                HttpWebRequest request = null;
                //如果是发送HTTPS请求  
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((o, certificate, chain, error) => { return true; });
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36";
                Encoding requestEncoding = Encoding.UTF8;
                //如果需要POST数据  
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                #region parameter
                parameters.Add("input", "%257B%2522newitems%2522%253A%257B%2522pregnanthistory%2522%253A%255B%257B%2522index%2522%253A%25221%2522%252C%2522pregstatus%2522%253A%2522%25E6%2597%25A0%2522%252C%2522babysex%2522%253A%25221%2522%252C%2522pregnantage%2522%253A%25222019.8%2522%252C%2522otherpregnanthistory%2522%253A%2522%2520%2522%257D%252C%257B%2522index%2522%253A%25222%2522%252C%2522pregstatus%2522%253A%2522%25E8%25B6%25B3%25E6%259C%2588%25E4%25BA%25A7-%25E5%2581%25A5%2522%252C%2522babysex%2522%253A%25222%2522%252C%2522pregnantage%2522%253A%25222018.2%2522%252C%2522otherpregnanthistory%2522%253A%2522%2522%257D%255D%257D%252C%2522olditems%2522%253A%257B%257D%252C%2522allitems_value%2522%253A%257B%2522personname%2522%253A%2522%25E5%25BC%25A0%25E6%2599%2593%25E7%258E%25B2%2522%252C%2522mobilenumber%2522%253A%252213566262593%2522%252C%2522patientaccount%2522%253A%25222012040000545393%2522%252C%2522phonenumber%2522%253A%2522%25E5%25AE%25B6%25E5%25BA%25AD%25E7%2594%25B5%25E8%25AF%259D%25E5%25AE%25B6%25E5%25BA%25AD%25E7%2594%25B5%25E8%25AF%259D%2522%252C%2522idtype%2522%253A%25221%2522%252C%2522idcard%2522%253A%2522330304199012189760%2522%252C%2522sexcode%2522%253A%25222%2522%252C%2522birthday%2522%253A%25221990-12-18%2522%252C%2522nationcode%2522%253A%252201%2522%252C%2522nationalitycode%2522%253A%2522156%2522%252C%2522maritalstatuscode%2522%253A%25221%2522%252C%2522educationcode%2522%253A%2522%2522%252C%2522workname%2522%253A%2522%25E5%258A%259E%25E4%25BA%258B%25E4%25BA%25BA%25E5%2591%2598%25E5%2592%258C%25E6%259C%2589%25E5%2585%25B3%25E4%25BA%25BA%25E5%2591%2598%2522%252C%2522workplace%2522%253A%2522%2522%252C%2522bloodtypecode%2522%253A%2522%2522%252C%2522rhbloodcode%2522%253A%2522%2522%252C%2522ownerarea%2522%253A%2522%2522%252C%2522registrationtype%2522%253A%2522%2522%252C%2522isagrregister%2522%253A%25222%2522%252C%2522healthplace%2522%253A%2522%2522%252C%2522restregioncode%2522%253A%2522%2522%252C%2522restregiontext%2522%253A%2522%2522%252C%2522homeaddress%2522%253A%2522330100000000%2522%252C%2522homeaddress_text%2522%253A%2522%25E6%2588%25B7%25E7%25B1%258D%25E8%25AF%25A6%25E6%2583%2585%25E6%2588%25B7%25E7%25B1%258D%25E8%25AF%25A6%25E6%2583%2585%2522%252C%2522liveplace%2522%253A%2522330100000000%2522%252C%2522liveplace_text%2522%253A%2522%25E6%25B5%2599%25E6%25B1%259F%25E7%259C%2581%25E6%25B8%25A9%25E5%25B7%259E%25E5%25B8%2582%25E7%2593%25AF%25E6%25B5%25B7%25E5%258C%25BA%25E6%2596%25B0%25E6%25A1%25A5%25E8%25A1%2597%25E9%2581%2593%25E7%25AB%2599%25E5%2589%258D%25E8%25B7%25AF%25EF%25BC%2591%25EF%25BC%2599%25EF%25BC%2597%25E5%258F%25B7%25EF%25BC%2591%25EF%25BC%2591%25E5%25B9%25A2%25EF%25BC%2594%25EF%25BC%2590%25EF%25BC%2595%25E5%25AE%25A4%2522%252C%2522iscreatebook%2522%253A%25222%2522%252C%2522createage%2522%253A%252229%2522%252C%2522pregnantbookid%2522%253A%2522%2522%252C%2522createbookunit%2522%253A%2522%2522%252C%2522husbandname%2522%253A%252233%2522%252C%2522husbandworkname%2522%253A%2522%2522%252C%2522husbandworkplace%2522%253A%2522%2522%252C%2522husbandfamilyhistorytext%2522%253A%2522%25E6%2597%25A0%2522%252C%2522husbandidtype%2522%253A%25221%2522%252C%2522husbandidcard%2522%253A%2522333333333333333%2522%252C%2522husbandbirthday%2522%253A%25221935-10-03%2522%252C%2522husbandage%2522%253A%252284%2522%252C%2522husbandhomeaddress%2522%253A%252212%2522%252C%2522husbandhomeaddress_text%2522%253A%2522%25E6%2588%25B7%25E7%25B1%258D%25E8%25AF%25A6%25E6%2583%2585%25E6%2588%25B7%25E7%25B1%258D%25E8%25AF%25A6%25E6%2583%2585%2522%252C%2522husbandliveaddresscode%2522%253A%252211%2522%252C%2522husbandliveaddresstext%2522%253A%2522%25E5%25B1%2585%25E4%25BD%258F%25E5%259C%25B0%25E5%259D%2580%25E8%25AF%25A6%25E6%2583%2585%25E5%25B1%2585%25E4%25BD%258F%25E5%259C%25B0%25E5%259D%2580%25E8%25AF%25A6%25E6%2583%2585%2522%252C%2522husbandmobile%2522%253A%25221111111111111%2522%252C%2522husbandbloodtypecode%2522%253A%2522%2522%252C%2522husbandrhbloodcode%2522%253A%2522%2522%252C%2522weight%2522%253A%2522%2522%252C%2522height%2522%253A%2522%2522%252C%2522bmi%2522%253A%2522%2522%252C%2522menarcheage%2522%253A%2522%2522%252C%2522menstrualperiodmin%2522%253A%2522%2522%252C%2522menstrualperiodmax%2522%253A%2522%2522%252C%2522cyclemin%2522%253A%2522%2522%252C%2522cyclemax%2522%253A%2522%2522%252C%2522menstrualblood%2522%253A%2522%2522%252C%2522dysmenorrhea%2522%253A%2522%2522%252C%2522sbp%2522%253A%2522%2522%252C%2522dbp%2522%253A%2522%2522%252C%2522pulse%2522%253A%2522%2522%252C%2522lastmenstrualperiod%2522%253A%25222020-01-01%2522%252C%2522dateofprenatal%2522%253A%25222020-10-07%2522%252C%2522dateofprenatalmodifyreason%2522%253A%2522%2522%252C%2522tpregnancymanner%2522%253A%25223%2522%252C%2522tpregnancymanner_text%2522%253A%2522%25E6%2580%2580%25E5%25AD%2595%25E8%25AF%25A6%25E6%2583%2585%25E6%2580%2580%25E5%25AD%2595%25E8%25AF%25A6%25E6%2583%2585%2522%252C%2522eggretrievaltime%2522%253A%25222020-03-04%2522%252C%2522implanttime%2522%253A%25222020-03-05%2522%252C%2522gravidity%2522%253A%25221%2522%252C%2522parity%2522%253A%25220%2522%252C%2522vaginaldeliverynum%2522%253A%25222%2522%252C%2522caesareansectionnum%2522%253A%25223%2522%252C%2522bloodtransfution%2522%253A%2522%25E8%25BE%2593%25E8%25A1%2580%25E5%258F%25B2%25E8%25BE%2593%25E8%25A1%2580%25E5%258F%25B2%2522%252C%2522personalhistory%2522%253A%2522%2522%252C%2522gestationneuropathy%2522%253A%2522%25E6%2597%25A0%2522%252C%2522pasthistory%2522%253A%2522%25E6%2597%25A0%2522%252C%2522familyhistory%2522%253A%2522%25E7%2588%25B6%25E4%25BA%25B2%253A%25E6%2597%25A0%25E7%2588%25B6%25E4%25BA%25B2%25E7%2596%25BE%25E7%2597%2585%25E5%258F%25B2%2520%25E6%25AF%258D%25E4%25BA%25B2%253A%25E6%2597%25A0%25E6%25AF%258D%25E4%25BA%25B2%25E7%2596%25BE%25E7%2597%2585%25E5%258F%25B2%2520%25E5%2585%2584%25E5%25BC%259F%25E5%25A7%2590%25E5%25A6%25B9%253A%25E6%2597%25A0%25E5%2585%2584%25E5%25BC%259F%25E5%25A7%2590%25E5%25A6%25B9%25E7%2596%25BE%25E7%2597%2585%25E5%258F%25B2%2520%25E5%25AD%2590%25E5%25A5%25B3%253A%25E6%2597%25A0%25E5%25AD%2590%25E5%25A5%25B3%25E7%2596%25BE%25E7%2597%2585%25E5%258F%25B2%2522%252C%2522operationhistory%2522%253A%2522%25E6%2597%25A0%25E6%2589%258B%25E6%259C%25AF%25E5%258F%25B2%2522%252C%2522gynecologyops%2522%253A%2522%25E5%25A6%2587%25E7%25A7%2591%25E6%2589%258B%25E6%259C%25AF%25E5%258F%25B2%25E5%25A6%2587%25E7%25A7%2591%25E6%2589%258B%25E6%259C%25AF%25E5%258F%25B2%2522%252C%2522allergichistory%2522%253A%2522%25E6%259C%2589%2522%252C%2522poisontouchhis%2522%253A%2522%25E6%25AF%2592%25E7%2589%25A9%25E6%258E%25A5%25E8%25A7%25A6%25E5%258F%25B2%25E6%25AF%2592%25E7%2589%25A9%25E6%258E%25A5%25E8%25A7%25A6%25E5%258F%25B2%2522%252C%2522heredityfamilyhistory%2522%253A%2522%25E6%2597%25A0%2522%252C%2522create_localuser%2522%253A%2522%25E7%25AE%25A1%25E7%2590%2586%25E5%2591%2598%2522%252C%2522sourceunit%2522%253A%2522330302014%2522%252C%2522editorname%2522%253A%2522%25E7%25AE%25A1%25E7%2590%2586%25E5%2591%2598%2522%252C%2522createdate%2522%253A%25222020-03-13%2522%252C%2522gestationalweeks%2522%253A%252210%2522%252C%2522gestationaldays%2522%253A%25222%2522%252C%2522changein_ascription%2522%253A%2522%2522%252C%2522changein_unit%2522%253A%2522%2522%252C%2522changein_date%2522%253A%2522%2522%252C%2522id%2522%253A%252264116%2522%252C%2522editorcode%2522%253A%2522DBA%2522%252C%2522patientid%2522%253A%25221429738%2522%257D%252C%2522allitems_text%2522%253A%257B%2522personname%2522%253A%2522%25E5%25BC%25A0%25E6%2599%2593%25E7%258E%25B2%2522%252C%2522mobilenumber%2522%253A%252213566262593%2522%252C%2522patientaccount%2522%253A%25222012040000545393%2522%252C%2522phonenumber%2522%253A%2522%25E5%25AE%25B6%25E5%25BA%25AD%25E7%2594%25B5%25E8%25AF%259D%25E5%25AE%25B6%25E5%25BA%25AD%25E7%2594%25B5%25E8%25AF%259D%2522%252C%2522idtype%2522%253A%2522%25E5%25B1%2585%25E6%25B0%2591%25E8%25BA%25AB%25E4%25BB%25BD%25E8%25AF%2581%2522%252C%2522idcard%2522%253A%2522330304199012189760%2522%252C%2522sexcode%2522%253A%2522%25E5%25A5%25B3%2522%252C%2522birthday%2522%253A%25221990-12-18%2522%252C%2522nationcode%2522%253A%2522%25E6%25B1%2589%25E6%2597%258F%2522%252C%2522nationalitycode%2522%253A%2522%25E4%25B8%25AD%25E5%259B%25BD%2522%252C%2522maritalstatuscode%2522%253A%2522%25E6%259C%25AA%25E5%25A9%259A%2522%252C%2522educationcode%2522%253A%2522%2522%252C%2522workname%2522%253A%2522%25E5%258A%259E%25E4%25BA%258B%25E4%25BA%25BA%25E5%2591%2598%25E5%2592%258C%25E6%259C%2589%25E5%2585%25B3%25E4%25BA%25BA%25E5%2591%2598%2522%252C%2522workplace%2522%253A%2522%2522%252C%2522bloodtypecode%2522%253A%2522%2522%252C%2522rhbloodcode%2522%253A%2522%2522%252C%2522ownerarea%2522%253A%2522%2522%252C%2522registrationtype%2522%253A%2522%2522%252C%2522isagrregister%2522%253A%2522%25E5%2590%25A6%2522%252C%2522healthplace%2522%253A%2522%2522%252C%2522restregioncode%2522%253A%2522%2522%252C%2522restregiontext%2522%253A%2522%2522%252C%2522homeaddress%2522%253A%2522%25E6%25B5%2599%25E6%25B1%259F%25E7%259C%2581%25E6%259D%25AD%25E5%25B7%259E%25E5%25B8%2582%2522%252C%2522homeaddress_text%2522%253A%2522%25E6%2588%25B7%25E7%25B1%258D%25E8%25AF%25A6%25E6%2583%2585%25E6%2588%25B7%25E7%25B1%258D%25E8%25AF%25A6%25E6%2583%2585%2522%252C%2522liveplace%2522%253A%2522%25E6%25B5%2599%25E6%25B1%259F%25E7%259C%2581%25E6%259D%25AD%25E5%25B7%259E%25E5%25B8%2582%2522%252C%2522liveplace_text%2522%253A%2522%25E6%25B5%2599%25E6%25B1%259F%25E7%259C%2581%25E6%25B8%25A9%25E5%25B7%259E%25E5%25B8%2582%25E7%2593%25AF%25E6%25B5%25B7%25E5%258C%25BA%25E6%2596%25B0%25E6%25A1%25A5%25E8%25A1%2597%25E9%2581%2593%25E7%25AB%2599%25E5%2589%258D%25E8%25B7%25AF%25EF%25BC%2591%25EF%25BC%2599%25EF%25BC%2597%25E5%258F%25B7%25EF%25BC%2591%25EF%25BC%2591%25E5%25B9%25A2%25EF%25BC%2594%25EF%25BC%2590%25EF%25BC%2595%25E5%25AE%25A4%2522%252C%2522iscreatebook%2522%253A%2522%25E5%2590%25A6%2522%252C%2522createage%2522%253A%252229%2522%252C%2522pregnantbookid%2522%253A%2522%2522%252C%2522createbookunit%2522%253A%2522%2522%252C%2522husbandname%2522%253A%252233%2522%252C%2522husbandworkname%2522%253A%2522%2522%252C%2522husbandworkplace%2522%253A%2522%2522%252C%2522husbandfamilyhistorytext%2522%253A%2522%25E6%2597%25A0%2522%252C%2522husbandidtype%2522%253A%2522%25E5%25B1%2585%25E6%25B0%2591%25E8%25BA%25AB%25E4%25BB%25BD%25E8%25AF%2581%2522%252C%2522husbandidcard%2522%253A%2522333333333333333%2522%252C%2522husbandbirthday%2522%253A%25221935-10-03%2522%252C%2522husbandage%2522%253A%252284%2522%252C%2522husbandhomeaddress%2522%253A%2522%25E5%25A4%25A9%25E6%25B4%25A5%25E5%25B8%2582%2522%252C%2522husbandhomeaddress_text%2522%253A%2522%25E6%2588%25B7%25E7%25B1%258D%25E8%25AF%25A6%25E6%2583%2585%25E6%2588%25B7%25E7%25B1%258D%25E8%25AF%25A6%25E6%2583%2585%2522%252C%2522husbandliveaddresscode%2522%253A%2522%25E5%258C%2597%25E4%25BA%25AC%25E5%25B8%2582%2522%252C%2522husbandliveaddresstext%2522%253A%2522%25E5%25B1%2585%25E4%25BD%258F%25E5%259C%25B0%25E5%259D%2580%25E8%25AF%25A6%25E6%2583%2585%25E5%25B1%2585%25E4%25BD%258F%25E5%259C%25B0%25E5%259D%2580%25E8%25AF%25A6%25E6%2583%2585%2522%252C%2522husbandmobile%2522%253A%25221111111111111%2522%252C%2522husbandbloodtypecode%2522%253A%2522%2522%252C%2522husbandrhbloodcode%2522%253A%2522%2522%252C%2522weight%2522%253A%2522%2522%252C%2522height%2522%253A%2522%2522%252C%2522bmi%2522%253A%2522%2522%252C%2522menarcheage%2522%253A%2522%2522%252C%2522menstrualperiodmin%2522%253A%2522%2522%252C%2522menstrualperiodmax%2522%253A%2522%2522%252C%2522cyclemin%2522%253A%2522%2522%252C%2522cyclemax%2522%253A%2522%2522%252C%2522menstrualblood%2522%253A%2522%2522%252C%2522dysmenorrhea%2522%253A%2522%2522%252C%2522sbp%2522%253A%2522%2522%252C%2522dbp%2522%253A%2522%2522%252C%2522pulse%2522%253A%2522%2522%252C%2522lastmenstrualperiod%2522%253A%25222020-01-01%2522%252C%2522dateofprenatal%2522%253A%25222020-10-07%2522%252C%2522dateofprenatalmodifyreason%2522%253A%2522%2522%252C%2522tpregnancymanner%2522%253A%2522%25E8%25AF%2595%25E7%25AE%25A1%25E5%25A9%25B4%25E5%2584%25BF%2522%252C%2522tpregnancymanner_text%2522%253A%2522%25E6%2580%2580%25E5%25AD%2595%25E8%25AF%25A6%25E6%2583%2585%25E6%2580%2580%25E5%25AD%2595%25E8%25AF%25A6%25E6%2583%2585%2522%252C%2522eggretrievaltime%2522%253A%25222020-03-04%2522%252C%2522implanttime%2522%253A%25222020-03-05%2522%252C%2522gravidity%2522%253A%25221%2522%252C%2522parity%2522%253A%25220%2522%252C%2522vaginaldeliverynum%2522%253A%25222%2522%252C%2522caesareansectionnum%2522%253A%25223%2522%252C%2522pregstatus%2522%253A%2522%25E8%25B6%25B3%25E6%259C%2588%25E4%25BA%25A7-%25E5%2581%25A5%2522%252C%2522babysex%2522%253A%2522%25E5%25A5%25B3%2522%252C%2522pregnantage%2522%253A%25222018.2%2522%252C%2522otherpregnanthistory%2522%253A%2522%2522%252C%2522bloodtransfution%2522%253A%2522%25E8%25BE%2593%25E8%25A1%2580%25E5%258F%25B2%25E8%25BE%2593%25E8%25A1%2580%25E5%258F%25B2%2522%252C%2522personalhistory%2522%253A%2522%2522%252C%2522gestationneuropathy%2522%253A%2522%25E6%2597%25A0%2522%252C%2522pasthistory%2522%253A%2522%25E6%2597%25A0%2522%252C%2522familyhistory%2522%253A%2522%25E7%2588%25B6%25E4%25BA%25B2%253A%25E6%2597%25A0%25E7%2588%25B6%25E4%25BA%25B2%25E7%2596%25BE%25E7%2597%2585%25E5%258F%25B2%2520%25E6%25AF%258D%25E4%25BA%25B2%253A%25E6%2597%25A0%25E6%25AF%258D%25E4%25BA%25B2%25E7%2596%25BE%25E7%2597%2585%25E5%258F%25B2%2520%25E5%2585%2584%25E5%25BC%259F%25E5%25A7%2590%25E5%25A6%25B9%253A%25E6%2597%25A0%25E5%2585%2584%25E5%25BC%259F%25E5%25A7%2590%25E5%25A6%25B9%25E7%2596%25BE%25E7%2597%2585%25E5%258F%25B2%2520%25E5%25AD%2590%25E5%25A5%25B3%253A%25E6%2597%25A0%25E5%25AD%2590%25E5%25A5%25B3%25E7%2596%25BE%25E7%2597%2585%25E5%258F%25B2%2522%252C%2522operationhistory%2522%253A%2522%25E6%2597%25A0%25E6%2589%258B%25E6%259C%25AF%25E5%258F%25B2%2522%252C%2522gynecologyops%2522%253A%2522%25E5%25A6%2587%25E7%25A7%2591%25E6%2589%258B%25E6%259C%25AF%25E5%258F%25B2%25E5%25A6%2587%25E7%25A7%2591%25E6%2589%258B%25E6%259C%25AF%25E5%258F%25B2%2522%252C%2522allergichistory%2522%253A%2522%25E6%259C%2589%2522%252C%2522poisontouchhis%2522%253A%2522%25E6%25AF%2592%25E7%2589%25A9%25E6%258E%25A5%25E8%25A7%25A6%25E5%258F%25B2%25E6%25AF%2592%25E7%2589%25A9%25E6%258E%25A5%25E8%25A7%25A6%25E5%258F%25B2%2522%252C%2522heredityfamilyhistory%2522%253A%2522%25E6%2597%25A0%2522%252C%2522create_localuser%2522%253A%2522%25E7%25AE%25A1%25E7%2590%2586%25E5%2591%2598%2522%252C%2522sourceunit%2522%253A%2522330302014%2522%252C%2522editorname%2522%253A%2522%25E7%25AE%25A1%25E7%2590%2586%25E5%2591%2598%2522%252C%2522createdate%2522%253A%25222020-03-13%2522%252C%2522gestationalweeks%2522%253A%252210%2522%252C%2522gestationaldays%2522%253A%25222%2522%252C%2522changein_ascription%2522%253A%2522%2522%252C%2522changein_unit%2522%253A%2522%2522%252C%2522changein_date%2522%253A%2522%2522%252C%2522id%2522%253A%252264116%2522%252C%2522editorcode%2522%253A%2522DBA%2522%252C%2522patientid%2522%253A%25221429738%2522%257D%252C%2522id%2522%253A64116%252C%2522reqitems%2522%253A%255B%255D%252C%2522jobnumber%2522%253A%2522DBA%2522%252C%2522doctorname%2522%253A%2522%25E7%25AE%25A1%25E7%2590%2586%25E5%2591%2598%2522%252C%2522mechanism%2522%253A%2522107%2522%252C%2522departmentcode%2522%253A%2522107%2522%252C%2522departmentname%2522%253A%2522%25E4%25BA%25A7%25E7%25A7%2591%2522%252C%2522bingrenid%2522%253A%25221429738%2522%252C%2522shenfenzh%2522%253A%2522330304199012189760%2522%252C%2522bingrenxm%2522%253A%2522%25E5%25BC%25A0%25E6%2599%2593%25E7%258E%25B2%2522%252C%2522jiuzhenid%2522%253A%25221000189789%2522%252C%2522jigoudm%2522%253A%2522330302014%2522%252C%2522userid%2522%253A%2522DBA%2522%252C%2522username%2522%253A%2522%25E7%25AE%25A1%25E7%2590%2586%25E5%2591%2598%2522%252C%2522timestamp%2522%253A%252220200327153716%2522%257D"); 
                #endregion
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                    byte[] data = requestEncoding.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                var response = request.GetResponse() as HttpWebResponse;
                Console.WriteLine($"{Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
            }));
            #endregion
            #region Common
            cmds.Add(new Command("---------------------Common-------------------", () => { }));
            cmds.Add(new Command("config", () =>
            {
                var config = Configuration.ConfigurationHelper.Build(@"Configuration/appsettings.json");
                Console.WriteLine(config["MessageQueue:Name"]);
                var messageQueue = config.GetSection("MessageQueue");
                Console.WriteLine(messageQueue["Name"]);
            }));

            #endregion
            #region Others
            cmds.Add(new Command("---------------------Others-------------------", () => { }));
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
            cmds.Add(new Command("parseTXJH", () =>
            {
                var logger = new FileLogger();
                try
                {
                    var dataFile = @"D:\Sync\1_20200313121056.bin";
                    var xmlFile = @"D:\Sync\1_20200313121056.xml";
                    if (!File.Exists(dataFile) || !File.Exists(xmlFile))
                    {
                        logger.Info($"内容尚未齐全,dataFile:{dataFile},xmlFile:{xmlFile}");
                        return;
                    }
                    logger.Info($"开始解析,dataFile:{dataFile},xmlFile:{xmlFile}");

                    #region 数据解析
                    //基础数据解析
                    string binglih = "";
                    DateTime startTime = DateTime.MinValue;
                    var xml = System.IO.File.ReadAllText(xmlFile);
                    Regex regexMRID = new Regex(@"\<MRID\>(\w+)\</MRID\>");
                    var matchMRID = regexMRID.Match(xml);
                    if (matchMRID.Groups.Count != 2)
                    {
                        logger.Error("必要数据项缺失,MRID");
                        return;
                    }
                    else
                    {
                        binglih = matchMRID.Groups[1].Value;
                    }
                    Regex regexStartTime = new Regex(@"\<StartTime\>([\d\s]+)\</StartTime\>");
                    var matchStartTime = regexStartTime.Match(xml);
                    if (matchStartTime.Groups.Count != 2)
                    {
                        logger.Error("必要数据项缺失,StartTime");
                        return;
                    }
                    else
                    {
                        var timeStr = matchStartTime.Groups[1].Value;
                        var indexer = 0;
                        if (timeStr.Length != 14)
                        {
                            logger.Error($"无效的数据,Length:{timeStr.Length},StartTime:{timeStr}");
                            return;
                        }
                        if (!int.TryParse(timeStr.Substring(indexer, 4), out int year))
                        {
                            logger.Error("无效的数据,StartTime:" + timeStr);
                            return;
                        }
                        indexer += 4;
                        if (!int.TryParse(timeStr.Substring(indexer, 2), out int month))
                        {
                            logger.Error("无效的数据,StartTime:" + timeStr);
                            return;
                        }
                        indexer += 2;
                        if (!int.TryParse(timeStr.Substring(indexer, 2), out int day))
                        {
                            logger.Error("无效的数据,StartTime:" + timeStr);
                            return;
                        }
                        indexer += 2;
                        if (!int.TryParse(timeStr.Substring(indexer, 2), out int hour))
                        {
                            logger.Error("无效的数据,StartTime:" + timeStr);
                            return;
                        }
                        indexer += 2;
                        if (!int.TryParse(timeStr.Substring(indexer, 2), out int minite))
                        {
                            logger.Error("无效的数据,StartTime:" + timeStr);
                            return;
                        }
                        indexer += 2;
                        if (!int.TryParse(timeStr.Substring(indexer, 2), out int second))
                        {
                            logger.Error("无效的数据,StartTime:" + timeStr);
                            return;
                        }
                        startTime = new DateTime(year, month, day, hour, minite, second);
                    }
                    //仪器数据解析
                    var data = System.IO.File.ReadAllBytes(dataFile);
                    var model = new FileSystemWatcher.DrawTXJHModel();
                    var dataCount = data.Length / 17;
                    model.data1 = new int[dataCount];
                    model.data2 = new int[dataCount];
                    model.data3 = new int[dataCount];
                    model.data4 = new int[dataCount];
                    model.data5 = new int[dataCount];
                    for (var i = 0; i < data.Length; ++i)
                    {
                        var dataIndex = i / 17;
                        switch (i % 17)
                        {
                            case 3: model.data1[dataIndex] = data[i]; break;
                            case 7: model.data2[dataIndex] = data[i]; break;
                            case 11: model.data3[dataIndex] = data[i]; break;
                            case 15: model.data4[dataIndex] = data[i]; break;
                            case 16: model.data5[dataIndex] = data[i]; break;
                        }
                    }
                    var entity = new FileSystemWatcher.GetDataForTXJHModel();
                    entity.RecordCode = binglih;
                    entity.StartTime = startTime;
                    entity.FetalHeartData = string.Join(",", model.data1);
                    entity.UCData = string.Join(",", model.data3);
                    #endregion

                    using (var connection = FileSystemWatcher.DBHelper.GetSQLServerDbConnection(@"Data Source=192.168.50.102;Initial Catalog=fmpt;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=huzfypt;Password=huz3305@2018."))
                    //using (var connection = DBHelper.GetSQLServerDbConnection(@"Data Source=10.31.102.24,1434;Initial Catalog=fmpt;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=HELETECHUSER;Password=HELEtech123"))
                    {
                        var command = connection.CreateCommand();
                        try
                        {
                            connection.Open();
                            command.CommandText = "select count(*) from FM_TXJH where binglih = @binglih and CONVERT(varchar(100), FM_TXJH.StartTime,20)=@FormatStartTime;";
                            command.Parameters.Add(new SqlParameter("@binglih", entity.RecordCode));
                            command.Parameters.Add(new SqlParameter("@FormatStartTime", entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss")));//2020-03-16 15:20:57
                            var result = (int)command.ExecuteScalar();
                            if (result > 0)
                            {
                                logger.Info($"已有该数据,binglih:{binglih},StartTime:{entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                                return;
                            }

                            command.Parameters.Clear();
                            command.CommandText = "insert into fm_TXJH(binglih,StartTime,FetalHeartData,UCData)values(@binglih,@StartTime,@FetalHeartData,@UCData)";
                            command.Parameters.Add(new SqlParameter("@binglih", entity.RecordCode));
                            command.Parameters.Add(new SqlParameter("@StartTime", entity.StartTime));
                            command.Parameters.Add(new SqlParameter("@FetalHeartData", entity.FetalHeartData));
                            command.Parameters.Add(new SqlParameter("@UCData", entity.UCData));
                            command.ExecuteNonQuery();
                            command.Dispose();
                            connection.Close();
                            logger.Info("数据同步成功");
                        }
                        catch (Exception ex)
                        {
                            logger.Error("插入数据库时报错,", ex);
                            connection.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("出现异常," + ex.ToString());
                }
            })); 
            #endregion
            cmds.Start();
        }
}
#region MyRegion

public class LogData
{
    public LogData(string message)
    {
        Message = message;
    }
    public LogData(string className, string fuctionName, string message)
    {
        ClassName = className;
        FuctionName = fuctionName;
        Message = message;
    }

    public LogData(string className, string fuctionName, string sesction, string message)
    {
        ClassName = className;
        FuctionName = fuctionName;
        Section = sesction;
        Message = message;
    }

    public string ClassName { set; get; }
    public string FuctionName { set; get; }
    public string Section { set; get; }
    public string Message { set; get; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("----------------------------");
        sb.AppendLine(string.Format("-------ClassName  :{0}", ClassName));
        sb.AppendLine(string.Format("-------FuctionName:{0}", FuctionName));
        sb.AppendLine(string.Format("-------Section    :{0}", Section));
        sb.AppendLine(string.Format("-------LogTime    :{0}", DateTime.Now));
        sb.AppendLine(string.Format("-------Message    :{0}", Message));
        return sb.ToString();
    }
}

public class FileLogger
{
    static string _Path = @"D:\Sync\SyncLog.txt";
    static string Path { get { return _Path; } }

    public void Info(LogData locator)
    {
        File.AppendAllText(Path, locator.ToString());
    }

    public void Info(string message)
    {
        Info(new LogData(message));
    }

    public void Info(string message, Exception ex)
    {
        Info(new LogData(message + ex.ToString()));
    }

    public void Error(LogData locator)
    {
        File.AppendAllText(Path, locator.ToString());
    }

    public void Error(string message)
    {
        Error(new LogData(message));
    }

    public void Error(string message, Exception ex)
    {
        Error(new LogData(message + ex.ToString()));
    }
}
#endregion

#region CommandMode
public class CommandCollection : List<Command>
{
    public void Start()
    {
        Console.WriteLine("wait for a command,enter `q` to close");
        string s = "ls";
        do
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
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("wait for a command,enter `q` to close");
        }
        while ((s = Console.ReadLine().ToLower()) != "q");
    }
}
public class Command
{
    public Command(string name, Action exe)
    {
        Name = name.ToLower();
        Execute = exe;
    }

    public string Name { set; get; }

    public Action Execute { set; get; }
}
    #endregion
}