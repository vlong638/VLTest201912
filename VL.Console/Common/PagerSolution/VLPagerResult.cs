using System.Collections.Generic;

namespace VL.Consolo_Core.Common.PagerSolution
{
    /// <summary>
    /// 分页出参规范
    /// </summary>
    public class VLPagerResult<T>
    {
        public int Count { set; get; }
        public int CurrentIndex { set; get; }
        public IEnumerable<T> List { set; get; }
    }
    /// <summary>
    /// 分页出参规范
    /// </summary>
    public class VLPagerTableResult<T>
    {
        public int Count { set; get; }
        public int CurrentIndex { set; get; }
        public T DataTable { set; get; }
    }
}
