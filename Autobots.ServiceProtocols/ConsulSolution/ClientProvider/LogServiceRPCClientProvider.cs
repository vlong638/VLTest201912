using Grpc.Core;
using System;
using static Autobots.B2Service.B2ServiceDefinition;
using static Autobots.Infrastracture.LogCenter.LogServiceDefinition;

namespace Autobots.ServiceProtocols
{
    public class LogServiceRPCClientProvider : IRPCClientProvider<LogServiceDefinitionClient>
    {
        IRPCServiceConfigProvider ConfigProvider;

        public LogServiceRPCClientProvider(IRPCServiceConfigProvider configProvider)
        {
            ConfigProvider = configProvider;
        }

        public LogServiceDefinitionClient GetClient(string consulAddress, int consulPort)
        {
            string serviceName = nameof(Infrastracture.LogCenter);
            var serviceConfig = ConfigProvider.GetService(serviceName, consulAddress, consulPort);
            if (serviceConfig == null)
            {
                throw new NotImplementedException("依赖的服务无效");
            }
            Channel channel = new Channel($"{serviceConfig.Address}:{serviceConfig.Port}", ChannelCredentials.Insecure);
            return new LogServiceDefinitionClient(channel);
        }
    }
}
