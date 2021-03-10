using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autobots.Infrastracture.Common.DBSolution
{
    /// <summary>
    /// 数据库访问上下文
    /// </summary>
    public class DbContext: UnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        public DbGroup DbGroup { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DbContext()
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public DbContext(IDbConnection connection)
        {
            DbGroup = new DbGroup(connection);
        }

        #region 基础方法

        public int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return DbGroup.Connection.Execute(sql, param, DbGroup.Transaction, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return (T)DbGroup.Connection.ExecuteScalar( sql, param, DbGroup.Transaction, commandTimeout, commandType);
        }
        public object ExecuteScalar(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return DbGroup.Connection.ExecuteScalar(sql, param, DbGroup.Transaction, commandTimeout, commandType);
        }

        public T Query<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return DbGroup.Connection.Query<T>(sql, param, DbGroup.Transaction).FirstOrDefault();
        }

        public List<T> QueryList<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return DbGroup.Connection.Query<T>(sql, param, DbGroup.Transaction).ToList();
        }

        #endregion
    }
    /// <summary>
    /// 工作单元模式
    /// 负责组合事务的定义
    /// </summary>
    public interface UnitOfWork
    {
        
        
    }
}
