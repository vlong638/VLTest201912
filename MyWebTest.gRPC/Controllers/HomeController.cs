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
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            var client = new VLService01.VLService01Client(channel);
            var t1 = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    client.SayHello(new HelloRequest { Name = i.ToString() });
                }
            });
            var localService = new SampleService();
            var t2 = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                }
            });
            //万次
            // grpc  {00:00:07.8723112}
            // local {00:00:00.0010067}
            return View();
        }

    }
}