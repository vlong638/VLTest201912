using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Atp;
using NPOI.SS.Formula.Functions;
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
    public class UserController : APIBaseController
    {
        /// <summary>
        /// 账户
        /// </summary>
        public UserController()
        {
        }


        /// <summary>
        /// 获取 角色列表
        /// </summary>
        [HttpPost]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public APIResult<List<Dictionary<string, object>>, int> GetCommonSelectOfRole([FromServices] SharedService sharedService, GetCommonSelectRequest request)
        {
            var viewConfig = SystemController.GetViewConfigByName("Role");
            var sqlConfig = SystemController.GetSQLConfigByName("Role");
            sqlConfig.PageIndex = request.page;
            sqlConfig.PageSize = request.limit;
            sqlConfig.UpdateWheres(request.search);
            sqlConfig.UpdateOrderBy(request.field, request.order);
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
