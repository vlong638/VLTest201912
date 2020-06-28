using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{

    public class MQDA_READ_NEWResponse
    {
        public string sj { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string scr { set; get; }
        public List<MQDA_READ_NEWData> data { set; get; }

    }
    public class MQDA_READ_NEWData
    {
        public string D1 { set; get; } //D1:"2020-03-12", //末次月经        
        public string D2 { set; get; } //D2:"2020-12-17", //预产期
        public string D3 { set; get; } //D3:"自然受孕",	//受孕方式
        public string D4 { set; get; } //"D4":"2020-06-02", //受精或移植日期
        public string D5 { set; get; } //"D5":"尿妊娠试验,B超检查",//妊娠确认方法
        public string D6 { set; get; } //"D6":"4",//B超确认孕周
        public string D7 { set; get; } //"D7":"有",//早孕反应
        public string D8 { set; get; } //"D8":"5",//开始孕周
        public string D9 { set; get; } //"D9":"6",//胎动开始孕周
        public string D10 { set; get; } //"D10":"头痛（开始孕周    ）,//自觉症状
        public string D11 { set; get; } //"D11":"冠心病,肾脏疾病",//既往史
        public string D12 { set; get; } //"D12":"无",//
        public string D13 { set; get; } //"D13":"剖宫产术,腹腔镜子宫肌瘤剔除术",//手术史
        public string D14 { set; get; } //"D14":"未发现",//
        public string D15 { set; get; } //"D15":"青霉素,喹诺酮类",//过敏史
        public string D16 { set; get; } //"D16":"有",//输血史
        public string D17 { set; get; } //"D17":"",//
        public string D18 { set; get; } //"D18":"地中海贫血,G6PD缺乏症",//本人家族史
        public string D19 { set; get; } //"D19":"2",//亲缘关系
        public string D20 { set; get; } //"D20":"地中海贫血,G6PD缺乏症",//配偶家族史
        public string D21 { set; get; } //"D21":"2",//亲缘关系
        public string D22 { set; get; } //"D22":"",//
        public string D23 { set; get; } //"D23":"11",//月经初潮年龄
        public string D24 { set; get; } //"D24":"22",//月经持续时间
        public string D25 { set; get; } //"D25":"33",//间歇时间
        public string D26 { set; get; } //"D26":"中",//经量
        public string D27 { set; get; } //"D27":"有",//痛经
        public string D28 { set; get; } //"D28":"",//
        public string D29 { set; get; } //"D29":""，//
        public string D30 { set; get; } //"D30":"风疹病毒,流感病毒",//病毒感染
        public string D31 { set; get; } //"D31":"无",//发热
        public string D32 { set; get; } //"D32":"有害化学物质,有害生物物质",//接触有害物质
        public string D33 { set; get; } //"D33":"2",//饮酒      //  饮酒
        public string D34 { set; get; } //"D34":"2",//两/天、
        public string D35 { set; get; } //"D35":"无",//服用药物  // 预防接种
        public string D36 { set; get; } //"D36":"无",//服用药物
        public string D37 { set; get; } //"D37":"33",//
        public string D38 { set; get; } //"D38":"22",//
        public string D39 { set; get; } //"D39":"",//
        public string D40 { set; get; } //"D40":"",//
        public string D41 { set; get; } //"D41":"2020-06-03",//早期B超时间
        public string D42 { set; get; } //"D42":"1",//胎数
        public string D43 { set; get; } //"D43":"2",//吸毒
        public string D44 { set; get; } //"D44":"2",//
        public string D45 { set; get; } //"D45":"2020-06-01",//取卵日期
        public string D46 { set; get; } //"D46":"1",//本孕首次产检地点
        public string D47 { set; get; }  //"D47":"外伤史",//外伤史
        public string D48 { set; get; } //"D48":"遗传病史",//遗传病史
        public string D49 { set; get; } //"D49":"2,3",//残疾情况
    }
}
