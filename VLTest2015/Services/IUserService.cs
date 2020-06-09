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
        ServiceResult<User> Register(string userName, string password);

        /// <summary>
        /// 登录(按用户名+密码)
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="shouldLockout">该登录是否会锁死</param>
        /// <returns></returns>
        ServiceResult<User> PasswordSignIn(string userName, string password, bool shouldLockout);

        /// <summary>
        /// 编辑用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorityIds"></param>
        /// <returns></returns>
        ServiceResult<bool> EditUserAuthorities(long userId, IEnumerable<long> authorityIds);

        /// <summary>
        /// 编辑用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        ServiceResult<bool> EditUserRoles(long userId, IEnumerable<long> roleIds);

        /// <summary>
        /// 查询用户当前所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ServiceResult<IEnumerable<long>> GetAllUserAuthorityIds(long userId);

        /// <summary>
        /// 查询分页用户列表
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        ServiceResult<PagerResponse<User>> GetUserPageList(GetUserPageListRequest request);

        /// <summary>
        /// 获取用户对应的角色列表
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        ServiceResult<IEnumerable<UserRoleInfo>> GetRoleInfoByUserIds(params long[] enumerable);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>Id</returns>
        ServiceResult<long> CreateRole(string roleName);

        /// <summary>
        /// 编辑角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authorityIds"></param>
        /// <returns></returns>
        ServiceResult<bool> EditRoleAuthorities(long roleId, IEnumerable<long> authorityIds);

        /// <summary>
        /// 查询角色当前权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ServiceResult<IEnumerable<long>> GetRoleAuthorityIds(long roleId);

        /// <summary>
        /// 查询所有角色
        /// </summary>
        ServiceResult<IEnumerable<Role>> GetAllRoles();

        /// <summary>
        /// 更新自定义页面
        /// </summary>
        /// <param name="userMenu"></param>
        /// <returns></returns>
        ServiceResult<bool> UpdateUserMenu(UserMenu userMenu);

        /// <summary>
        /// 新建自定义页面
        /// </summary>
        /// <param name="userMenu"></param>
        /// <returns></returns>
        ServiceResult<long> CreateUserMenu(UserMenu userMenu);
    }
}
