using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Autobots.B2Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://localhost:5001")
            .ConfigureAppConfiguration((context, builder) =>
            {
                //显示设置当前程序运行目录
                builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                //Json配置
                builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);
            })
            .UseStartup<Startup>()
            .Build();
    }
}
