using System;
using System.Data.Common;
using System.Data.SqlClient;
using VLTest2015.Common;

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

        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="exec"></param>
        /// <returns></returns>
        public static ResponseResult<T> DelegateTransaction<T>(this DbConnection connection, Func<T> exec)
        {
            var transaction = connection.BeginTransaction();
            try
            {
                var result = exec();
                transaction.Commit();
                return new ResponseResult<T>(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseResult<T>()
                {
                    ErrorCode = 501,
                    ErrorMessage = ex.ToString(),
                    Status = false,
                };
            }
        }
    }
}