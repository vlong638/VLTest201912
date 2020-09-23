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
                for (int i = 0; i < 1000; i++)
                {
                   var result = localService.SayHello(new HelloRequest() { Name = i.ToString() });
                }
            });
            //WebService
            ServiceReference2.SampleWebServiceSoapClient webService = new ServiceReference2.SampleWebServiceSoapClient();
            var tWebService = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = webService.HelloWorld(new ServiceReference2.HelloRequest() { Name = i.ToString() });
                }
            });
            //WebAPI
            var container = new System.Net.CookieContainer();
            var tWebAPI = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = HttpHelper.Get("http://localhost:8017/api/values/" + i, "", ref container);
                }
            });
            //WebAPI
            var tWebAPIList = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = HttpHelper.Get("http://localhost:8017/api/values" , "", ref container);
                }
            });
            //gRPC
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            var client = new VLService01.VLService01Client(channel);
            var tRPC = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = client.SayHello(new HelloRequest { Name = i.ToString() });
                }
            });

            var summary = $@"local:{tLocal.TotalMilliseconds}
WebService:{tWebService.TotalMilliseconds}
WebAPI:{tWebAPI.TotalMilliseconds}
WebAPIList:{tWebAPIList.TotalMilliseconds}
tRPC:{tRPC.TotalMilliseconds}
";
        //千次 毫秒数 数据量极小值
        //local: 7.9977
        //WebService: 948.5253
        //WebAPI: 26639.0061
        //tRPC: 767.9921

        //千次 毫秒数 数据量极小值
        //local: 0.9968
        //WebService: 873
        //WebAPI: 1461.112
        //WebAPIList: 633.9954
        //tRPC: 1004.9978


            return View();
        }
        public ActionResult Compare10kb()
        {
            //local
            var localService = new SampleService();
            var tLocal = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = localService.TransTest10kb(new HelloRequest() { Name = i.ToString() });
                }
            });
            //WebService
            ServiceReference2.SampleWebServiceSoapClient webService = new ServiceReference2.SampleWebServiceSoapClient();
            var tWebService = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = webService.TransTest10kb(new ServiceReference2.HelloRequest() { Name = i.ToString() });
                }
            });
            //WebAPI
            var container = new System.Net.CookieContainer();
            var tWebAPI = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = HttpHelper.Get("http://localhost:8017/api/hello/" + i, "", ref container);
                }
            });
            //gRPC
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            var client = new VLService01.VLService01Client(channel);
            var tRPC = TimeSpanHelper.GetTimeSpan(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var result = client.TransTest10kb(new HelloRequest { Name = i.ToString() });
                }
            });

            var summary = $@"local:{tLocal.TotalMilliseconds}
WebService:{tWebService.TotalMilliseconds}
WebAPI:{tWebAPI.TotalMilliseconds}
tRPC: { tRPC.TotalMilliseconds}
";
        //千次 毫秒数 数据量10kb
        //local: 2.0007
        //WebService: 1218.0075
        //WebAPI: 1605.9833
        //tRPC: 1168.0022



            return View();
        }

    }
}