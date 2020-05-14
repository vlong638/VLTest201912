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
        [VLAuthentication(Authority.查看孕妇档案)]
        public ActionResult PregnantInfoList()
        {
            return View();
        }

        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案)]
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

        //[HttpPost]
        //[VLAuthentication]
        //public APIResult<VLPageResult<PagedListOfPregnantInfoModel>> GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        //{
        //    var serviceResult = _PregnantService.GetPagedListOfPregnantInfo(request);
        //    if (!serviceResult.IsSuccess)
        //        return Error(serviceResult.Data, serviceResult.Messages);
        //    return Success(serviceResult.Data);
        //}
    }
}


