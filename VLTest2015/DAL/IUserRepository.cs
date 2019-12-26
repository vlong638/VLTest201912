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
        User GetBy(object userName, string password);
    }
}