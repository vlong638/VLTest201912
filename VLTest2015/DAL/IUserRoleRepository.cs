using System.Collections.Generic;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        /// <summary>
        /// 根据用户Id查询角色用户关联集合
        /// </summary>
        /// <param name="UserRoleName"></param>
        /// <returns></returns>
        IEnumerable<UserRole> GetBy(long userId);

        /// <summary>
        /// 根据UserId删除用户角色关联
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int DeleteBy(long userId);
    }
}