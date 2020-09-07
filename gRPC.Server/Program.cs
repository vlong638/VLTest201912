using Grpc.Core;
using System;
using System.Threading.Tasks;
using VL.Consolo_Core.Common.ValuesSolution;
using VLservice;

namespace gRPC.VLServer
{
    class VLservice01Impl : VLservice01.VLservice01Base
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine("From Client," + request.ToJson());
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }
    }

    class Program
    {
        const string Server = "localhost";
        const int Port = 30051;

        static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { VLservice01.BindService(new VLservice01Impl()) },
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
