using Autobots.B1ServiceDefinition;
using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Threading.Tasks;
using static Autobots.B1ServiceDefinition.B1Service;

namespace Autobots.B1Service
{
    public class B1ServiceImpl : B1ServiceBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, global::Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("From Client," + request.ToJson());
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });

        }
    }
}
