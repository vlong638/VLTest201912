using BBee.Common.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BBee
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

                ////XML配置
                //builder.AddXmlFile("configs/log4net.config", optional: false, reloadOnChange: false);

                var configs = builder.Build();
                APIContraints.DHFConfig = configs.GetSection("DHF").Get<DHFConfig>();
                APIContraints.DBConfig = configs.GetSection("DB").Get<DBConfig>();
            })
            .ConfigureWebHostDefaults(webBuilder =>           {
                webBuilder.UseStartup<Startup>();
            });
    }

    /// <summary>
    /// 静态量,常量
    /// </summary>
    public class APIContraints
    {
        /// <summary>
        /// 大黄蜂相关配置
        /// </summary>
        public static DHFConfig DHFConfig { set; get; }
        /// <summary>
        /// 数据库配置
        /// </summary>
        public static DBConfig DBConfig { set; get; }
    }
}
