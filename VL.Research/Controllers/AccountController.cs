using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Atp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VL.Consolo_Core.Common.ControllerSolution;
using VL.Research.Common;
using VL.Research.Models;
using VL.Research.Services;

namespace VL.Research.Controllers
{
    /// <summary>
    /// 登录状态枚举
    /// </summary>
    public enum SignInStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Success = 0,
        /// <summary>
        /// 
        /// </summary>
        LockedOut = 1,
        /// <summary>
        /// 
        /// </summary>
        RequiresVerification = 2,
        /// <summary>
        /// 
        /// </summary>
        Failure = 3
    }

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : APIBaseController
    {
        /// <summary>
        /// 账户
        /// </summary>
        public AccountController()
        {
        }


        /// <summary>
        /// 获取 自定义配置的孕妇档案列表
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页行数</param>
        /// <param name="name">参数(姓名)</param>
        /// <param name="field">参数(排序项)</param>
        /// <param name="order">参数(排序顺序:asc|desc)</param>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public APIResult<List<Dictionary<string, object>>, int> GetCommonSelectOfAccount([FromServices] SharedService sharedService, int page, int limit, string field, string order, string name)
        {
            var viewConfig = SystemController.GetViewConfigByName("Account");
            var sqlConfig = SystemController.GetSQLConfigByName("Account");
            Dictionary<string, object> wheres = new Dictionary<string, object>();
            wheres.Add("name", name);
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
    }
}
