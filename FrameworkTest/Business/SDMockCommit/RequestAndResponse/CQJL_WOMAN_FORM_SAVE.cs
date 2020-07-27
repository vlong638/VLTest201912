using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace FrameworkTest.Business.SDMockCommit
{
    public class CQJL_WOMAN_FORM_SAVE: List<CQJL_WOMAN_FORM_SAVE_Data>
    {
    }

    public class CQJL_WOMAN_FORM_SAVE_Data
    {
        public CQJL_WOMAN_FORM_SAVE_Data()
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
            this.D32 = "";
            this.D33 = "";
            this.D34 = "";
            this.D35 = "";
            this.D36 = "";
            this.D37 = "";
            this.D38 = "";
            this.D39 = "";
            this.D40 = "";
            this.D41 = "";
            this.D42 = "";
            this.D43 = "";
            this.D44 = "";
            this.D45 = "";
            this.D46 = "";
            this.D47 = "";
            this.D48 = "";
            this.D49 = "";
        }

        internal void Update(CQJL_WOMAN_FORM_READ_Data data)
        {
            this.D1 = data.D1;
            this.D2 = data.D2;
            this.D3 = data.D3;
            this.D4 = data.D4;
            this.D5 = data.D5;
            this.D6 = data.D6;
            this.D7 = data.D7;
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
            this.D23 = data.D23;
            this.D24 = data.D24;
            this.D25 = data.D25;
            this.D26 = data.D26;
            this.D27 = data.D27;
            this.D28 = data.D28;
            this.D29 = data.D29;
            this.D30 = data.D30;
            this.D31 = data.D31;
            this.D32 = data.D32;
            this.D33 = data.D33;
            this.D34 = data.D34;
            this.D35 = data.D35;
            this.D36 = data.D36;
            this.D37 = data.D37;
            this.D38 = data.D38;
            this.D39 = data.D39;
            this.D40 = data.D40;
            this.D41 = data.D41;
            this.D42 = "1";
            this.D43 = data.D46;
            this.D44 = "";
            this.D45 = data.D45;
            this.D46 = data.D46;
            this.D47 = data.D53;
            this.D48 = data.D54;
            this.D49 = data.D55;
        }

        internal void Update(PregnantDischarge_SourceData sourceData)
        {
            this.D47 = sourceData.SourceData.FMRQDate?.ToDateTime()?.ToString(VLConstraints.DateTime.DateFormatter) ?? "";
            this.D2 = sourceData.SourceData.xingming ?? "";
            this.D3 = sourceData.SourceData.createage ?? "";
            this.D4 = sourceData.SourceData.shouji ?? "";
            this.D5 = sourceData.SourceData.restregioncode.GetSubStringOrEmpty(0, 2) ?? "";
            this.D6 = sourceData.SourceData.restregioncode.GetSubStringOrEmpty(0, 4) ?? "";
            this.D7 = sourceData.SourceData.restregioncode.GetSubStringOrEmpty(0, 6) ?? "";
            this.D8 = sourceData.SourceData.restregioncode.GetSubStringOrEmpty(0, 9) ?? "";
            this.D9 = sourceData.SourceData.restregioncode.GetSubStringOrEmpty(0, 12) ?? "";
            this.D43 = sourceData.SourceData.restregiontext ?? "";//"广东省佛山市顺德区勒流街道龙眼村委会龙眼村委永安路43号", //产后休养详细地址
            this.D46 = sourceData.SourceData.restregiontext ?? "";//"广东省佛山市顺德区勒流街道龙眼村委会龙眼村委永安路43号", //产后休养详细地址
            this.D10 = sourceData.SourceData.chuyuanrqfixed?.ToDateTime()?.ToString(VLConstraints.DateTime.DateFormatter) ?? "";
            this.D11 = sourceData.SourceData.TWData ?? "";
            this.D12 = sourceData.SourceData.XYData ?? "";
            this.D12_1 = D12.GetSubStringOrEmpty(D12.IndexOf("/") + 1);
            this.D13 = (sourceData.SourceData.RFQKData?.Contains("正常") == true ? "1" : "") ?? "";//"1",           //乳房
            //TODO
            this.D14 = VLConstraints.Get_FundusUteri_FS_By_FundusUteri_FM(sourceData.SourceData.gdgddata);//"3",           //宫底
            //TODO
            this.D15 = "";//"1",           //腹部伤口
            //TODO
            this.D16 = "";//"2",           //会阴伤口
            //TODO
            this.D17 = (sourceData.SourceData.RFQKData?.Contains("正常") == true ? "1" : "2") ?? "";//"2",           //恶露
            //TODO
            this.D18 = "";//"3",           //量
            //TODO
            this.D19 = "";//"3",           //色
            //TODO
            //默认`无异味`
            this.D20 = "";//"2",           //味

            this.D21 = "";//"2",           //妊娠合并症
            this.D22 = "";//"3,8",         //产后并发症
            this.D45 = "";//"5,14"          //妊娠并发症

            //TODO
            this.D23 = "";//"2",           //高危妊娠
            //TODO
            this.D24 = "";//"",            //高危因素
            //TODO
            this.D25 = "";//"",            //高危因素是否纠正

            //TODO
            //默认`正常`
            this.D26 = "";//"1",           //产妇分类

            this.D27 = "";//"",            //死亡原因

            //TODO
            //默认`否`
            this.D28 = "";//"",            //是否转诊
            this.D29 = "";//"",            //转诊原因
            this.D30 = "";//"",            //拟转入机构

            //TODO
            this.D31 = "";//"",            //出院诊断
            //TODO
            this.D32 = "";//"",            //出院指导

            this.D33 = "";//"1",           //血红蛋白检测
            this.D34 = "";//"66",          //血红蛋白结果(g/L)
            this.D35 = "";//"1",           //HBsAg检测
            this.D36 = "";//"1",           //HBsAg
            this.D37 = "";//"1",           //HIV抗体检测
            this.D38 = "";//"1",           //梅毒螺旋体抗体检测
            this.D39 = "";//"2",           //非梅毒螺旋体抗体检测
            this.D44 = "";//
            this.D47 = sourceData.SourceData.FMFSData?.ToDateTime()?.ToString(VLConstraints.DateTime.DateFormatter) ?? ""; //2020-07-15    //分娩日期

            //TODO
            this.D48 = "";//1              //分娩方式 
        }

        internal void Init(UserInfo userInfo, PregnantDischarge_SourceData sourceData, string fMMainId)
        {
            this.D1 = sourceData.inp_no;
            this.D40 = userInfo.UserName;
            this.D41 = DateTime.Now.ToString(VLConstraints.DateTime.DateFormatter);
            this.D42 = "1";
            this.D49 = userInfo.OrgId;
        }

        public string D1 { get; set; }//"0000265533",    //住院号        
        public string D2 { get; set; }//"蓝艳云",       //姓名
        public string D3 { get; set; }//"31",           //年龄
        public string D4 { get; set; }//"13929563960",  //手机号
        public string D5 { get; set; }//"44",           //地址
        public string D6 { get; set; }//"4406",         //地址
        public string D7 { get; set; }//"440606",       //地址
        public string D8 { get; set; }//"440606004",    //地址
        public string D9 { get; set; }//"440606004215", //地址
        public string D10 { get; set; }//"2020-07-21",  //出院日期
        public string D11 { get; set; }//"36.1",        //温度
        public string D12 { get; set; }//"121/70",      //血压
        public string D12_1 { get; set; }//70",      //血压低值
        public string D13 { get; set; }//"1",           //乳房
        public string D14 { get; set; }//"3",           //宫底
        public string D15 { get; set; }//"1",           //腹部伤口
        public string D16 { get; set; }//"2",           //会阴伤口
        public string D17 { get; set; }//"2",           //恶露
        public string D18 { get; set; }//"3",           //量
        public string D19 { get; set; }//"3",           //色
        public string D20 { get; set; }//"2",           //味
        public string D21 { get; set; }//"2",           //妊娠合并症
        public string D22 { get; set; }//"3,8",         //产后并发症
        public string D23 { get; set; }//"2",           //高危妊娠
        public string D24 { get; set; }//"",            //高危因素
        public string D25 { get; set; }//"",            //高危因素是否纠正
        public string D26 { get; set; }//"1",           //产妇分类
        public string D27 { get; set; }//"",            //死亡原因
        public string D28 { get; set; }//"",            //是否转诊
        public string D29 { get; set; }//"",            //转诊原因
        public string D30 { get; set; }//"",            //拟转入机构
        public string D31 { get; set; }//"",            //出院诊断
        public string D32 { get; set; }//"",            //出院指导
        public string D33 { get; set; }//"1",           //血红蛋白检测
        public string D34 { get; set; }//"66",          //血红蛋白结果(g/L)
        public string D35 { get; set; }//"1",           //HBsAg检测
        public string D36 { get; set; }//"1",           //HBsAg
        public string D37 { get; set; }//"1",           //HIV抗体检测
        public string D38 { get; set; }//"1",           //梅毒螺旋体抗体检测
        public string D39 { get; set; }//"2",           //非梅毒螺旋体抗体检测
        public string D40 { get; set; }//"赵卓姝",      //医生
        public string D41 { get; set; }//"2020-07-20",  //创建时间?
        public string D42 { get; set; }//1
        public string D43 { get; set; }//"广东省佛山市顺德区勒流街道龙眼村委会龙眼村委永安路43号", //产后休养详细地址
        public string D44 { get; set; }//
        public string D45 { get; set; }//"5,14"          //妊娠并发症
        public string D46 { get; set; }//"广东省佛山市顺德区勒流街道龙眼村委会龙眼村委永安路43号", //产后休养详细地址
        public string D47 { get; set; } //2020-07-15    //分娩日期
        public string D48 { get; set; }//1              //分娩方式 
        public string D49 { get; set; }//"45608491-9",  //机构Id
    }
}