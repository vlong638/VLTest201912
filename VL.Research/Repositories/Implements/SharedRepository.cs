using Dapper;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ExcelExportSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Consolo_Core.Common.TimeSpanSolution;
using BBee.Common;
using BBee.Models;

namespace BBee.Repositories
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
        public DataTable GetCommonSelect(VL.Consolo_Core.Common.ExcelExportSolution.SQLConfigSource config, int skip = 0, int limit = 0)
        {
            if (config.SQLs.Texts.Count == 1)
            {
                var sql = config.GetListSQL(config.SQLs.Texts.First(), skip, limit);
                var pars = config.GetParams();
                APIContraints.DHFConfig.DoLog(sql, pars);
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
                    APIContraints.DHFConfig.DoLog(sql, pars);
                    DataTable temp = null;
                    var ts = TimeSpanHelper.GetTimeSpan(() =>
                    {
                        var reader = context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: _transaction);
                        temp = new DataTable("MyTable");
                        temp.Load(reader);
                    });
                    APIContraints.DHFConfig.DoLog("执行时间:" + ts.TotalMilliseconds, null);
                    //总表初始化
                    if (table == null)
                    {
                        table = temp;
                        continue;
                    }
                    //结构整合
                    ts = TimeSpanHelper.GetTimeSpan(() =>
                    {
                        foreach (DataColumn column in temp.Columns)
                        {
                            if (!table.Columns.Contains(column.ColumnName))
                            {
                                table.Columns.Add(column.ColumnName, column.DataType);
                            }
                        }
                    });
                    APIContraints.DHFConfig.DoLog("结构整合,执行时间:" + ts.TotalMilliseconds, null);
                    //数据整合
                    ts = TimeSpanHelper.GetTimeSpan(() =>
                    {
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
                    });
                    APIContraints.DHFConfig.DoLog("数据整合,执行时间:" + ts.TotalMilliseconds, null);
                }
                return table;
            }
        }

        internal bool GenerateIndicatorsForPrematureBabyManagement()
        {
            //更新指标
            Dictionary<string, string> dics = new Dictionary<string, string>()
            {
                { "CheckAtMonth1","p.visitMonth = 1"},
                { "CheckAtMonth2","p.visitMonth = 2"},
                { "CheckAtMonth3","p.visitMonth = 3"},
                { "CheckAtMonth4","p.visitMonth = 4"},
                { "CheckAtMonth5","p.visitMonth = 5"},
                { "CheckAtMonth6","p.visitMonth = 6"},
                { "CheckAtMonth7_8","p.visitMonth >= 7 and p.visitMonth <= 8"},
                { "CheckAtMonth9_10","p.visitMonth >= 9 and p.visitMonth <= 10"},
                { "CheckAtMonth11_12","p.visitMonth >= 11 and p.visitMonth <= 12"},
                { "CheckAtMonth13_15","p.visitMonth >= 13 and p.visitMonth <= 15"},
                { "CheckAtMonth16_18","p.visitMonth >= 16 and p.visitMonth <= 18"},
                { "CheckAtMonth19_21","p.visitMonth >= 19 and p.visitMonth <= 21"},
                { "CheckAtMonth22_24","p.visitMonth >= 22 and p.visitMonth <= 24"},
            };
            foreach (var dic in dics)
            {
                context.DbGroup.Connection.Execute($@"
update a
set a.{dic.Key} = temp.cc
from Analysis_SpecialCase a
LEFT JOIN
(
	select p.ChildId,Count(1) cc from Analysis_SpecialCase a 
	left join cc_physicalexam_new p on p.ChildId = a.ChildId
	where {dic.Value} and a.FinishedAt is null
	group by p.ChildId
) temp on a.ChildId = temp.ChildId and temp.cc!=0
", transaction: context.DbGroup.Transaction);
            }
            //更新指标 指标汇总
            context.DbGroup.Connection.Execute($@"
update a 
set a.GeneratedAtMonth = datediff(dd,g.DeliveryDate,GetDate())/30 + 1
,a.GeneratedAt=GetDate()
,a.IsValidCase = (case when a.GeneratedAtMonth = 1 and a.CheckAtMonth1 >0 then 1 
when a.GeneratedAtMonth = 2 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 then 1 
when a.GeneratedAtMonth = 3 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 then 1 
when a.GeneratedAtMonth = 4 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 then 1 
when a.GeneratedAtMonth = 5 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 then 1 
when a.GeneratedAtMonth = 6 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 and a.CheckAtMonth6 >0 then 1 
when a.GeneratedAtMonth >= 7 and a.GeneratedAtMonth <= 8 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 and a.CheckAtMonth6 >0 and a.CheckAtMonth7_8 >0 then 1 
when a.GeneratedAtMonth >= 9 and a.GeneratedAtMonth <= 10 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 and a.CheckAtMonth6 >0 and a.CheckAtMonth7_8 >0 and a.CheckAtMonth9_10 >0 then 1 
when a.GeneratedAtMonth >= 11 and a.GeneratedAtMonth <= 12 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 and a.CheckAtMonth6 >0 and a.CheckAtMonth7_8 >0 and a.CheckAtMonth9_10 >0  and a.CheckAtMonth11_12 >0 then 1 
when a.GeneratedAtMonth >= 13 and a.GeneratedAtMonth <= 15 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 and a.CheckAtMonth6 >0 and a.CheckAtMonth7_8 >0 and a.CheckAtMonth9_10 >0  and a.CheckAtMonth11_12 >0  and a.CheckAtMonth13_15 >0 then 1 
when a.GeneratedAtMonth >= 16 and a.GeneratedAtMonth <= 18 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 and a.CheckAtMonth6 >0 and a.CheckAtMonth7_8 >0 and a.CheckAtMonth9_10 >0  and a.CheckAtMonth11_12 >0  and a.CheckAtMonth13_15 >0 and a.CheckAtMonth16_18 >0 then 1 
when a.GeneratedAtMonth >= 19 and a.GeneratedAtMonth <= 21 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 and a.CheckAtMonth6 >0 and a.CheckAtMonth7_8 >0 and a.CheckAtMonth9_10 >0  and a.CheckAtMonth11_12 >0  and a.CheckAtMonth13_15 >0 and a.CheckAtMonth16_18 >0 and a.CheckAtMonth19_21 >0 then 1 
when a.GeneratedAtMonth >= 22 and a.CheckAtMonth1 >0 and a.CheckAtMonth2 >0 and a.CheckAtMonth3 >0 and a.CheckAtMonth4 >0 and a.CheckAtMonth5 >0 and a.CheckAtMonth6 >0 and a.CheckAtMonth7_8 >0 and a.CheckAtMonth9_10 >0  and a.CheckAtMonth11_12 >0  and a.CheckAtMonth13_15 >0 and a.CheckAtMonth16_18 >0 and a.CheckAtMonth19_21 >0 and a.CheckAtMonth22_24 >0  then 1 
else 0 end)
from Analysis_SpecialCase a
left join cc_generalinfo g on g.childid = a.childid
where a.FinishedAt is null
", transaction: context.DbGroup.Transaction);
            //更新指标 周期边界
            context.DbGroup.Connection.Execute($@"
update a 
set a.IsValidCase = 0,a.FinishedAt = GetDate()
from Analysis_SpecialCase a
left join cc_generalinfo g on g.childid = a.childid
where datediff(dd,g.DeliveryDate,GetDate())>712
", transaction: context.DbGroup.Transaction);
            return true;
        }

        internal bool GenerateSourceForPrematureBabyManagement()
        {
            //生成完整列表
            context.DbGroup.Connection.Execute(@"
insert into Analysis_SpecialCase ([ChildId], [GeneratedAtMonth], [IsValidCase])
select a.* from
(
	select g.ChildId,null as GeneratedAtMonth,null as IsValidCase from cc_generalinfo g
	left join cc_highrisk h on h.childid = g.childid 
	left join Analysis_SpecialCase a on g.childid = a.childid
	where a.childid is null and g.childId is not null 
	and (h.highrisktype like '%01%' or h.highrisktype like '%18%')
	GROUP BY g.childid
) as a
", transaction: context.DbGroup.Transaction);
            return true;
        }
    }
}

