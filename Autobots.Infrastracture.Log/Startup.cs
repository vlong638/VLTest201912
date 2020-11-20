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
            //注册全局配置
            services.AddOptions();

            //注册RPC配置
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //注册RPC服务
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

            // 添加健康检查路由地址
            app.Map("/health", (app) =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("OK");
                });
            });

            // 服务注册
            RegisterConsul(app, serviceConfig.Value);
            // 启动Rpc服务
            service.Start();
        }


        // 服务注册
        public static IApplicationBuilder RegisterConsul(IApplicationBuilder app, ServiceDisvoveryOptions serviceConfig)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceConfig.ConsulService.DnsEndpoint.Address}:{serviceConfig.ConsulService.DnsEndpoint.Port}"));
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), //服务启动多久后注册
                Interval = TimeSpan.FromSeconds(3), //健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{serviceConfig.HealthCheckService.Address}:{serviceConfig.HealthCheckService.Port}/health", //健康检查地址
                //Timeout = TimeSpan.FromSeconds(5) //超时时间
            };
            var serviceId = $"{serviceConfig.RPCService.Name}_{serviceConfig.RPCService.Address}:{serviceConfig.RPCService.Port}";
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = serviceId,
                Name = serviceConfig.RPCService.Name,
                Address = serviceConfig.RPCService.Address,
                Port = serviceConfig.RPCService.Port,
                Tags = new[] { $"urlprefix-/{serviceConfig.RPCService.Tag}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };
            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            return app;
        }
    }
}
