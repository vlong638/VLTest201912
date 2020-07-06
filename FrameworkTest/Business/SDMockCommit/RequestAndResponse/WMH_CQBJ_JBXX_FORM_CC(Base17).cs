using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class WMH_CQBJ_JBXX_FORM_CC
    {
        public string total { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string scr { set; get; }
        public List<WMH_CQBJ_JBXX_FORM_CC_Data> data { set; get; }
    }
    public class WMH_CQBJ_JBXX_FORM_CC_Data
    {
        public string D1 { set; get; } //:"A94521D025C475B5E05355FE8013B97B",
        public string D2 { set; get; } //:"马欣萍",
        public string D3 { set; get; } //:"",
        public string D4 { set; get; } //:"4406060121510248",
        public string D5 { set; get; } //:"45212619890415336X",
        public string D6 { set; get; } //:"1989-04-15",
        public string D7 { set; get; } //:"佛山市顺德区妇幼保健院",
        public string D8 { set; get; } //:"2020-06-30",
        public string D9 { set; get; } //:"",
        public string D10 { set; get; } //:"",
        public string D11 { set; get; } //:"",
        public string D12 { set; get; } //:"",
        public string D13 { set; get; } //:"",
        public string D14 { set; get; } //:"",
        public string D15 { set; get; } //:"",
        public string D16 { set; get; } //:"",
        public string D17 { set; get; } //:""

        public string PersonName { get { return D2; } }
        public string CareId { get { return D4; } }
        public string IdCard { get { return D5; } }
    }
}
