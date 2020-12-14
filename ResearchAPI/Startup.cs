using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResearchAPI.Services;
using System;

namespace ResearchAPI
{
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
            //接口注册
            //services.AddControllers();
            services.AddMvc(option =>
            {
                //option.Filters.Add<CustomerExceptionFilter>();
            });

            //服务注册
            services.AddScoped<ReportTaskService, ReportTaskService>();

            //服务接口管理
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ResearchAPI", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "ResearchAPI.xml");
                p.CustomSchemaIds(x => x.FullName);
            });

            //允许跨域
            services.AddCors(option => option.AddPolicy("AllCors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));//.AllowCredentials()
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

            //允许跨域
            app.UseCors("AllCors");

            //注册鉴权中间件
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
