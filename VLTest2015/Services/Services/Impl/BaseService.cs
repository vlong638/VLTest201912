using System;
using System.Data;
using VLTest2015.Utils;

namespace VLTest2015.Services
{
    public class BaseService
    {
        internal IDbConnection _connection;
        internal IDbCommand _command;
        internal IDbTransaction _transaction;

        public BaseService()
        {
            _connection = DBHelper.GetDbConnection();
            _command = _connection.CreateCommand();
        }

        ~BaseService()
        {
            _command.Dispose();
            _connection.Dispose();
        }

        public ResponseResult<T> Success<T>(T data)
        {
            return new ResponseResult<T>()
            {
                Data = data,
                Status = true,
            };
        }
        public ResponseResult<T> Error<T>(string errorMessage = "", int errorCode = -1)
        {
            return new ResponseResult<T>()
            {
                Status = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
            };
        }

        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="exec"></param>
        /// <returns></returns>
        public ResponseResult<T> DelegateTransaction<T>(Func<T> exec)
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            _command.Transaction = _transaction;
            try
            {
                var result = exec();
                _transaction.Commit();
                _connection.Close();
                return new ResponseResult<T>(result);
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                _connection.Close();
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
