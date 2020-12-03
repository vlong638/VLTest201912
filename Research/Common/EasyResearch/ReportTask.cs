using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Research.Common
{
    ///// <summary>
    ///// 数据集单元集合
    ///// </summary>
    //public class BusinessEntitySet
    //{
    //    public BusinessEntitySet(string displayName)
    //    {
    //        DisplayName = displayName;
    //    }

    //    public string DisplayName { get; set; }
    //    public List<BusinessEntity> Properties { set; get; }
    //}
    /// <summary>
    /// 报告单元
    /// </summary>
    public class ReportTask
    {
        public ReportTask(string reportName)
        {
            Name = reportName;
        }

        public long Id { set; get; }
        public string Name { set; get; }
        public List<BusinessEntityProperty> Properties { get; set; } = new List<BusinessEntityProperty>();
        /// <summary>
        /// 纳入标准
        /// </summary>
        public List<IWhere> MainConditions { get; set; } = new List<IWhere>();
        /// <summary>
        /// 排除标准
        /// </summary>
        public List<IWhere> ExceptionConditions { get; set; } = new List<IWhere>();

        internal List<Router> GetRouters()
        {
            var targets = Properties.Select(c => c.From).Distinct();
            var routers = new List<Router>();
            foreach (var target in targets)
            {
                AddRouter(ref routers, target);
            }
            return routers;
        }

        private static void AddRouter(ref List<Router> routers, string target)
        {
            var router = BusinessContext.Routers.FirstOrDefault(c => c.To == target);
            if (router != null)
            {
                if (routers.Contains(router))
                    return;

                var reference = routers.FirstOrDefault(c => c.From == target) ;
                var index = reference != null ? routers.IndexOf(reference) : 0;
                routers.Insert(index, router);
                if (router.From!= BusinessContext.Root)
                {
                    AddRouter(ref routers, router.From);
                }
            }
        }

        internal Dictionary<string, object> GetParameters()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            foreach (var where in MainConditions)
            {
                var para = where.GetParameter();
                if (para != null)
                    args.Add(para.Value.Key, para.Value.Value);
            }
            return args;
        }

        internal string GetSQL()
        {
            var properties = Properties;
            var routers = GetRouters();
            var conditions = MainConditions;
            var sql = $@"
{GetSelect(properties, GetTableAlias(routers, properties))}
{GetFrom(routers, properties)}
{GetWhere(conditions, GetTableAlias(routers, properties))}
";
            return sql;
        }

        private Dictionary<string, string> GetTableAlias(List<Router> routers, List<BusinessEntityProperty> properties)
        {
            var result = new Dictionary<string, string>();
            if (routers.Count==0)
            {
                var tableName = properties.First().From;
                result.Add(tableName, tableName);
            }
            else
            {
                foreach (var router in routers)
                {
                    if (!result.ContainsKey(router.From))
                    {
                        result.Add(router.From, router.FromAlias);
                    }
                    if (!result.ContainsKey(router.To))
                    {
                        result.Add(router.To, router.ToAlias);
                    }
                }
            }
            return result;
        }
        private string GetWhere(List<IWhere> conditions, Dictionary<string, string> tableAlias)
        {
            if (conditions.Count == 0)
            {
                return "";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("where 1=1 ");
                foreach (var condition in conditions)
                {
                    var sql = condition.ToSQL(tableAlias);
                    sb.Append(" and " + sql);
                }
                return sb.ToString();
            }
        }

        private string GetFrom(List<Router> routers, List<BusinessEntityProperty> properties)
        {
            if (routers.Count==0)
            {
                var tableName = properties.First().From;
                return "from " + tableName;
            }
            else
            {
                var from = routers.First(c => c.From == BusinessContext.Root);
                var joins = routers.Select(c => $@"{c.RouteType.ToSQL()} {c.To} {c.ToAlias} on {string.Join(" and ", c.Ons.Select(o => o.ToSQL(c)))}");
                return $" from {from.From} {from.FromAlias} \r\n {string.Join("\r\n", joins)}";
            }
        }

        private string GetSelect(List<BusinessEntityProperty> properties, Dictionary<string, string> tableAlias)
        {
            return "select "+ string.Join(",", properties.Select(c => tableAlias[c.From] + "." + c.ColumnName));
        }
    }
}
