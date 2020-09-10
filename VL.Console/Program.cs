using Dapper;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ValuesSolution;
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
using System.Threading.Tasks;
using System.Xml.Linq;
using VL.Consoling.Entities;
using VL.Consoling.RabitMQUtils;
using VL.Consoling.Utils;
using System.Data;
using System.Collections;

namespace VL.Consoling
{
    class Program
    {
        static string LogPath = @"D:\log.txt";
        static bool IsFileLog = false;
        static string MQHost = "192.168.99.100";
        static string LocalMSSQL = "Data Source=127.0.0.1,1433;Initial Catalog=VLTest;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sa;Password=123";
        static string HeleOuterMSSQL = "Data Source=heletech.asuscomm.com,8082;Initial Catalog=HELEESB;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=ESBUSER;Password=ESBPWD";
        static string HeleInnerMSSQL = "Data Source=192.168.50.102,8082;Initial Catalog=HELEESB;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=ESBUSER;Password=ESBPWD";

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                ArrayList arrayList = json.FromJson<ArrayList>();
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch(Exception e)
            {
                var ex = e.ToString();
            }
            result = dataTable;
            return result;
        }

        static void Main(string[] args)
        {
            //0826 newtonsoft支持json直接转datatable
            var t0826 = @"[{""yizhirq"":"""",""shuhouts"":"""",""hcg"":""123"",""yindaolx"":"""",""fuzhang"":""""},{""yizhirq"":"""",""shuhouts"":"""",""hcg"":""321"",""yindaolx"":"""",""fuzhang"":""""}]";
            var t0826Json = t0826.FromJson<dynamic>();
            var t0826DataTable = t0826.FromJson<DataTable>();
            var sumhcgValue = t0826DataTable.AsEnumerable().Sum(c => {
                var text = c.Field<string>("hcg");
                return text?.ToInt().Value ?? 0;
            });
            var sumyizhirqValue = t0826DataTable.AsEnumerable().Sum(c => {
                var text = c.Field<string>("yizhirq");
                var value = text?.ToInt();
                return value.HasValue ? value.Value : 0;
            });

            ///命令对象有助于代码的版本控制,集体非方法的形式堆在一起不利于
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
                //var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost, Port = 5672 };
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "localhost", Port = 5672 };
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
                var factory = new ConnectionFactory() { HostName = MQHost };
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
                var factory = new ConnectionFactory() { HostName = MQHost };
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
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };
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
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };//192.168.99.100
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
                    var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };
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
            cmds.Add(new Command($"p23f,Push,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName3}", () =>
            {
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName3;
                var routingKey = "Insert";//证明该配置对类型Fanout无效
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };//192.168.99.100
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
                    var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };
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
                var factory = new ConnectionFactory() { HostName = MQHost };
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
                //routingKey:Insert
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                string queueName;
                var routingKey = "Insert";
                var factory = new ConnectionFactory() { HostName = MQHost };
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
                //routingKey:Update
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                string queueName;
                var routingKey = "Update";
                var factory = new ConnectionFactory() { HostName = MQHost };
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
            cmds.Add(new Command($"r221f,Receive,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
            {
                //queue:hele-gj-queue-zyysz
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                string queueName;
                var routingKey = "Update";
                var factory = new ConnectionFactory() { HostName = MQHost };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare("hele-gj-queue-zyysz", true, false, false, null);
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
            cmds.Add(new Command($"r222f,Receive,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
            {
                //queue:hele-gj-queue-zyysz2
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                string queueName;
                var routingKey = "";
                var factory = new ConnectionFactory() { HostName = MQHost };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare("hele-gj-queue-zyysz2", true, false, false, null);
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
            cmds.Add(new Command($"r223f,Receive,Funout,{RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1}", () =>
            {
                //queue:hele-gj-queue-zyysz3
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.FunoutExchangeName1;
                string queueName;
                var routingKey = "";
                var factory = new ConnectionFactory() { HostName = MQHost };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare("hele-gj-queue-zyysz3", true, false, false, null);
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
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };
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
                var factory = new ConnectionFactory() { HostName = MQHost };
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

            cmds.Add(new Command($"rGJ,Receive,Funout", () =>
            {
                //dzblMqIp = 10.31.102.49
                //dzblMqName = hele
                //dzblMqPwd = hl@gj2019
                //dzblMqQueueName = hl_mq
                //dzblQueueName = hele-gj-queue-zyysz-record

                //routingKey:Update
                var exchangeType = ExchangeType.Fanout;
                var exchangeName = "hl_mq";
                var queueName = "hele-gj-queue-zyysz-record";
                var factory = new ConnectionFactory() { HostName = "10.31.102.49" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare(queueName).QueueName;
                        channel.QueueBind(queue: queueName,
                                          exchange: exchangeName,
                                          routingKey: "");
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
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        //string message = Newtonsoft.Json.JsonConvert.SerializeObject(new NamedMessage1(DateTime.Now.Ticks));
                        //var body = System.Text.Encoding.UTF8.GetBytes(message);
                        var routingKey = "A";
                        channel.BasicPublish(exchange: exchangeName,
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: System.Text.Encoding.UTF8.GetBytes(routingKey));
                        routingKey = "B";
                        channel.BasicPublish(exchange: exchangeName,
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: System.Text.Encoding.UTF8.GetBytes(routingKey));
                        Console.WriteLine($" [x] Sent messages");
                    }
                }
            }));
            cmds.Add(new Command($"r31d,Receive,Direct,{RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1}", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1;
                string queueName = "QueueForDirectA";
                var routingKey = "A";
                var factory = new ConnectionFactory() { HostName = MQHost };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare(queueName).QueueName;
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
                        var tag = channel.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine($" started for {queueName},tag:{tag}");
                        Console.ReadLine();
                    }
                }
            }));
            cmds.Add(new Command($"r32d,Receive,Direct,{RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1}", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1;
                string queueName = "QueueForDirectB";
                var routingKey = "B";
                var factory = new ConnectionFactory() { HostName = MQHost };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare(queueName).QueueName;
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
            cmds.Add(new Command($"r33d,Receive,Direct,{RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1}", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1;
                string queueName = "QueueForDirectC";
                var routingKey = "C";
                var factory = new ConnectionFactory() { HostName = MQHost };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare(queueName).QueueName;
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
            cmds.Add(new Command($"r34d,Receive,Direct,{RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1}", () =>
            {
                var exchangeType = ExchangeType.Direct;
                var exchangeName = RabbitMQUtils.RabbitMQHelper.Direct_Exchange_Name1;
                string queueName = "QueueForDirectA2";
                var routingKey = "A";
                var factory = new ConnectionFactory() { HostName = MQHost };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                        queueName = channel.QueueDeclare(queueName).QueueName;
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
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };
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
                var factory = new ConnectionFactory() { HostName = MQHost };
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
                var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = MQHost };
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
                var factory = new ConnectionFactory() { HostName = MQHost };
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
            #region SQL Generate
            cmds.Add(new Command("---------------------SQL Generate-------------------", () => { }));
            cmds.Add(new Command("g0408,生成差异化sql for 临安2.0升级 ", () =>
            {
                using (var connection = new SqlConnection(HeleOuterMSSQL))
                {
                    var allFields = connection.Query<Information_Schema>(@"
    select v2.* 
    from 
    (
		SELECT 'v2' as version,col.*,tb.table_type
		FROM HL_APP.INFORMATION_SCHEMA.COLUMNS col
		LEFT JOIN HL_APP.INFORMATION_SCHEMA.TABLEs tb on col.table_name = tb.table_name
		where tb.table_type = 'BASE TABLE'
    )
    as v2
    left join (
		SELECT 'v1' as version,a.*
		FROM LA_Test1.INFORMATION_SCHEMA.COLUMNS a
    ) v1 on v2.Table_Name = v1.Table_Name and v2.Column_Name = v1.Column_Name
    where v1.Column_Name is null
    union
    select v2.* 
    from 
    (
		SELECT 'v2' as version,col.*,tb.table_type
		FROM HL_Manage.INFORMATION_SCHEMA.COLUMNS col
		LEFT JOIN HL_Manage.INFORMATION_SCHEMA.TABLEs tb on col.table_name = tb.table_name
		where tb.table_type = 'BASE TABLE'
    )
    as v2
    left join (
		SELECT 'v1' as version,a.*
		FROM LA_Test1.INFORMATION_SCHEMA.COLUMNS a
    ) v1 on v2.Table_Name = v1.Table_Name and v2.Column_Name = v1.Column_Name
    where v1.Column_Name is null
    union
    select v2.* 
    from 
    (
		SELECT 'v2' as version,col.*,tb.table_type
		FROM HL_Pregnant.INFORMATION_SCHEMA.COLUMNS col
		LEFT JOIN HL_Pregnant.INFORMATION_SCHEMA.TABLEs tb on col.table_name = tb.table_name
		where tb.table_type = 'BASE TABLE'
    )
    as v2
    left join (
		SELECT 'v1' as version,a.*
		FROM LA_Test1.INFORMATION_SCHEMA.COLUMNS a
    ) v1 on v2.Table_Name = v1.Table_Name and v2.Column_Name = v1.Column_Name
    where v1.Column_Name is null
    union
    select v2.* 
    from 
    (
		SELECT 'v2' as version,col.*,tb.table_type
		FROM HL_ReportCard.INFORMATION_SCHEMA.COLUMNS col
		LEFT JOIN HL_ReportCard.INFORMATION_SCHEMA.TABLEs tb on col.table_name = tb.table_name
		where tb.table_type = 'BASE TABLE'
    )
    as v2
    left join (
		SELECT 'v1' as version,a.*
		FROM LA_Test1.INFORMATION_SCHEMA.COLUMNS a
    ) v1 on v2.Table_Name = v1.Table_Name and v2.Column_Name = v1.Column_Name
    where v1.Column_Name is null
    union
    select v2.* 
    from 
    (
		SELECT 'v2' as version,col.*,tb.table_type
		FROM HL_Share.INFORMATION_SCHEMA.COLUMNS col
		LEFT JOIN HL_Share.INFORMATION_SCHEMA.TABLEs tb on col.table_name = tb.table_name
		where tb.table_type = 'BASE TABLE'
    )
    as v2
    left join (
		SELECT 'v1' as version,a.*
		FROM LA_Test1.INFORMATION_SCHEMA.COLUMNS a
    ) v1 on v2.Table_Name = v1.Table_Name and v2.Column_Name = v1.Column_Name
    where v1.Column_Name is null
    order by Table_Name,Column_Name;
").ToList();
                    var fileName = @"D:\sqlGenerate.txt";
                    StringBuilder sb = new StringBuilder();
                    var tables = allFields.GroupBy(c => c.TABLE_NAME);
                    foreach (var fields in tables)
                    {
                        sb.AppendLine($"alter table [{fields.Key}] add");
                        foreach (var field in fields)
                        {
                            sb.AppendLine($"[{field.COLUMN_NAME}] " +
                                $"{field.DATA_TYPE}{GetFieldLength(field)}" +
                                $" {(field.IS_NULLABLE.ToUpper() == "YES" ? "null" : "not null")}" +
                                $" {(field.COLUMN_DEFAULT == null ? "" : "default " + field.COLUMN_DEFAULT)}" +
                                $"{(fields.Last() == field ? "" : ",")}");
                        }
                        sb.AppendLine($";");
                    }
                    File.WriteAllText(fileName, sb.ToString());
                    Console.WriteLine("Task Completed");
                }
            }));
            cmds.Add(new Command("g0420,生成差异化sql for 哈密产科 ", () =>
            {
                using (var connection = new SqlConnection(HeleOuterMSSQL))
                {
                    #region v1
                    //                    var allFields = connection.Query<Information_Schema>(@"
                    //select v2.* 
                    //from 
                    //(
                    //SELECT 'v2' as version,col.*,tb.table_type
                    //FROM [fypt-dev].INFORMATION_SCHEMA.COLUMNS col
                    //LEFT JOIN [fypt-dev].INFORMATION_SCHEMA.TABLEs tb on col.table_name = tb.table_name
                    //where tb.table_type = 'BASE TABLE'
                    //)
                    //as v2
                    //left join (
                    //SELECT 'v1' as version,a.*
                    //FROM DataExchange.INFORMATION_SCHEMA.COLUMNS a
                    //) v1 on v2.Table_Name = v1.Table_Name and v2.Column_Name = v1.Column_Name
                    //where v1.Column_Name is null
                    //and v2.Table_Name in ('PregnantInfo','MHC_FirstVisitRecord')
                    //order by Table_Name,Column_Name;
                    //").ToList(); 
                    #endregion
                    #region v2
                    var allFields = connection.Query<Information_Schema>(@"
select 
a.Table_Name AS 表名,
a.Column_Name AS 列名,
a.Data_Type as 数据类型,
a.Character_maximum_length AS 占用字节数,
a.Numeric_Precision AS 数字长度,
a.Numeric_Scale AS 小数位数,
a.Is_Nullable  AS 是否允许空,
(
	select top 1 prop.Value 
	from HL_Pregnant.sys.columns col 
	left join HL_Pregnant.sys.extended_properties prop on (col.object_id = prop.major_id AND prop.minor_id = col.column_id)
	where (col.name = a.Column_Name) and prop.value !=''
) as 注释,
a.* 
from (
	select v2.* 
	from 
	(
	SELECT 'v2' as version,col.*,tb.table_type
	FROM [HL_Pregnant].INFORMATION_SCHEMA.COLUMNS col
	LEFT JOIN HL_Pregnant.INFORMATION_SCHEMA.TABLEs tb on col.table_name = tb.table_name
	where tb.table_type = 'BASE TABLE'
	)
	as v2
	left join (
	SELECT 'v1' as version,a.*
	FROM DataExchange.INFORMATION_SCHEMA.COLUMNS a
	) v1 on v2.Table_Name = v1.Table_Name and v2.Column_Name = v1.Column_Name
	where v1.Column_Name is null
	and v2.Table_Name in ('PregnantInfo','MHC_FirstVisitRecord')
) a
order by Table_Name,Column_Name;
").ToList();
                    #endregion
                    // generate alter sql
                    var fileName = "";
                    StringBuilder sb = null;
                    var tables = allFields.GroupBy(c => c.TABLE_NAME);
                    foreach (var fields in tables)
                    {
                        fileName = $@"D:\Generate_AlterTable_{fields.Key}.txt";
                        sb = new StringBuilder();
                        sb.AppendLine($"alter table [{fields.Key}] add");
                        foreach (var field in fields)
                        {
                            sb.AppendLine($"[{field.COLUMN_NAME}] " +
                                $"{field.DATA_TYPE}{GetFieldLength(field)}" +
                                $" {(field.IS_NULLABLE.ToUpper() == "YES" ? "null" : "not null")}" +
                                $" {(field.COLUMN_DEFAULT == null ? "" : "default " + field.COLUMN_DEFAULT)}" +
                                $"{(fields.Last() == field ? "" : ",")}");
                        }
                        sb.AppendLine($";");
                        File.WriteAllText(fileName, sb.ToString());
                    }
                    // generate C# entity
                    tables = allFields.GroupBy(c => c.TABLE_NAME);
                    foreach (var fields in tables)
                    {
                        fileName = $@"D:\Generate_entity_{fields.Key}.txt";
                        sb = new StringBuilder();
                        foreach (var field in fields)
                        {
                            sb.AppendLine($"// " + field.注释);
                            var csType = GetCSType(field.数据类型);
                            sb.AppendLine($"public {csType}{(csType == "string" || field.IS_NULLABLE != "YES" ? "" : "?")} {field.列名}{{get; set;}}");
                        }
                        File.WriteAllText(fileName, sb.ToString());
                    }

                    Console.WriteLine("Task Completed");
                }
            }));
            cmds.Add(new Command("s0000,数据库连接测试", () =>
            {
                var connectingString = "Data Source=127.0.0.1;Initial Catalog=VL_DEP;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=admin;Password=123";
                try
                {
                    using (var connection = new SqlConnection(connectingString))
                    {
                        connection.Open();
                        connection.Close();
                    }
                    Console.WriteLine("数据库连接成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("数据库连接失败," + ex.ToString());
                }
            }));
            cmds.Add(new Command("s0525,数据库插入性能测试", () =>
            {
                var amount = 1000000;
                Console.WriteLine($"基于连接:{nameof(LocalMSSQL)}测试");
                //一次连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}
                if (false)
                {
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        var id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                        id++;
                        var t1 = DateTime.Now;
                        for (long i = id; i < id + amount; i++)
                        {
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = 0.ToString(),
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            var sql = @$"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                            var result = connection.Execute(sql, entity);
                        }
                        var t2 = DateTime.Now;
                        var ts = t2 - t1;
                        Console.WriteLine($"一次连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}");
                    } 
                }
                //独立连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}
                if (false)
                {
                    var id = 0L;
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                    }
                    id++;
                    var t1 = DateTime.Now;
                    for (long i = id; i <= id + amount; i++)
                    {
                        using (var connection = new SqlConnection(LocalMSSQL))
                        {
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = "0",
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            var sql = @$"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                            var result = connection.Execute(sql, entity);
                        }
                    }
                    var t2 = DateTime.Now;
                    var ts = t2 - t1;
                    Console.WriteLine($"独立连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}");
                }
                //独立连接{amount}次插入(有事务),耗时:{ts.TotalSeconds}
                if (false)
                {
                    var id = 0L;
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                    }
                    id++;
                    var t1 = DateTime.Now;
                    for (long i = id; i <= id + amount; i++)
                    {
                        using (var connection = new SqlConnection(LocalMSSQL))
                        {
                            connection.Open();
                            var transaction = connection.BeginTransaction();
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = "0",
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            var sql = @$"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                            var result = connection.Execute(sql, entity, transaction);
                            transaction.Commit();
                            connection.Close();
                        }
                    }
                    var t2 = DateTime.Now;
                    var ts = t2 - t1;
                    Console.WriteLine($"独立连接{amount}次插入(有事务),耗时:{ts.TotalSeconds}");
                }
                //一次连接{amount}次一批次{bach}条插入(无事务),耗时:{ts.TotalSeconds}
                if (false)
                {
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        var bach = 100;
                        var id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                        id++;
                        var t1 = DateTime.Now;
                        var currentBach = 1;
                        for (long i = id; i < id + amount; i++)
                        {
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = "0",
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            for (int j = currentBach; j <= bach; j++)
                            {
                                var sql = @$"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                                var result = connection.Execute(sql, entity);
                                i++;
                            }
                        }
                        var t2 = DateTime.Now;
                        var ts = t2 - t1;
                        Console.WriteLine($"一次连接{amount}次一批次{bach}条插入(无事务),耗时:{ts.TotalSeconds}");
                    }
                }
                //一次连接{amount}次插入(无事务+SqlBulkCopy),耗时:{ts.TotalSeconds}
                if (true)
                {
                    var id = 0L;
                    using (var connection = new SqlConnection(LocalMSSQL))
                    {
                        id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                        id++;
                    }
                    List<O_LabResult> entities = new List<O_LabResult>();
                    List<string> items = new List<string>() {
                        "胆红素",
                        "滴虫",
                        "电解质六项",
                        "外观",
                        "球菌",
                        "有核红细胞",
                        "抗链球菌溶血素O",
                        "中性粒细胞百分比",
                        "尿比重",
                        "尿隐血",
                        "促甲状腺激素",
                        };
                    var itemsCount = items.Count;
                    Random r = new Random();
                    for (long i = id; i < id + amount; i++)
                    {
                        var personId = i % 777777;
                        var itemId = r.Next(0, itemsCount);
                        O_LabResult entity = new O_LabResult()
                        {
                            ID = i,
                            patientid = "E" + personId,
                            //idcard = "3303" + personId.ToString().ToMD5().Substring(0, 12),//330326 6 19910508 8 0033 4
                            name = personId.ToString(),
                            orderid = i.ToString(),
                            setid = null,
                            itemid = itemId.ToString(),
                            itemname = items[itemId],
                            value = r.Next(1, 1000).ToString(),
                            deliverydate = DateTime.Now.AddDays(-r.Next(1, 100)),
                        };
                        entities.Add(entity);
                    }
                    var dt = entities.ToDataTable();
                    var t1 = DateTime.Now;
                    using (SqlBulkCopy sbc = new SqlBulkCopy(LocalMSSQL))
                    {
                        sbc.BatchSize = dt.Rows.Count;
                        sbc.BulkCopyTimeout = 10;
                        sbc.DestinationTableName = "O_LabResult";
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sbc.ColumnMappings.Add(dt.Columns[i].ColumnName, i);
                        }
                        //全部写入数据库
                        sbc.WriteToServer(dt);
                    }
                    var t2 = DateTime.Now;
                    var ts = t2 - t1;
                    Console.WriteLine($"一次连接{amount}次插入(无事务+SqlBulkCopy),耗时:{ts.TotalSeconds}");
                }
                Console.WriteLine($"基于连接:{nameof(HeleInnerMSSQL)}测试");
                //一次连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}
                if (false)
                {
                    using (var connection = new SqlConnection(HeleInnerMSSQL))
                    {
                        var id = connection.ExecuteScalar<long>(@"select max(id) from O_LabResult");
                        id++;
                        var t1 = DateTime.Now;
                        for (long i = id; i < id + amount; i++)
                        {
                            O_LabResult entity = new O_LabResult()
                            {
                                ID = i,
                                patientid = 0.ToString(),
                                idcard = i.ToString(),
                                name = i.ToString(),
                                orderid = i.ToString(),
                                setid = 0,
                                deliverydate = DateTime.Now,
                            };
                            var sql = @$"insert into O_LabResult(ID,patientid,idcard,name,orderid,setid,deliverydate) values(@ID,@patientid,@idcard,@name,@orderid,@setid,@deliverydate)";
                            var result = connection.Execute(sql, entity);
                        }
                        var t2 = DateTime.Now;
                        var ts = t2 - t1;
                        Console.WriteLine($"一次连接{amount}次插入(无事务),耗时:{ts.TotalSeconds}");
                    }
                }

            }));
            #endregion
            #region XML
            cmds.Add(new Command("---------------------XML-------------------", () => { }));
            cmds.Add(new Command("c2,CompareTwo", () =>
            {
                if (false)
                {
                    //第一种规格
                    var file = Path.Combine(System.Environment.CurrentDirectory, @"docs\test1.xml");
                    XDocument doc = new XDocument();//创建XML文档
                    var root = new XElement("Employees"); //创建根元素
                    root.Add(new XElement("Name", "Bob Smith"));
                    root.Add(new XElement("Name", "Sally Jones"));
                    doc.Add(root);
                    doc.Save(file);     //保存文件
                    var readXML = XDocument.Load(file);  //加载XML文档
                    var elements = readXML.Root.Elements("Name");
                    var employees = elements.Select(c => new { Name = c.Value });
                }
                if (false)
                {
                    //第二种规格
                    var file = Path.Combine(System.Environment.CurrentDirectory, @"docs\test2.xml");
                    XDocument doc = new XDocument();//创建XML文档
                    var root = new XElement("Employees"); //创建根元素
                    root.Add(new XElement("Employee", new XAttribute("Name", "Bob"), new XAttribute("Age", "16")));
                    root.Add(new XElement("Employee", new XAttribute("Name", "Sally")));
                    doc.Add(root);
                    doc.Save(file);     //保存文件
                    var readXML = XDocument.Load(file);  //加载XML文档
                    var elements = readXML.Root.Elements("Employee");
                    var employees = elements.Select(c => new { Name = c.Attribute("Name")?.Value, Age = c.Attribute("Age")?.Value });
                }
                if (false)
                {
                    //运用反射解析对象
                    //TODO
                }
                if (true)
                {
                    var reflects_DZBL = XDocument.Load(Path.Combine(System.Environment.CurrentDirectory, @"docs\电子病历\SelectionReflect.xml"))
                        .Root.Elements(nameof(Reflect))
                        .Select(c => new Reflect { id = c.Attribute("id")?.Value, value = c.Attribute("value")?.Value });
                    var selections_DZBL = XDocument.Load(Path.Combine(System.Environment.CurrentDirectory, @"docs\电子病历\SelectionXML.xml"))
                        .Root.Elements(nameof(Select))
                        .Select(c => new Select
                        {
                            id = c.Attribute("id")?.Value,
                            sourece = c.Attribute("value")?.Value,
                            Options = c.Elements(nameof(Option)).Select(o => new Option()
                            {
                                id = o.Attribute("id")?.Value,
                                value = o.Attribute("value")?.Value,
                                text = o.Attribute("text")?.Value,
                                selected = o.Attribute("selected")?.Value,
                                groupno = o.Attribute("groupno")?.Value,
                                rule = o.Attribute("rule")?.Value,
                                remark = o.Attribute("remark")?.Value,
                                editable = o.Attribute("editable")?.Value,
                                dictionary = o.Attribute("dictionary")?.Value,
                                classfiction = o.Attribute("classfiction")?.Value,
                                auto = o.Attribute("auto")?.Value,
                                version = o.Attribute("version")?.Value,
                                parentid = o.Attribute("parentid")?.Value,
                            }).ToList()
                        });
                    var reflects_FYPT = XDocument.Load(Path.Combine(System.Environment.CurrentDirectory, @"docs\妇幼平台\SelectionReflect.xml"))
                        .Root.Elements("Reflect")
                        .Select(c => new Reflect { id = c.Attribute("id")?.Value, value = c.Attribute("value")?.Value });
                    var selections_FYPT = XDocument.Load(Path.Combine(System.Environment.CurrentDirectory, @"docs\妇幼平台\SelectionXML.xml"))
                        .Root.Elements(nameof(Select))
                        .Select(c => new Select
                        {
                            id = c.Attribute("id")?.Value,
                            sourece = c.Attribute("value")?.Value,
                            Options = c.Elements(nameof(Option)).Select(o => new Option()
                            {
                                id = o.Attribute("id")?.Value,
                                value = o.Attribute("value")?.Value,
                                text = o.Attribute("text")?.Value,
                                selected = o.Attribute("selected")?.Value,
                                groupno = o.Attribute("groupno")?.Value,
                                rule = o.Attribute("rule")?.Value,
                                remark = o.Attribute("remark")?.Value,
                                editable = o.Attribute("editable")?.Value,
                                dictionary = o.Attribute("dictionary")?.Value,
                                classfiction = o.Attribute("classfiction")?.Value,
                                auto = o.Attribute("auto")?.Value,
                                version = o.Attribute("version")?.Value,
                                parentid = o.Attribute("parentid")?.Value,
                            }).ToList()
                        });
                    var diffs = new List<SelectDifferent>();
                    var errrors = new List<string>();
                    foreach (var reflect_DZBL in reflects_DZBL)
                    {
                        var reflect_FYPT = reflects_FYPT.FirstOrDefault(c => c.id == reflect_DZBL.id);
                        if (reflect_FYPT == null)
                            continue;
                        var select_FYPT = selections_FYPT.FirstOrDefault(c => c.id == reflect_FYPT.value);
                        if (select_FYPT == null)
                        {
                            errrors.Add("缺少FYPT.Selection配置,配置项:" + reflect_FYPT.value);
                            continue;
                        }
                        var select_DZBL = selections_DZBL.FirstOrDefault(c => c.id == reflect_DZBL.value);
                        if (select_DZBL == null)
                        {
                            errrors.Add("缺少DZBL.Selection配置,配置项:" + reflect_DZBL.value);
                            continue;
                        }
                        var differentType = 0;
                        Option option_DZBL = null, option_FYPT = null;
                        if (select_FYPT.id != select_DZBL.id)
                        {
                            differentType = 1;//缺少匹配的 Select 项
                            AddDiff(diffs, reflect_DZBL, reflect_FYPT, select_FYPT, select_DZBL, differentType, option_DZBL, option_FYPT);
                        }
                        else if (select_FYPT.Options.Count != select_DZBL.Options.Count())
                        {
                            differentType = 2;//Options 数不一致
                            AddDiff(diffs, reflect_DZBL, reflect_FYPT, select_FYPT, select_DZBL, differentType, option_DZBL, option_FYPT);
                        }
                        else
                        {
                            foreach (var option in select_FYPT.Options)
                            {
                                option_FYPT = option;
                                option_DZBL = select_DZBL.Options.FirstOrDefault(c => c.value == option_FYPT.value);
                                if (option_DZBL == null)
                                {
                                    differentType = 3;//缺少匹配的 Option 项
                                    AddDiff(diffs, reflect_DZBL, reflect_FYPT, select_FYPT, select_DZBL, differentType, option_DZBL, option_FYPT);
                                    continue;
                                }
                                if (option_FYPT.text != option_DZBL.text)
                                {
                                    differentType = 4;//Option 值不一致
                                    AddDiff(diffs, reflect_DZBL, reflect_FYPT, select_FYPT, select_DZBL, differentType, option_DZBL, option_FYPT);
                                    continue;
                                }
                            }
                        }
                    }
                    var reports = diffs.Select(c => $"DifferentType:{c.DifferentType}\r\n" +
                    $"DZBL 项:{c.SourceReflect.id},Select:{c.SourceSelect?.id},Option:{c.SourceOption?.value},{c.SourceOption?.text}\r\n" +
                    $"FYPT 项:{c.TargetReflect.id},Select:{c.TargetSelect?.id},Option:{c.TargetOption?.value},{c.TargetOption?.text}\r\n\r\n");
                    var reportStr = string.Join("", reports);
                    var errorStr = string.Join("\r\n", errrors);

                }
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
                    var model = new DrawTXJHModel();
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
                    var entity = new GetDataForTXJHModel();
                    entity.RecordCode = binglih;
                    entity.StartTime = startTime;
                    entity.FetalHeartData = string.Join(",", model.data1);
                    entity.UCData = string.Join(",", model.data3);
                    #endregion

                    using (var connection = DBHelper.GetDbConnection(@"Data Source=192.168.50.102;Initial Catalog=fmpt;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=huzfypt;Password=huz3305@2018."))
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
            #region Algorithm,算法
            cmds.Add(new Command("---------------------Algorithm,算法-------------------", () => { }));
            cmds.Add(new Command("a1,冒泡排序", () =>
            {
                //内容很少,就两个for循环,一个变量交换
                //核心就是第二个for的边界 -i
                //基本思想就是:~

                int[] arr = { 23, 44, 66, 76, 98, 11, 3, 9, 7, 23, 44, 66, 98, 11, 3, 9, 7, 23, 44, 66, 76, 98, 11, 3, 9, 7 };
                Console.WriteLine("排序前的数组：");
                foreach (int item in arr)
                {
                    Console.Write(item + ",");
                }
                Console.WriteLine();
                arr.BubbleSort();
                Console.WriteLine("排序后的数组：");
                foreach (int item in arr)
                {
                    Console.Write(item + ",");
                }
                Console.WriteLine();
                Console.ReadKey();

            }));
            cmds.Add(new Command("a2,快速排序", () =>
            {
                //时间复杂度
                //在a取数组的第一项时候，是最糟的情况，完成每层需要的时间是n，栈的高度是n，时间复杂度就是n²
                //当取中间的值得时候，完成每层的时间是n，但是调用栈的高度变成了logn

                int[] arr = { 23, 44, 66, 76, 98, 11, 3, 9, 7, 23, 98, 11, 3, 9, 7, 23, 44, 66, 44, 66, 76, 98, 11, 3, 9, 7 };
                Console.WriteLine("排序前的数组：");
                foreach (int item in arr)
                {
                    Console.Write(item + ",");
                }
                Console.WriteLine();
                arr.QuickSort();
                Console.WriteLine("排序后的数组：");
                foreach (int item in arr)
                {
                    Console.Write(item + ",");
                }
                Console.WriteLine();
                Console.ReadKey();
            }));
            cmds.Add(new Command("a3,插入排序", () =>
            {
                //内容很少,就两个for循环,一个变量交换
                //核心就是第二个for的边界 -i
                //基本思想就是:~

                int[] arr = { 23, 44, 66, 76, 98, 11, 3, 9, 7, 23, 44, 66, 98, 11, 3, 9, 7, 23, 44, 66, 76, 98, 11, 3, 9, 7 };
                Console.WriteLine("排序前的数组：");
                foreach (int item in arr)
                {
                    Console.Write(item + ",");
                }
                Console.WriteLine();
                arr.InsertionSort();
                Console.WriteLine("排序后的数组：");
                foreach (int item in arr)
                {
                    Console.Write(item + ",");
                }
                Console.WriteLine();
                Console.ReadKey();
            }));
            cmds.Add(new Command("a999,对比性能", () =>
            {
                Console.WriteLine($"请输入模拟的数量");
                var s = Console.ReadLine();
                int count = s.ToInt().Value;
                int[] arr = new int[count];
                Random r = new Random();
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = r.Next(10000);
                }
                int[] arr1 = new int[arr.Length];
                int[] arr2 = new int[arr.Length];
                int[] arr3 = new int[arr.Length];
                arr.CopyTo(arr1, 0);
                arr.CopyTo(arr2, 0);
                arr.CopyTo(arr3, 0);
                Console.WriteLine($"数据总数：{arr.Length}");
                var t1 = TimeSpanHelper.GetTimeSpan(() =>
                {
                    arr1.BubbleSort();
                });
                Console.WriteLine($"BubbleSort：{t1.TotalMilliseconds}");
                var t2 = TimeSpanHelper.GetTimeSpan(() =>
                {
                    arr2.InsertionSort();
                });
                Console.WriteLine($"InsertionSort：{t2.TotalMilliseconds}");
                var t3 = TimeSpanHelper.GetTimeSpan(() =>
                {
                    arr3.QuickSort();
                });
                Console.WriteLine($"QuickSort：{t3.TotalMilliseconds}");

                //数据总数：1000
                //BubbleSort：4.3723
                //InsertionSort：0.9022
                //QuickSort：0.592
                //数据总数：10000
                //BubbleSort：277.8679
                //InsertionSort：77.777
                //QuickSort：1.4341
                //数据总数：100000
                //BubbleSort：30267.2289
                //InsertionSort：7307.5606
                //QuickSort：13.502
            }));


            #endregion
            #region Gang Of 4 Patterns,21种设计模式
            #region Creational
            //Creational design patterns are design patterns that deal with object creation mechanisms, trying to create objects in a manner suitable to the situation.


            /// Factory Method-工厂方法
            /// It creates objects without specifying the exact class to create.
            /// Abstract Factory-抽象工厂
            /// It provides a way to encapsulate a group of individual factories that have a common theme.
            /// Builder-构造器模式,隔离复杂对象的创建和表现
            /// It constructs complex objects by separating construction and representation.
            /// Prototype-原型
            /// It creates objects by cloning an existing object.
            /// Singleton-单例
            /// It restricts object creation for a class to only one instance.


            #endregion
            #region Structural
            //Structural design patterns are design patterns that ease the design by identifying a simple way to realize relationships between entities.


            /// Adapter - 适配结构
            /// It allows classes with incompatible interfaces to work together by wrapping its own interface around that of an already existing class.
            /// Bridge-异化结构
            /// It decouples an abstraction from its implementation so that the two can vary independently.
            /// Composite-集合结构
            /// It composes zero-or-more similar objects so that they can be manipulated as one object.
            /// Decorator-扩展结构,额外的装饰类,可以动态更改主体行为
            /// It dynamically adds/overrides behavior in an existing method of an object.
            /// Facade-界面结构
            /// It provides a simplified interface to a large body of code.
            /// Flyweight-共享结构
            /// A large quantity of objects share a common properties object to save space.
            /// It reduces the cost of creating and manipulating a large number of similar objects.
            /// Proxy-代理结构
            /// It provides a placeholder for another object to control access, reduce cost, and reduce complexity.


            #endregion
            #region Behavioral
            //Behavioral design patterns are design patterns that identify common communication patterns between objects and realize these patterns.


            /// Chain of responsibility-责任链-行为传递
            /// It delegates commands to a chain of processing objects.
            /// Command-命令-封装
            /// It creates objects which encapsulate all information needed to perform an action or trigger an event at a later time.
            /// Interpreter-翻译-转译
            /// It implements a specialized language.
            /// Iterator-迭代-查阅
            /// It accesses the elements of an object sequentially without exposing its underlying representation.
            /// Mediator-中介-协调
            /// It allows loose coupling between classes by being the only class that has detailed knowledge of their methods.
            /// Memento-备忘录-备份
            /// It provides the ability to restore an object to its previous state.
            /// Observer-观测者-一对多订阅
            /// It is a publish/subscribe pattern which allows a number of observer objects to see an event.
            /// State-状态-状态影响系列行为,强调状态与行为的归整,易更改
            /// It allows an object to alter its behavior when its internal state changes.
            /// Strategy-策略-策略主导行为, 强调行为的多样性, 策略化的易替换
            /// It allows one of a family of algorithms to be selected on-the-fly at runtime.
            /// Template method-模板-套路
            /// It defines the skeleton of an algorithm as an abstract class, allowing its subclasses to provide concrete behavior.
            /// Visitor-访问者-查阅
            /// It separates an algorithm from an object structure by moving the hierarchy of methods into one object.


            #endregion
            #endregion
            #region Architectural Patterns,企业应用设计模式

            //Active Record,活动记录
            //Action-Domain-Responder,ADR
            //Data Access Object,DAO
            //Data Transfer Object,DTO
            //Front Controller,前端控制器
            //Identity Map,标识地图
            //Interceptor,拦截者
            //Inversion of Control,IoC
            //Model-View-Controller,MVC
            //n-tier,分层
            //Naked Objects,裸对象
            //Publish-Subscribe,发布/订阅
            //Service Locator,服务定位器
            //Specification,规格

            #endregion
            #region Scheduler
            cmds.Add(new Command("---------------------Scheduler-------------------", () => { }));
            cmds.Add(new Command("s1,Scheduler", () =>
            {
                Task.Factory.StartNew(() => {
                    Console.WriteLine("任务一已启用");
                    while (true)
                    {
                        System.Threading.Thread.Sleep(3 * 1000);
                        Console.WriteLine("任务一正在执行");
                    }
                });
                Task.Factory.StartNew(() => {
                    Console.WriteLine("任务二已启用");
                    while (true)
                    {
                        System.Threading.Thread.Sleep(3 * 1000);
                        Console.WriteLine("任务二正在执行");
                    }
                });
                Console.WriteLine();
                Console.ReadKey();
            }));
            #endregion
            cmds.Start();
        }

        class O_LabResult
        {
            public long ID { set; get; }
            public string patientid { set; get; }
            public string idcard { set; get; }
            public string name { set; get; }
            public string orderid { set; get; }
            public int? setid { set; get; }
            public string itemid { set; get; }
            public string itemname { set; get; }
            public string value { set; get; }
            public string unit { set; get; }
            public string reference { set; get; }
            public long resultflag { set; get; }
            public long status { set; get; }
            public DateTime deliverydate { set; get; }
        }

        private static string GetCSType(string 数据类型)
        {
            switch (数据类型)
            {
                case "varchar": return "string";
                case "nchar": return "string";
                case "char": return "string";
                case "date": return "DateTime";
                case "datetime": return "DateTime";
                case "datetime2": return "DateTime";
                case "decimal": return "decimal";
                case "int": return "int";
                default: return "";
                    //throw new NotImplementedException("");
            }
        }

        private static void AddDiff(List<SelectDifferent> diffs, Reflect reflect_DZBL, Reflect reflect_FYPT, Select select_FYPT, Select select_DZBL, int differentType, Option option_DZBL, Option option_FYPT)
        {
            diffs.Add(new SelectDifferent()
            {
                SourceReflect = reflect_DZBL,
                SourceSelect = select_DZBL,
                SourceOption = option_DZBL,

                TargetReflect = reflect_FYPT,
                TargetSelect = select_FYPT,
                TargetOption = option_FYPT,

                DifferentType = differentType,
            });
        }

        private static string GetFieldLength(Information_Schema field)
        {
            if (string.IsNullOrEmpty(field.CHARACTER_MAXIMUM_LENGTH))
                return "";


            if (field.CHARACTER_MAXIMUM_LENGTH == "-1")
                return "(max)";

            return "(" + field.CHARACTER_MAXIMUM_LENGTH + ")";
        }
    }

    #region FileWatcher

    /// <summary>
    /// 胎心监护数据
    /// </summary>
    public class DrawTXJHModel
    {
        public int[] data1 { set; get; }
        public int[] data2 { set; get; }
        public int[] data3 { set; get; }
        public int[] data4 { set; get; }
        public int[] data5 { set; get; }
    }

    public class GetDataForTXJHModel
    {
        /// <summary>
        /// 病历号
        /// </summary>
        public string RecordCode { set; get; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 胎心数据
        /// </summary>
        public string FetalHeartData { set; get; }
        /// <summary>
        /// 宫缩数据
        /// </summary>
        public string UCData { set; get; }
    }

    #endregion

    #region SQL Generate

    public class Information_Schema
    {
        public string 表名 { get; set; }
        public string 列名 { get; set; }
        public string 数据类型 { get; set; }
        public string 占用字节数 { get; set; }
        public string 数字长度 { get; set; }
        public string 小数位数 { get; set; }
        public string 是否允许空 { get; set; }
        public string 注释 { get; set; }

        public string TABLE_CATALOG { get; set; }
        public string TABLE_SCHEMA { get; set; }
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public string ORDINAL_POSITION { get; set; }
        public string COLUMN_DEFAULT { get; set; }
        public string IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
        public string CHARACTER_MAXIMUM_LENGTH { get; set; }
        public string CHARACTER_OCTET_LENGTH { get; set; }
        public string NUMERIC_PRECISION { get; set; }
        public string NUMERIC_PRECISION_RADIX { get; set; }
        public string NUMERIC_SCALE { get; set; }
        public string DATETIME_PRECISION { get; set; }
        public string CHARACTER_SET_CATALOG { get; set; }
        public string CHARACTER_SET_SCHEMA { get; set; }
        public string CHARACTER_SET_NAME { get; set; }
        public string COLLATION_CATALOG { get; set; }
        public string COLLATION_SCHEMA { get; set; }
        public string COLLATION_NAME { get; set; }
        public string DOMAIN_CATALOG { get; set; }
        public string DOMAIN_SCHEMA { get; set; }
        public string DOMAIN_NAME { get; set; }
    }

    #endregion

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
                catch (Exception e
                )
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