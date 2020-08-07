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
        /// <param name="field">参数(排序项)</param>
        /// <param name="order">参数(排序顺序:asc|desc)</param>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public APIResult<List<Dictionary<string, object>>, int> GetConfigurablePagedListOfPregnantInfo([FromServices] PregnantService pregnantService, int page, int limit, string field, string order, string personname)
        {
            ViewConfig viewConfig;
            viewConfig = SystemController.GetViewConfigByName("PregnantInfo");
            var pars = new GetPagedListOfPregnantInfoRequest()
            {
                PageIndex = page,
                PageSize = limit,
                PersonName = personname,
                Orders = new Dictionary<string, bool>(),
            };

            //更新字段参数
            viewConfig.UpdatePropertiesToSelect(pars.FieldNames);
            //更新排序参数
            viewConfig.UpdateOrderBy(pars.Orders, field, order);
            //获取数据
            var serviceResult = pregnantService.GetConfigurablePagedListOfPregnantInfo(pars);
            //更新显示映射(枚举,函数,脱敏)
            viewConfig.UpdateValues(serviceResult.PagedData.SourceData);

            if (!serviceResult.IsSuccess)
                return Error(data1: serviceResult.PagedData.SourceData, data2: serviceResult.PagedData.Count, messages: serviceResult.Messages);
            return Success(serviceResult.PagedData.SourceData, serviceResult.PagedData.Count, serviceResult.Messages);
        }

        /// <summary>
        /// 获取 自定义配置的孕妇档案列表
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页行数</param>
        /// <param name="personname">参数(姓名)</param>
        /// <param name="field">参数(排序项)</param>
        /// <param name="order">参数(排序顺序:asc|desc)</param>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public APIResult<List<Dictionary<string, object>>, int> GetCommonSelectOfPregnantInfo([FromServices] SharedService sharedService, int page, int limit, string field, string order, string personname)
        {
            var viewConfig = SystemController.GetViewConfigByName("PregnantInfo");
            var sqlConfig = SystemController.GetSQLConfigByName("PregnantInfo");
            Dictionary<string, object> wheres = new Dictionary<string, object>();
            wheres.Add("PersonName", personname);
            sqlConfig.PageIndex = page;
            sqlConfig.PageSize = limit;
            sqlConfig.UpdateWheres(wheres);
            sqlConfig.UpdateOrderBy(field, order);
            //获取数据
            var serviceResult = sharedService.GetCommonSelect(sqlConfig);
            //更新显示映射(枚举,函数,脱敏)
            viewConfig.UpdateValues(serviceResult.PagedData.SourceData);

            if (!serviceResult.IsSuccess)
                return Error(data1: serviceResult.PagedData.SourceData, data2: serviceResult.PagedData.Count, messages: serviceResult.Messages);
            return Success(serviceResult.PagedData.SourceData, serviceResult.PagedData.Count, serviceResult.Messages);
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
                return Error(data: default(PregnantInfo), messages: "缺少有效的 pregnantInfoId");
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
                return Error(data: serviceResult.PagedData, messages: serviceResult.Message);
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
                return Error(data:serviceResult.PagedData, "");
            return Success(serviceResult.PagedData);
        }
    }
}
