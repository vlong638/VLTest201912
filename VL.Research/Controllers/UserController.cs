using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VL.Consolo_Core.Common.ControllerSolution;
using VL.Consolo_Core.Common.ValuesSolution;
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
        /// 可选树形列表
        /// </summary>
        public class CheckableTreeResponse
        {
            public CheckableTreeResponse()
            {
            }

            public CheckableTreeResponse(long id, string name, bool open)
            {
                this.id = id;
                this.name = name;
                this.open = open;
            }

            /// <summary>
            /// 
            /// </summary>
            public long id { set; get; }
            /// <summary>
            /// 
            /// </summary>
            public string name { set; get; }
            /// <summary>
            /// 
            /// </summary>
            public bool open { set; get; }
            /// <summary>
            /// 
            /// </summary>
            public long pId { set; get; }
            /// <summary>
            /// 
            /// </summary>
            public bool @checked { set; get; }
        }

        /// <summary>
        /// 获取 当前用户的角色
        /// </summary>
        [HttpGet]
        //[VLAuthentication(Authority.编辑用户角色)]
        public APIResult<List<CheckableTreeResponse>> GetUserRoleListByCurrentUser([FromServices] UserService userService)
        {
            var user = GetCurrentUser();
            if (user.UserId <= 0)
            {
                return Error<List<CheckableTreeResponse>>(messages: "需选中用户");
            }

            var roles = userService.GetAllRoles().Data;
            var userRoles = userService.GetRoleInfoByUserIds(user.UserId).Data;
            List<CheckableTreeResponse> result = roles.Select(c => new CheckableTreeResponse()
            {
                id = c.Id,
                pId = 0,
                name = c.Name,
                @checked = userRoles.FirstOrDefault(d => d.RoleId == c.Id) != null
            }).ToList();
            return Success(result);
        }

        /// <summary>
        /// 获取 角色权限`
        /// </summary>
        [HttpGet]
        //[VLAuthentication(Authority.编辑角色权限)]
        public APIResult<List<CheckableTreeResponse>> GetRoleAuthorities([FromServices] UserService userService, long Id)
        {
            if (Id <= 0)
            {
                return Error<List<CheckableTreeResponse>>(messages: "需选中角色");
            }
            var authorities = typeof(Authority).GetAllEnums<Authority>();
            var roleAuthorities = userService.GetRoleAuthorityIds(Id).Data;
            var result = authorities.Select(c => new CheckableTreeResponse() { 
                id = (long)c,
                pId = ((int)c).ToString().GetSubStringOrEmpty(0,3).ToLong().Value,
                name = c.GetDescription(),
                @checked = roleAuthorities.ToList().Contains((long)c) }).ToList();
            result.Add(new CheckableTreeResponse(101, "分娩信息", false));
            result.Add(new CheckableTreeResponse(102, "孕妇档案", false));
            result.Add(new CheckableTreeResponse(999, "账户系统", false));
            return Success(result);
        }

        /// <summary>
        /// 编辑 角色权限
        /// </summary>
        [HttpPost]
        //[VLAuthentication(Authority.编辑角色权限)]
        public APIResult<bool> EditRoleAuthority([FromServices] UserService userService, EditRoleAuthorityRequest data)
        {
            var roleId = data.roleId.ToLong();
            if (!roleId.HasValue || roleId.Value == 0)
            {
                return Error<bool>(messages: "缺少有效的角色Id");
            }
            var result = userService.EditRoleAuthorities(roleId.Value, data.authorityIds);
            return Success<bool>(result.Data, "保存成功");
        }

        public class EditRoleAuthorityRequest
        {
            public string roleId { set; get; }
            public List<long> authorityIds { set; get; }
        }
    }
}
