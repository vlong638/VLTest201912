using System.Net;

namespace Autobots.Infrastracture.Common.ConsulSolution
{
    public class ServiceDisvoveryOptions
    {
        public ServiceConfig RPCService { get; set; }

        public ServiceConfig HealthCheckService { get; set; }

        public ConsulOptions ConsulService { get; set; }
    }

    public class ServiceConfig
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string Tag { get; set; }
    }

    public class ConsulOptions
    {
        public string HttpEndpoint { get; set; }

        public DnsEndpoint DnsEndpoint { get; set; }
    }

    public class DnsEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}
