using System.Collections.Generic;

namespace VLVLTest2015.Common.Pager
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
