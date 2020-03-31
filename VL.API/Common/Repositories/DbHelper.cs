using System.Data.Common;
using System.Data.SqlClient;

namespace VL.API.Common.Repositories
{
    public class DbHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// VLTODO 可以优为连接池
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetSQLServerDbConnection(string connectingString)
        {
            return new SqlConnection(connectingString);
        }
        ///// <summary>
        ///// 创建数据库连接
        ///// </summary>
        ///// <returns></returns>
        //public static DbConnection GetOracleDbConnection(string connectingString)
        //{
        //    return new OracleConnection(connectingString);
        //}
    }
}
