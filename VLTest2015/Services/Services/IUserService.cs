using System.Collections.Generic;
using VLTest2015.Common;

namespace VLTest2015.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Id</returns>
        ResponseResult<long> CreateUser(string userName, string password);

        /// <summary>
        /// 按用户名密码查询用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ResponseResult<long> PasswordSignIn(string userName, string password, bool rememberMe, bool shouldLockout);

        /// <summary>
        /// 编辑用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorityIds"></param>
        /// <returns></returns>
        ResponseResult<bool> EditUserAuthorities(long userId, IEnumerable<long> authorityIds);

        /// <summary>
        /// 编辑用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        ResponseResult<bool> EditUserRoles(long userId, IEnumerable<long> roleIds);

        /// <summary>
        /// 查询用户当前权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<long>> GetUserAuthorities(long userId);
    }
}
