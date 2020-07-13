using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FS.SyncManager.Controllers
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

        /// <summary>
        /// 获取同步记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPageListOfSyncOrder(GetPageListOfSyncOrderRequest request)
        {
            //var syncOrders =new List<> 

            //return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List.ToList() });

            return new JsonResult();
        }

        public class GetPageListOfSyncOrderRequest
        {
            public int page { set; get; }
            public int rows { set; get; }
            public string Name { set; get; }
            public string StartTime { set; get; }
            public string EndTime { set; get; }
            public string SyncType { set; get; }
        }
    }
}