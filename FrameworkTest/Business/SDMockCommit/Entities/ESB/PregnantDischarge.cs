using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 孕妇出院登记
    /// </summary>
    [Table("[V_FWPT_GY_ZHUYUANFM]")]
    public class PregnantDischarge
    {
        public string inp_no { get; set; } //住院号
        public string FMRQDate { get; set; }//分娩日期
        public string FMFSData { get; set; }//分娩方式
        public string ZCJGData { get; set; }//助产机构
        public string TWData { get; set; }//体温
        public string XYData { get; set; }//血压
        public string RFQKData { get; set; }//乳房
        public string ELUData { get; set; }//恶露
        public string SFZXSRK { get; set; }//是否转新生儿科
        public string SFZXSRKYY { get; set; }//转新生儿科原因
        public string CLJZDData { get; set; }//处理及指导

        public PregnantDischarge()
        {
        }
    }
}
