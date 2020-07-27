using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;

namespace FrameworkTest.Business.SDMockCommit
{
    public class CQJL_CHILD_FORM_SAVE : List<CQJL_CHILD_FORM_SAVE_Data>
    {
    }

    public class CQJL_CHILD_FORM_SAVE_Data
    {
        public CQJL_CHILD_FORM_SAVE_Data()
        {
            this.D1 = "";
            this.D2 = "";
            this.D3 = "";
            this.D4 = "";
            this.D5 = "";
            this.D6 = "";
            this.D7 = "";
            this.D8 = "";
            this.D9 = "";
            this.D10 = "";
            this.D11 = "";
            this.D12 = "";
            this.D13 = "";
            this.D14 = "";
            this.D15 = "";
            this.D16 = "";
            this.D17 = "";
            this.D18 = "";
            this.D19 = "";
            this.D20 = "";
            this.D21 = "";
            this.D22 = "";
            this.D23 = "";
            this.D24 = "";
            this.D25 = "";
            this.D26 = "";
            this.D27 = "";
            this.D28 = "";
            this.D29 = "";
            this.D30 = "";
            this.D31 = "";
        }

        internal void Update(CQJL_CHILD_FORM_READ_Data data)
        {
            this.D1 = data.D1;
            this.D2 = data.D2;
            this.D3 = data.D3;
            this.D4 = data.D4;
            this.D5 = data.D5;
            this.D6 = data.D6;
            this.D7 = data.D7?.ToDateTime()?.ToString(VLConstraints.DateTime.DateFormatter) ?? "";
            this.D8 = data.D8;
            this.D9 = data.D9;
            this.D10 = data.D10;
            this.D11 = data.D11;
            this.D12 = data.D12;
            this.D13 = data.D13;
            this.D14 = data.D14;
            this.D15 = data.D15;
            this.D16 = data.D16;
            this.D17 = data.D17;
            this.D18 = data.D18;
            this.D19 = data.D19;
            this.D20 = data.D20;
            this.D21 = data.D21;
            this.D22 = data.D22;
            this.D23 = data.D23?.ToDateTime()?.ToString(VLConstraints.DateTime.DateFormatter) ?? "";
            this.D24 = data.D24;
            this.D25 = data.D25;
            this.D26 = data.D26;
            this.D27 = data.D27?.ToDateTime()?.ToString(VLConstraints.DateTime.DateHHmmFormatter) ?? "";
            this.D28 = data.D28;
            this.D29 = data.D29;
            this.D30 = data.D30?.ToDateTime()?.ToString(VLConstraints.DateTime.DateHHmmFormatter) ?? "";
            this.D31 = data.D31;
        }

