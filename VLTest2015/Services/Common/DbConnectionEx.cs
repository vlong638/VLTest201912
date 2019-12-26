using System;
using System.Data.Common;

namespace VLTest2015.Services
{
    public static class DbConnectionEx
    {
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