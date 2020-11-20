using Autobots.Infrastracture.LogCenter;
using Autobots.ServiceProtocols;
using Consul;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace Autobots.LogCenter
{
    public interface IRPCService
    {
        void Start();
    }

    public class RPCService : IRPCService
    {
        private static Server _server;
        static ServiceDisvoveryOptions ServiceConfig;

        public RPCService(IOptions<ServiceDisvoveryOptions> serviceConfig)
        {
            ServiceConfig = serviceConfig.Value;
        }

        public void Start()
        {
            _server = new Server
            {
                Services = { LogServiceDefinition.BindService(new LogServiceImpl()) },
                Ports = { new ServerPort(ServiceConfig.RPCService.Address, ServiceConfig.RPCService.Port, ServerCredentials.Insecure) }
            };
            _server.Start();

            Console.WriteLine($"Grpc ServerListening On Port {ServiceConfig.RPCService.Port}");
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ע��ȫ������
            services.AddOptions();

            //ע��RPC����
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //ע��RPC����
            services.AddSingleton<IRPCService, RPCService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ServiceDisvoveryOptions> serviceConfig, IRPCService service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // ��ӽ������·�ɵ�ַ
            app.Map("/health", (app) =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("OK");
                });
            });

            // ����ע��
            RegisterConsul(app, serviceConfig.Value);
            // ����Rpc����
            service.Start();
        }


        // ����ע��
        public static IApplicationBuilder RegisterConsul(IApplicationBuilder app, ServiceDisvoveryOptions serviceConfig)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceConfig.ConsulService.DnsEndpoint.Address}:{serviceConfig.ConsulService.DnsEndpoint.Port}"));
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), //����������ú�ע��
                Interval = TimeSpan.FromSeconds(3), //�������ʱ���������߳�Ϊ�������
                HTTP = $"http://{serviceConfig.HealthCheckService.Address}:{serviceConfig.HealthCheckService.Port}/health", //��������ַ
                //Timeout = TimeSpan.FromSeconds(5) //��ʱʱ��
            };
            var serviceId = $"{serviceConfig.RPCService.Name}_{serviceConfig.RPCService.Address}:{serviceConfig.RPCService.Port}";
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = serviceId,
                Name = serviceConfig.RPCService.Name,
                Address = serviceConfig.RPCService.Address,
                Port = serviceConfig.RPCService.Port,
                Tags = new[] { $"urlprefix-/{serviceConfig.RPCService.Tag}" }//��� urlprefix-/servicename ��ʽ�� tag ��ǩ���Ա� Fabio ʶ��
            };
            consulClient.Agent.ServiceRegister(registration).Wait();//��������ʱע�ᣬ�ڲ�ʵ����ʵ����ʹ�� Consul API ����ע�ᣨHttpClient����
            return app;
        }
    }
}
