using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class WMH_GWYCF_GW_LIST1
    {
        public string total { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string scr { set; get; }
        public List<WMH_GWYCF_GW_LIST1_Data> data { set; get; }
    }
    public class WMH_GWYCF_GW_LIST1_Data
    {
        internal string Id { get { return D9; } }
        internal string Name { get { return D5; } }
        internal string ColorText { get { return D6; } }

        public string D1 { set; get; }//:"A9C39D1285D0A96BE05355FE8013EFEA",
        public string D2 { set; get; }//:"A8A7AEAD72C162A2E05355FE801348F3", //MainId 但有些有 有些没
        public string D3 { set; get; }//:"基本情况",
        public string D4 { set; get; }//:"年龄",
        public string D5 { set; get; }//:"年龄：≤18岁",
        public string D6 { set; get; }//:"黄色(一般风险）",
        public string D7 { set; get; }//:"转高危妊娠门诊",
        public string D8 { set; get; }//:"0",
        public string D9 { set; get; }//:"1",
        public string D10 { set; get; }//:"",
        public string D11 { set; get; }//:"取本次评估时年龄值",
        public string D12 { set; get; }//:"Ⅱ级",
        public string D13 { set; get; }//:"1",
        public string D14 { set; get; }//:"",
        public string D15 { set; get; }//:""
    }
}
