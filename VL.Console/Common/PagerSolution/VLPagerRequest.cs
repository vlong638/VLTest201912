using System.Collections.Generic;
using System.Linq;

namespace VL.Consolo_Core.Common.PagerSolution
{
    /// <summary>
    /// 分页入参规范
    /// </summary>
    public class VLPagerRequest
    {
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

        public string GetLimitCondition()
        {
            return $"offset {Skip} rows fetch next {Limit} rows only;";
        }
        #endregion

        #region 排序
        /// <summary>
        /// 排序 true：asc,false:desc
        /// </summary>
        public Dictionary<string, bool> Orders { get; set; } = new Dictionary<string, bool>();

        public string GetOrderCondition()
        {
            if (Orders.Count == 0)
                return "";
            return "order by " + string.Join(",", Orders.Keys.Select(c => c + " " + (Orders[c] ? "asc" : "desc")));
        }
        #endregion
    }
}
