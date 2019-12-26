using System.Collections.Generic;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public interface IRoleAuthorityRepository : IRepository<RoleAuthority>
    {
        /// <summary>
        /// 根据角色Id查询权限
        /// </summary>
        /// <param name="RoleAuthorityName"></param>
        /// <returns></returns>
        IEnumerable<RoleAuthority> GetBy(params long[] roleIds);

        /// <summary>
        /// 根据角色Id删除权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int DeleteBy(long roleId);
    }
}