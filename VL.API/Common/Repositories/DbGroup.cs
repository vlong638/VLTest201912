using System;
using System.Data;
using System.Data.Common;
using VL.API.Common.Services;

namespace VL.API.Common.Repositories
{
    /// <summary>
    /// 数据库访问单元:支持事务及跨库协作
    /// </summary>
    public class DbGroup
    {
        internal IDbConnection Connection;
        internal IDbCommand Command;
        internal IDbTransaction Transaction;

        public DbGroup(DbConnection dbConnection)
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
