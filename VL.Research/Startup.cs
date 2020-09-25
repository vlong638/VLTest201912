using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using VL.Consolo_Core.AuthenticationSolution;
using VL.Consolo_Core.Common.LogSolution;
using VL.Consolo_Core.Common.RedisSolution;
using VL.Research.Common;
using VL.Research.Common.Configuration;
using VL.Research.Services;

namespace VL.Research
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //注册业务层配置
            services.AddOptions();
            //配置文件
            services.Configure<DBConfig>(Configuration.GetSection("DB"));
            services.Configure<AuthenticationOptions>(Configuration.GetSection("Authentication"));

            #region 接口及服务

            //Controller
            services.AddMvc(option =>
            {
                option.Filters.Add<CustomerExceptionFilter>();
            });

            //基础设施
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<APIContext>();
            services.AddSingleton<ILoggerHelper, ExceptionlessLogger>();

            
            //服务设施
            services.AddScoped<PregnantService>();
            services.AddScoped<SharedService>();
            services.AddScoped<UserService>();
            services.AddSingleton(p => new RedisCache(Configuration["Redis:ConnectionString"], Configuration["Redis:Prefix"]));

            //服务接口管理
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "VL.Research", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "VL.Research.xml");
                p.CustomSchemaIds(x => x.FullName);
            });

            #endregion

            services.AddAuthenticationCore(options => options.AddScheme<VLAuthenticationHandler>(VLAuthenticationHandler.ShemeName, "demo scheme"));

            //允许跨域
            services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin()));

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            //注册重定向中间件
            app.UseHttpsRedirection();
            //注册静态资源
            app.UseStaticFiles();
            //注册路由中间件
            app.UseRouting();
            //注册认证中间件
            app.UseAuthentication();
            //注册鉴权中间件
            app.UseAuthorization();
            //注册路由配置
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "VL API");
            });

            #region 认证代理

            //// 登录
            //app.Map("/Home/Login", builder => builder.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        var claimIdentity = new ClaimsIdentity();
            //        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, "jim"));
            //        await context.SignInAsync(VLAuthenticationHandler.ShemeName, new ClaimsPrincipal(claimIdentity));
            //    };
            //}));

            //// 退出
            //app.Map("/logout", builder => builder.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        await context.SignOutAsync(VLAuthenticationHandler.ShemeName);
            //    };
            //}));

            //// 认证
            //app.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        var result = await context.AuthenticateAsync(VLAuthenticationHandler.ShemeName);
            //        if (result?.Principal != null) context.User = result.Principal;
            //        await next(context);
            //    };
            //});

            //// 授权
            //app.Use(async (context, next) =>
            //{
            //    var user = context.User;
            //    if (user?.Identity?.IsAuthenticated ?? false)
            //    {
            //        if (user.Identity.Name == "") await context.ForbidAsync(VLAuthenticationHandler.ShemeName);
            //        else await next();
            //    }
            //    else
            //    {
            //        await context.ChallengeAsync(VLAuthenticationHandler.ShemeName);
            //    }
            //});

            //// 访问受保护资源
            //app.Map("/resource", builder => builder.Run(async (context) => await context.Response.WriteAsync("Hello, ASP.NET Core!")));

            #endregion
        }
    }
}
