using System;

namespace VLTest2015.Services
{
    public class BaseService
    {
        public DbContext _context;

        public BaseService()
        {
            //VLTODO,上下文可以以注册发现的形式实现
            _context = new DbContext();
        }

        public ServiceResult<T> Success<T>(T data)
        {
            return new ServiceResult<T>(data);
        }

        public ServiceResult<T> Error<T>(string errorMessage = "", int errorCode = -1)
        {
            return new ServiceResult<T>(default(T), errorCode, errorMessage);
        }

        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="exec"></param>
        /// <returns></returns>
        public ServiceResult<T> DelegateTransaction<T>(Func<T> exec)
        {
            _context.Connection.Open();
            _context.Transaction = _context.Connection.BeginTransaction();
            _context.Command.Transaction = _context.Transaction;
            try
            {
                var result = exec();
                _context.Transaction.Commit();
                _context.Connection.Close();
                return new ServiceResult<T>(result);
            }
            catch (Exception ex)
            {
                _context.Transaction.Rollback();
                _context.Connection.Close();
                return new ServiceResult<T>(default(T), ex.Message);
            }
        }
    }
}
