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

        internal BusinessEntityTemplate Clone()
        {
            return new BusinessEntityTemplate()
            {
                Id = this.Id,
                ConnectionString = this.ConnectionString,
                BusinessEntity = new COBusinessEntity(this.BusinessEntity),
                Router = this.Router,
                SQLConfig = new SQLConfigV3(this.SQLConfig),
            };
        }
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
        /// <summary>
        /// 纳入标准V2
        /// </summary>
        public BOGroupedCondition GroupedCondition { get; set; }
        
        /// <summary>
        /// 模板用筛选条件
        /// </summary>
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
                switch (item.Operator)
                {
                    case WhereOperator.None:
                        break;
                    case WhereOperator.Like:
                        args.Add(item.GetParameterName(), $"%{item.Value}%");
                        break;
                    case WhereOperator.Equal:
                    case WhereOperator.IsNotNull:
                    case WhereOperator.IsNull:
                    case WhereOperator.GreatThan:
                    case WhereOperator.LessThan:
                    case WhereOperator.GreatOrEqualThan:
                    case WhereOperator.LessOrEqualThan:
                    default:
                        args.Add(item.GetParameterName(), item.Value);
                        break;
                }
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
            var wheres = GroupedCondition;
            var templates = Templates;
            var sql = $@"
{GetSelect(properties)}
{GetFrom(routers, properties, customBusinessEntities, templates, TemplateConditions)}
{GetWhere(wheres)}
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

        private string GetWhere(BOGroupedCondition conditions)
        {
            if (conditions == null)
            {
                return "";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("where 1=1 ");
                sb.Append("and " + conditions.GetSQL());
                return sb.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TempTable> TempTables { set; get; } = new List<TempTable>();

        private string GetFrom(List<Router> routers, List<COBusinessEntityProperty> properties, COCustomBusinessEntitySet customBusinessEntities, List<BusinessEntityTemplate> templates, List<Field2ValueWhere> templateWheres)
        {
            if (routers.Count == 0)
            {
                var tableName = properties.First().From;
                return "from " + tableName;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                //var root = "PregnantInfo";
                var root = "PF_GeneralInfo";
                sb.AppendLine($" from [{root}] ");
                AppendRoute(sb, routers, root, templates, templateWheres);
                return sb.ToString();
            }
        }

        private void AppendRoute(StringBuilder sb, List<Router> routers, string fromName, List<BusinessEntityTemplate> templates, List<Field2ValueWhere> templateWheres)
        {
            var tos = routers.Where(c => c.From == fromName);
            if (tos == null)
                return;
            foreach (var item in tos)
            {
                if (item.TemplateId > 0)
                {
                    var template = templates.FirstOrDefault(c => c.Id == item.TemplateId && c.BusinessEntity.SourceName == item.To);
                    var alias = "temp_" + item.To + "_" + Guid.NewGuid().ToString().Replace('-', '_'); //加GUID避免临时表撞表
                    var tempTalbe = new TempTable()
                    {
                        Alias = alias, 
                        SQL = $"select t.* into {alias} from ({ template.SQLConfig.SQLEntity.GetSQL(templateWheres).Replace("@", "@" + item.To + "_") }) as t"
                    };
                    TempTables.Add(tempTalbe);
                    sb.AppendLine($"left join {tempTalbe.Alias} as [{item.To}] on {string.Join(" and ", item.Ons.Select(o => $"[{item.From}].{o.FromField} = [{item.To}].{o.ToField}"))} ");
                }
                else
                {
                    sb.AppendLine($"left join [{item.To}] on {string.Join(" and ", item.Ons.Select(o => $"[{item.From}].{o.FromField} = [{item.To}].{o.ToField}"))} ");
                }
                AppendRoute(sb, routers, item.To, templates, templateWheres);
            }
        }

        private string GetSelect(List<COBusinessEntityProperty> properties)
        {
            return "select " + string.Join(",", properties.Select(c => $"[{c.From}].{c.SourceName} as {c.GetFieldAlias()}"));
        }

        private string GetGroupBy(List<COBusinessEntityProperty> properties)
        {
            return "group by " + string.Join(",", properties.Select(c => $"[{c.From}].{c.SourceName}"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectIndicators"></param>
        /// <param name="taskWheres"></param>
        /// <param name="groupedCondition"></param>
        /// <param name="customBusinessEntities"></param>
        /// <param name="customBusinessEntityWheres"></param>
        /// <param name="routers"></param>
        /// <param name="templates"></param>
        /// <param name="reportTask"></param>
        public void Update(List<ProjectIndicator> projectIndicators, List<ProjectTaskWhere> taskWheres, BOGroupedCondition groupedCondition, List<CustomBusinessEntity> customBusinessEntities, List<CustomBusinessEntityWhere> customBusinessEntityWheres, Routers routers, List<BusinessEntityTemplate> templates, ReportTask reportTask)
        {
            List<long> businessEntityIds = new List<long>();
            foreach (var item in projectIndicators)
            {
                var custom = customBusinessEntities.FirstOrDefault(c => c.Id == item.BusinessEntityId);
                if (custom != null)
                {
                    if (!businessEntityIds.Exists(c => c == item.BusinessEntityId))
                    {
                        var orientTemplate = templates.FirstOrDefault(c => c.Id == custom.TemplateId);
                        if (orientTemplate != null)
                        {
                            //Router
                            var template = orientTemplate.Clone();
                            var router = new Router();
                            template.Router.MapTo(router);
                            var entitySourceName = item.GetUniqueEntitySourceName();
                            router.To = entitySourceName;

                            template.BusinessEntity.SourceName = entitySourceName;
                            template.SQLConfig.UpdateEntitySourceName(entitySourceName);

                            router.TemplateId = template.Id;
                            if (!reportTask.Routers.Contains(router))
                                reportTask.Routers.Add(router);

                            //Tempalte
                            reportTask.Templates.Add(template);
                        }
                    }
                    //Property
                    reportTask.Properties.Add(new COBusinessEntityProperty()
                    {
                        SourceName = item.PropertySourceName,
                        DisplayName = item.PropertyDisplayName,
                        From = item.GetUniqueEntitySourceName(),
                    });
                }
                else
                {
                    //Router
                    var router = routers.FirstOrDefault(c => c.To == item.EntitySourceName);
                    if (router != null && !reportTask.Routers.Contains(router))
                        reportTask.Routers.Add(router);
                    //Property
                    reportTask.Properties.Add(new COBusinessEntityProperty()
                    {
                        SourceName = item.PropertySourceName,
                        DisplayName = item.PropertyDisplayName,
                        From = item.EntitySourceName,
                    });
                }
                businessEntityIds.Add(item.BusinessEntityId);
            }

            reportTask.GroupedCondition = groupedCondition;
            reportTask.Conditions.AddRange(taskWheres.Select(c => new Field2ValueWhere()
            {
                EntityName = c.EntityName,
                FieldName = c.PropertyName,
                Operator = (WhereOperator)Enum.Parse(typeof(WhereOperator), ((int)c.Operator).ToString()),
                Value = c.Value,
            }));
            reportTask.TemplateConditions.AddRange(customBusinessEntityWheres.Select(c => new Field2ValueWhere()
            {
                EntityName = customBusinessEntities.First(d => d.Id == c.BusinessEntityId).Id.ToString(),
                FieldName = c.ComponentName,
                Value = c.Value,
            }));

        }
    }

    public class TempTable
    {
        public string Alias { set; get; }
        public string SQL { set; get; }
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
            To = element.Attribute(nameof(To))?.Value;
            RouteType = element.Attribute(nameof(RouteType))?.Value.ToEnum<RouteType>() ?? RouteType.None;
            Ons = element.Descendants(RouterOn.ElementName).Select(c => new RouterOn(c)).ToList();
        }

        public string From { set; get; }
        public string To { set; get; }
        public RouteType RouteType { set; get; }
        public List<RouterOn> Ons { set; get; } = new List<RouterOn>();
        public long TemplateId { get; set; }

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
    }
}
