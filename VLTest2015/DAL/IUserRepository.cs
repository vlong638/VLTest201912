using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public interface IUserRepository : IRepository<TUser>
    {
        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        TUser GetBy(string userName);
    }
}