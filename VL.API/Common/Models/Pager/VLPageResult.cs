using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.API.Common.Models
{

    /// <summary>
    /// 分页出参规范
    /// </summary>
    public class VLPageResult<T>
    {
        public int Count { set; get; }
        public int CurrentIndex { set; get; }
        public List<T> Data { set; get; }
    }
}
