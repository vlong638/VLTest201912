using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 1.基本情况
    /// </summary>
    public class WMH_CQBJ_JBXX_FORM_SAVEData
    {
        public WMH_CQBJ_JBXX_FORM_SAVEData()
        {
        }

        public WMH_CQBJ_JBXX_FORM_SAVEData(WMH_CQBJ_JBXX_FORM_READData data)
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
            this.D42 = data.D42;
            this.D43 = data.D43;
            this.D44 = data.D44;
            this.D45 = data.D45;
            this.D46 = data.D46;
            this.D47 = data.D47;
            this.D48 = data.D48;
            this.D49 = data.D49;
            this.D50 = data.D50;
            this.D51 = data.D51;
            this.D52 = data.D52;
            this.D53 = data.D53;
            this.D54 = data.D54;
            this.D55 = data.D55;
            this.D56 = data.D56;
            this.D57 = data.D57;
            this.D58 = data.D58;
            this.D59 = data.D59;
            this.D60 = data.D60;
            this.D61 = data.D61;
            this.D62 = data.D62;
            this.D63 = data.D63;
            this.D64 = data.D64;
            this.D65 = data.D65;
            this.D66 = data.D66;
            this.D67 = data.D67;
            this.D68 = data.D68;
            this.D69 = data.D69;
            this.D70 = data.D70;
            this.D71 = data.D71;
        }

        public string D1 { set; get; } //登录用户Id
        public string D2 { set; get; } //@保健号
        public string D3 { set; get; } //孕妇姓名
        public string D4 { set; get; } //孕妇国籍 对照表 2) 1)  国籍代码GB/T 2659
        public string D5 { set; get; } //孕妇民族 1)  民族代码GB/T 3304
        public string D6 { set; get; } //孕妇证件类型1)   证件类型CV02.01.101
        public string D7 { set; get; } //身份证
        public string D8 { set; get; } //生日
        public string D9 { set; get; } //孕妇年龄
        public string D10 { set; get; } //孕妇文化程度 1)  文化程度STD_CULTURALDEG
        public string D11 { set; get; } //手机号码
        public string D12 { set; get; } //孕妇职业 1)  职业STD_OCCUPATION
        public string D13 { set; get; } //孕妇工作单位
        public string D14 { set; get; } //孕妇籍贯
        public string D15 { set; get; } //孕妇户籍地址 [TODO 对照表] 省2位,市2位,县/区2位,乡镇街道3位,社区/村3位
        public string D16 { set; get; } //孕妇户籍地址 [TODO 对照表]
        public string D17 { set; get; } //孕妇户籍地址 [TODO 对照表]
        public string D18 { set; get; } //孕妇户籍地址 [TODO 对照表]
        public string D19 { set; get; } //孕妇户籍地址 [TODO 对照表]
        public string D20 { set; get; } //户籍详细地址
        public string D21 { set; get; } //孕妇现住地址 [TODO 对照表]
        public string D22 { set; get; } //孕妇现住地址 [TODO 对照表]
        public string D23 { set; get; } //孕妇现住地址 [TODO 对照表]
        public string D24 { set; get; } //孕妇现住地址 [TODO 对照表]
        public string D25 { set; get; } //孕妇现住地址 [TODO 对照表]
        public string D26 { set; get; } //产后休养地址
        public string D27 { set; get; } //产后休养地址 [TODO 对照表]
        public string D28 { set; get; } //产后休养地址 [TODO 对照表]
        public string D29 { set; get; } //产后休养地址 [TODO 对照表]
        public string D30 { set; get; } //产后休养地址 [TODO 对照表]
        public string D31 { set; get; } //产后休养地址 [TODO 对照表]
        public string D32 { set; get; } //产后详细地址
        public string D33 { set; get; } //孕妇户籍类型 1)  户籍类型STD_REGISTERT2PE
        public string D34 { set; get; } //孕妇户籍分类 非户籍:2 ,户籍:1
        public string D35 { set; get; } //来本地居住时间 
        public string D36 { set; get; } //近亲结婚  [TODO 对照表]
        public string D37 { set; get; } //孕妇结婚年龄 
        public string D38 { set; get; } //丈夫结婚年龄 
        public string D39 { set; get; } //丈夫姓名
        public string D40 { set; get; } //丈夫国籍  [TODO 对照表]
        public string D41 { set; get; } //丈夫民族  [TODO 对照表]
        public string D42 { set; get; } //丈夫证件类型  [TODO 对照表]
        public string D43 { set; get; } //丈夫证件号码
        public string D44 { set; get; } //丈夫出生日期
        public string D45 { set; get; } //丈夫登记年龄
        public string D46 { set; get; } //丈夫职业  [TODO 对照表]
        public string D47 { set; get; }  //丈夫工作单位
        public string D48 { set; get; } //丈夫联系电话
        public string D49 { set; get; } //丈夫健康状况   [TODO 对照表]
        public string D50 { set; get; } //丈夫嗜好   [TODO 对照表]
        public string D51 { set; get; } //丈夫现在地址   [TODO 对照表]
        public string D52 { set; get; } //丈夫现在地址
        public string D53 { set; get; } //丈夫现在地址
        public string D54 { set; get; } //丈夫现在地址
        public string D55 { set; get; } //丈夫现在地址
        public string D56 { set; get; } //现住详细地址
        public string D57 { set; get; }
        public string D58 { set; get; } //创建时间
        public string D59 { set; get; } //创建机构
        public string D60 { set; get; } //创建人员
        public string D61 { set; get; } //病案号
        public string D62 { set; get; } //婚姻状况  [TODO 对照表]
        public string D63 { set; get; } //医疗费用支付方式  [TODO 对照表]
        public string D64 { set; get; } //厨房排风设施 PASS
        public string D65 { set; get; } //燃料类型 PASS
        public string D66 { set; get; } //饮水 PASS
        public string D67 { set; get; } //厕所  PASS
        public string D68 { set; get; } //禽畜栏 PASS
        public string D69 { set; get; } //创建机构名称:佛山市妇幼保健院
        public string D70 { set; get; } //健康码
        public string D71 { set; get; } //健康码ID
        public string curdate1 { set; get; }
    }
}
