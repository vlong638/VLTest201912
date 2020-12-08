using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace Research.Common
{
    public class BusinessEntities: List<BusinessEntity>
    {
        #region 预设配置

        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "BusinessEntities";

        /// <summary>
        /// 
        /// </summary>
        public BusinessEntities()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public BusinessEntities(XElement element)
        {
            var businessEntities = element.Elements(BusinessEntity.ElementName);
            this.AddRange(businessEntities.Select(c => new BusinessEntity(c)));
        }

        #endregion

        public BusinessEntity GetByName(string name )
        {
            return this.FirstOrDefault(c => c.DisplayName == name);
        }
    }

    public class CustomBusinessEntitySet : List<CustomBusinessEntity>
    {
        
    }

    public class CustomBusinessEntity
    {
        public const string ElementName = "CustomBusinessEntity";

        public CustomBusinessEntity()
        {
        }
        public CustomBusinessEntity(XElement element)
        {
            ReportName = element.Attribute(nameof(ReportName))?.Value;
            Template = element.Attribute(nameof(Template))?.Value;
            Properties.AddRange(element.Descendants(BusinessEntityProperty.ElementName).Select(c => new BusinessEntityProperty(c)));
        }

        public string ReportName { set; get; }
        public string Template { set; get; }
        public List<BusinessEntityProperty> Properties { set; get; } = new List<BusinessEntityProperty>();
        public List<BusinessEntityWhere> Wheres { set; get; } = new List<BusinessEntityWhere>();
    }

    public class BusinessEntityWhere
    {
        public string ComponentName { set; get; }
        public WhereOperator Operator { set; get; }
        public string Value { set; get; }
    }


    /// <summary>
    /// 查询sql配置
    /// </summary>
    public class SQLConfig
    {
        #region 预设配置
        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "SQLConfig";

        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<SQLConfigWhere> Wheres { set; get; }
        /// <summary>
        /// 预设SQL
        /// </summary>
        public string SQL { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public SQLConfig()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfig(XElement element)
        {
            Wheres = element.Descendants(SQLConfigWhere.ElementName).Select(c => new SQLConfigWhere(c)).ToList();
            SQL = element.Descendants(nameof(SQL))?.FirstOrDefault().ToString().TrimStart("<SQL>").TrimEnd("</SQL>");
  
            //SQL = WebUtility.HtmlDecode(SQL);
            //CountSQL = WebUtility.HtmlDecode(CountSQL);
        }
        #endregion

        public string GetListSQL(string sql, int skip = 0, int limit = 0)
        {
            //Where
            UpdateIf(ref sql, Wheres);
            sql = WebUtility.HtmlDecode(sql);
            var wheresIsOn = Wheres.Where(c => c.IsOn).Select(c => c.SQL);
            var wheres = wheresIsOn.Count() == 0 ? "" : $"where {string.Join(" and ", wheresIsOn)}";
            sql = sql.Replace("@Wheres", wheres);
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
    }

    public class IfCondition
    {
        public static string ElementName = "If";
        public string Operator { set; get; }
        public string ComponentName { set; get; }
        public string Value { set; get; }
        public string Text { set; get; }

        public IfCondition(XElement element)
        {
            Operator = element.Attribute(nameof(Operator)).Value;
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Value = element.Attribute(nameof(Value))?.Value;
            Text = WebUtility.HtmlDecode(element.Value);
        }

        internal string GetSQL(List<SQLConfigWhere> wheres)
        {
            switch (Operator)
            {
                case "NotEmpty":
                    var where = wheres.FirstOrDefault(c => c.ComponentName == ComponentName);
                    if (where != null && !where.Value.IsNullOrEmpty())
                    {
                        return Text;
                    }
                    break;
                case "eq":
                    where = wheres.FirstOrDefault(c => c.ComponentName == ComponentName);
                    if (where != null && where.Value == Value)
                    {
                        return Text;
                    }
                    break;
                default:
                    break;
            }
            return "";
        }

        internal string GetSQL(Dictionary<string, object> parameters)
        {
            var where = parameters.FirstOrDefault(c => c.Key == ComponentName);
            if (where.Key != null)
            {
                return Text;
            }
            return "";
        }
    }

    public class SQLConfigWhere
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Where";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigWhere(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Formatter = element.Attribute(nameof(Formatter))?.Value;
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? false;
            Required = element.Attribute(nameof(Required))?.Value.ToBool() ?? false;
            SQL = element.Value;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 是否必须
        /// </summary>
        public bool Required { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string SQL { set; get; }
        /// <summary>
        /// 格式化
        /// </summary>
        public string Formatter { get; set; }
    }

    public class BusinessEntity
    {
        public const string ElementName = "BusinessEntity";

        public BusinessEntity()
        {
        }
        public BusinessEntity(XElement element)
        {
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            Template = element.Attribute(nameof(Template))?.Value;
            Properties.AddRange(element.Descendants(BusinessEntityProperty.ElementName).Select(c => new BusinessEntityProperty(c)));
        }

        public string DisplayName { set; get; }
        public List<BusinessEntityProperty> Properties { set; get; } = new List<BusinessEntityProperty>();
        public string Template { get; set; }
        public SQLConfig SQLConfig { get; internal set; }
    }
    public class BusinessEntityProperty
    {
        public const string ElementName = "Property";

        public BusinessEntityProperty()
        {
        }
        public BusinessEntityProperty(XElement element)
        {
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            From = element.Attribute(nameof(From))?.Value;
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
        }

        public BusinessEntityProperty(string displayName, string from, string columnName)
        {
            DisplayName = displayName;
            From = from;
            ColumnName = columnName;
        }

        public string DisplayName { set; get; }
        public string From { set; get; }
        public string ColumnName { set; get; }
    }
}
