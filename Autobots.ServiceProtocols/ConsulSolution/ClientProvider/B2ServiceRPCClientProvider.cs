using Grpc.Core;
using System;
using static Autobots.B2Service.B2ServiceDefinition;

namespace Autobots.ServiceProtocols
{
    public class B2ServiceRPCClientProvider : IRPCClientProvider<B2ServiceDefinitionClient>
    {
        IRPCServiceConfigProvider ConfigProvider;

        public B2ServiceRPCClientProvider(IRPCServiceConfigProvider configProvider)
        {
            ConfigProvider = configProvider;
        }

        public B2ServiceDefinitionClient GetClient(string consulAddress, int consulPort)
        {
            string serviceName = nameof(B2Service);
            var serviceConfig = ConfigProvider.GetService(serviceName, consulAddress, consulPort);
            if (serviceConfig == null)
            {
                throw new NotImplementedException("依赖的服务无效");
            }
            Channel channel = new Channel($"{serviceConfig.Address}:{serviceConfig.Port}", ChannelCredentials.Insecure);
            return new B2ServiceDefinitionClient(channel);
        }
    }
}
