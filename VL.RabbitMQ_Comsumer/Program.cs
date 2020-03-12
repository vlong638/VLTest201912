using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using VL.Consoling.RabitMQUtils;

namespace VL.RabbitMQComsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //默认用户 guest/guest
            //管理后台 http://127.0.0.1:15672/

            //ConsumerForSimple();
            Console.WriteLine("type a value to route consumer ");
            string s;
            while ((s = Console.ReadLine()) != "q")
            {
                int i = 0;
                int.TryParse(s, out i);
                switch (i)
                {
                    case 0:
                        ConsumerFor<NamedMessage1>(ExchangeType.Fanout, nameof(NamedMessage1), nameof(NamedMessage1) + "无需");
                        break;
                    case 1:
                        ConsumerFor<NamedMessage2>(ExchangeType.Fanout, nameof(NamedMessage2), nameof(NamedMessage2) + "无需");
                        break;
                    case 2:
                        ConsumerFor<NamedMessage3>(ExchangeType.Direct, Consoling.RabbitMQUtils.RabbitMQHelper.DirectExchangeName, nameof(NamedMessage3));
                        break;
                    case 3:
                        ConsumerFor<NamedMessage4>(ExchangeType.Direct, Consoling.RabbitMQUtils.RabbitMQHelper.DirectExchangeName, nameof(NamedMessage4));
                        break;
                    case 4:
                        ConsumerFor<NamedMessage4>(ExchangeType.Direct, Consoling.RabbitMQUtils.RabbitMQHelper.DirectExchangeName + "必要", nameof(NamedMessage4));
                        break;
                    case 5:
                        ConsumerFor<NamedMessage4>(ExchangeType.Direct, Consoling.RabbitMQUtils.RabbitMQHelper.DirectExchangeName, nameof(NamedMessage4) + "必要");
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ConsumerFor<T>(string exchangeType, string exchangeName, string messageName)
        {
            var name = "Worker" + DateTime.Now.Second.ToString();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                      exchange: exchangeName,
                                      routingKey: messageName);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = System.Text.Encoding.UTF8.GetString(body);
                        Console.WriteLine($" [{DateTime.Now.ToString()}][{name}],type:{exchangeType} Received {message}");
                        var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(message);
                        System.Threading.Thread.Sleep(1 * 1000);
                    };

                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                    Console.WriteLine($"type:{exchangeType},exchange:{exchangeName},message:{messageName}");
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        private static void ConsumerForSimple()
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
                        var body = ea.Body;
                        var message = System.Text.Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };
                    channel.BasicConsume(queue: "hello",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
