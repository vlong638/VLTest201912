using Autobots.B2ServiceDefinition;
using Autobots.Infrastracture.Common.ValuesSolution;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace Autobots.B2Service
{
    public class B2ServiceImpl : B2ServiceDefinition.B2Service.B2ServiceBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, global::Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("From Client," + request.ToJson());
            return Task.FromResult(new HelloReply { Message = "B2ServiceImpl => Hello " + request.Name });

        }
    }
}
