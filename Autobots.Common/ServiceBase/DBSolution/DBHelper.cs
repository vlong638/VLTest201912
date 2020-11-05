using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Autobots.EMRServices.DBSolution
{
    public static class DBHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetSqlDbConnection(string connectingString)
        {
            return new SqlConnection(connectingString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbContext GetSqlDbContext(string connectingString)
        {
            var connection = GetSqlDbConnection(connectingString);
            return new DbContext(connection);
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetOracleDbConnection(string connectingString)
        {
            return new OracleConnection(connectingString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbContext GetOracleDbContext(string connectingString)
        {
            var connection = GetOracleDbConnection(connectingString);
            return new DbContext(connection);
        }

        public static string GetCreateTableSQL(string tableName,string fields, char splitter)
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append($@"            
CREATE TABLE [{tableName}](
	[Id] [bigint] IDENTITY(1,2) NOT NULL");
            foreach (var field in fields.Split(splitter))
            {
                sb.Append($",[{field}] nvarchar(20) NULL");
            }
            sb.Append($@"            
 CONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
");
            return sb.ToString();
        }
    }
}
