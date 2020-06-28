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

        public string CareId { get { return data.First().D1; } }
        public string MainId { get { return data.First().D2; } }
        public string IdCard { get { return data.First().D4; } }
        public string BaseId { get { return data.First().D8; } }
        public bool IsAvailable { get { return data.Count != 0; } }
    }
    public class WCQBJ_CZDH_DOCTOR_READData
    {
        /// <summary>
        /// CareId
        /// </summary>
        public string D1 { set; get; }//CareId
        /// <summary>
        /// MainId
        /// </summary>
        public string D2 { set; get; }//MainId
        public string D3 { set; get; }//姓名
        public string D4 { set; get; }//身份证
        public string D5 { set; get; }
        public string D6 { set; get; }//创建机构
        public string D7 { set; get; }//某日期
        /// <summary>
        /// BaseId
        /// </summary>
        public string D8 { set; get; }//基档,BaseId

        public string CareId { get { return D1; } }
        public string MainId { get { return D2; } }
        public string BaseId { get { return D8; } }
    }
}
