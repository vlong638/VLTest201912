using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Threading.Tasks;

namespace Autobots.B2Service
{
    public class B2ServiceImpl : B2ServiceDefinition.B2ServiceDefinitionBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, global::Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("From Client," + request.ToJson());
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name +" From Server B2" });

        }
    }
}
