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
    public class ChildDischargeModel
    {
        #region V_FWPT_GY_BINGRENXXZY
        /// <summary>
        /// 出院日期
        /// </summary>
        public string chuyuanrqfixed { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string shenfenzh { get; set; } 
        #endregion


        public string inp_no { get; set; } //住院号 (似乎实际存的是病人号)
        public string visit_id { get; set; } //就诊号 (似乎实际存的是住院号)
        /// <summary>
        /// 产妇姓名
        /// </summary>
        public string PATNAME { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string xsrsex { get; set; }
        /// <summary>
        /// 胎儿娩出时间
        /// </summary>
        public string temcdate { get; set; }
        /// <summary>
        /// 本次胎次
        /// </summary>
        public string yccdata { get; set; }
        /// <summary>
        /// 新生儿窒息
        /// </summary>
        public string xsrzx { get; set; }
        /// <summary>
        /// 脐带
        /// </summary>
        public string QBQKData { get; set; }
        /// <summary>
        /// 转诊原因
        /// </summary>
        public string SFZXSRKYY { get; set; }
        /// <summary>
        /// 母乳喂养
        /// </summary>
        public string WYData { get; set; }
        /// <summary>
        /// 母乳喂养早接触
        /// </summary>
        public string mypfzjcdata { get; set; }

        public ChildDischargeModel()
        {
        }
    }
}



