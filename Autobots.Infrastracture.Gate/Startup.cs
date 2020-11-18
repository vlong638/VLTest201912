using Autobots.B2ServiceDefinition;
using Autobots.Infrastracture.Common.ConsulSolution;
using Consul;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Autobots.Infrastracture.Gate
{
    public interface IRPCServiceConfigProvider
    {
        ServiceConfig GetService(string serviceName);
    }

    public class RPCServiceConfigProvider : IRPCServiceConfigProvider
    {
        ServiceDisvoveryOptions ServiceConfig;

        public RPCServiceConfigProvider(IOptions<ServiceDisvoveryOptions> serviceConfig)
        {
            ServiceConfig = serviceConfig.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public ServiceConfig GetService(string serviceName)
        {
            //获取所有注册的服务
            using (var consul = new Consul.ConsulClient(c =>
            {
                c.Address = new Uri($"http://{ServiceConfig.ConsulService.DnsEndpoint.Address}:{ServiceConfig.ConsulService.DnsEndpoint.Port}");
            }))
            {
                //取在Consul注册的全部服务
                var services = consul.Agent.Services().Result.Response;
                foreach (var s in services.Values)
                {
                    var serviceNodes = services.Values.Where(c => c.Service.ToLower() == serviceName.ToLower()).ToList();
                    if (serviceNodes.Count()>0)
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
        T GetClient();
    }

    public class B2ServiceRPCClientProvider : IRPCClientProvider<B2Service.B2ServiceClient>
    {
        IRPCServiceConfigProvider ConfigProvider;

        public B2ServiceRPCClientProvider(IRPCServiceConfigProvider configProvider)
        {
            ConfigProvider = configProvider;
        }

        public B2Service.B2ServiceClient GetClient()
        {
            string serviceName = "B2Service";
            var serviceConfig = ConfigProvider.GetService(serviceName);
            if (serviceConfig==null)
            {
                throw new NotImplementedException("依赖的服务无效");
            }
            Channel channel = new Channel($"{serviceConfig.Address}:{serviceConfig.Port}", ChannelCredentials.Insecure);
            return new B2ServiceDefinition.B2Service.B2ServiceClient(channel);
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
            services.AddControllers();

            //注册RPC配置
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IRPCServiceConfigProvider, RPCServiceConfigProvider>();
            services.AddSingleton<B2ServiceRPCClientProvider, B2ServiceRPCClientProvider>();

            //服务接口管理
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Autobots.Infrastracture.Gateway", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "Autobots.Infrastracture.Gateway.xml");
                p.CustomSchemaIds(x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "VL API");
            });
        }
    }
}
