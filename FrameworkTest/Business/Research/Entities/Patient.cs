using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.Research
{
    [Table(TableName)]
    public class Patient
    {
        public const string TableName = "R_Patient";

        public string id { set; get; }
        public string idcard { set; get; }
        public string age { set; get; }
        public string height { set; get; }
        public string gravidity { set; get; }
        public string parity { set; get; }
        public string lastmenstrualperiod { set; get; }
        public string bloodtypecode { set; get; }
        public string rhbloodcode { set; get; }
        public string homeplace { set; get; }
        public string address { set; get; }
        public string homeaddress { set; get; }
        public string birthday { set; get; }
        public string educationcode { set; get; }
        public string contact { set; get; }
        public string contactphone { set; get; }
        public string workplace { set; get; }
        public string predeliverymode { set; get; }
        public string sbp { set; get; }
        public string dbp { set; get; }
        public string menarcheage { set; get; }
        public string menstrualperiod { set; get; }
        public string cycle { set; get; }
        public string menstrualblood { set; get; }
        public string dysmenorrhea { set; get; }
        public string lastmenstrualperiod2 { set; get; }
    }
}
