using Autobots.Infrastracture.Common.ControllerSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Services;
using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Controllers
{
    /// <summary>
    /// 账户权限接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class HomeController : APIBaseController
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<string> Login([FromServices] APIContext apiContext, [FromServices] AccountService service, [FromBody] LoginRequest request)
        {
            var result = service.PasswordSignIn(request.UserName, request.Password);
            if (result.IsSuccess)
            {
                var token = apiContext.SetCurrentUser(new CurrentUser(result.Data));
                return new APIResult<string>(token, result.Messages);
            }
            return new APIResult<string>(null, result.Messages);
        }

        /// <summary>
        /// 获取用户管理列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<VLPagerResult<List<GetUserModel>>> GetUsers([FromServices] AccountService service, int page,int limit, string username, string nickname)
        {
            var result = service.GetPagedUsers(page, limit, username, nickname);
            return new APIResult<VLPagerResult<List<GetUserModel>>>(result);
        }
    }
}
