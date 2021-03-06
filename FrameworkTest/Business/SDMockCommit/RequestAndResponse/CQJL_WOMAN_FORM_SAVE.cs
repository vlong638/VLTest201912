﻿using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
            this.D50 = "";
            this.D51 = "";
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

        internal void Update(PregnantDischarge_SourceData sourceData, List<HighRiskEntity> highRisks, IEnumerable<Diagnosis> diagnosis, List<Advice> advices, List<Inspection> inspections)
        {
            this.D50 = "01";// 证件类型 1=身份证
            this.D51 = sourceData.idcard;
            this.D47 = sourceData.SourceData.FMRQDate?.ToDateTime()?.ToString(VLConstraints.DateTime.DateFormatter) ?? ""; //2020-07-15    //分娩日期
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
            this.D13 = VLConstraints.Get_BreastStatus_FS_By_BreastStatus_FM(sourceData.SourceData.RFQKData);//"1",          //乳房
            this.D14 = VLConstraints.Get_FundusUteri_FS_By_FundusUteri_FM(sourceData.SourceData.gdgddata);//"3",            //宫底
            this.D48 = VLConstraints.GetDeliveryMannerByDeliveryManner_FM(sourceData.SourceData.FMFSData);//                //分娩方式
            switch (this.D48)
            {
                case "1"://顺产
                    this.D16 = "2";//"无=1",       //会阴伤口
                    break;
                case "2"://剖宫产
                    this.D16 = "1";//"愈合好=2",   //会阴伤口
                    break;
                default:
                    this.D16 = "2";//"无=1",       //会阴伤口
                    break;
            }
            this.D15 = "1";//"1",          //腹部伤口 //无法传值
            this.D17 = "2";//"2",           //恶露 //默认`有`
            this.D18 = "";//"3",           //量
            this.D19 = "";//"3",           //色
            switch (this.D17)
            {
                case "2"://有
                    this.D18 = "3";//"3",           //量
                    this.D19 = "3";//"3",           //色
                    break;
                case "1"://无
                    break;
                default:
                    break;
            }
            this.D20 = "1";//"2",          //味  //默认`无异味`
            this.D21 = VLConstraints.GetPregnancyComplicationsA(diagnosis);//"2",           //妊娠合并症
            this.D22 = VLConstraints.GetPostnatalComplications(diagnosis);//"3,8",         //产后并发症
            this.D45 = VLConstraints.GetPregnancyComplicationsB(diagnosis);//"5,14"         //妊娠并发症
            this.D23 = highRisks.Count() > 0 ? "1" : "2";//"2",           //高危妊娠
            this.D24 = string.Join(",", highRisks.Select(c => c.T));      //"",  //高危因素
            this.D25 = "1";//"",           //高危因素是否纠正 //默认`是`
            this.D26 = "1";//"1",          //产妇分类 //默认`正常`,`死亡`不传
            this.D27 = "";//"",            //死亡原因
            this.D28 = "";//"",            //是否转诊  产妇分类默认`正常` `正常`时,是否转诊不可填写
            this.D29 = "";//"",            //转诊原因
            this.D30 = "";//"",            //拟转入机构
            this.D31 = string.Join(",", diagnosis.Select(c => c.diag_desc));    //"",            //出院诊断
            this.D32 = string.Join(",", advices.Select(c => c.yizhumc));   //"",            //出院指导
            this.D33 = inspections.FirstOrDefault(c => c.chinesename == "血红蛋白") != null ? "1" : "2";//"1",           //血红蛋白检测
            this.D34 = inspections.OrderByDescending(c=>c.measuretime).FirstOrDefault(c => c.chinesename == "血红蛋白")?.testresult ?? "";//"66",          //血红蛋白结果(g/L)
            this.D35 = inspections.FirstOrDefault(c => c.chinesename == "乙肝表面抗原") != null ? "1" : "2";//"1",           //HBsAg检测   
            var D36text = inspections.OrderByDescending(c => c.measuretime).FirstOrDefault(c => c.chinesename == "乙肝表面抗原")?.testresult ?? "";
            this.D36 = D36text.StartsWith("阴性") == true ? "1" : (D36text.StartsWith("阳性") == true ? "2" : "");//"1",           //HBsAg
            this.D37 = inspections.FirstOrDefault(c => c.chinesename == "人免疫缺陷病毒抗体测定") != null ? "1" : "2"; ;//"1",           //HIV抗体检测
            this.D38 = inspections.FirstOrDefault(c => c.chinesename == "梅毒螺旋体特异抗体测定") != null ? "1" : "2"; ;//"1",           //梅毒螺旋体抗体检测
            this.D39 = "";
            //不对此类做对接
            //this.D38 = inspections.FirstOrDefault(c => c.chinesename == "梅毒螺旋体非特异抗体(TRUST)") != null ? "1" : "2"; ;//"1",           //梅毒螺旋体抗体检测
            //没有此类对接数据
            //this.D39 = inspections.FirstOrDefault(c => c.chinesename == "	非梅毒抗体") != null ? "1" : "2"; ;//"2",           //非梅毒螺旋体抗体检测
            this.D44 = "";//
        }

        internal void Init(UserInfo userInfo, PregnantDischarge_SourceData sourceData, string fMMainId)
        {
            this.D1 = sourceData.inp_no;
            this.D40 = userInfo.UserName;
            this.D41 = DateTime.Now.ToString(VLConstraints.DateTime.DateFormatter);
            this.D42 = "1"; // 产妇存活
            this.D49 = userInfo.OrgId;
        }

        internal ValidateResult Validate()
        {
            var result = new ValidateResult(ValidateResultCode.Success, "");
            if (D1.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D1))));
            if (D2.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D2))));
            if (D3.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D3))));
            if (D4.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D4))));
            if (D10.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D10))));
            if (D11.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D11))));
            if (D12.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D12))));
            if (D13.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D13))));
            if (D14.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D14))));
            if (D15.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D15))));
            if (D16.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D16))));
            if (D17.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D17))));
            if (D18.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D18))));
            if (D19.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D19))));
            if (D20.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D20))));
            if (D21.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D21))));
            if (D22.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D22))));
            if (D23.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D23))));
            //if (D24.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D24))));
            if (D25.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D25))));
            if (D26.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D26))));
            if (D31.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D31))));
            if (D32.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D32))));
            //if (D33.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D33))));
            if (D33 == "1")
                if (D34.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D34))));
            //if (D35.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D35))));
            //if (D36.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D36))));
            if (!D36.IsNullOrEmpty())
                if (D37.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D37))));
            //if (D38.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D38))));
            //if (D39.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D39))));
            if (D43.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D43))));
            if (D47.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D47))));
            if (D48.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D48))));
            if (D49.IsNullOrEmpty()) return new ValidateResult(ValidateResultCode.IsNullOrEmpty, ValidateResult.GetNullOrEmptyMessage(this.GetProperty(nameof(D49))));
            return result;
        }


        [Description("住院号")]
        public string D1 { get; set; }//"0000265533",    //住院号        
        [Description("孕妇姓名")]
        public string D2 { get; set; }//"蓝艳云",       //姓名
        [Description("年龄")]
        public string D3 { get; set; }//"31",           //年龄
        [Description("联系电话")]
        public string D4 { get; set; }//"13929563960",  //手机号
        public string D5 { get; set; }//"44",           //地址
        public string D6 { get; set; }//"4406",         //地址
        public string D7 { get; set; }//"440606",       //地址
        public string D8 { get; set; }//"440606004",    //地址
        public string D9 { get; set; }//"440606004215", //地址
        [Description("出院日期")]
        public string D10 { get; set; }//"2020-07-21",  //出院日期
        [Description("体温")]
        public string D11 { get; set; }//"36.1",        //温度
        [Description("血压")]
        public string D12 { get; set; }//"121/70",      //血压
        public string D12_1 { get; set; }//70",      //血压低值
        [Description("乳房")]
        public string D13 { get; set; }//"1",           //乳房
        [Description("宫底")]
        public string D14 { get; set; }//"3",           //宫底
        [Description("腹部伤口")]
        public string D15 { get; set; }//"1",           //腹部伤口
        [Description("会阴伤口")]
        public string D16 { get; set; }//"2",           //会阴伤口
        [Description("恶露")]
        public string D17 { get; set; }//"2",           //恶露
        [Description("量")]
        public string D18 { get; set; }//"3",           //量
        [Description("色")]
        public string D19 { get; set; }//"3",           //色
        [Description("味")]
        public string D20 { get; set; }//"2",           //味
        [Description("妊娠合并症")]
        public string D21 { get; set; }//"2",           //妊娠合并症
        [Description("产后并发症")]
        public string D22 { get; set; }//"3,8",         //产后并发症
        [Description("高危妊娠")]
        public string D23 { get; set; }//"2",           //高危妊娠
        [Description("高危因素")]
        public string D24 { get; set; }//"",            //高危因素
        [Description("高危因素是否纠正")]
        public string D25 { get; set; }//"",            //高危因素是否纠正
        [Description("产妇分类")]
        public string D26 { get; set; }//"1",           //产妇分类
        public string D27 { get; set; }//"",            //死亡原因
        public string D28 { get; set; }//"",            //是否转诊
        public string D29 { get; set; }//"",            //转诊原因
        public string D30 { get; set; }//"",            //拟转入机构
        [Description("出院诊断")]
        public string D31 { get; set; }//"",            //出院诊断
        [Description("出院指导")]
        public string D32 { get; set; }//"",            //出院指导
        [Description("血红蛋白检测")]
        public string D33 { get; set; }//"1",           //血红蛋白检测
        [Description("血红蛋白结果")]
        public string D34 { get; set; }//"66",          //血红蛋白结果(g/L)
        [Description("HBsAg检测")]
        public string D35 { get; set; }//"1",           //HBsAg检测
        [Description("HBsAg结果")]
        public string D36 { get; set; }//"1",           //HBsAg结果
        [Description("HIV抗体检测")]
        public string D37 { get; set; }//"1",           //HIV抗体检测
        [Description("梅毒螺旋体抗体检测")]
        public string D38 { get; set; }//"1",           //梅毒螺旋体抗体检测
        [Description("非梅毒螺旋体抗体检测")]
        public string D39 { get; set; }//"2",           //非梅毒螺旋体抗体检测
        public string D40 { get; set; }//"赵卓姝",      //医生
        public string D41 { get; set; }//"2020-07-20",  //创建时间?
        public string D42 { get; set; }//1              //产妇存活
        [Description("产后休养地址")]
        public string D43 { get; set; }//"广东省佛山市顺德区勒流街道龙眼村委会龙眼村委永安路43号", //产后休养详细地址
        public string D44 { get; set; }//
        public string D45 { get; set; }//"5,14"          //妊娠并发症
        public string D46 { get; set; }//"广东省佛山市顺德区勒流街道龙眼村委会龙眼村委永安路43号", //产后休养详细地址
        [Description("分娩日期")]
        public string D47 { get; set; } //2020-07-15    //分娩日期
        [Description("分娩方式")]
        public string D48 { get; set; }//1              //分娩方式 
        [Description("分娩医院")]
        public string D49 { get; set; }//"45608491-9",  //机构Id
        public string D50 { get; set; }//证件类型 1 = 身份证
        public string D51 { get; set; }//身份证Id
    }
}