using System;
using System.Data;
using VLTest2015.Utils;

namespace VLTest2015.Services
{
    public class DbContext
    {
        internal IDbConnection Connection;
        internal IDbCommand Command;
        internal IDbTransaction Transaction;

        public DbContext()
        {
            Connection = DBHelper.GetDbConnection();
            Command = Connection.CreateCommand();
        }

        ~DbContext()
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
        public ServiceResponse<T> DelegateTransaction<T>(Func<T> exec)
        {
            Connection.Open();
            Transaction = Connection.BeginTransaction();
            Command.Transaction = Transaction;
            try
            {
                var result = exec();
                Transaction.Commit();
                Connection.Close();
                return new ServiceResponse<T>(result);
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                Connection.Close();
                return new ServiceResponse<T>()
                {
                    ErrorCode = 501,
                    ErrorMessage = ex.ToString(),
                    Status = false,
                };
            }
        }
    }
}
