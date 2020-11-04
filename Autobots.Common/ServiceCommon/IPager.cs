using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autobots.Common.ServiceCommon
{
    /// <summary>
    /// 分页出参规范
    /// </summary>
    public class IPager<T>
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
        public T SourceData;
    }
}
