using Autobots.Infrastracture.Common.ConfigSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Autobots.ServiceProtocols;
using System;
using System.Threading.Tasks;
using VLAutobots.Infrastracture.Common.FileSolution;

namespace Autobots.B2Service
{
    public class B2ServiceImpl : B2ServiceDefinition.B2ServiceDefinitionBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, global::Grpc.Core.ServerCallContext context)
        {
            var message = "From Client," + request.ToJson();

            #region logService
            var currentDirectory = FileHelper.GetDirectory("configs");
            var configText = FileHelper.ReadAllText(currentDirectory, "config.xml");
            var config = ConfigHelper.GetVLConfig(configText);
            var consulAddress = config.GetKey("ConsulService", "Address");
            var consulPort = config.GetKey("ConsulService", "Port").ToInt();
            var configProvider = new RPCServiceConfigProvider();
            var logService = new LogServiceRPCClientProvider(configProvider).GetClient(consulAddress, consulPort.Value);
            logService.InfoAsync(new Infrastracture.LogCenter.LogRequest()
            {
                Locator = nameof(SayHello),
                Message = message
            }); 
            #endregion

            Console.WriteLine(message);
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name +" From Server B2" });

        }
    }
}
