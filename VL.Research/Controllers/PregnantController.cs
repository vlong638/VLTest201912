using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ControllerSolution;
using VL.Consolo_Core.Common.PagerSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Research.Common;
using VL.Research.Models;
using VL.Research.Services;

namespace VL.Research.Controllers
{
    /// <summary>
    /// 控制器 孕妇信息
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PregnantController : APIBaseController
    {
        /// <summary>
        /// 控制器 孕妇信息
        /// </summary>
        public PregnantController()
        {
        }

        /// <summary>
        /// 获取 孕妇档案列表
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="page">页码</param>
        /// <param name="rows">每页行数</param>
        /// <param name="name">参数(姓名)</param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案列表)]
        public APIResult<VLPagerResult<PagedListOfPregnantInfoModel>> GetPagedListOfPregnantInfo([FromServices] PregnantService pregnantService, int page, int rows, string name)
        {
            var pars = new GetPagedListOfPregnantInfoRequest()
            {
                PersonName = name,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = pregnantService.GetPagedListOfPregnantInfo(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.PagedData, serviceResult.Messages);
            return Success(serviceResult.PagedData);
        }

        /// <summary>
        /// 获取 自定义配置的孕妇档案列表
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页行数</param>
        /// <param name="personname">参数(姓名)</param>
        /// <param name="sort">参数(排序项)</param>
        /// <param name="order">参数(排序顺序:asc|desc)</param>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public PagedAPIResult<List<Dictionary<string, object>>> GetConfigurablePagedListOfPregnantInfo([FromServices] PregnantService pregnantService, int page, int limit, string sort, string order, string personname)
        {
            var pars = new GetPagedListOfPregnantInfoRequest()
            {
                PageIndex = page,
                PageSize = limit,
                PersonName = personname,
                Orders = sort == null ? new Dictionary<string, bool>() : (new Dictionary<string, bool>() { { sort, (order == "asc") } }),
            };
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListPages.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(ViewConfig.NodeElementName);
            var tableConfigs = tableElements.Select(c => new ViewConfig(c));
            var tableConfig = tableConfigs.FirstOrDefault(c => c.ViewName == "PregnantInfo");
            var displayProperties = tableConfig.Properties.Where(c => c.IsNeedOnPage);
            pars.FieldNames = displayProperties.Select(c => c.ColumnName).ToList();

            var serviceResult = pregnantService.GetConfigurablePagedListOfPregnantInfo(pars);
            if (!serviceResult.IsSuccess)
                return new PagedAPIResult<List<Dictionary<string, object>>>()
                {
                    code = 200,
                    msg = "",
                    Count = 0,
                    SourceData = null,
                };
            return new PagedAPIResult<List<Dictionary<string, object>>>()
            {
                code = 200,
                msg = "",
                Count = serviceResult.PagedData.Count,
                SourceData = serviceResult.PagedData.SourceData,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class PagedAPIResult<T> : APIResult
        {
            /// <summary>
            /// 列表总数
            /// </summary>
            public int Count { set; get; }
            /// <summary>
            /// 列表数据
            /// </summary>
            public T SourceData { set; get; }
        }

        /// <summary>
        /// 获取 孕妇档案详情
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="pregnantInfoId">孕妇档案Id</param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案详情)]
        public APIResult<PregnantInfo> GetPregnantInfo([FromServices] PregnantService pregnantService, long pregnantInfoId)
        {
            if (pregnantInfoId == 0)
            {
                return Error(default(PregnantInfo), "缺少有效的 pregnantInfoId");
            }
            var serviceResult = pregnantService.GetPregnantInfoByPregnantInfoId(pregnantInfoId);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.PagedData, serviceResult.Messages);
            return Success(serviceResult.PagedData);
        }

        /// <summary>
        /// 获取 产检列表
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="page">页码</param>
        /// <param name="rows">每页行数</param>
        /// <param name="pregnantInfoId">参数(孕妇档案Id)</param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看产检列表)]
        public APIResult<VLPagerResult<PagedListOfVisitRecordModel>> GetPagedListOfVisitRecord([FromServices] PregnantService pregnantService, int page, int rows, long pregnantInfoId)
        {
            var pars = new GetPagedListOfVisitRecordRequest()
            {
                PregnantInfoId = pregnantInfoId,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = pregnantService.GetPagedListOfVisitRecord(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.PagedData, serviceResult.Message);
            return Success(serviceResult.PagedData);
        }

        /// <summary>
        /// 获取 检查列表
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="page">页码</param>
        /// <param name="rows">每页行数</param>
        /// <param name="pregnantInfoId">参数(孕妇档案Id)</param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看检查列表)]
        public APIResult<VLPagerResult<PagedListOfLabOrderModel>> GetPagedListOfLabOrder([FromServices] PregnantService pregnantService, int page, int rows, long pregnantInfoId)
        {
            var pars = new GetPagedListOfLabOrderRequest()
            {
                PregnantInfoId = pregnantInfoId,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = pregnantService.GetPagedListOfLabOrder(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.PagedData, serviceResult.Messages);
            return Success(serviceResult.PagedData);
        }

        /// <summary>
        /// 获取 全统计汇总
        /// 顺产人数
        /// 剖宫产人数
        /// 引产人数
        /// 顺转剖人数
        /// 侧切人数
        /// 裂伤人数
        /// 新生儿人数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public APIResult<AllStaticsModel> GetAllStatics()
        {
            var serviceResult = new ServiceResult<AllStaticsModel>(new AllStaticsModel()
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
                return Error(serviceResult.PagedData, "");
            return Success(serviceResult.PagedData);
        }
    }
}
