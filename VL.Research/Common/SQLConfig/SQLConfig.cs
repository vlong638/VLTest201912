using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ExcelExportSolution;
using VL.Consolo_Core.Common.ValuesSolution;
using BBee.Models;

namespace BBee.Common
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
        /// 
        /// </summary>
        public SQLConfigSource Source { set; get; }

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
            ViewName = element.Attribute(nameof(ViewName))?.Value;
            Source = element.Descendants(SQLConfigSource.ElementName).Select(c => new SQLConfigSource(c)).First();
        }
        #endregion

        #region 动态更新
        internal void UpdateOrderBy(string field, string order)
        {
            var tempField = field.IsNullOrEmpty() ? Source.DefaultComponentName : field;
            var tempOrder = field.IsNullOrEmpty() ? Source.DefaultOrder : order;
            foreach (var OrderBy in Source.OrderBys)
            {
                OrderBy.IsOn = false;

                if (tempField == OrderBy.ComponentName)
                {
                    OrderBy.IsOn = true;
                    OrderBy.IsAsc = tempOrder == "asc";
                }
            }
        }
        internal void UpdateWheres(List<VLKeyValue> wheres)
        {
            if (wheres == null)
                return;

            foreach (var where in wheres)
            {
                var whereConfig = Source.Wheres.FirstOrDefault(c => c.ComponentName.ToLower() == where.Key.ToLower());
                if (where != null && !where.Value.IsNullOrEmpty() && whereConfig != null)
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

        //internal string GetListSQL()
        //{
        //    return Source.GetListSQL(Skip, Limit);
        //}

        internal string GetCountSQL()
        {
            return Source.GetCountSQL();
        }

        internal Dictionary<string, object> GetParams()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            foreach (var where in Source.Wheres)
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
            if (where.Value.IsNullOrEmpty())
            {
                return where.Value;
            }
            return where.Value.ToString();
        }

        internal string CheckWheres(GetCommonSelectRequest request)
        {
            return Source.CheckWheres(request.search);
        }

        #endregion
    }
}
