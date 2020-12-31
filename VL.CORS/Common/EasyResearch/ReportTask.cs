using Autobots.Infrastracture.Common.ValuesSolution;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class BusinessEntityTemplate
    {
        public const string ElementName = "BusinessEntityTemplate";

        public BusinessEntityTemplate()
        {
        }
        public BusinessEntityTemplate(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            ConnectionString = element.Attribute(nameof(ConnectionString))?.Value;
            BusinessEntity = new COBusinessEntity(element.Element(COBusinessEntity.ElementName));
            SQLConfig = new SQLConfigV3(element.Descendants(SQLConfigV3.ElementName).First());
            Router = new Router(element.Element(Router.ElementName));
        }

        public long Id { set; get; }
        public string ConnectionString { set; get; }
        public COBusinessEntity BusinessEntity { set; get; }
        public SQLConfigV3 SQLConfig { set; get; }
        public Router Router { set; get; }
    }

    public class BusinessContext
    {
        public static string Root = "PregnantInfo";
    }

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
        public long ProjectId { set; get; }
        public string Name { set; get; }
        public List<COBusinessEntityProperty> Properties { get; set; } = new List<COBusinessEntityProperty>();
        public COCustomBusinessEntitySet CustomBusinessEntities { set; get; } = new COCustomBusinessEntitySet();
        public Routers Routers { set; get; } = new Routers();

        /// <summary>
        /// 纳入标准
        /// </summary>
        public List<Field2ValueWhere> Conditions { get; set; } = new List<Field2ValueWhere>();
        public List<Field2ValueWhere> TemplateConditions { get; set; } = new List<Field2ValueWhere>();
        /// <summary>
        /// 排除标准
        /// </summary>
        public List<SQLConfigV3Where> ExceptionConditions { get; set; } = new List<SQLConfigV3Where>();
        public List<BusinessEntityTemplate> Templates { get; internal set; } = new List<BusinessEntityTemplate>();

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
            foreach (var item in Conditions)
            {
                args.Add(item.EntityName + "_" + item.FieldName, item.Value);
            }
            foreach (var item in TemplateConditions)
            {
                args.Add(item.EntityName + "_" + item.FieldName, item.Value);
            }
            return args;
        }

        internal string GetSQL()
        {
            var customBusinessEntities = CustomBusinessEntities;
            var properties = Properties;
            var routers = Routers;
            var conditions = Conditions;
            var templates = Templates;
            var sql = $@"
{GetSelect(properties)}
{GetFrom(routers, properties, customBusinessEntities, templates)}
{GetWhere(conditions)}
{GetGroupBy(properties)}
";
            UpdateIf(ref sql, TemplateConditions);
            return sql;
        }

        private void UpdateIf(ref string sql, List<Field2ValueWhere> wheres)
        {
            var ifItems = sql.GetMatches("<If", "</If>");
            foreach (var ifItem in ifItems)
            {
                var xItem = new XDocument(new XElement("root", XElement.Parse(ifItem)));
                var ifEntity = xItem.Descendants(IfCondition.ElementName).Select(c => new IfCondition(c)).First();
                sql = sql.Replace(ifItem, ifEntity.GetSQL(wheres));
            }
        }

        private Dictionary<string, string> GetTableAlias(List<Router> routers, List<COBusinessEntityProperty> properties)
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
        private string GetWhere(List<Field2ValueWhere> conditions)
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
                    var sql = condition.ToSQL();
                    sb.Append(" and " + sql);
                }
                return sb.ToString();
            }
        }

        private string GetFrom(List<Router> routers, List<COBusinessEntityProperty> properties, COCustomBusinessEntitySet customBusinessEntities, List<BusinessEntityTemplate> templates)
        {
            if (routers.Count == 0)
            {
                var tableName = properties.First().From;
                return "from " + tableName;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                var root = "PregnantInfo";
                sb.AppendLine($" from [{root}] ");
                AppendRoute(sb, routers, root, templates);
                return sb.ToString();
            }
        }

        private void AppendRoute(StringBuilder sb, List<Router> routers, string fromName, List<BusinessEntityTemplate> templates)
        {
            var tos = routers.Where(c => c.From == fromName);
            if (tos == null)
                return;
            foreach (var item in tos)
            {
                if (item.IsFromTemplate)
                {
                    var template = templates.FirstOrDefault(c => c.Id == item.To.TrimStart("t").ToLong().Value);
                    sb.AppendLine($"left join ({template.SQLConfig.SQL.Replace("@","@"+item.To+"_")})as [{item.To}] on {string.Join(",", item.Ons.Select(o => $"[{item.From}].{o.FromField} = [{item.To}].{o.ToField}"))} ");
                }
                else
                {
                    sb.AppendLine($"left join [{item.To}] on {string.Join(",", item.Ons.Select(o => $"[{item.From}].{o.FromField} = [{item.To}].{o.ToField}"))} ");
                }
                AppendRoute(sb, routers, item.To, templates);
            }
        }

        private string GetSelect(List<COBusinessEntityProperty> properties)
        {
            return "select " + string.Join(",", properties.Select(c => $"[{c.From}].{c.SourceName} as {c.From}_{c.SourceName}"));
        }

        private string GetGroupBy(List<COBusinessEntityProperty> properties)
        {
            return "group by " + string.Join(",", properties.Select(c => $"[{c.From}].{c.SourceName}"));
        }

        public void Update(List<ProjectIndicator> projectIndicators, List<ProjectTaskWhere> taskWheres, List<CustomBusinessEntity> customBusinessEntities, List<CustomBusinessEntityWhere> customBusinessEntityWheres, Routers routers, List<BusinessEntityTemplate> templates, ReportTask reportTask)
        {
            foreach (var entityName in projectIndicators.Select(c => c.EntitySourceName).Distinct())
            {
                var router = routers.FirstOrDefault(c => c.To == entityName);
                if (router != null)
                {
                    reportTask.Routers.Add(router);
                }
                else
                {
                    var template = templates.FirstOrDefault(c => "t" + c.Id == entityName);
                    if (template != null)
                    {
                        router = template.Router;
                        router.To = entityName;
                        router.IsFromTemplate = true;
                        reportTask.Routers.Add(router);
                    }
                }
            }
            var templateIds = customBusinessEntities.Select(c => c.TemplateId);
            reportTask.Templates.AddRange(templates.Where(c => templateIds.Contains(c.Id)));
            reportTask.Properties.AddRange(projectIndicators.Select(c => new COBusinessEntityProperty()
            {
                SourceName = c.PropertySourceName,
                DisplayName = c.PropertyDisplayName,
                From = c.EntitySourceName,
            }));
            reportTask.Conditions.AddRange(taskWheres.Select(c => new Field2ValueWhere()
            {
                EntityName = c.EntityName,
                FieldName = c.PropertyName,
                Operator = (WhereOperator)Enum.Parse(typeof(WhereOperator), ((int)c.Operator).ToString()),
                Value = c.Value,
            }));
            reportTask.TemplateConditions.AddRange(customBusinessEntityWheres.Select(c => new Field2ValueWhere()
            {
                EntityName = customBusinessEntities.First(d => d.Id == c.BusinessEntityId).Name,
                FieldName = c.ComponentName,
                Value = c.Value,
            }));
        }
    }



    public class Routers : List<Router>
    {
        #region 预设配置

        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "Routers";

        /// <summary>
        /// 
        /// </summary>
        public Routers()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public Routers(XElement element)
        {
            var routers = element.Elements(Router.ElementName);
            this.AddRange(routers.Select(c => new Router(c)));
        }

        #endregion
    }

    public class Router
    {
        public const string ElementName = "Router";

        public Router()
        {
        }
        public Router(XElement element)
        {
            From = element.Attribute(nameof(From))?.Value;
            FromAlias = element.Attribute(nameof(FromAlias))?.Value;
            To = element.Attribute(nameof(To))?.Value;
            ToAlias = element.Attribute(nameof(ToAlias))?.Value;
            RouteType = element.Attribute(nameof(RouteType))?.Value.ToEnum<RouteType>() ?? RouteType.None;
            Ons = element.Descendants(RouterOn.ElementName).Select(c => new RouterOn(c)).ToList();
        }

        public string From { set; get; }
        public string FromAlias { set; get; }
        public string To { set; get; }
        public string ToAlias { set; get; }
        public RouteType RouteType { set; get; }
        public List<RouterOn> Ons { set; get; } = new List<RouterOn>();
        public bool IsFromTemplate { get; set; }

        internal string GetSQL()
        {
            throw new NotImplementedException();
        }
    }

    public class RouterOn
    {
        public const string ElementName = "On";

        public RouterOn()
        {
        }
        public RouterOn(XElement element)
        {
            FromField = element.Attribute(nameof(FromField))?.Value;
            ToField = element.Attribute(nameof(ToField))?.Value;
        }

        public RouterOn(string fromField, string toField)
        {
            FromField = fromField;
            ToField = toField;
        }

        public string FromField { set; get; }
        public string ToField { set; get; }

        internal string ToSQL(Router c)
        {
            return $@"{c.FromAlias}.{FromField} = {c.ToAlias}.{ToField}";
        }
    }
}
