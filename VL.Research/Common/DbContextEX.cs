using System;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ServiceSolution;

namespace VL.Research.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbContextEX
    {
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateTransaction<T>(this DbContext context, Func<DbGroup, T> exec)
        {
            context.DbGroup.Connection.Open();
            context.DbGroup.Transaction = context.DbGroup.Connection.BeginTransaction();
            context.DbGroup.Command.Transaction = context.DbGroup.Transaction;
            try
            {
                var result = exec(context.DbGroup);
                context.DbGroup.Transaction.Commit();
                context.DbGroup.Connection.Close();
                return new ServiceResult<T>(result);
            }
            catch (Exception ex)
            {
                context.DbGroup.Transaction.Rollback();
                context.DbGroup.Connection.Close();

                //集成Log4Net
                Log4NetLogger.Error("DelegateTransaction Exception", ex);

                return new ServiceResult<T>(default(T), ex.Message);
            }
        }
    }
}
