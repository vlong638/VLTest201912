using System.Collections.Generic;

namespace VLTest2015.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>Id</returns>
        ResponseResult<long> Register(string userName, string password);

        /// <summary>
        /// 登录(按用户名+密码)
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="rememberMe">记住登录</param>
        /// <param name="shouldLockout">该登录是否会锁死</param>
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
        /// 查询用户当前所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<long>> GetAllUserAuthorityIds(long userId);
    }
}
