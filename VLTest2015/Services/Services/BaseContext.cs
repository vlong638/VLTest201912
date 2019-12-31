using System;
using System.Data;
using VLTest2015.Utils;

namespace VLTest2015.Services
{
    public class BaseContext
    {
        internal IDbConnection _connection;
        internal IDbCommand _command;
        internal IDbTransaction _transaction;

        public BaseContext()
        {
            _connection = DBHelper.GetDbConnection();
            _command = _connection.CreateCommand();
        }

        ~BaseContext()
        {
            _command.Dispose();
            _connection.Dispose();
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
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            _command.Transaction = _transaction;
            try
            {
                var result = exec();
                _transaction.Commit();
                _connection.Close();
                return new ServiceResponse<T>(result);
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                _connection.Close();
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
