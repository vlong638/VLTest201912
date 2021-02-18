using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.LoggerSolution;
using Autobots.Infrastracture.Common.ServiceSolution;
using System;
using System.Data;

namespace Autobots.Infrastracture.Common.TransactionSolution
{

    /// <summary>
    /// 
    /// </summary>
    public static class DbGroupEX
    {
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateTransaction<T>(this DbGroup dbGroup, Func<DbGroup, T> exec, ILogger logger)
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
                    logger.Error("DelegateTransaction Exception", ex);

                    return new ServiceResult<T>(default(T), code: 500, ex.Message);
                }
                finally
                {
                    dbGroup.Connection.Close();
                }
            }
            catch (Exception e)
            {
                logger.Error("打开数据库连接配置失败,当前数据库连接," + dbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), code: 500, e.Message);
            }
        }
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateNonTransaction<T>(this DbGroup dbGroup, Func<DbGroup, T> exec, ILogger logger)
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
                    logger.Error("DelegateTransaction Exception", ex);
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
                logger.Error("打开数据库连接配置失败,当前数据库连接," + dbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), e.Message);
            }
        }
    }
}
