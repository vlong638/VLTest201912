using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.Research.Models;

namespace VL.Research.Common
{
    /// <summary>
    /// 查询sql配置
    /// </summary>
    public class SQLConfig
    {
        #region 预设配置
        /// <summary>
        /// 
        /// </summary>
        public static string RootElementName = "Views";
        /// <summary>
        /// 
        /// </summary>
        public static string NodeElementName = "View";

        /// <summary>
        /// 页面名称
        /// </summary>
        public string ViewName { set; get; }

        /// <summary>
        /// 页面字段
        /// </summary>
        public List<SQLConfigProperty> Properties { set; get; }
        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<SQLConfigWhere> Wheres { set; get; }
        /// <summary>
        /// 页面排序项
        /// </summary>
        public List<SQLConfigOrderBy> OrderBys { set; get; }
        /// <summary>
        /// 默认排序项
        /// </summary>
        public string DefaultComponentName { get; private set; }
        /// <summary>
        /// 默认排序项
        /// </summary>
        public string DefaultOrder { get; private set; }
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
            ViewName = element.Attribute(nameof(ViewName)).Value;
            Properties = element.Descendants(SQLConfigProperty.ElementName).Select(c => new SQLConfigProperty(c)).ToList();
            Wheres = element.Descendants(SQLConfigWhere.ElementName).Select(c => new SQLConfigWhere(c)).ToList();
            OrderBys = element.Descendants(SQLConfigOrderBy.ElementName).Select(c => new SQLConfigOrderBy(c)).ToList();
            SQL = element.Descendants("SQL").First().Value;
            var OrderBysRoot = element.Descendants(SQLConfigOrderBy.RootElementName).First();
            DefaultComponentName = OrderBysRoot.Attribute(nameof(DefaultComponentName))?.Value ?? "";
            DefaultOrder = OrderBysRoot.Attribute(nameof(DefaultOrder))?.Value ?? "";
        }
        #endregion

        #region 动态更新
        internal void UpdateOrderBy(string field, string order)
        {
            var tempField = field.IsNullOrEmpty() ? DefaultComponentName : field;
            var tempOrder = field.IsNullOrEmpty() ? DefaultOrder : order;
            foreach (var OrderBy in OrderBys)
            {
                OrderBy.IsOn = false;

                if (tempField == OrderBy.ComponentName)
                {
                    OrderBy.IsOn = true;
                    OrderBy.IsAsc = tempOrder == "asc";
                }
            }
        }
        internal void UpdateWheres(List<KeyValue> wheres)
        {
            if (wheres == null)
                return;

            foreach (var where in wheres)
            {
                var whereConfig = Wheres.FirstOrDefault(c => c.ComponentName.ToLower() == where.Key.ToLower());
                if (!where.Value.IsNullOrEmpty())
                {
                    whereConfig.IsOn = true;
                    whereConfig.Value = where.Value;
                }
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

        #endregion
        #endregion

        #region 结果使用

        internal string GetListSQL()
        {
            var sql = SQL;
            var propertiesIsOn = Properties.Where(c => c.IsOn).Select(c => c.Alias);
            var fields = propertiesIsOn.Count() == 0 ? "*" : string.Join(",", propertiesIsOn);
            sql = sql.Replace("@Fields", fields);
            var wheresIsOn = Wheres.Where(c => c.IsOn).Select(c => c.SQL);
            var wheres = wheresIsOn.Count() == 0 ? "" : $"where {string.Join(" and ", wheresIsOn)}";
            sql = sql.Replace("@Wheres", wheres);
            var orderByIsOn = OrderBys.FirstOrDefault(c => c.IsOn) ?? OrderBys.First();
            var orderBy = orderByIsOn.Alias;
            var order = orderByIsOn.IsAsc ? "asc" : "desc";
            sql = sql.Replace("@OrderBy", $"order by {orderBy} {order}");
            sql = sql.Replace("@Pager", $"offset {Skip} rows fetch next {Limit} rows only");
            return sql;
        }

        internal string GetCountSQL()
        {
            var sql = SQL;
            sql = sql.Replace("@Fields", "count(*)");
            var wheresIsOn = Wheres.Where(c => c.IsOn).Select(c => c.SQL);
            var wheres = wheresIsOn.Count() == 0 ? "" : $"where {string.Join(",", wheresIsOn)}";
            sql = sql.Replace("@Wheres", wheres);
            sql = sql.Replace("@OrderBy", $"");
            sql = sql.Replace("@Pager", $"");
            return sql;
        }

        internal Dictionary<string, object> GetParams()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            foreach (var where in Wheres)
            {
                if (where.IsOn)
                {
                    args.Add(where.ComponentName, where.Formatter.IsNullOrEmpty() ? where.Value : where.Formatter.Replace("@" + where.ComponentName, GetFormattedValue(where)));
                }
            }
            return args;
        }

        private static string GetFormattedValue(SQLConfigWhere where)
        {
            return where.Value.ToString();
        }

        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SQLConfigProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Property";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigProperty(XElement element)
        {
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? false;
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 查询用名称
        /// </summary>
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(IsOn), IsOn.ToString());
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }
    /// <summary>
    /// 
    /// </summary>
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
            SQL = element.Value;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { set; get; }
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Formatter), Formatter);
            property.SetValue(SQL);
            return property;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SQLConfigOrderBy
    {
        /// <summary>
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
        public SQLConfigOrderBy(XElement element)
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
