using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{

    public class WMH_CQBJ_CQJC_PRE_READ
    {
        public string total { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string scr { set; get; }
        public List<WMH_CQBJ_CQJC_PRE_READ_Data> data { set; get; }

    }
    public class WMH_CQBJ_CQJC_PRE_READ_Data
    {
        public string IssueDate { get { return D2 + "-" + D3; } }

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
    }
}
