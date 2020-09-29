using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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