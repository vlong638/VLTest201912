using System.Collections.Generic;

namespace VL.API.Common.Models
{
    /// <summary>
    /// 分页入参规范
    /// </summary>
    public class VLPageRequest
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
            get { return PageSize; }
        }

        public string GetLimitCondition()
        {
            return $"limit {Skip},{Limit}";
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
            return "order by " + string.Join(",", Orders);
        }
        #endregion
    }
}
