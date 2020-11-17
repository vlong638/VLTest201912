using System.Collections.Generic;

namespace Autobots.Common.Utils
{
    /// <summary>
    /// 数据库访问中间件
    /// </summary>
    public interface DatabaseMiddware
    {
        /// <summary>
        /// 根据用户信息进行数据库的路由
        /// 负载均衡
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        string GetDBConnectingString(UserInfo userInfo);
    }

    /// <summary>
    /// 数据库访问帮助类
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 调用配置中心获取服务配置
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetDBConfig() { return null; }
    }
}
