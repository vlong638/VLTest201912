using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VL.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                //显示设置当前程序运行目录
                builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                //设置内存中的 .NET 对象
                var arrayDict = new Dictionary<string, string>()
                {
                    { "ConfigSample01","张三"},
                    { "ConfigSample02","李四"},
                    { "ConfigSample03","王武"},
                };
                builder.AddInMemoryCollection(arrayDict);
                //设置文件, optional选择项为false时 必需存在该文件
                builder.AddJsonFile("config.json", optional: false, reloadOnChange: false);
                builder.AddXmlFile("config.xml", optional: false, reloadOnChange: false);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
