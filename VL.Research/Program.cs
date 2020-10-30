using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using BBee.Common.Configuration;

namespace BBee
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Host��������
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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

                ////XML����
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
    /// ��̬��,����
    /// </summary>
    public class APIContraints
    {
        /// <summary>
        /// ��Ʒ��������
        /// </summary>
        public static DHFConfig DHFConfig { set; get; }
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public static DBConfig DBConfig { set; get; }
    }
}
