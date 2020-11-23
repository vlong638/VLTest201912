using System;
using System.Linq;

namespace Autobots.ServiceProtocols
{
    public interface IRPCServiceConfigProvider
    {
        ServiceConfig GetServiceConfig(string serviceName, string consulAddress, int consulPort);
    }

    public class RPCServiceConfigProvider : IRPCServiceConfigProvider
    {
        public RPCServiceConfigProvider()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServiceConfig GetServiceConfig(string serviceName, string consulAddress, int consulPort)
        {
            //获取所有注册的服务
            using (var consul = new Consul.ConsulClient(c =>
            {
                c.Address = new Uri($"http://{consulAddress}:{consulPort}");
            }))
            {
                //取在Consul注册的全部服务
                var services = consul.Agent.Services().Result.Response;
                foreach (var s in services.Values)
                {
                    var serviceNodes = services.Values.Where(c => c.Service.ToLower() == serviceName.ToLower()).ToList();
                    if (serviceNodes.Count() > 0)
                    {
                        var serviceNode = serviceNodes[DateTime.Now.Millisecond % serviceNodes.Count()];
                        return new ServiceConfig()
                        {
                            Name = serviceNode.Service,
                            Address = serviceNode.Address,
                            Port = serviceNode.Port,
                        };
                    }
                    Console.WriteLine($"ID={s.ID},Service={s.Service},Addr={s.Address},Port={s.Port}");
                }
            }
            return null;
        }
    }

    public interface IRPCClientProvider<T>
    {
        T GetClient(string consulAddress, int consulPort);
    }

}
