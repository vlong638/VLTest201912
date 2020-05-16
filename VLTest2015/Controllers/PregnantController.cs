using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using VLTest2015.Attributes;
using VLTest2015.Authentication;
using VLTest2015.Common.Controllers;
using VLTest2015.Common.Models.RequestDTO;
using VLTest2015.Models;
using VLTest2015.Services;
using VLVLTest2015.Common.Pager;

namespace VLTest2015.Controllers
{
    public class PregnantController : BaseController
    {
        private PregnantService _PregnantService;

        public PregnantController()
        {
            this._PregnantService = new PregnantService();
        }

        [HttpGet]
        [VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult PregnantInfoList()
        {
            return View();
        }

        [HttpGet]
        [VLAuthentication(Authority.查看孕妇档案详情)]
        public ActionResult PregnantInfo(long pregnantInfoId)
        {
            return View();
        }

        [HttpGet]
        [VLAuthentication(Authority.查看检查列表)]
        public ActionResult LabOrderList(long pregnantInfoId)
        {
            return View();
        }

        [HttpGet]
        [VLAuthentication(Authority.查看产检列表)]
        public ActionResult VisitRecordList(long pregnantInfoId)
        {
            return View();
        }

        [HttpGet]
        [VLAuthentication(Authority.查看检查详情)]
        public ActionResult LabOrderDetail(long pregnantInfoId)
        {
            return View();
        }

        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案列表)]
        public JsonResult GetPagedListOfPregnantInfo(int page, int rows, string name)
        {
            var pars = new GetPagedListOfPregnantInfoRequest()
            {
                Name = name,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = _PregnantService.GetPagedListOfPregnantInfo(pars);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案详情)]
        public JsonResult GetPregnantInfo(long pregnantInfoId)
        {
            if (pregnantInfoId == 0)
            {
                return Error(default(T_PregnantInfo), messages: "缺少有效的 pregnantInfoId");
            }
            var serviceResult = _PregnantService.GetPregnantInfoByPregnantInfoId(pregnantInfoId);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Success(serviceResult.Data);
        }

        [HttpPost]
        [VLAuthentication(Authority.查看产检列表)]
        public JsonResult GetPagedListOfVisitRecord(int page, int rows, long pregnantInfoId)
        {
            var pars = new GetPagedListOfVisitRecordRequest()
            {
                PregnantInfoId = pregnantInfoId,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = _PregnantService.GetPagedListOfVisitRecord(pars);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [VLAuthentication(Authority.查看检查列表)]
        public JsonResult GetPagedListOfLabOrder(int page, int rows, long pregnantInfoId)
        {
            var pars = new GetPagedListOfLabOrderRequest()
            {
                PregnantInfoId = pregnantInfoId,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = _PregnantService.GetPagedListOfLabOrder(pars);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List }, JsonRequestBehavior.AllowGet);
        }
    }
}


