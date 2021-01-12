using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ResearchAPI.CORS;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Services;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ResearchAPI.Test.xUnit
{
    public class UnitTest1
    {
        public UnitTest1(ITestOutputHelper outputHelper)
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    //显示设置当前程序运行目录
                    builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                    //Json配置
                    builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);
                    //常量配置
                    var configs = builder.Build();
                    APIContraints.DBConfig = configs.GetSection("DB").Get<DBConfig>();
                    //静态常量
                    var dbConnectiongString = APIContraints.DBConfig.ConnectionStrings.FirstOrDefault(c => c.Key == APIContraints.ResearchDbContext).Value;
                    var dbContext = new DbContext(DBHelper.GetDbConnection(dbConnectiongString));
                    DomainConstraits.InitData(new ReportTaskService(dbContext));
                })
                .UseStartup<Startup>());
            Client = server.CreateClient();
            Output = outputHelper;

        }

        public HttpClient Client { get; }
        public ITestOutputHelper Output { get; }

        [Fact]
        public async void Login()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { UserName = "admin", Password = "123456" }), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await Client.PostAsync("/api/Home/Login", content);
            var responseData = response.Content.ReadAsStringAsync();
            var data = responseData.Result.FromJsonToAnonymousType(new { Data = "" }).Data;
            Assert.NotNull(data);
        }
    }
}
