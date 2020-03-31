using System;
using System.Data;
using System.Data.Common;

namespace VL.API.Common.Repositories
{
    /// <summary>
    /// 数据库访问单元:支持事务及跨库协作
    /// </summary>
    public class DbGroup
    {
        internal IDbConnection Connection;
        internal IDbCommand Command;
        internal IDbTransaction Transaction;

        public DbGroup(DbConnection dbConnection)
        {
            this.Connection = dbConnection;
            this.Command = Connection.CreateCommand();
        }

        ~DbGroup()
        {
            Command.Dispose();
            Connection.Dispose();
        }

        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="exec"></param>
        /// <returns></returns>
        public T DelegateTransaction<T>(Func<T> exec)
        {
            Connection.Open();
            Transaction = Connection.BeginTransaction();
            Command.Transaction = Transaction;
            try
            {
                var result = exec();
                Transaction.Commit();
                Connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                Connection.Close();
                throw ex;
            }
        }
    }
}
