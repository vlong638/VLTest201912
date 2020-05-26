using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Kettle
{
    public class O_LabResult
    {
        public long ID { set; get; }
        public long patientid { set; get; }
        public string idcard { set; get; }
        public string name { set; get; }
        public string orderid { set; get; }
        public int setid { set; get; }
        public string itemid { set; get; }
        public string itemname { set; get; }
        public string value { set; get; }
        public string unit { set; get; }
        public string reference { set; get; }
        public long resultflag { set; get; }
        public long status { set; get; }
        public DateTime deliverydate { set; get; }
    }
}
