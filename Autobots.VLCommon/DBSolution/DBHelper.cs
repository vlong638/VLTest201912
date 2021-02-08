using System.Data.Common;
using System.Data.SqlClient;

namespace Autobots.Infrastracture.Common.DBSolution
{
    /// <summary>
    /// 
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetDbConnection(string connectingString)
        {
            return new SqlConnection(connectingString);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbContext GetDbContext(string connectingString)
        {
            var connection = GetDbConnection(connectingString);
            return new DbContext(connection);
        }
    }
}
