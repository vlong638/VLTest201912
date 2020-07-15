using FrameworkTest.Common.ControllerSolution;
using FrameworkTest.Common.DBSolution;
using FS.SyncManager.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FS.SyncManager.Controllers
{
    public class SyncController : BaseController
    {
        [HttpGet]
        public ActionResult PregnantInfoList()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var serviceResult = new ServiceContext().SyncService.GetPagedListOfPregnantInfo(request);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List.ToList() });
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