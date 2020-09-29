using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.Research
{
    [Table(TableName)]
    public class Check
    {
        public const string TableName = "R_Check";

        public string PatientId { set; get; }
        public string CheckOrderId { set; get; }
        public string InspectionId { set; get; }
        public string InspectionName { set; get; }
        public string Value { set; get; }
        public string IssueTime { set; get; }
        public string IssueTime2 { set; get; }
    }
}
