using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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
            #region RabbitMQ Simple
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
            #region RabbitMQ Fanout
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
            #region RabbitMQ Direct
            cmds.Add(new Command("---------------------RabbitMQ Direct-------------------", () => { }));
            cmds.Add(new Command($"p3d,Push,Direct,{RabbitMQUtils.RabbitMQHelper.DirectExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.DirectExchangeName1;
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
            cmds.Add(new Command($"r3d,Receive,Direct,{RabbitMQUtils.RabbitMQHelper.DirectExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.DirectExchangeName1;
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
            #region RabbitMQ Topic
            cmds.Add(new Command("---------------------RabbitMQ Topic-------------------", () => { }));
            cmds.Add(new Command($"p4d,Push,Topic,{RabbitMQUtils.RabbitMQHelper.TopicExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Topic;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.TopicExchangeName1;
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
            cmds.Add(new Command($"r4d,Receive,Topic,{RabbitMQUtils.RabbitMQHelper.TopicExchangeName1}", () =>
            {
                var exchangeType = ExchangeType.Topic;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.TopicExchangeName1;
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
            #region RabbitMQ Headers
            cmds.Add(new Command("---------------------RabbitMQ Headers-------------------", () => { }));

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