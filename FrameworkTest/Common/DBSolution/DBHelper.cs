using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Data.SqlClient;

namespace FrameworkTest.Common.DBSolution
{
    public static class DBHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetSqlDbConnection(string connectingString)
        {
            return new SqlConnection(connectingString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbContext GetSqlDbContext(string connectingString)
        {
            var connection = GetSqlDbConnection(connectingString);
            return new DbContext(connection);
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetOracleDbConnection(string connectingString)
        {
            return new OracleConnection(connectingString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbContext GetOracleDbContext(string connectingString)
        {
            var connection = GetOracleDbConnection(connectingString);
            return new DbContext(connection);
        }

    }
}
