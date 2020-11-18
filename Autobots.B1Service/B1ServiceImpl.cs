﻿using Autobots.B1ServiceDefinition;
using Autobots.Infrastracture.Common.ValuesSolution;
using Grpc.Core;
using Grpc.Health.V1;
using System;
using System.Threading.Tasks;

namespace Autobots.B1Service
{

    public class HealthServiceImpl : Health.HealthBase
    {
        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            return base.Check(request, context);
        }

        public override Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context)
        {
            return base.Watch(request, responseStream, context);
        }
    }
    public class B1ServiceImpl : B1ServiceDefinition.B1Service.B1ServiceBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, global::Grpc.Core.ServerCallContext context)
        {
            Console.WriteLine("From Client," + request.ToJson());
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });

        }
    }
}
