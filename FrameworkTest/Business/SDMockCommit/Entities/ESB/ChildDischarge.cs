using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 婴儿出院登记
    /// </summary>
    [Table("[V_FWPT_GY_ZHUYUANFMYE]")]
    public class ChildDischarge
    {
        public string inp_no { get; set; } //住院号
        public string WYData { get; set; } //喂养

        public ChildDischarge()
        {
        }
    }
}
