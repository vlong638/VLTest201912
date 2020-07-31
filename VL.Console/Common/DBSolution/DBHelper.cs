using System.Data.Common;
using System.Data.SqlClient;
using VL.Consolo_Core.Common.DBSolution;

namespace VL.Consolo_Core.Common.DBSolution
{
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
