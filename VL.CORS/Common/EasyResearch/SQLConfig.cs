using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace ResearchAPI.Common
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
}
