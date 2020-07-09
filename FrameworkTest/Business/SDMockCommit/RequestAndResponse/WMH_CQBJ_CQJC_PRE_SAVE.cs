using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class WMH_CQBJ_CQJC_PRE_SAVE
    {
        public WMH_CQBJ_CQJC_PRE_SAVE(WMH_CQBJ_CQJC_PRE_READ_Data data)
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
        }

        public WMH_CQBJ_CQJC_PRE_SAVE()
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
        }

        public void UpdateEnquiry(PregnantInfo pregnantInfo, pregnanthistory pregnanthistory)
        {
            //[{"index":"0","pregstatus":"剖宫产,剖宫产-足月","babysex":"2","babyweight":"2900","pregnantage":"2012-11"},{"index":"2","pregstatus":"人流","babysex":"9","babyweight":"","pregnantage":"2014-05"},{"index":"3","pregstatus":"人流","babysex":"9","babyweight":"","pregnantage":"2018-10"}]
            var issueDataStr = pregnanthistory.pregnantage;
            var issueDataValue = issueDataStr.Split('-');
            //public string D1 { set; get; } //生育史Id  KO
            this.D2 = pregnanthistory.PregnantageIndex?.ToString() ?? "";//public string D2 { set; get; } //孕次
            this.D3 = issueDataValue.Length == 2 ? issueDataValue[0] : "";//public string D3 { set; get; } //年  KO
            this.D4 = issueDataValue.Length == 2 ? issueDataValue[1] : "";//public string D4 { set; get; } //月  KO
            this.D5 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("自然流产")) != null ? "1" : ""; //public string D5 { set; get; } //流产-自然  KO
            this.D6 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("人流")) != null ? "1" : "";//public string D6 { set; get; } //流产-人工  KO
            this.D7 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("引产")) != null ? "1" : "";//public string D7 { set; get; } //流产-引产  KO            
            this.D8 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("葡萄胎")) != null ? "1" : "";//public string D8 { set; get; } //葡萄胎  KO
            this.D9 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("宫外孕")) != null ? "1" : "";//public string D9 { set; get; } //宫外孕  KO
            this.D10 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("死胎")) != null ? "1" : "";//public string D10 { set; get; } //死胎  KO
            this.D11 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("死产")) != null ? "1" : "";//public string D11 { set; get; } //死产   KO
            this.D12 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("早产")) != null ? "1" : "";//public string D12 { set; get; } //早产   KO
            this.D13 = VLConstraints.GetVaginalDeliveryType(pregnanthistory.Pregstatuss);//public string D13 { set; get; } //阴道分娩  KO 可以配对
            this.D14 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("剖宫")) != null ? "1" : "";//public string D14 { set; get; } //剖宫产  KO
            this.D15 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("足月")) != null ? "1" : "";//public string D15 { set; get; } //足月产  KO
            this.D16 = VLConstraints.Get_BabySex_By_BabySex_HELE(pregnanthistory.babysex);//public string D16 { set; get; } //性别 KO 可以配对
            this.D17 = pregnanthistory.babyweight;  //public string D17 { set; get; } //出生体重/g KO 可以配对
            this.D18 = pregnanthistory.Pregstatuss.FirstOrDefault(c => c.Contains("顺产")) != null ? "√" : ""; //public string D18 { set; get; } //存活 KO "√"
            //public string D19 { set; get; } //死亡年龄 KO
            //public string D20 { set; get; } //死亡原因 KO
            //public string D21 { set; get; } //出生缺陷 KO
            //public string D22 { set; get; } //母婴并发症 KO
            //public string D23 { set; get; } //指征 KO
            //public string D24 { set; get; } //分娩地点 KO
            //public string D25 { set; get; } //备注 KO


            //<Option value="1" text="足月产-健"/>
            //<Option value="23" text="巨大胎"/>
            //<Option value="2" text="足月产-亡"/>
            //<Option value="3" text="早产-健"/>
            //<Option value="4" text="早产-亡"/>
            //<Option value="5" text="畸形-健"/>
            //<Option value="6" text="畸形-亡"/>
            //<Option value="7" text="双胎"/>
            //<Option value="8" text="死胎"/>
            //<Option value="9" text="死产"/>
            //<Option value="10" text="胎位异常"/>
            //<Option value="11" text="妊高症"/>
            //<Option value="12" text="前置胎盘"/>
            //<Option value="13" text="胎盘早剥"/>
            //<Option value="14" text="阴道手术"/>
            //<Option value="15" text="剖宫产"/>
            //<Option value="16" text="人流"/>
            //<Option value="17" text="自然流产"/>
            //<Option value="18" text="引产"/>
            //<Option value="19" text="药流"/>
            //<Option value="20" text="胎停"/>
            //<Option value="21" text="宫外孕"/>
        }


        public string _id { set; get; }
        public string _uid { set; get; } 
        public string _state { set; get; } 
        public string D1 { set; get; } //生育史Id  KO
        public string D2 { set; get; } //孕次  KO
        public string D3 { set; get; } //年  KO
        public string D4 { set; get; } //月  KO
        public string D5 { set; get; } //流产-自然  KO
        public string D6 { set; get; } //流产-人工  KO
        public string D7 { set; get; } //流产-引产  KO
        public string D8 { set; get; } //葡萄胎  KO
        public string D9 { set; get; } //宫外孕  KO
        public string D10 { set; get; } //死胎  KO
        public string D11 { set; get; } //死产   KO
        public string D12 { set; get; } //早产   KO
        public string D13 { set; get; } //阴道分娩  KO
        public string D14 { set; get; } //剖宫产  KO
        public string D15 { set; get; } //足月产  KO
        public string D16 { set; get; } //性别 KO
        public string D17 { set; get; } //出生体重/g KO
        public string D18 { set; get; } //存活 KO "√"

        public string D19 { set; get; } //死亡年龄 KO
        public string D20 { set; get; } //死亡原因 KO
        public string D21 { set; get; } //出生缺陷 KO
        public string D22 { set; get; } //母婴并发症 KO
        public string D23 { set; get; } //指征 KO
        public string D24 { set; get; } //分娩地点 KO
        public string D25 { set; get; } //备注 KO

        internal bool Validate(ref StringBuilder sb)
        {
            if (string.IsNullOrEmpty(D2))
            {
                sb.AppendLine("无效的{孕次}数据");
                return false;
            }
            if (string.IsNullOrEmpty(D3))
            {
                sb.AppendLine("无效的{年}数据");
                return false;
            }
            if (string.IsNullOrEmpty(D4))
            {
                sb.AppendLine("无效的{月}数据");
                return false;
            }
            return true;
        }
    }
}
