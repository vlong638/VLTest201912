using Microsoft.Extensions.Options;
using System;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Research.Common.Configuration;

namespace VL.Research.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class APIContext
    {
        /// <summary>
        /// 
        /// </summary>
        public DbGroup DbGroup { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public APIContext(IOptions<DBConfig> loggingConfig)
        {
            var connectingStr = DBHelper.GetDbConnection(loggingConfig.Value.ConnectionString);
            DbGroup = new DbGroup(connectingStr);
        }

        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
