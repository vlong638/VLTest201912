using Dapper;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ExcelExportSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Research.Common;
using VL.Research.Models;

namespace VL.Research.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class SharedRepository : Repository<object>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SharedRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetCommonSelect(SQLConfig sqlConfig)
        {
            var sql = sqlConfig.GetListSQL();
            var pars = sqlConfig.GetParams();
            DataTable table = new DataTable("MyTable");
            var reader = context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: _transaction);
            table.Load(reader);
            return table;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCommonSelectCount(SQLConfig sqlConfig)
        {
            var sql = sqlConfig.GetCountSQL();
            var pars = sqlConfig.GetParams();
            return context.DbGroup.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetCommonSelect(Consolo_Core.Common.ExcelExportSolution.SQLConfigSource config)
        {
            var sql = config.GetListSQL();
            var pars = config.GetParams();
            DataTable table = new DataTable("MyTable");
            var reader = context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: _transaction);
            table.Load(reader);
            return table;
        }

    }
}