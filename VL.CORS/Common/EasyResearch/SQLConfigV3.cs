using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
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
        public string SQL { set; get; }

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
