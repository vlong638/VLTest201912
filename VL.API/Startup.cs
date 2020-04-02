using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VL.API.PT.Services;
using static VL.API.Controllers.PTController;

namespace VL.API
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
            services.AddControllers();

            ////Ìí¼ÓCache
            //services.AddSingleton<ICache>(p => new RedisCache(Configuration["Cache:Redis"], Configuration["Cache:Prefix"]));

            //Services
            services.AddScoped<SampleService>();

            //Configs
            services.Configure<LoggingConfig>(Configuration.GetSection("Logging"));

            //swagger
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "VL.API", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "VL.API.xml");
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
            //·ÃÎÊÂ·¾¶ https://localhost:44347/swagger
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "VL API");
            });
        }
    }
}
