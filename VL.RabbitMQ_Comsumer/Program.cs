using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace VL.RabbitMQComsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //默认用户 guest/guest
            //管理后台 http://127.0.0.1:15672/

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
