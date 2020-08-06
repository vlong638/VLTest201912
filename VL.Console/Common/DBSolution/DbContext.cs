﻿using VL.Consolo_Core.Common.DBSolution;
using System;
using System.Data;
using VL.Consolo_Core.Common.ServiceSolution;

namespace VL.Consolo_Core.Common.DBSolution
{
    public class DbContext
    {
        public DbGroup DbGroup { set; get; }

        public DbContext()
        { }

        public DbContext(IDbConnection connection)
        {
            DbGroup = new DbGroup(connection);
        }

        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="exec"></param>
        /// <returns></returns>
        public ServiceResult<T> DelegateTransaction<T>(Func<DbGroup, T> exec)
        {
            DbGroup.Connection.Open();
            DbGroup.Transaction = DbGroup.Connection.BeginTransaction();
            DbGroup.Command.Transaction = DbGroup.Transaction;
            try
            {
                var result = exec(DbGroup);
                DbGroup.Transaction.Commit();
                DbGroup.Connection.Close();
                return new ServiceResult<T>(result);
            }
            catch (Exception ex)
            {
                DbGroup.Transaction.Rollback();
                DbGroup.Connection.Close();
                return new ServiceResult<T>(default(T), ex.Message);
            }
        }
    }
}