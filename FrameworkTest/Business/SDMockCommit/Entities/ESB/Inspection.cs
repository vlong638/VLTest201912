using Dapper.Contrib.Extensions;
using System;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 医嘱
    /// </summary>
    [Table("[v_fwpt_jy_jianyanjg]")]
    public class Inspection
    {
        /// <summary>
        /// 检验名称
        /// </summary>
        public string chinesename { get; set; }
        /// <summary>
        /// 检验结果
        /// </summary>
        public string testresult { get; set; }
        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime measuretime { get; set; }

    }
}
