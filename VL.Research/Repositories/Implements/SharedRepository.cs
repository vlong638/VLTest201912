using Dapper;
using NPOI.SS.Formula.Functions;
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetCommonSelect(SQLConfig sqlConfig)
        //{
        //    var sql = sqlConfig.GetListSQL();
        //    var pars = sqlConfig.GetParams();
        //    DataTable table = new DataTable("MyTable");
        //    var reader = context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: _transaction);
        //    table.Load(reader);
        //    return table;
        //}
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
        public DataTable GetCommonSelect(Consolo_Core.Common.ExcelExportSolution.SQLConfigSource config, int skip = 0, int limit = 0)
        {
            if (config.SQLs.Texts.Count == 1)
            {
                var sql = config.GetListSQL(config.SQLs.Texts.First(), skip, limit);
                var pars = config.GetParams();
                DataTable table = new DataTable("MyTable");
                var reader = context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: _transaction);
                table.Load(reader);
                return table;
            }
            else
            {
                DataTable table = null;
                foreach (var text in config.SQLs.Texts)
                {
                    var sql = config.GetListSQL(text, skip, limit);
                    var pars = config.GetParams();
                    var reader = context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: _transaction);
                    DataTable temp = new DataTable("MyTable");
                    temp.Load(reader);

                    //总表初始化
                    if (table == null)
                    {
                        table = temp;
                        continue;
                    }
                    //结构整合
                    foreach (DataColumn column in temp.Columns)
                    {
                        if (!table.Columns.Contains(column.ColumnName))
                        {
                            table.Columns.Add(column.ColumnName, column.DataType);
                        }
                    }
                    //数据整合
                    for (int j = 0; j < temp.Rows.Count; j++)
                    {
                        var tempRow = temp.Rows[j];
                        var tempKey = tempRow[config.SQLs.UnitedBy].ToString();
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var mainRow = table.Rows[i];
                            var mainKey = mainRow[config.SQLs.UnitedBy].ToString();
                            if (tempKey == mainKey)
                            {
                                foreach (DataColumn column in temp.Columns)
                                {
                                    mainRow[column.ColumnName] = tempRow[column.ColumnName];
                                }
                                break;
                            }
                        }
                    }
                }
                return table;
            }
        }

    }
}