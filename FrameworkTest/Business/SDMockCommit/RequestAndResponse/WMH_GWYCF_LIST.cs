using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class WMH_GWYCF_LIST
    {
        public string total { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string scr { set; get; }
        public List<WMH_GWYCF_LIST_Data> data { set; get; }
    }
    public class WMH_GWYCF_LIST_Data
    {
        internal string Id { get { return D17; } }
        internal string Name { get { return D3; } }
        internal string ColorText { get { return D16; } }


        public string D1 { set; get; }//D1 :"2020-07-07",
        public string D2 { set; get; }//D2 :"28+6",
        public string D3 { set; get; }//D3 :"早产",
        public string D4 { set; get; }//D4 :"",
        public string D5 { set; get; }//D5 :"2020-07-07",
        public string D6 { set; get; }//D6 :"28+6",
        public string D7 { set; get; }//D7 :"",
        public string D8 { set; get; }//D8 :"",
        public string D9 { set; get; }//D9 :"",
        public string D10 { set; get; }//D10 :"",
        public string D11 { set; get; }//D11 :"2020-07-07",
        public string D12 { set; get; }//D12 :"",
        public string D13 { set; get; }//D13 :"",
        public string D14 { set; get; }//D14 :"",
        public string D15 { set; get; }//D15 :"A9D11542F2F395F7E05355FE8013B8D1",
        public string D16 { set; get; }//D16 :"黄色(一般风险）",
        public string D17 { set; get; }//D17 :"10"
    }
}
