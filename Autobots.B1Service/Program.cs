using Autobots.Infrastracture.Common.ConsulSolution;
using Consul;
using Grpc.Core;
using System;

namespace Autobots.B1Service
{
    class Program
    {
        const string ServiceName = "B1Service";
        const string Host = "127.0.0.1";
        const int Port = 30051;

        static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { B1ServiceDefinition.B1Service.BindService(new B1ServiceImpl()) , Grpc.Health.V1.Health.BindService(new HealthServiceImpl()) },
                Ports = { new ServerPort(Host, Port, ServerCredentials.Insecure) }
            };
            server.Start();

            var serviceId = $"{ServiceName}_{Host}:{Port}";
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(3),
                HTTP = new Uri("http://" + Host).OriginalString
            };
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                Address = Host,
                ID = serviceId,
                Name = ServiceName,
                Port = Port
            };
            var consul = new ConsulClient();
            consul.Config.Address = new Uri("http://127.0.0.1:8500");
            consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }
    }
}
