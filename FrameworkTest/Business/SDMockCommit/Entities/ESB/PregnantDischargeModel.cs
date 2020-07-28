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
    public class PregnantDischargeModel
    {
        public string inp_no { get; set; } //住院号
        public string visit_id { get; set; } // 就诊号
        public string FMRQDate { get; set; } //分娩日期
        public string FMFSData { get; set; } //分娩方式
        public string ZCJGData { get; set; } //助产机构
        public string TWData { get; set; }//体温
        public string XYData { get; set; }//血压
        public string RFQKData { get; set; }//乳房
        public string gdgddata { set; get; }//宫底
        public string hyskdata { set; get; }//会阴伤口
        public string ELUData { get; set; }//恶露

        //来源 病人表 
        //select chuyuanrq,chuyuanrqfixed,* from V_FWPT_GY_BINGRENXXZY br where br.bingrenid =fm.inp_no
        public string chuyuanrqfixed { set; get; } //出院日期
        public string xingming { get; set; }         //孕妇姓名
        public string shouji { get; set; }           //联系电话

        //来源PregnantInfo 
        public string idcard { get; set; }//身份证
        public string createage { get; set; }//年龄
        public string restregioncode { get; set; }//产后休养地址1
        public string restregiontext { get; set; }//产后修养详情地址

        //来源 住院表 一对多(可能)
        //left join V_FWPT_GY_ZHUYUANZD zd on zd.patient_id = fm.inp_no and zd.inp_no = fm.visit_id
        //zd.diag_desc
        public string zhuyuanzd { get; set; }//出院诊断

        //来源 V_FWPT_MZ_YIJI住院医嘱在这个里面字段yizhumc 一对多
        //select top 10 * from V_FWPT_MZ_YIJI yz where yz.bingrenid = fm.inp_no
        public string CLJZDData { get; set; }//处理及指导

        ////来源MHC_VisitRecord 一对多
        //public string HighRisks { set; get; }//高危因素

        public PregnantDischargeModel()
        {
        }
    }
}
