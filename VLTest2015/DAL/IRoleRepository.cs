using System.Collections.Generic;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// 根据角色名称查询角色
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Role GetBy(string name);

        /// <summary>
        /// 根据userIds查询角色集合
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        IEnumerable<UserRoleInfo> GetUserRoleInfosBy(long[] userIds);

        /// <summary>
        /// 查找所有角色
        /// </summary>
        /// <returns></returns>
        IEnumerable<Role> GetAll();
    }
}