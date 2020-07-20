using FrameworkTest.Common.ControllerSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.PagerSolution;
using FS.SyncManager.Models;
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
        #region PregnantInfo

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PregnantInfoList()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var viewConfig = HomeController.LoadDefaultConfig("Sync_PregnantInfo");
            request.UpdateFieldNames(viewConfig);

            var serviceResult = new ServiceContext().SyncService.GetPagedListOfPregnantInfo(request);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            viewConfig.UpdateValues(serviceResult.Data.List);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List.ToList() });
        }

        #endregion

        #region VisitRecord

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VisitRecordList()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPagedListOfVisitRecord(GetPagedListOfVisitRecordRequest request)
        {
            var viewConfig = HomeController.LoadDefaultConfig("Sync_VisitRecord");
            request.UpdateFieldNames(viewConfig);

            var serviceResult = new ServiceContext().SyncService.GetPagedListOfVisitRecord(request);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            viewConfig.UpdateValues(serviceResult.Data.List);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List.ToList() });
        }

        #endregion

        #region SyncOrder

        /// <summary>
        /// 删除同步记录
        /// </summary>
        [HttpPost]
        public JsonResult DeleteSyncOrder(long syncOrderId)
        {
            if (syncOrderId<=0)
                return Error(false, "无效的Id");

            var serviceResult = new ServiceContext().SyncService.DeleteSyncOrderById(syncOrderId);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Success(serviceResult.Data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SyncOrderList()
        {
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
        public JsonResult GetPageListOfSyncOrder(GetPagedListOfSyncOrderRequest request)
        {
            var viewConfig = HomeController.LoadDefaultConfig("Sync_SyncOrder");
            request.UpdateFieldNames(viewConfig);

            var serviceResult = new ServiceContext().SyncService.GetPagedListOfSyncOrder(request);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            viewConfig.UpdateValues(serviceResult.Data.List);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List.ToList() });
        }

        #endregion
    }
}


