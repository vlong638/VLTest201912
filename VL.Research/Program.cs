using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VL.Research
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
                //��ʾ���õ�ǰ��������Ŀ¼
                builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());

                ////�ڴ�����
                //var arrayDict = new Dictionary<string, string>()
                //{
                //    { "ConfigSample01","����"},
                //    { "ConfigSample02","����"},
                //    { "ConfigSample03","����"},
                //};
                //builder.AddInMemoryCollection(arrayDict);

                //Json����
                builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);

                //XML����
                //builder.AddXmlFile("configs/config.xml", optional: false, reloadOnChange: false);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
