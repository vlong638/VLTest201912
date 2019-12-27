using System.Collections.Generic;

namespace VLTest2015.Services
{
    /// <summary>
    /// 分页规格
    /// </summary>
    public class PagerResponse<T>
    {
        public List<T> Data { set; get; }
        public int TotalCount { set; get; }
        public int CurrentIndex { set; get; }
    }
}