using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface SQLEntity {
        /// <summary>
        /// 
        /// </summary>
        string GetSQL(List<Field2ValueWhere> wheres);
    }
    /// <summary>
    /// 纯SQL
    /// </summary>
    public class RawSQL : SQLEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public RawSQL(string sql)
        {
            this.SQL = sql;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SQL{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSQL(List<Field2ValueWhere> wheres)
        {
            return WebUtility.HtmlDecode(SQL);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IfSQL : SQLEntity
    {
        internal static string ElementName = "If";
        internal string Operator { set; get; }
        internal string ComponentName { set; get; }
        internal string Value { set; get; }
        internal List<SQLEntity> SQLEntities { get; } = new List<SQLEntity>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        public IfSQL(string sql)
        {
            var xItem = new XDocument(new XElement("root", XElement.Parse(sql)));
            var element = xItem.Descendants(IfCondition.ElementName).First();
            Operator = element.Attribute(nameof(Operator)).Value;
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Value = element.Attribute(nameof(Value))?.Value;

            var nestedContent = element.ToString().GetNestedContent(">", "</If>");
            var splitMatched = nestedContent.SplitByMatchesWithNested("<If", "</If>");
            if (splitMatched.Count>0)
            {
                foreach (var item in splitMatched)
                {
                    if (item.StartsWith("<If"))
                    {
                        SQLEntities.Add(new IfSQL(item));
                    }
                    else
                    {
                        SQLEntities.Add(new RawSQL(item));
                    }
                }
            }
            else
            {
                SQLEntities.Add(new RawSQL(nestedContent));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSQL(List<Field2ValueWhere> wheres)
        {
            if (Pass(wheres))
            {
                return string.Join(" ", SQLEntities.Select(c => c.GetSQL(wheres)));
            }
            else
            {
                return "";
            }
        }

        private bool Pass(List<Field2ValueWhere> wheres)
        {
            switch (Operator)
            {
                case "NotEmpty":
                    var where = wheres.FirstOrDefault(c => c.EntityName + "_" + c.FieldName == ComponentName);
                    if (where != null && !where.Value.IsNullOrEmpty())
                    {
                        return true;
                    }
                    break;
                case "eq":
                    where = wheres.FirstOrDefault(c => c.EntityName + "_" + c.FieldName == ComponentName);
                    if (where != null && where.Value == Value)
                    {
                        return true;
                    }
                    break;
                case "in":
                    where = wheres.FirstOrDefault(c => c.EntityName + "_" + c.FieldName == ComponentName);
                    if (where != null && Value.Split(",").Contains(where.Value))
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RootSQL : SQLEntity {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        public RootSQL(string sql)
        {
            var result = sql.SplitByMatchesWithNested("<If", "</If>");
            foreach (var item in result)
            {
                if (item.StartsWith("<If"))
                {
                    SQLEntities.Add(new IfSQL(item));
                }
                else
                {
                    SQLEntities.Add(new RawSQL(item));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SQLEntity> SQLEntities { get; } = new List<SQLEntity>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSQL(List<Field2ValueWhere> wheres)
        {
            return string.Join(" ", SQLEntities.Select(c => c.GetSQL(wheres)));
        }
    }

    /// <summary>
    /// 查询sql配置
    /// </summary>
    public class SQLConfigV3
    {
        #region 预设配置
        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "SQLConfig";

        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<SQLConfigV3Where> Wheres { set; get; }
        /// <summary>
        /// 页面排序项
        /// </summary>
        public List<SQLConfigV3OrderBy> OrderBys { set; get; }

        /// <summary>
        /// 预设SQL
        /// </summary>
        public RootSQL SQLEntity { set; get; }
        /// <summary>
        /// 原始SQL
        /// </summary>
        public string RawSQL { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public SQLConfigV3()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigV3(XElement element)
        {
            Wheres = element.Descendants(SQLConfigV3Where.ElementName).Select(c => new SQLConfigV3Where(c)).ToList();
            var sql = element.Descendants("SQL")?.FirstOrDefault().ToString().TrimStart("<SQL>").TrimEnd("</SQL>");
            RawSQL = sql;
            SQLEntity = new RootSQL(RawSQL);

            //SQL = WebUtility.HtmlDecode(SQL);
            //CountSQL = WebUtility.HtmlDecode(CountSQL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public SQLConfigV3(SQLConfigV3 c)
        {
            if (c == null)
                return;

            Wheres = c.Wheres.Select(c => new SQLConfigV3Where(c)).ToList();
            RawSQL = c.RawSQL;
            SQLEntity = new RootSQL(RawSQL);            
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
        private void UpdateIf(ref string sql, List<SQLConfigV3Where> wheres)
        {
            var ifItems = sql.GetMatches("<If", "</If>");
            foreach (var ifItem in ifItems)
            {
                var xItem = new XDocument(new XElement("root", XElement.Parse(ifItem)));
                var ifEntity = xItem.Descendants(IfCondition.ElementName).Select(c => new IfCondition(c)).First();
                sql = sql.Replace(ifItem, ifEntity.GetSQL(wheres));
            }
        }

        #region 分页
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
        public int Skip
        {
            get
            {
                var skip = (PageIndex - 1) * PageSize;
                return skip > 0 ? skip : 0;
            }
        }
        public int Limit
        {
            get { return PageSize > 0 ? PageSize : 10; }
        }

        internal void UpdateOrderBy(string field, string order)
        {
            foreach (var OrderBy in OrderBys)
            {
                OrderBy.IsOn = false;
            }
        }

        internal void UpdateWheres(List<VLKeyValue> wheres)
        {
            if (wheres == null)
                return;

            foreach (var where in wheres)
            {
                var whereConfig = Wheres.FirstOrDefault(c => c.ComponentName.ToLower() == where.Key.ToLower());
                if (where != null && !where.Value.IsNullOrEmpty() && whereConfig != null)
                {
                    whereConfig.IsOn = true;
                    whereConfig.Value = where.Value;
                }
            }
        }

        internal void UpdateEntitySourceName(string entitySourceName)
        {
            foreach (var where in Wheres)
            {
                where.ComponentName = entitySourceName + "_" + where.ComponentName;
            }

            var newSQL = RawSQL.Replace(@"ComponentName=""", @"ComponentName=""" + entitySourceName + "_");
            SQLEntity = new RootSQL(newSQL);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class SQLConfigV3OrderBy
    {
        /// 
        /// </summary>
        public const string RootElementName = "OrderBys";
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "OrderBy";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigV3OrderBy(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }
}
