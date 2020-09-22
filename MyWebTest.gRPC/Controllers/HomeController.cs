using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.TimeSpanSolution;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VLService;

namespace MyWebTest.gRPC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        const string Server = "localhost";
        const int Port = 30051;
        public ActionResult Compare01()
        {
            //local
            var localService = new SampleService();
            var tLocal = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                   var result = localService.SayHello(new HelloRequest() { Name = i.ToString() });
                }
            });
            //webAPI
            var container = new System.Net.CookieContainer();
            var tWebAPI = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    var result = HttpHelper.Get("https://localhost:44327/api/values", "", ref container);
                }
            });
            //gRPC
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            var client = new VLService01.VLService01Client(channel);
            var tRPC = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    var result = client.SayHello(new HelloRequest { Name = i.ToString() });
                }
            });

            var summary = $@"local:{tLocal.TotalMilliseconds}
webAPI:{tWebAPI.TotalMilliseconds}
tRPC:{tRPC.TotalMilliseconds}
";
            //万次 毫秒数 数据量极小值
            //local: 0
            //webAPI: 17123.4465
            //tRPC: 8004.4685

            //万次 毫秒数 数据量10kb

            //万次 毫秒数 数据量100kb


            return View();
        }

    }
}