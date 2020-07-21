using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class CQJL_LIST
    {
        public string total { set; get; }
        public string dsc { set; get; }
        public string code { set; get; }
        public string scr { set; get; }
        public List<CQJL_LIST_Data> data { set; get; }
    }
	public class CQJL_LIST_Data
	{
		internal string FMMainId { get { return D18; } }

		public string D1 { set; get; } //:"佛山市顺德区妇幼保健院",
		public string D2 { set; get; } //:"0000309329", //住院号
		public string D3 { set; get; } //:"梁嘉仪",
		public string D4 { set; get; } //:"26",
		public string D5 { set; get; } //:"2020-04-23",
		public string D6 { set; get; } //:"剖宫产",
		public string D7 { set; get; } //:"1",
		public string D8 { set; get; } //:"",
		public string D9 { set; get; } //:"",
		public string D10 { set; get; } //:"1",
		public string D11 { set; get; } //:"未填写",
		public string D12 { set; get; } //:"未填写",
		public string D13 { set; get; } //:"未填写",
		public string D14 { set; get; } //:"未填写",
		public string D15 { set; get; } //:"P",
		public string D16 { set; get; } //:"45608491-9",
		public string D17 { set; get; } //:"A3EE7138B614156DE05355FE8013B717",
		public string D18 { set; get; } //:"A3F0A29DECC6FCF2E05355FE8013A12A", //基础主键Id
		public string D19 { set; get; } //:"A3EE860028BA0203E05355FE8013C14F",
		public string D20 { set; get; } //:"2",
		public string D21 { set; get; } //:"440681199312163700"
	}
}
