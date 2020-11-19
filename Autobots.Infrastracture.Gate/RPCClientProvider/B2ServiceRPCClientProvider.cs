using Grpc.Core;
using System;

namespace Autobots.Infrastracture.Gateway
{
    public class B2ServiceRPCClientProvider : IRPCClientProvider<B2ServiceDefinition.B2Service.B2ServiceClient>
    {
        IRPCServiceConfigProvider ConfigProvider;

        public B2ServiceRPCClientProvider(IRPCServiceConfigProvider configProvider)
        {
            ConfigProvider = configProvider;
        }

        public B2ServiceDefinition.B2Service.B2ServiceClient GetClient()
        {
            string serviceName = "B2Service";
            var serviceConfig = ConfigProvider.GetService(serviceName);
            if (serviceConfig == null)
            {
                throw new NotImplementedException("依赖的服务无效");
            }
            Channel channel = new Channel($"{serviceConfig.Address}:{serviceConfig.Port}", ChannelCredentials.Insecure);
            return new B2ServiceDefinition.B2Service.B2ServiceClient(channel);
        }
    }
}
