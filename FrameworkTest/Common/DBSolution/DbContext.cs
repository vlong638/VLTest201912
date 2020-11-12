using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.LoggerSolution;
using FrameworkTest.Common.ServiceSolution;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace FrameworkTest.Common.DBSolution
{
    public class DbContext
    {
        public DbGroup DbGroup { set; get; }
        public VLLogger VLLogger { set; get; }

        public DbContext(IDbConnection connection)
        {
            DbGroup = new DbGroup(connection);
            VLLogger = new VLLogger(FileHelper.GetDirectory("Logs"));
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
                VLLogger.Log(ex.ToString());
                DbGroup.Transaction.Rollback();
                DbGroup.Connection.Close();
                //return new ServiceResult<T>(default(T), ex.Message);
                throw ex;
            }
        }
    }
}
