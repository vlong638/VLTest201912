using Autobots.Infrastracture.Common.ConfigSolution;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VLAutobots.Infrastracture.Common.FileSolution;

namespace Autobots.Infrastracture.Gate
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            GetWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder GetWebHostBuilder(string[] args)
        {
            var currentDirectory = FileHelper.GetDirectory("configs");
            var configText = FileHelper.ReadAllText(currentDirectory, "config.xml");
            var config = ConfigHelper.GetVLConfig(configText);

            var builder = WebHost.CreateDefaultBuilder(args)
            .UseUrls($"http://{config.GetKey("APIService", "Address")}:{config.GetKey("APIService", "Port")}")
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);
            })
            .UseStartup<Startup>();
            return builder;
        }

        //public static IHostBuilder GetHostBuilder(string[] args)
        //{
        //    var currentDirectory = FileHelper.GetDirectory("configs");
        //    var configText = FileHelper.ReadAllText(currentDirectory, "config.xml");
        //    var config = ConfigHelper.GetVLConfig(configText);

        //    var builder = Host.CreateDefaultBuilder(args)
        //    .UseUrls($"http://{config.GetKey("APIService", "Address")}:{config.GetKey("HealthCheckService", "Port")}")
        //    .ConfigureAppConfiguration((context, builder) =>
        //    {
        //        builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
        //        builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);
        //    })
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder.UseStartup<Startup>();
        //    });
        //    return builder;
        //}
    }
}
