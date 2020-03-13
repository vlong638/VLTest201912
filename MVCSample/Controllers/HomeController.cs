using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSample.Controllers
{
    /// <summary>
    /// 胎心监护
    /// </summary>
    public class DrawTXJHModel
    {
        public int[] data1 { set; get; }
        public int[] data2 { set; get; }
        public int[] data3 { set; get; }
        public int[] data4 { set; get; }
        public int[] data5 { set; get; }
    }

    public class HomeController : Controller
    {
        /// <summary>
        /// 胎心监护样例数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDataForTXJH()
        {
            var filePath = @"D:\Project\VL.Tests\MVCSample\Data\7_20191030103133.bin";
            var source = System.IO.File.ReadAllBytes(filePath);

            var model = new DrawTXJHModel();
            var dataCount = source.Length / 17;
            model.data1 = new int[dataCount];
            model.data2 = new int[dataCount];
            model.data3 = new int[dataCount];
            model.data4 = new int[dataCount];
            model.data5 = new int[dataCount];
            for (var i = 0; i < source.Length; ++i)
            {
                var dataIndex = i / 17;
                switch (i % 17)
                {
                    case 3: model.data1[dataIndex] = source[i]; break;
                    case 7: model.data2[dataIndex] = source[i]; break;
                    case 11: model.data3[dataIndex] = source[i]; break;
                    case 15: model.data4[dataIndex] = source[i]; break;
                    case 16: model.data5[dataIndex] = source[i]; break;
                }
            }
            return View(model);
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
    }
}