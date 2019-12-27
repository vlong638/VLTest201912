using System.Collections.Generic;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        User GetBy(string userName);

        /// <summary>
        /// 按照用户名和密码查询
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User GetBy(string userName, string password);

        /// <summary>
        /// 获取用户列表分页数据统计
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        int GetUserPageListCount(GetUserPageListRequest paras);

        /// <summary>
        /// 获取用户列表分页数据
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        List<User> GetUserPageListData(GetUserPageListRequest paras);
    }
}