using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FrameworkTest.Business.Research
{

    public class CommonRepository : RepositoryBase<object>
    {
        public CommonRepository(DbContext context) : base(context)
        {
        }

        public DataTable GetDataTable(DbGroup group, string sql)
        {
            DataTable table = new DataTable("MyTable");
            var reader = group.Connection.ExecuteReader(sql, transaction: _transaction);
            table.Load(reader);
            return table;
        }

        public void InsertDataRow(DbGroup group, DataRow row, string tableName)
        {
            if (row == null)
                return;
            var dataTable = row.Table;
            StringBuilder sb = new StringBuilder();
            sb.Append($"insert into {tableName}");
            sb.Append("(");
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                sb.Append($"[{dataTable.Columns[i].ColumnName}]");
                if (i == dataTable.Columns.Count - 1)
                {
                    sb.Append(")");
                }
                else
                {
                    sb.Append(",");
                }
            }
            sb.Append(" Values ");
            sb.Append("(");
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                sb.Append($"{row[dataTable.Columns[j].ColumnName].ToString().ToMSSQLValue()}");
                if (j == dataTable.Columns.Count - 1)
                {
                    sb.Append(")");
                }
                else
                {
                    sb.Append(",");
                }
            }
            group.Connection.Execute(sb.ToString(), transaction: group.Transaction);
        }

        public void InsertDataTable(DbGroup group, DataTable dataTable, string tableName)
        {
            if (dataTable.Rows.Count == 0)
                return;
            StringBuilder sb = new StringBuilder();
            sb.Append($"insert into {tableName}");
            sb.Append("(");
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                sb.Append($"[{dataTable.Columns[i].ColumnName}]");
                if (i == dataTable.Columns.Count-1)
                {
                    sb.Append(")");
                }
                else
                {
                    sb.Append(",");
                }
            }
            sb.Append(" Values ");
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                sb.Append("(");
                var row = dataTable.Rows[i];
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    sb.Append($"{row[dataTable.Columns[j].ColumnName].ToString().ToMSSQLValue()}");
                    if (j == dataTable.Columns.Count - 1)
                    {
                        sb.Append(")");
                    }
                    else
                    {
                        sb.Append(",");
                    }
                }
                if (i != dataTable.Rows.Count - 1)
                {
                    sb.Append(",");
                }
            }
            group.Connection.Execute(sb.ToString(), transaction: group.Transaction);
        }

        public void BulkInsertDataTable(DbGroup group, DataTable dataTable, string tableName)
        {
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(group.Connection as SqlConnection);
            sqlBulkCopy.DestinationTableName = tableName;
            if (dataTable != null && dataTable.Rows.Count != 0)
            {
                sqlBulkCopy.WriteToServer(dataTable);
            }
            sqlBulkCopy.Close();
        }

        public int Execute(DbGroup group, string sql)
        {
            return group.Connection.Execute(sql, transaction: group.Transaction);
        }



        //public IEnumerable<LabCheck> GetAll()
        //{
        //    return context.DbGroup.Connection.Query<LabCheck>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        //}
    }
}