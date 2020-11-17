using Grpc.Core;
using System;

namespace Autobots.B1Service
{
    class Program
    {
        const string Server = "localhost";
        const int Port = 30051;

        static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { B1ServiceDefinition.B1Service.BindService(new B1ServiceImpl()) },
                Ports = { new ServerPort(Server, Port, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }
    }
}
