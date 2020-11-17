using System.Data;

namespace Autobots.Infrastracture.Common.DBSolution
{
    /// <summary>
    /// 数据库访问上下文
    /// </summary>
    public class DbContext: UnitOfWork
    {
        public DbGroup DbGroup { set; get; }

        public DbContext()
        { }

        public DbContext(IDbConnection connection)
        {
            DbGroup = new DbGroup(connection);
        }
    }
    /// <summary>
    /// 工作单元模式
    /// 负责组合事务的定义
    /// </summary>
    public interface UnitOfWork
    {
        
        
    }
}
