using Autobots.Infrastracture.Common.RedisSolution;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Services;
using System;
using System.IO;

namespace VL.CORS
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
            //服务注册
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<APIContext>();
            //services.AddSingleton<DomainConstraits, DomainConstraits>();
            services.AddScoped<ReportTaskService, ReportTaskService>();
            services.AddScoped<AccountService, AccountService>();
            services.AddSingleton(p => new RedisCache(Configuration["Redis:ConnectionString"], Configuration["Redis:Prefix"]));

            //鉴权
            services.AddControllers(option =>
            {
                //option.Filters.Add(typeof(VLActionFilterAttribute));
            });

            //允许跨域
            services.AddCors(option => option.AddPolicy("AllCors", policy => policy
            //.AllowAnyOrigin()
            .WithOrigins(
                "http://localhost:63342" //文欣
                , "http://localhost:8848" //一帆
                , "http://192.168.50.172:8848" //一帆
                , "http://localhost:14314" //vl
                , "http://localhost:8065" //vl
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            ));

            //服务接口管理
            services.AddSwaggerGen(p =>
            {
                p.OperationFilter<AuthHeaderFilter>();
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ResearchAPI", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "ResearchAPI.CORS.xml");
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

            //允许跨域
            app.UseCors("AllCors");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //启用静态文件
            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export")),
                RequestPath = "/Export"
            });;

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "VL API");
            });
        }
    }
}
