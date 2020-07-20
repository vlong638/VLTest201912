using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FrameworkTest.Business.SDMockCommit
{
    [Table("cnarea")]
    public class cnarea
    {
        public string id { set; get; }
        public string level { set; get; }
        public string parent_code { set; get; }
        public string area_code { set; get; }
        public string zip_code { set; get; }
        public string city_code { set; get; }
        public string name { set; get; }
        public string short_name { set; get; }
        public string merger_name { set; get; }
        public string pinyin { set; get; }
        public string lng { set; get; }
        public string lat { set; get; }
        public string message { set; get; }
    }
}
