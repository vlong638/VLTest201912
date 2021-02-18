using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ServiceSolution;
using System;
using System.Data;

namespace FrameworkTest.Common.TransactionSolution
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbGroupEX
    {
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateTransaction<T>(this DbGroup dbGroup, Func<DbGroup, T> exec)
        {
            try
            {
                if (dbGroup.Connection.State != ConnectionState.Open)
                    dbGroup.Connection.Open();
                dbGroup.Transaction = dbGroup.Connection.BeginTransaction();
                dbGroup.Command.Transaction = dbGroup.Transaction;
                try
                {
                    var result = exec(dbGroup);
                    dbGroup.Transaction.Commit();
                    return new ServiceResult<T>(result);
                }
                catch (Exception ex)
                {
                    dbGroup.Transaction.Rollback();
                    Log4NetLogger.Error("DelegateTransaction Exception", ex);

                    return new ServiceResult<T>(default(T), code: 500, ex.Message);
                }
                finally
                {
                    dbGroup.Connection.Close();
                }
            }
            catch (Exception e)
            {
                Log4NetLogger.Error("打开数据库连接配置失败,当前数据库连接," + dbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), code: 500, e.Message);
            }
        }
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateNonTransaction<T>(this DbGroup dbGroup, Func<DbGroup, T> exec)
        {
            try
            {
                if (dbGroup.Connection.State != ConnectionState.Open)
                    dbGroup.Connection.Open();
                try
                {
                    var result = exec(dbGroup);
                    return new ServiceResult<T>(result);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Error("DelegateTransaction Exception", ex);
                    return new ServiceResult<T>(default(T), ex.Message);
                }
                finally
                {
                    dbGroup.Connection.Close();
                }
            }
            catch (Exception e)
            {
                //集成Log4Net
                Log4NetLogger.Error("打开数据库连接配置失败,当前数据库连接," + dbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), e.Message);
            }
        }
    }
}
