using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class WMH_CQBJ_CQJC_TGJC_LIST
    {
        public WMH_CQBJ_CQJC_TGJC_LIST()
        {
        }

        public string total { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string src { set; get; }

        public List<WMH_CQBJ_CQJC_TGJC_LIST_Data> data { set; get; }
    }
    public class WMH_CQBJ_CQJC_TGJC_LIST_Data
    {
        public string D1 { set; get; } //TGId
        public string D2 { set; get; } //Date
        public string D3 { set; get; }
        public string D4 { set; get; }
        public string D5 { set; get; }
        public string D6 { set; get; }
        public string D7 { set; get; }
    }

    public class WMH_TODAY_CQJC_ID_READ
    {
        public string d1 { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string src { set; get; }
    }


    public class WMH_CQBJ_CQJC_TGJC_NEW_READ
    {
        public WMH_CQBJ_CQJC_TGJC_NEW_READ()
        {
        }

        public string sj { set; get; }
        public string zt { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string src { set; get; }
        public string D47 { set; get; } //身高
        public string D48 { set; get; } //体重
        public string D49 { set; get; } //孕前BMI指数

        public List<WMH_CQBJ_CQJC_TGJC_NEW_READ_Data> data { set; get; }
    }
    public class WMH_CQBJ_CQJC_TGJC_NEW_READ_Data
    {
        public string D1 { set; get; }//:"2020-06-28",
        public string D2 { set; get; }//:"77",
        public string D3 { set; get; }//:"111",
        public string D4 { set; get; }//:"55",
        public string D5 { set; get; }//:"170",
        public string D6 { set; get; }//:"",
        public string D8 { set; get; }//:"44",
        public string D10 { set; get; }//:"正常",
        public string D11 { set; get; }//:"正常",
        public string D12 { set; get; }//:"正常",
        public string D13 { set; get; }//:"无异常",
        public string D14 { set; get; }//:"1",
        public string D15 { set; get; }//:"",
        public string D16 { set; get; }//:"无肿大",
        public string D17 { set; get; }//:"",
        public string D18 { set; get; }//:"正常",
        public string D19 { set; get; }//:"",
        public string D20 { set; get; }//:"未见异常",
        public string D21 { set; get; }//:"软",
        public string D22 { set; get; }//:"居中",
        public string D23 { set; get; }//:"无",
        public string D24 { set; get; }//:"未见异常",
        public string D25 { set; get; }//:"突",
        public string D26 { set; get; }//:"未见异常",
        public string D27 { set; get; }//:"齐",
        public string D28 { set; get; }//:"未闻及病理性杂音",
        public string D29 { set; get; }//:"无",
        public string D30 { set; get; }//:"未触及",
        public string D31 { set; get; }//:"未触及",
        public string D32 { set; get; }//:"生理性弯曲",
        public string D33 { set; get; }//:"未见异常",
        public string D34 { set; get; }//:"无",
        public string D35 { set; get; }//:"未做",
        public string D36 { set; get; }//:"未做",
        public string D37 { set; get; }//:"未做",
        public string D38 { set; get; }//:"未做",
        public string D39 { set; get; }//:"未做",
        public string D40 { set; get; }//:"1",
        public string D41 { set; get; }//:"",
        public string D42 { set; get; }//:"无",
        public string D43 { set; get; }//:"19.38",
        public string D44 { set; get; }//:"",
        public string D45 { set; get; }//:"无",
        public string D46 { set; get; }//:"",
        public string D47 { set; get; }//:"2",
        public string D48 { set; get; }//:"",
        public string D49 { set; get; }//:"",
        public string D50 { set; get; }//:"56"
    }

    
    public class WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data
    {

        public WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data()
        {
            Init();
        }

        private void Init()
        {
            this.D1 = DateTime.Now.ToString("yyyy-MM-dd");
            this.D44 = "";
            this.D6 = "";
            this.D7 = "";
            this.D8 = "";
            this.D9 = "";
            this.D2 = "";
            this.D3 = "";
            this.D48 = "";
            this.D49 = "";
            this.D4 = "";
            this.D50 = "";
            this.D5 = "";
            this.D43 = "";
            this.D47 = "2";
            this.D10 = "正常";
            this.D11 = "正常";
            this.D12 = "正常";
            this.D13 = "无异常";
            this.D14 = "1";
            this.D15 = "";
            this.D16 = "无肿大";
            this.D17 = "";
            this.D18 = "正常";
            this.D19 = "";
            this.D20 = "未见异常";
            this.D21 = "软";
            this.D22 = "居中";
            this.D23 = "无";
            this.D24 = "未见异常";
            this.D25 = "突";
            this.D26 = "未见异常";
            this.D46 = "";
            this.D27 = "齐";
            this.D28 = "未闻及病理性杂音";
            this.D45 = "无";
            this.D30 = "未触及";
            this.D31 = "未触及";
            this.D32 = "生理性弯曲";
            this.D33 = "未见异常";
            this.D34 = "无";
            this.D29 = "无";
            this.D35 = "未做";
            this.D36 = "未做";
            this.D37 = "未做";
            this.D38 = "未做";
            this.D39 = "未做";
            this.D40 = "1";
            this.D41 = "";
            this.D42 = "无";
            this.pagesize = "";
        }

        public WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data(WMH_CQBJ_CQJC_TGJC_NEW_READ_Data physicalExamination)
        {
            Init();

            this.D1 = physicalExamination.D1;
            this.D44 = physicalExamination.D44;
            this.D6 = physicalExamination.D6;
            //this.D7 = physicalExamination.D7;
            this.D8 = physicalExamination.D8;
            //this.D9 = physicalExamination.D9;
            this.D2 = physicalExamination.D2;
            this.D3 = physicalExamination.D3;
            this.D48 = physicalExamination.D48;
            this.D49 = physicalExamination.D49;
            this.D4 = physicalExamination.D4;
            this.D50 = physicalExamination.D50;
            this.D5 = physicalExamination.D5;
            this.D43 = physicalExamination.D43;
            this.D47 = physicalExamination.D47;
            this.D10 = physicalExamination.D10;
            this.D11 = physicalExamination.D11;
            this.D12 = physicalExamination.D12;
            this.D13 = physicalExamination.D13;
            this.D14 = physicalExamination.D14;
            this.D15 = physicalExamination.D15;
            this.D16 = physicalExamination.D16;
            this.D17 = physicalExamination.D17;
            this.D18 = physicalExamination.D18;
            this.D19 = physicalExamination.D19;
            this.D20 = physicalExamination.D20;
            this.D21 = physicalExamination.D21;
            this.D22 = physicalExamination.D22;
            this.D23 = physicalExamination.D23;
            this.D24 = physicalExamination.D24;
            this.D25 = physicalExamination.D25;
            this.D26 = physicalExamination.D26;
            this.D46 = physicalExamination.D46;
            this.D27 = physicalExamination.D27;
            this.D28 = physicalExamination.D28;
            this.D45 = physicalExamination.D45;
            this.D30 = physicalExamination.D30;
            this.D31 = physicalExamination.D31;
            this.D32 = physicalExamination.D32;
            this.D33 = physicalExamination.D33;
            this.D34 = physicalExamination.D34;
            this.D29 = physicalExamination.D29;
            this.D35 = physicalExamination.D35;
            this.D36 = physicalExamination.D36;
            this.D37 = physicalExamination.D37;
            this.D38 = physicalExamination.D38;
            this.D39 = physicalExamination.D39;
            this.D40 = physicalExamination.D40;
            this.D41 = physicalExamination.D41;
            this.D42 = physicalExamination.D42;
            //this.pagesize = physicalExamination.pagesize;
        }

        public void UpdateExamination(PhysicalExaminationModel examination)
        {
            //--孕前体重 D50 PregnantInfo.weight
            this.D50 = examination.pi_weight ?? "";
            //-- 身高 D5   PregnantInfo.height
            this.D5 = examination.pi_height ?? "";
            //-- 孕前BMI指数 D43 PregnantInfo.bmi
            this.D43 = examination.pi_bmi ?? "";
            //--体温 D44 temperature
            this.D44 = examination.temperature ?? "";
            //--脉搏 D6 heartrate
            this.D6 = examination.heartrate ?? "";
            //--呼吸 D8 未找到
            //--体重 D4 weight
            this.D4 = examination.weight ?? "";
            //-- 是否初检 D47
            this.D47 = examination.firstvisitdate == examination.lastestvisitdate ? "1" : "2";
            //dbp 舒张压 低值
            this.D3 = examination.dbp;
            //sbp 收缩压 高值
            this.D2 = examination.sbp;

            //以下都为文本
            //未见异常
            //异常：
            //未做
            //fs外阴 D35    hl外阴 tmpl_vulva
            this.D35 = VLConstraints.Get_Common_AbnormalCheck(examination.vulva);
            //fs阴道 D36    hl阴道 tmpl_vagina
            this.D36 = VLConstraints.Get_Common_AbnormalCheck(examination.vagina);
            //fs宫颈 D37    hl宫颈 tmpl_cervix
            this.D37 = VLConstraints.Get_Common_AbnormalCheck(examination.cervix);
            //fs宫体 D38    hl子宫 tmpl_uterus
            this.D38 = VLConstraints.Get_Common_AbnormalCheck(examination.uterus);
            //fs附件 D39    hl附件1 tmpl_appendages
            this.D39 = VLConstraints.Get_Common_AbnormalCheck(examination.appendages);
        }

        public string D1 { set; get; }//":"2020-06-29" //日期
        public string D44 { set; get; }//":"" //体温
        public string D6 { set; get; }//":"" //脉搏
        public string D7 { set; get; }//":"" 
        public string D8 { set; get; }//":"" //呼吸
        public string D9 { set; get; }//":"" 
        public string D2 { set; get; }//":"88" //血压低值
        public string D3 { set; get; }//":"122" //血压高值
        public string D48 { set; get; }//":""
        public string D49 { set; get; }//":"" //
        public string D4 { set; get; }//":"66" //体重
        public string D50 { set; get; }//":"56" //孕前体重
        public string D5 { set; get; }//":"170" //身高
        public string D43 { set; get; }//":"19.38" //孕前BMI指数
        public string D47 { set; get; }//":"2" //是否初检
        public string D10 { set; get; }//":"正常"
        public string D11 { set; get; }//":"正常"
        public string D12 { set; get; }//":"正常"
        public string D13 { set; get; }//":"无异常"
        public string D14 { set; get; }//":"1"
        public string D15 { set; get; }//":""
        public string D16 { set; get; }//":"无肿大"
        public string D17 { set; get; }//":""
        public string D18 { set; get; }//":"异常"
        public string D19 { set; get; }//":""
        public string D20 { set; get; }//":"未见异常"
        public string D21 { set; get; }//":"硬"
        public string D22 { set; get; }//":"居中"
        public string D23 { set; get; }//":"无"
        public string D24 { set; get; }//":"未见异常"
        public string D25 { set; get; }//":"突"
        public string D26 { set; get; }//":"未见异常"
        public string D46 { set; get; }//":""
        public string D27 { set; get; }//":"齐"
        public string D28 { set; get; }//":"未闻及病理性杂音"
        public string D45 { set; get; }//":"无"
        public string D30 { set; get; }//":"未触及"
        public string D31 { set; get; }//":"未触及"
        public string D32 { set; get; }//":"生理性弯曲"
        public string D33 { set; get; }//":"未见异常"
        public string D34 { set; get; }//":"无"
        public string D29 { set; get; }//":"无"
        public string D35 { set; get; }//":"未做"
        public string D36 { set; get; }//":"未做"
        public string D37 { set; get; }//":"未做"
        public string D38 { set; get; }//":"未做"
        public string D39 { set; get; }//":"未做"
        public string D40 { set; get; }//":"1"
        public string D41 { set; get; }//":""
        public string D42 { set; get; }//":"无"
        public string pagesize { set; get; }//":""
    }
}
