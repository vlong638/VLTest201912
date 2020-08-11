using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            //�����ļ�
            services.Configure<DBConfig>(Configuration.GetSection("DB"));
            services.Configure<AuthenticationOptions>(Configuration.GetSection("Authentication"));

            #region �ӿڼ�����

            //Controller
            services.AddControllersWithViews();

            //������ʩ
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<APIContext>();

            //������ʩ
            services.AddScoped<PregnantService>();
            services.AddScoped<SharedService>();
            services.AddScoped<UserService>();

            //����ӿڹ���
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "VL.Research", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "VL.Research.xml");
                p.CustomSchemaIds(x => x.FullName);
            });

            #endregion

            #region ��֤

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(b =>
            {
                //��½��ַ
                b.LoginPath = "/Home/Login";
                b.Cookie.Name = "My_SessionId";
                // b.Cookie.Domain = "shenniu.core.com";
                b.Cookie.Path = "/";
                b.Cookie.HttpOnly = true;
                b.Cookie.Expiration = new TimeSpan(0, 0, 30);
                b.ExpireTimeSpan = new TimeSpan(0, 0, 30);
            });

            services.AddAuthenticationCore(options => options.AddScheme<VLAuthenticationHandler>(VLAuthenticationHandler.ShemeName, "VL Scheme"));
            services.AddDataProtection();
            services.AddWebEncoders();
            services.AddScoped<IAuthenticationService, UserService>();//���Ķ���һ ��֤����
            services.AddSingleton<IClaimsTransformation, NoopClaimsTransformation>();
            services.AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();//���Ķ���� ��֤ģʽ
            services.AddSingleton<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();//���Ķ����� ��֤����

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

            //ע���ض����м��
            app.UseHttpsRedirection();
            //ע�ᾲ̬��Դ
            app.UseStaticFiles();
            //ע��·���м��
            app.UseRouting();
            //ע����֤�м��
            app.UseAuthorization();
            //ע��·������
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


            #region ��֤����

            //// ��¼
            //app.Map("/Home/Login", builder => builder.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        var claimIdentity = new ClaimsIdentity();
            //        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, "jim"));
            //        await context.SignInAsync(VLAuthenticationHandler.ShemeName, new ClaimsPrincipal(claimIdentity));
            //    };
            //}));

            //// �˳�
            //app.Map("/logout", builder => builder.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        await context.SignOutAsync(VLAuthenticationHandler.ShemeName);
            //    };
            //}));

            // ��֤
            app.Use(next =>
            {
                return async (context) =>
                {
                    var result = await context.AuthenticateAsync(VLAuthenticationHandler.ShemeName);
                    if (result?.Principal != null) context.User = result.Principal;
                    await next(context);
                };
            });

            // ��Ȩ
            app.Use(async (context, next) =>
            {
                var user = context.User;
                if (user?.Identity?.IsAuthenticated ?? false)
                {
                    if (user.Identity.Name != "jim") await context.ForbidAsync(VLAuthenticationHandler.ShemeName);
                    else await next();
                }
                else
                {
                    await context.ChallengeAsync(VLAuthenticationHandler.ShemeName);
                }
            });

            // �����ܱ�����Դ
            app.Map("/resource", builder => builder.Run(async (context) => await context.Response.WriteAsync("Hello, ASP.NET Core!")));

            #endregion
        }
    }
}
