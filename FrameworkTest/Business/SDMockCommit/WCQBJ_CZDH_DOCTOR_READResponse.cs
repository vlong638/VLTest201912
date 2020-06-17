using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class WCQBJ_CZDH_DOCTOR_READResponse
    {
        public string total { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string scr { set; get; }
        public List<WCQBJ_CZDH_DOCTOR_READData> data { set; get; }
    }
    public class WCQBJ_CZDH_DOCTOR_READData
    {
        public string D1 { set; get; }//
        public string D2 { set; get; }//
        public string D3 { set; get; }//姓名
        public string D4 { set; get; }//身份证
        public string D5 { set; get; }
        public string D6 { set; get; }
        public string D7 { set; get; }
        public string D8 { set; get; }
    }
}
