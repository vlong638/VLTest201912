using System.Collections.Generic;

namespace Autobots.Infrastracture.Common.PagerSolution
{
    /// <summary>
    /// 分页出参规范
    /// </summary>
    public class VLPagerResult<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Count { set; get; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentIndex { set; get; }
        /// <summary>
        /// 内容
        /// </summary>
        public T List { set; get; }
    }
}
