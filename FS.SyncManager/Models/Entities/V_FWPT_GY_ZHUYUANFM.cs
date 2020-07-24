using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.SyncManager.Models
{
    //替换用 (\w+)\s+(\w+)\s+\d+\s+[-\d]+\s+[-\d]+\s+[-\d]+\s+([\w:\(\)]+).+
    //public $2 $1 {set;get;} //$3
    [Table(TableName)]
    public class V_FWPT_GY_ZHUYUANFM
    {
        public const string TableName = "V_FWPT_GY_ZHUYUANFM";

        //出院日期 
        public DateTime chuyuanrqfixed { set; get; }//select chuyuanrq, chuyuanrqfixed from V_FWPT_GY_BINGRENXXZY where chuyuanrq is not null and chuyuanrqfixed is null

        public string zcjgdata { set; get; } //zcjgdata varchar	255		False 助产机构
        public string inp_no { set; get; } //inp_no varchar	255		False 住院号
        public string visit_id { set; get; } //public string visit_id {set;get;} //visit_id varchar	255		False 流水号
        public string ryrqdata { set; get; } //ryrqdata varchar	255		False 入院日期
        public string fmrqdate { set; get; } //fmrqdate varchar	255		False 分娩日期
        public string fmfsdata { set; get; } //fmfsdata varchar	255		False 分娩方式
        public string yzweek { set; get; } //yzweek varchar	255		False 孕周
        public string hyqkdata { set; get; } //hyqkdata varchar	255		False 会阴情况
        public string zccdata { set; get; } //zccdata varchar	255		False 总产程
        public string chcxdata { set; get; } //chcxdata varchar	255		False 产后出血
        public string xsrsex { set; get; } //xsrsex varchar	255		False 新生儿性别
        public string xsrtz { set; get; } //xsrtz varchar	255		False 新生儿体重
        public string xsrsg { set; get; } //xsrsg varchar	255		False 新生儿身高
        public string apgarpf1 { set; get; } //apgarpf1 varchar	255		False apgar评分1分钟
        public string apgarpf5 { set; get; } //apgarpf5 varchar	255		False apgar评分5分钟
        public string apgarpf10 { set; get; } //apgarpf10 varchar	255		False apgar评分10分钟
        public string ycffldata { set; get; } //ycffldata varchar	255		False 孕产妇分类
        public string ycfchzd { set; get; } //ycfchzd varchar	255		False 孕产妇产后诊断
        public string zsdata { set; get; } //zsdata varchar	255		False 主诉                    
        public string jkqkdata { set; get; } //jkqkdata varchar	255		False 健康情况
        public string cxl24hour { set; get; } //cxl24hour varchar	255		False		24小时出血量
        public string twdata { set; get; } //twdata  varchar	255		False 体温
        public string xydata { set; get; } //xydata varchar	255		False 血压
        public string rfqkdata { set; get; } //rfqkdata varchar	255		False 乳房
        public string zgfjqk { set; get; } //zgfjqk varchar	255		False 子宫复旧
        public string skqkdata { set; get; } //skqkdata varchar	255		False 伤口
        public string eludata { set; get; } //eludata varchar	255		False 恶露
        public string cljzddata { set; get; } //cljzddata varchar	255		False 处理及指导
        public DateTime downloadtime { set; get; } //downloadtime datetime2	0		False
        public string patname { set; get; } //patname varchar	255		False 孕妇姓名
        public string patage { set; get; } //patage varchar	255		False 年龄
        public string yccdata { set; get; } //yccdata varchar	255		False 孕产次
        public string temcdate { set; get; } //temcdate varchar	255		False 胎儿娩出时间
        public string xsrzx { set; get; } //xsrzx varchar	255		False 新生儿窒息
        public string csqxdata { set; get; } //csqxdata varchar	255		False 出生缺陷
        public string mypfzjcdata { set; get; } //mypfzjcdata varchar	255		False 母婴皮肤早接触
        public string zsxdata { set; get; } //zsxdata varchar	255		False 早吸吮
        public string gdgddata { set; get; } //gdgddata varchar	255		False 宫底
        public string fbskdata { set; get; } //fbskdata varchar	255		False 腹部伤口
        public string hyskdata { set; get; } //hyskdata varchar	255		False 会阴伤口
    }
}
