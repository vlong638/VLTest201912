using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;
using VLTest2015.Attributes;
using VLTest2015.Authentication;
using VLTest2015.Common.Controllers;
using VLTest2015.Common.Models.RequestDTO;
using VLTest2015.Models;
using VLTest2015.Services;
using VLTest2015.Utils;
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
                PersonName = name,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = _PregnantService.GetPagedListOfPregnantInfo(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List.ToList() });
        }

        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案列表)]
        public JsonResult GetConfigurablePagedListOfPregnantInfo(int page, int rows, string name, string sort, string order)
        {
            var pars = new GetPagedListOfPregnantInfoRequest()
            {
                PersonName = name,
                PageIndex = page,
                PageSize = rows,
                Orders = sort == null ? new Dictionary<string, bool>() : (new Dictionary<string, bool>() { { sort, (order == "asc") } }),
            };
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListPages.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants("Table");
            var tableConfigs = tableElements.Select(c => new EntityAppConfigTable(c));
            var tableConfig = tableConfigs.FirstOrDefault(c => c.TableName == "O_PregnantInfo");
            var displayProperties = tableConfig.Properties.Where(c => c.IsNeedOnPage);
            pars.FieldNames = displayProperties.Select(c => c.ColumnName).ToList();

            var serviceResult = _PregnantService.GetConfigurablePagedListOfPregnantInfo(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            var result = Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.DataTable.ToList() });
            return result;
        }

        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案详情)]
        public JsonResult GetPregnantInfo(long pregnantInfoId)
        {
            if (pregnantInfoId == 0)
            {
                return Error(default(PregnantInfo), messages: "缺少有效的 pregnantInfoId");
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
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List });
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
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Json(new { total = serviceResult.Data.Count, rows = serviceResult.Data.List });
        }

        #region 统计方案测试

        [HttpGet]
        [VLAuthentication]
        public ActionResult Test()
        {
            return View();
        }

        [HttpGet]
        [VLAuthentication]
        public ActionResult AllStatistics()
        {
            return View();
        }

        /// <summary>
        /// 全统计汇总
        ///顺产人数
        ///剖宫产人数
        ///引产人数
        ///顺转剖人数
        ///侧切人数
        ///裂伤人数
        ///新生儿人数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllStatics()
        {
            var serviceResult = new ServiceResult<AllStatics>(new AllStatics()
            {
                EutociaCount = 100,
                CesareanCount = 22,
                OdinopoeiaCount = 3,
                EutociaChangeToCesarean = 1,
                CutCount = 44,
                BreakCount = 55,
                ChildCount = 111,
            });
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, "");
            return Success(serviceResult.Data);
        }

        public class AllStatics
        {
            /// <summary>
            /// 顺产人数
            /// </summary>
            public int EutociaCount { set; get; }
            /// <summary>
            /// 剖宫产人数
            /// </summary>
            public int CesareanCount { set; get; }
            /// <summary>
            /// 引产人数
            /// </summary>
            public int OdinopoeiaCount { set; get; }
            /// <summary>
            /// 顺转剖人数
            /// </summary>
            public int EutociaChangeToCesarean { set; get; }
            /// <summary>
            /// 侧切人数
            /// </summary>
            public int CutCount { set; get; }
            /// <summary>
            /// 裂伤人数
            /// </summary>
            public int BreakCount { set; get; }
            /// <summary>
            /// 新生儿人数
            /// </summary>
            public int ChildCount { set; get; }
        } 

        #endregion
    }
}


