using System.Collections.Generic;

namespace Consolo_Core.Common.PagerSolution
{
    /// <summary>
    /// 分页出参规范
    /// </summary>
    public class VLPageResult<T>
    {
        public int Count { set; get; }
        public int CurrentIndex { set; get; }
        public IEnumerable<T> List { set; get; }
    }
}
