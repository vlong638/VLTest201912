

using Grpc.Core;
using System;
using VL.Consolo_Core.Common.ValuesSolution;
using VLservice;

namespace gRPC.Client
{
    class Program
    {
        const string Server = "localhost";
        const int Port = 30051;

        static void Main(string[] args)
        {
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);

            var client = new VLservice.VLservice01.VLservice01Client(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("FromServer, " + reply.ToJson());

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
