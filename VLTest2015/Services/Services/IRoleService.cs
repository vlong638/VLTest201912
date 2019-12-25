using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VLTest2015.Services
{
    public interface IRoleService
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>Id</returns>
        long CreateRole(string roleName);

        /// <summary>
        /// 编辑角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authorityIds"></param>
        /// <returns></returns>
        bool EditRoleAuthorities(long roleId, IEnumerable<long> authorityIds);

        /// <summary>
        /// 查询角色当前权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<long> GetRoleAuthorities(long roleId);
    }
}
