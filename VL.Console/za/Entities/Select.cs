using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consoling.Entities
{
    public class Select
    {
        public string id { set; get; }
        public string sourece { set; get; }
        public List<Option> Options { set; get; } = new List<Option>();
    }

    public class Option
    {
        public string id { set; get; }
        public string value { set; get; }
        public string text { set; get; }
        public string selected { set; get; }
        public string groupno { set; get; }
        public string rule { set; get; }
        public string remark { set; get; }
        public string editable { set; get; }
        public string dictionary { set; get; }
        public string classfiction { set; get; }
        public string auto { set; get; }
        public string version { set; get; }
        public string parentid { set; get; }
    }
}
