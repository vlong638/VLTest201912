using Consul;
using FrameworkTest.Common.ConfigSolution;
using FrameworkTest.Common.FileSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Autobots.ConsulSample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Consul注册
            ////获取服务器地址
            //var hostname= System.Net.Dns.GetHostName();
            //var address = System.Net.Dns.Resolve(hostname).AddressList;
            //考虑全部基于配置

            //简易配置方案
            var file = FileHelper.ReadAllText(FileHelper.GetDirectory("configs"), "config.xml");
            var config = ConfigHelper.GetVLConfig(file);
            var serviceName = config.GetKey("Consul_ServiceName");
            var serviceHost = config.GetKey("Consul_ServiceHost");
            var servicePort = config.GetKey("Consul_ServicePort");
        }

    }

    public class ServiceDisvoveryOptions
    {
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
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
