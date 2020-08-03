using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.Research.Common;

namespace VL.Research.Models
{
    /// <summary>
    /// 页面配置 模型
    /// </summary>
    public class GetListConfigModel
    {
        /// <summary>
        /// 个性化配置Id
        /// </summary>
        public long CustomConfigId { set; get; }
        /// <summary>
        /// 页面配置
        /// </summary>
        public ViewConfig ViewConfig { set; get; }
    }
}
