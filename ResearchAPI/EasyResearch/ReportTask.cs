using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ResearchAPI.Common
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

        public CustomBusinessEntitySet CustomBusinessEntities { set; get; } = new CustomBusinessEntitySet();
        public Routers CustomRouters { set; get; } = new Routers();

        /// <summary>
        /// 纳入标准
        /// </summary>
        public List<IWhere> MainConditions { get; set; } = new List<IWhere>();
        /// <summary>
        /// 排除标准
        /// </summary>
        public List<SQLConfigWhere> ExceptionConditions { get; set; } = new List<SQLConfigWhere>();

        internal List<Router> GetRouters(Routers routerSource)
        {
            var targets = Properties.Select(c => c.From).Distinct();
            var routers = new List<Router>();
            foreach (var target in targets)
            {
                AddRouter(ref routers, routerSource, target);
            }
            return routers;
        }

        private static void AddRouter(ref List<Router> routers, Routers routerSource, string target)
        {
            var router = routerSource.FirstOrDefault(c => c.To == target);
            if (router != null)
            {
                if (routers.Contains(router))
                    return;

                var reference = routers.FirstOrDefault(c => c.From == target);
                var index = reference != null ? routers.IndexOf(reference) : 0;
                routers.Insert(index, router);
                if (router.From != BusinessContext.Root)
                {
                    AddRouter(ref routers, routerSource, router.From);
                }
            }
        }

        internal Dictionary<string, object> GetParameters()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            //foreach (var where in MainConditions)
            //{
            //    var para = where.GetParameter();
            //    if (para != null)
            //        args.Add(para.Value.Key, para.Value.Value);
            //}
            return args;
        }

        internal string GetSQL()
        {
            var routerSource = new Routers();
            routerSource.AddRange(BusinessContext.Routers);
            routerSource.AddRange(CustomRouters);
            var customBusinessEntities = CustomBusinessEntities;
            var properties = Properties;
            var routers = GetRouters(routerSource);
            var conditions = MainConditions;
            var sql = $@"
{GetSelect(properties, GetTableAlias(routers, properties))}
{GetFrom(routers, properties, customBusinessEntities, BusinessContext.Templates)}
{GetWhere(conditions, GetTableAlias(routers, properties))}
";
            UpdateIf(ref sql, new List<SQLConfigWhere>());
            return sql;
        }

        private void UpdateIf(ref string sql, List<SQLConfigWhere> wheres)
        {
            var ifItems = sql.GetMatches("<If", "</If>");
            foreach (var ifItem in ifItems)
            {
                var xItem = new XDocument(new XElement("root", XElement.Parse(ifItem)));
                var ifEntity = xItem.Descendants(IfCondition.ElementName).Select(c => new IfCondition(c)).First();
                sql = sql.Replace(ifItem, ifEntity.GetSQL(wheres));
            }
        }

        private Dictionary<string, string> GetTableAlias(List<Router> routers, List<BusinessEntityProperty> properties)
        {
            var result = new Dictionary<string, string>();
            if (routers.Count == 0)
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

        private string GetFrom(List<Router> routers, List<BusinessEntityProperty> properties, CustomBusinessEntitySet customBusinessEntities, List<BusinessEntityTemplate> templates)
        {
            if (routers.Count == 0)
            {
                var tableName = properties.First().From;
                return "from " + tableName;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                var from = routers.First(c => c.From == BusinessContext.Root);
                sb.AppendLine($" from [{from.From}] ");
                foreach (var router in routers)
                {
                    var customBE = customBusinessEntities.FirstOrDefault(b => b.ReportName == router.To);
                    router.ToAlias = customBE.Template;
                    var template = templates.First(c => c.BusinessEntity.DisplayName == customBE.Template);
                    sb.AppendLine($" {router.RouteType.ToSQL()} ({template.SQLConfig.SQL} \r\n ) as {customBE.Template} on {string.Join(",", router.Ons.Select(o => o.ToSQL(router)))} ");
                }
                return sb.ToString();
            }
        }

        private string GetSelect(List<BusinessEntityProperty> properties, Dictionary<string, string> tableAlias)
        {
            return "select " + string.Join(",", properties.Select(c => "[" + (tableAlias[c.From] ?? c.From) + "]." + c.ColumnName));
        }
    }
}
