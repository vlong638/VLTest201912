using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.MyAuth.Models;
using VLService;

namespace VL.MyAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            SimpleRPCTest(channel);

            return View();
        }

        const string Server = "localhost";
        const int Port = 30051;
        void SimpleRPCTest(Channel channel)
        {
            var client = new VLService.VLService01.VLService01Client(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("FromServer, " + reply.ToJson());

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
