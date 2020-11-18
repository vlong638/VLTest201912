using Autobots.Infrastracture.Common.ConsulSolution;
using Consul;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Autobots.B2Service
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
                Services = { B2ServiceDefinition.B2Service.BindService(new B2ServiceImpl()) },
                Ports = { new ServerPort(ServiceConfig.Service.Address, ServiceConfig.Service.Port, ServerCredentials.Insecure) }
            };
            _server.Start();

            Console.WriteLine($"Grpc ServerListening On Port {ServiceConfig.Service.Port}");
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
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //ע��Rpc����
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
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceConfig.Consul.DnsEndpoint.Address}:{serviceConfig.Consul.DnsEndpoint.Port}"));
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), //����������ú�ע��
                Interval = TimeSpan.FromSeconds(3), //�������ʱ���������߳�Ϊ�������
                HTTP = $"http://{serviceConfig.Service.Address}:{5001}/health", //��������ַ
                //Timeout = TimeSpan.FromSeconds(5) //��ʱʱ��
            };
            var serviceId = $"{serviceConfig.Service.Name}_{serviceConfig.Service.Address}:{serviceConfig.Service.Port}";
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = serviceId,
                Name = serviceConfig.Service.Name,
                Address = serviceConfig.Service.Address,
                Port = serviceConfig.Service.Port,
                Tags = new[] { $"urlprefix-/{serviceConfig.Service.Tag}" }//��� urlprefix-/servicename ��ʽ�� tag ��ǩ���Ա� Fabio ʶ��
            };
            consulClient.Agent.ServiceRegister(registration).Wait();//��������ʱע�ᣬ�ڲ�ʵ����ʵ����ʹ�� Consul API ����ע�ᣨHttpClient����
            return app;
        }
    }
}
