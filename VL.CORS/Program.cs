using Autobots.Infrastracture.Common.DBSolution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Services;
using System.Linq;

namespace ResearchAPI.CORS
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
                //��ʾ���õ�ǰ��������Ŀ¼
                builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                //Json����
                builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);
                //��������
                var configs = builder.Build();
                APIContraints.DBConfig = configs.GetSection("DB").Get<DBConfig>();
                APIContraints.EasyResearchConfig = configs.GetSection("EasyResearch").Get<EasyResearchConfig>();
                //��̬����
                var dbConnectiongString = APIContraints.DBConfig.ConnectionStrings.FirstOrDefault(c => c.Key == APIContraints.ResearchDbContext).Value;
                var dbContext = new DbContext(DBHelper.GetDbConnection(dbConnectiongString));
                DomainConstraits.InitData(new ReportTaskService(dbContext));
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
