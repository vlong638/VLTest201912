using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ResearchAPI.Services;

namespace VL.CORS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                //显示设置当前程序运行目录
                builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());

                //Json配置
                builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);

                //常量配置
                var configs = builder.Build();
                APIContraints.DBConfig = configs.GetSection("DB").Get<DBConfig>();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
