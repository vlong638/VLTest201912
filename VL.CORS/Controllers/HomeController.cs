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

        #region User
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

        /// <summary>
        /// 逻辑删除用户
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<bool> LogicDeleteUser([FromServices] AccountService service, [FromBody] LogicDeleteUserRequest request)
        {
            var result = service.LogicDeleteUser(request.UserId);
            return new APIResult<bool>(result);
        }
        /// <summary>
        /// 逻辑启用用户
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<bool> LogicUndoDeleteUser([FromServices] AccountService service, [FromBody] LogicUndoDeleteUserRequest request)
        {
            var result = service.LogicUndoDeleteUser(request.UserId);
            return new APIResult<bool>(result);
        } 
        #endregion

        #region Role

        /// <summary>
        /// 获取角色管理列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<VLPagerResult<List<GetRoleModel>>> GetRoles([FromServices] AccountService service, [FromBody] GetRolesRequest request)
        {
            var result = service.GetPagedRoles(request.Page, request.Limit, request.RoleName);
            return new APIResult<VLPagerResult<List<GetRoleModel>>>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetRolesRequest
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
            /// 角色名称
            /// </summary>
            public string RoleName { set; get; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class CreateRoleRequest
        {
            /// <summary>
            /// 角色
            /// </summary>
            public string RoleName { set; get; }
        }

        /// <summary>
        /// 新建角色
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<long> CreateRole([FromServices] AccountService service, [FromBody] CreateRoleRequest request)
        {
            var result = service.CreateRole(new Role()
            {
                Name = request.RoleName,
                Category = RoleCategory.SystemRole,
            });
            return new APIResult<long>(result);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<bool> EditRole([FromServices] AccountService service, [FromBody] EditRoleRequest request)
        {
            var result = service.EditRole(new Role()
            {
                Id = request.RoleId,
                Name = request.RoleName,
            });
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        public class EditRoleRequest
        {
            /// <summary>
            /// 角色Id
            /// </summary>
            public long RoleId { set; get; }
            /// <summary>
            /// 角色名称
            /// </summary>
            public string RoleName { set; get; }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<bool> DeleteRole([FromServices] AccountService service, [FromBody] DeleteRoleRequest request)
        {
            var result = service.DeleteRole(request.RoleId);
            return new APIResult<bool>(result);
        }

        #endregion

    }
}