        public void Update(ChildDischarge_SourceData sourceData)
        {
            //this.D1 = "";//:"0000265533", 住院号

            //TODO
            this.D2 = "";//:"蓝艳云婴", 儿童姓名
            //TODO
            this.D3 = sourceData.SourceData.xsrsex;//:"2", 性别
            //TODO
            this.D4 = sourceData.SourceData.temcdate;//:"66", 日龄
            //TODO
            this.D5 = sourceData.SourceData.PATNAME;//:"蓝艳云", 产妇姓名
            //TODO
            this.D6 = sourceData.SourceData.yccdata;//:"1", 本次胎次
            //TODO
            this.D7 = sourceData.SourceData.chuyuanrq?.ToDateTime()?.ToString(VLConstraints.DateTime.DateFormatter) ?? ""; //出院日期 需做格式修正
            //TODO
            this.D8 = sourceData.SourceData.QBQKData;//:"1", 脐带
            //TODO
            //默认`有`
            this.D9 = "";//:"1", 黄疸
            //TODO
            this.D10 = "";//:"", 总胆红素
            //TODO
            this.D11 = "";//:"无高危因素,颅内出血", 高危因素文本集
            //TODO
            this.D12 = "";//:"1", 出生缺陷
            //TODO
            this.D13 = "";//:"", 类型
            //默认``
            this.D14 = "";//:"2", 窒息抢救是否成功
            //TODO
            this.D15 = "";//:"1", 疾病转归
            //默认``
            this.D16 = "";//:"777", 死亡原因
            //TODO
            this.D17 = sourceData.SourceData.SFZXSRKYY;//:"666", 转诊原因
            //默认``
            this.D18 = "";//:"4406", 拟转入机构
            //TODO
            this.D19 = sourceData.SourceData.zhuyuanzd;//:"888", 出院诊断
            this.D20 = "";//:"1", 新生儿听力筛查
            this.D21 = "";//:"2", 乙肝免疫球蛋白注射
            this.D22 = "";//Init
            //this.D23 = "";//:"2020-07-20",	创建时间 需做格式修正
            //TODO
            this.D24 = "";//:"1,7", 高危因素值集
            //TODO
            this.D25 = "";//:"1", 出生窒息
            //TODO
            //默认`面部`
            this.D26 = "";//:"", 黄疸部位
            //尚不对接疫苗
            //默认``
            this.D27 = ""; //:"2020-07-23 10:38", 乙肝疫苗注射时间 需做格式修正
            this.D28 = string.IsNullOrEmpty(sourceData.SourceData.WYData) ? "" : sourceData.SourceData.WYData.Contains("母乳") ? "1" : "2";
            //TODO
            this.D29 = sourceData.SourceData.mypfzjcdata;//:"2", 母乳喂养早接触
            //尚不对接疫苗
            //默认``
            this.D30 = ""; //:"2020-07-24 10:34", 乙肝免疫球蛋白接种时间  需做格式修正
            this.D31 = "";//:"",
        }

        public void Init(UserInfo userInfo)
        {
            this.D22 = userInfo.UserName;
            this.D23 = DateTime.Now.ToString(VLConstraints.DateTime.DateFormatter);//:"2020-07-20",	创建时间 需做格式修正
        }

        public string D1 { set; get; }//:"0000265533", 住院号
        public string D2 { set; get; }//:"蓝艳云婴", 儿童姓名
        public string D3 { set; get; }//:"2", 性别
        public string D4 { set; get; }//:"66", 日龄
        public string D5 { set; get; }//:"蓝艳云", 产妇姓名
        public string D6 { set; get; }//:"1", 本次胎次
        public string D7 { set; get; }//:"2020-07-30", 出院日期 需做格式修正
        public string D8 { set; get; }//:"1", 脐带
        public string D9 { set; get; }//:"1", 黄疸
        public string D10 { set; get; }//:"", 总胆红素
        public string D11 { set; get; }//:"无高危因素,颅内出血", 高危因素文本集
        public string D12 { set; get; }//:"1", 出生缺陷
        public string D13 { set; get; }//:"", 类型
        public string D14 { set; get; }//:"2", 窒息抢救是否成功
        public string D15 { set; get; }//:"1", 疾病转归
        public string D16 { set; get; }//:"777", 死亡原因
        public string D17 { set; get; }//:"666", 转诊原因
        public string D18 { set; get; }//:"4406", 拟转入机构
        public string D19 { set; get; }//:"888", 出院诊断
        public string D20 { set; get; }//:"1", 新生儿听力筛查
        public string D21 { set; get; }//:"2", 乙肝免疫球蛋白注射
        public string D22 { set; get; }//:"赵卓姝", 医生姓名 //Init
        public string D23 { set; get; }//:"2020-07-20",	创建时间 需做格式修正
        public string D24 { set; get; }//:"1,7", 高危因素值集
        public string D25 { set; get; }//:"1", 出生窒息
        public string D26 { set; get; }//:"", 黄疸部位
        public string D27 { set; get; }//:"2020-07-23 10:38", 乙肝疫苗注射时间 需做格式修正
        public string D28 { set; get; }//:"1", 母乳喂养
        public string D29 { set; get; }//:"2", 母乳喂养早接触
        public string D30 { set; get; }//:"2020-07-24 10:34", 乙肝免疫球蛋白接种时间  需做格式修正
        public string D31 { set; get; }//:"",
    }
}