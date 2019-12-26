using System.Collections.Generic;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public interface IUserAuthorityRepository : IRepository<UserAuthority>
    {
        /// <summary>
        /// 根据用户Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserAuthority> GetBy(long userId);

        /// <summary>
        /// 按照用户Id删除用户权限关联
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int DeleteBy(long userId);
    }
}