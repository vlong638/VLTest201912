using System.Collections.Generic;

namespace VL.Consolo_Core.Common.PagerSolution
{
    /// <summary>
    /// 分页出参规范
    /// </summary>
    public class VLPagerResult<T>
    {
        /// <summary>
        /// 列表总数
        /// </summary>
        public int Count { set; get; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentIndex { set; get; }
        /// <summary>
        /// 列表数据
        /// </summary>
        public IEnumerable<T> List { set; get; }
    }
    /// <summary>
    /// 分页出参规范
    /// </summary>
    public class VLPagerTableResult<T>
    {
        /// <summary>
        /// 列表总数
        /// </summary>
        public int Count { set; get; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentIndex { set; get; }
        /// <summary>
        /// 列表数据
        /// </summary>
        public T SourceData { set; get; }
    }
}
