using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace VL.Research
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序启动入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Host启动配置
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) 
            => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                //显示设置当前程序运行目录
                builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());

                ////内存配置
                //var arrayDict = new Dictionary<string, string>()
                //{
                //    { "ConfigSample01","张三"},
                //    { "ConfigSample02","李四"},
                //    { "ConfigSample03","王武"},
                //};
                //builder.AddInMemoryCollection(arrayDict);

                //Json配置
                builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);

                //XML配置
                //builder.AddXmlFile("configs/config.xml", optional: false, reloadOnChange: false);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
