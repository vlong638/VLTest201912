using System.Collections.Generic;

namespace FrameworkTest.Business.SDMockCommit
{
    public class CQJL_WOMAN_FORM_READ
    {
        public string dsc { set; get; }
        public string code { set; get; }
        public string scr { set; get; }
        public List<CQJL_WOMAN_FORM_READ_Data> data { set; get; }
    }

    public class CQJL_WOMAN_FORM_READ_Data
    {
        public string DischargeId { get { return D47; } }

        public string D1 {get; set; }//"0000265533",    //住院号
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
        public string D28 { get; set; }//"",            //是否转正
        public string D29 { get; set; }//"",            //转正原因
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
        public string D42 { get; set; }//"2020-07-20",  //创建时间?
        public string D43 { get; set; }//"45608491-9",  //机构Id
        public string D44 { get; set; }//"35021069",    //用户Id
        public string D45 { get; set; }//"1",           //
        public string D46 { get; set; }//"广东省佛山市顺德区勒流街道龙眼村委会龙眼村委永安路43号", //产后休养详细地址
        public string D47 { get; set; }//"AADA00CCD8CE28ABE0535FF28213B34F",    //出院登记Id
        public string D48 { get; set; }//"AA7257100ECD6788E05355FE80134681",    //通用Id
        public string D49 { get; set; }//"P",           //分娩医院
        public string D50 { get; set; }//"45608491-9",  //机构Id
        public string D51 { get; set; }//"",            //???无页面项
        public string D52 { get; set; }//"5,14",        //妊娠并发症
        public string D53 { get; set; }//"2020-07-15",  //分娩日期
        public string D54 { get; set; }//"1",           //分娩方式
        public string D55 { get; set; }//"45608491-9"   //机构Id
    }
}