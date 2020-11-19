using Grpc.Core;
using System;
using static Autobots.B1Service.B1ServiceDefinition;

namespace Autobots.Infrastracture.Gateway
{
    public class B1ServiceRPCClientProvider : IRPCClientProvider<B1ServiceDefinitionClient>
    {
        IRPCServiceConfigProvider ConfigProvider;

        public B1ServiceRPCClientProvider(IRPCServiceConfigProvider configProvider)
        {
            ConfigProvider = configProvider;
        }

        public B1ServiceDefinitionClient GetClient()
        {
            string serviceName = "B1Service";
            var serviceConfig = ConfigProvider.GetService(serviceName);
            if (serviceConfig == null)
            {
                throw new NotImplementedException("依赖的服务无效");
            }
            Channel channel = new Channel($"{serviceConfig.Address}:{serviceConfig.Port}", ChannelCredentials.Insecure);
            return new B1ServiceDefinitionClient(channel);
        }
    }
}
