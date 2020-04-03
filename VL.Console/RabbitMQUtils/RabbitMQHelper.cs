using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consoling.RabbitMQUtils
{
    ///默认用户 guest/guest
    ///管理后台 http://127.0.0.1:15672/
    public class RabbitMQHelper
    {
        public static string FunoutExchangeName1 = "ExchangeType.Funout.Logs";
        public static string FunoutExchangeName2 = "ExchangeType.Funout2";

        public static string Direct_Exchange_Name1 = "ExchangeType.Direct1";
        public static string Direct_Exchange_Durable_Name1 = "ExchangeType.DurableDirect1";

        public static string TopicExchangeName1 = "ExchangeType.Topic1";

        //public static void Send()
        //{
        //    var factory = new ConnectionFactory() { HostName = "localhost" };
        //    using (var connection = factory.CreateConnection())
        //    {
        //        using (var channel = connection.CreateModel())
        //        {
        //            channel.QueueDeclare(queue: "hello",
        //                          durable: false,
        //                          exclusive: false,
        //                          autoDelete: false,
        //                          arguments: null);

        //            string message = "Hello World!";
        //            var body = Encoding.UTF8.GetBytes(message);

        //            channel.BasicPublish(exchange: "",
        //                                 routingKey: "hello",
        //                                 basicProperties: null,
        //                                 body: body);
        //            Console.WriteLine(" [x] Sent {0}", message);
        //        }
        //    }
        //}
        //public static void Receive()
        //{
        //    var factory = new ConnectionFactory() { HostName = "localhost" };
        //    using (var connection = factory.CreateConnection())
        //    {
        //        using (var channel = connection.CreateModel())
        //        {
        //            channel.QueueDeclare(queue: "hello",
        //                          durable: false,
        //                          exclusive: false,
        //                          autoDelete: false,
        //                          arguments: null);

        //            var consumer = new EventingBasicConsumer(channel);
        //            consumer.Received += (model, ea) =>
        //            {
        //                var body = ea.Body;
        //                var message = Encoding.UTF8.GetString(body);
        //                Console.WriteLine(" [x] Received {0}", message);
        //            };
        //            channel.BasicConsume(queue: "hello",
        //                                 autoAck: true,
        //                                 consumer: consumer);

        //            Console.WriteLine(" Press [enter] to exit.");
        //            Console.ReadLine();
        //        }
        //    }
        //}

    }
}
