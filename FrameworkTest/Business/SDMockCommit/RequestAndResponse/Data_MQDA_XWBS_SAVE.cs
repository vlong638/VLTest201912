using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 2.问询病史
    /// </summary>
    public class Data_MQDA_XWBS_SAVE
    {
        public Data_MQDA_XWBS_SAVE(MQDA_READ_NEWData data)
        {
            if (data != null)
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
                pagesize = "1000";
            }
            else
            {
                this.D1 = "";
                this.D2 = "";
                this.D3 = "";
                this.D4 = "";
                this.D5 = "尿妊娠试验";
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
                pagesize = "1000";
            }
        }

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
        public string pagesize { set; get; } //"1000"

        static string DateFormat = "yyyy-MM-dd";
        public void Update(PregnantInfo pregnantInfo)
        {
            this.D1 = pregnantInfo.lastmenstrualperiod?.ToString(DateFormat) ?? ""; //末次月经
            this.D2 = pregnantInfo.dateofprenatal?.ToString(DateFormat) ?? "";
            this.D3 = VLConstraints.GetPregnancyMannerByPregnancyManner_Hele(pregnantInfo.tpregnancymanner);
            this.D4 = pregnantInfo.implanttime?.ToString(DateFormat) ?? "";  //"D4":"2020-06-02", //受精或移植日期 //植入时间
            //this.D5   //"D5":"尿妊娠试验,B超检查",//妊娠确认方法
            //this.D6   //"D6":"4",//B超确认孕周
            //this.D7   //"D7":"有",//早孕反应
            //this.D8   //"D8":"5",//开始孕周
            //this.D9   //"D9":"6",//胎动开始孕周
            //this.D10  //"D10":"头痛（开始孕周    ）,//自觉症状
            this.D11 = pregnantInfo.pasthistory ?? "";
            //this.D12  //"D12":"无",//
            this.D13 = pregnantInfo.gynecologyops ?? ""; //"D13":"剖宫产术,腹腔镜子宫肌瘤剔除术",//手术史
            //this.D14 = pregnantInfo.D14; //"D14":"未发现",//
            this.D15 = pregnantInfo.allergichistory ?? ""; //"D15":"青霉素,喹诺酮类",//过敏史
            this.D16 = pregnantInfo.bloodtransfution ?? ""; //"D16":"有",//输血史
            //this.D17 = pregnantInfo.D17; //"D17":"",//
            //this.D18 = pregnantInfo.familyhistory; //"D18":"地中海贫血,G6PD缺乏症",//本人家族史
            //this.D19 = pregnantInfo.D19; //"D19":"2",//亲缘关系
            //this.D20 = pregnantInfo.D20; //"D20":"地中海贫血,G6PD缺乏症",//配偶家族史
            //this.D21 = pregnantInfo.D21; //"D21":"2",//亲缘关系
            //this.D22 = pregnantInfo.D22; //"D22":"",//
            this.D23 = pregnantInfo.menarcheage ?? ""; //"D23":"11",//月经初潮年龄
            this.D24 = pregnantInfo.menstrualperiodmin + "-" + pregnantInfo.menstrualperiodmax; //"D24":"22",//月经持续时间
            this.D25 = pregnantInfo.cyclemin + "-" + pregnantInfo.cyclemax; //"D25":"33",//间歇时间
            this.D26 = pregnantInfo.menstrualblood.GetDescription() ?? ""; //"D26":"中",//经量
            this.D27 = VLConstraints.GetDysmenorrheaByDysmenorrhea_Hele(pregnantInfo.dysmenorrhea); //"D27":"有",//痛经
            //this.D28 = pregnantInfo.D28; //"D28":"",//
            //this.D29 = pregnantInfo.D29; //"D29":""，//
            //this.D30 = pregnantInfo.D30; //"D30":"风疹病毒,流感病毒",//病毒感染
            //this.D31 = pregnantInfo.D31; //"D31":"无",//发热
            this.D32 = pregnantInfo.poisontouchhis ?? ""; //"D32":"有害化学物质,有害生物物质",//接触有害物质
            //this.D33 = pregnantInfo.D33; //"D33":"2",//饮酒      //  饮酒
            //this.D34 = pregnantInfo.D34; //"D34":"2",//两/天、
            //this.D35 = pregnantInfo.D35; //"D35":"无",//服用药物  // 预防接种
            //this.D36 = pregnantInfo.D36; //"D36":"无",//服用药物
            //this.D37 = pregnantInfo.D37; //"D37":"33",//
            //this.D38 = pregnantInfo.D38; //"D38":"22",//
            //this.D39 = pregnantInfo.D39; //"D39":"",//
            //this.D40 = pregnantInfo.D40; //"D40":"",//
            //this.D41 = pregnantInfo.D41; //"D41":"2020-06-03",//早期B超时间
            //var pregnanthistorys = pregnantInfo.pregnanthistory?.FromJson<List<pregnanthistory>>();
            //if (pregnanthistorys == null || pregnanthistorys.Count == 0)
            //{
            //    this.D42 = "1"; //"D42":"1",//胎数
            //}
            //else
            //{
            //    switch (pregnanthistorys.Count())
            //    {
            //        case 1:
            //            this.D42 = "1"; //"D42":"1",//胎数
            //            break;
            //        case 2:
            //            this.D42 = "2"; //"D42":"1",//胎数
            //            break;
            //        default:
            //            this.D42 = "3"; //"D42":"1",//胎数
            //            break;
            //    }
            //}
            //this.D43 = pregnantInfo.D43; //"D43":"2",//吸毒
            //this.D44 = pregnantInfo.D44; //"D44":"2",//
            this.D45 = pregnantInfo.eggretrievaltime?.ToString(DateFormat) ?? ""; //"D45":"2020-06-01",//取卵日期
            this.D46 = "1"; //"D46":"1",//本孕首次产检地点
            //this.D47 = pregnantInfo.D47;  //"D47":"外伤史",//外伤史
            this.D48 = pregnantInfo.heredityfamilyhistory ?? ""; //"D48":"遗传病史",//遗传病史
            //this.D49 = pregnantInfo.D49; //"D49":"2,3",//残疾情况
        }
    }
}
