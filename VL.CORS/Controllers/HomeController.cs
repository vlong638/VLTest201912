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
        public APIResult<VLPagerResult<List<GetUserModel>>> GetUsers([FromServices] AccountService service, [FromBody] GetUsersRequest request)
        {
            var result = service.GetPagedUsers(request.Page, request.Limit, request.UserName, request.NickName);
            return new APIResult<VLPagerResult<List<GetUserModel>>>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetUsersRequest
        {
            /// <summary>
            /// 页码
            /// </summary>
            public int Page { set; get; }
            /// <summary>
            /// 页面显示数量
            /// </summary>
            public int Limit { set; get; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { set; get; }
            /// <summary>
            /// 昵称
            /// </summary>
            public string NickName { set; get; }
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<long> CreateUser([FromServices] AccountService service, [FromBody] CreateUserRequest request)
        {
            var result = service.CreateUser(new User()
            {
                Name = request.UserName,
                Password = request.Password,
                NickName = request.NickName,
                Sex = request.Sex,
                Phone = request.Phone
            }, request.RoleIds
            );
            return new APIResult<long>(result);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<bool> EditUser([FromServices] AccountService service, [FromBody] EditUserRequest request)
        {
            var result = service.EditUser(new User()
            {
                NickName = request.NickName,
                Sex = request.Sex,
                Phone = request.Phone
            }, request.RoleIds
            );
            return new APIResult<bool>(result);
        }
    }
}
