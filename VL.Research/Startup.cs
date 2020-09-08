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
            //配置文件
            services.Configure<DBConfig>(Configuration.GetSection("DB"));
            services.Configure<AuthenticationOptions>(Configuration.GetSection("Authentication"));

            #region 接口及服务

            //认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, b =>
            {
                //登陆地址
                b.LoginPath = "/Home/Login";
                b.Cookie.Name = "My_SessionId";
                // b.Cookie.Domain = "shenniu.core.com";
                b.Cookie.Path = "/";
                b.Cookie.HttpOnly = true;
                b.Cookie.Expiration = new TimeSpan(0, 0, 30);
                b.ExpireTimeSpan = new TimeSpan(0, 0, 30);
            });

            //Controller
            services.AddControllersWithViews();

            //services.Configure<IdentityOptions>(options =>
            //{
            //    // Password settings.
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;

            //    // Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = false;
            //});

            //services.ConfigureApplicationCookie(options =>
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //    options.LoginPath = "/Identity/Account/Login";
            //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //    options.SlidingExpiration = true;
            //});

            ////Filters
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add<VLAuthorizeFilter>(); // 添加身份验证过滤器
            //    options.Filters.Add<VLActionFilterAttribute>(); // 添加身份验证过滤器 -- 菜单操作权限
            //});

            //基础设施
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<APIContext>();

            //服务设施
            services.AddScoped<PregnantService>();
            services.AddScoped<SharedService>();
            services.AddScoped<UserService>();

            //服务接口管理
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "VL.Research", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "VL.Research.xml");
                p.CustomSchemaIds(x => x.FullName);
            });

            #endregion

            #region 认证

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //})
            //.AddCookie(b =>
            //{
            //    //登陆地址
            //    b.LoginPath = "/Home/Login";
            //    b.Cookie.Name = "My_SessionId";
            //    // b.Cookie.Domain = "shenniu.core.com";
            //    b.Cookie.Path = "/";
            //    b.Cookie.HttpOnly = true;
            //    b.Cookie.Expiration = new TimeSpan(0, 0, 30);
            //    b.ExpireTimeSpan = new TimeSpan(0, 0, 30);
            //});

            //services.AddDataProtection();
            //services.AddWebEncoders();
            //services.AddScoped<IAuthenticationService, UserService>();//核心对象一 认证服务
            //services.AddSingleton<IClaimsTransformation, NoopClaimsTransformation>();
            //services.AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();//核心对象二 认证模式
            //services.AddSingleton<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();//核心对象三 认证代理

            #endregion
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

            // 访问受保护资源
            app.Map("/resource", builder => builder.Run(async (context) => await context.Response.WriteAsync("Hello, ASP.NET Core!")));

            #endregion
        }
    }
}
