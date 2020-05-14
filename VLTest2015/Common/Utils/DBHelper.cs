using System.Data.Common;
using System.Data.SqlClient;

namespace VLTest2015.Utils
{
    public static class DBHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// VLTODO 可以优化为连接池
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetDbConnection()
        {
            return new SqlConnection("Data Source=.;Initial Catalog=VLTest;Integrated Security=True;MultipleActiveResultSets=True");
        }
    }
}