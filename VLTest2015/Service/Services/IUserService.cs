using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VLTest2015.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Id</returns>
        long CreateUser(string userName, string password);

        /// <summary>
        /// 按用户名密码查询用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User GetUserBy(string userName, string password);

        /// <summary>
        /// 编辑用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorityIds"></param>
        /// <returns></returns>
        bool EditUserAuthorities(long userId, IEnumerable<long> authorityIds);

        /// <summary>
        /// 编辑用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        bool EditUserRoles(long userId, IEnumerable<long> roleIds);

        /// <summary>
        /// 查询用户当前权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<long> GetUserAuthorities(long userId);
    }
}
