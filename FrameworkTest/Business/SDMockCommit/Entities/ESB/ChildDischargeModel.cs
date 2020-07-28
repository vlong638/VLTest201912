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
        public string inp_no { get; set; } //住院号
        /// <summary>
        /// 喂养
        /// </summary>
        public string WYData { get; set; } 
        /// <summary>
        /// 性别
        /// </summary>
        public string xsrsex { get; internal set; }
        /// <summary>
        /// 日龄
        /// </summary>
        public string temcdate { get; internal set; }
        /// <summary>
        /// 产妇姓名
        /// </summary>
        public string PATNAME { get; internal set; }
        /// <summary>
        /// 本次胎次
        /// </summary>
        public string yccdata { get; internal set; }
        /// <summary>
        /// 出院日期
        /// </summary>
        public string chuyuanrq { get; internal set; }
        /// <summary>
        /// 脐带
        /// </summary>
        public string QBQKData { get; internal set; }
        /// <summary>
        /// 转诊原因
        /// </summary>
        public string SFZXSRKYY { get; internal set; }
        /// <summary>
        /// 出院诊断
        /// </summary>
        public string zhuyuanzd { get; internal set; }
        /// <summary>
        /// 母乳喂养早接触
        /// </summary>
        public string mypfzjcdata { get; internal set; }

        public ChildDischargeModel()
        {
        }
    }
}



