using System.Collections.Generic;
using System.Linq;

namespace Autobots.Infrastracture.Common.FileSolution
{
    /// <summary>
    /// 分页入参规范
    /// </summary>
    public class VLPagerRequest
    {
        #region 分页

        /// <summary>
        /// 分页码
        /// </summary>
        public int PageIndex { set; get; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { set; get; }

        internal int Skip
        {
            get
            {
                var skip = (PageIndex - 1) * PageSize;
                return skip > 0 ? skip : 0;
            }
        }
        internal int Limit
        {
            get { return PageSize > 0 ? PageSize : 10; }
        }
        internal string GetLimitCondition()
        {
            return $"offset {Skip} rows fetch next {Limit} rows only";
        }

        #endregion
    }
}
