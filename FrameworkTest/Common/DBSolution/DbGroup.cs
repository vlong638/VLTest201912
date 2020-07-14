using FrameworkTest.Common.ServiceSolution;
using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace FrameworkTest.Common.DBSolution
{
    /// <summary>
    /// 数据库访问单元:支持事务及跨库协作
    /// </summary>
    public class DbGroup
    {
        public IDbConnection Connection;
        public IDbCommand Command;
        public IDbTransaction Transaction;

        public DbGroup(IDbConnection dbConnection)
        {
            this.Connection = dbConnection;
            this.Command = Connection.CreateCommand();
        }

        ~DbGroup()
        {
            Command.Dispose();
            Connection.Dispose();
        }
    }

    public static class DbGroupEx
    {
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="exec"></param>
        /// <returns></returns>
        public static ServiceResult<T> DelegateTransaction<T>(this DbGroup dbGroup, Func<T> exec)
        {
            dbGroup.Connection.Open();
            dbGroup.Transaction = dbGroup.Connection.BeginTransaction();
            dbGroup.Command.Transaction = dbGroup.Transaction;
            try
            {
                var result = exec();
                dbGroup.Transaction.Commit();
                dbGroup.Connection.Close();
                return new ServiceResult<T>(result);
            }
            catch (Exception ex)
            {
                dbGroup.Transaction.Rollback();
                dbGroup.Connection.Close();
                return new ServiceResult<T>(default(T), ex.Message);
            }
        }
    }
}
