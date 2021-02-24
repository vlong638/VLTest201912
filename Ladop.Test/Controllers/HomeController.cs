using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ladop.Test.Models;
using System.IO;
using System.Data;
using System.Text;

namespace Ladop.Test.Controllers
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
            return View();
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

        public JsonResult PrintFile(string type)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Docs\\打印代码生成.txt");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var fileContent = Encoding.GetEncoding("GB2312").GetString(System.IO.File.ReadAllBytes(filePath)) ;
            var data = GetData();
            var line = data.Rows[0];
            fileContent = fileContent.Replace("#姓名", data.Rows[0]["姓名"].ToString());
            return Json(fileContent);
        }

        private DataTable GetData()
        {
            var dt = new DataTable();
            var name = "姓名";
            dt.Columns.Add(name);
            dt.Rows.Add(dt.NewRow());
            dt.Rows[0][name] = "张三";
            dt.Rows.Add(dt.NewRow());
            dt.Rows[1][name] = "李四";
            dt.Rows.Add(dt.NewRow());
            dt.Rows[2][name] = "王五";
            return dt;
        }
    }
}
