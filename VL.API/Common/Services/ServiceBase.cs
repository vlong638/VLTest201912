using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.API.Common.Repositories;

namespace VL.API.Common.Services
{
    public abstract class ServiceBase
    {
        protected ServiceContext ServiceContext;

        public ServiceBase()
        {
            this.ServiceContext = new ServiceContext();
        }

        #region ServiceResult,语法糖
        public ServiceResult<T> Success<T>(T data)
        {
            return new ServiceResult<T>(data);
        }
        public ServiceResult<T> Error<T>(T data, List<string> messages)
        {
            return new ServiceResult<T>(data, messages.ToArray());
        }
        public ServiceResult<T> Error<T>(T data, params string[] messages)
        {
            return new ServiceResult<T>(data, messages);
        }
        public ServiceResult<T> Error<T>(T data, int code, params string[] messages)
        {
            return new ServiceResult<T>(data, code, messages);
        }
        #endregion

        #region 事务,语法糖
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbGroup"></param>
        /// <param name="exec"></param>
        /// <returns></returns>
        public ServiceResult<T> DelegateTransaction<T>(DbGroup dbGroup, Func<T> exec)
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
                //TODO Log
                dbGroup.Transaction.Rollback();
                dbGroup.Connection.Close();
                return new ServiceResult<T>(default(T), ex.Message);
            }
        }
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// 多库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbGroup1"></param>
        /// <param name="dbGroup2"></param>
        /// <param name="exec"></param>
        /// <returns></returns>
        internal ServiceResult<T> DelegateTransaction<T>(DbGroup dbGroup1, DbGroup dbGroup2, Func<T> exec)
        {
            dbGroup1.Connection.Open();
            dbGroup2.Connection.Open();
            dbGroup1.Transaction = dbGroup1.Connection.BeginTransaction();
            dbGroup2.Transaction = dbGroup2.Connection.BeginTransaction();
            dbGroup1.Command.Transaction = dbGroup1.Transaction;
            dbGroup2.Command.Transaction = dbGroup2.Transaction;
            try
            {
                var result = exec();
                dbGroup1.Transaction.Commit();
                dbGroup2.Transaction.Commit();
                dbGroup1.Connection.Close();
                dbGroup2.Connection.Close();
                return new ServiceResult<T>(result);
            }
            catch (Exception ex)
            {
                //TODO Log
                dbGroup1.Transaction.Rollback();
                dbGroup2.Transaction.Rollback();
                dbGroup1.Connection.Close();
                dbGroup2.Connection.Close();
                return new ServiceResult<T>(default(T), ex.Message);
            }
        } 
        #endregion

    }
}
