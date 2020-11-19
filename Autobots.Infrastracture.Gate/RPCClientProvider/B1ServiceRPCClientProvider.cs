using Grpc.Core;
using System;

namespace Autobots.Infrastracture.Gateway
{
    public class B1ServiceRPCClientProvider : IRPCClientProvider<B1ServiceDefinition.B1Service.B1ServiceClient>
    {
        IRPCServiceConfigProvider ConfigProvider;

        public B1ServiceRPCClientProvider(IRPCServiceConfigProvider configProvider)
        {
            ConfigProvider = configProvider;
        }

        public B1ServiceDefinition.B1Service.B1ServiceClient GetClient()
        {
            string serviceName = "B1Service";
            var serviceConfig = ConfigProvider.GetService(serviceName);
            if (serviceConfig == null)
            {
                throw new NotImplementedException("依赖的服务无效");
            }
            Channel channel = new Channel($"{serviceConfig.Address}:{serviceConfig.Port}", ChannelCredentials.Insecure);
            return new B1ServiceDefinition.B1Service.B1ServiceClient(channel);
        }
    }
}
