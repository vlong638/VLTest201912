using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VL.MyAuth.Areas.Identity.Data;
using VL.MyAuth.Data;

[assembly: HostingStartup(typeof(VL.MyAuth.Areas.Identity.IdentityHostingStartup))]
namespace VL.MyAuth.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<VLMyAuthContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("VLMyAuthContextConnection")));

                services.AddDefaultIdentity<VLMyAuthUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<VLMyAuthContext>();
            });
        }
    }
}