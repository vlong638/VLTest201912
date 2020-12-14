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
            //�ӿ�ע��
            //services.AddControllers();
            services.AddMvc(option =>
            {
                //option.Filters.Add<CustomerExceptionFilter>();
            });

            //����ע��
            services.AddScoped<ReportTaskService, ReportTaskService>();

            //����ӿڹ���
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ResearchAPI", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "ResearchAPI.xml");
                p.CustomSchemaIds(x => x.FullName);
            });

            //�������
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

            //�������
            app.UseCors("AllCors");

            //ע���Ȩ�м��
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
