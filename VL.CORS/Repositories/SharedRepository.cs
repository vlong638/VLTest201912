using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
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
        public int GetCommonSelectCount(SQLConfigV2 sqlConfig)
        {
            var sql = sqlConfig.GetCountSQL();
            var pars = sqlConfig.GetParams();
            return context.DbGroup.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetCommonSelect(SQLConfigV2Source config, int skip = 0, int limit = 0)
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
                    DataTable temp = null;
                    var reader = context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: _transaction);
                    temp = new DataTable("MyTable");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, Dictionary<string, object> parameters)
        {
            DataTable table = new DataTable("MyTable");
            var reader = context.DbGroup.Connection.ExecuteReader(sql, parameters, transaction: _transaction);
            table.Load(reader);
            return table;
        }
    }
}