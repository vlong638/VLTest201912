using System.Data;

namespace FrameworkTest.Common
{
    /// <summary>
    /// 数据库访问单元:支持事务及跨库协作
    /// </summary>
    public class DbGroup
    {
        internal IDbConnection Connection;
        internal IDbCommand Command;
        internal IDbTransaction Transaction;

        public DbGroup(IDbConnection dbConnection)
        {
            this.Connection = dbConnection;
            this.Command = Connection.CreateCommand();
        }

        ~DbGroup()
        {
            Command.Dispose();
            Connection.Dispose();
        }
    }
}
