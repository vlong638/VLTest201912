﻿using RabbitMQ.Client;
using System;

namespace VL.RabbitMQProducer
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
        }
    }
}
