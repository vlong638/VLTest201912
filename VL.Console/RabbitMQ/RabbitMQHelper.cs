using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consoling.RabbitMQ
{
    public class RabbitMQHelper
    {

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
