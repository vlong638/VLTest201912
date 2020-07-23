using Dapper;
using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FrameworkTest.Business.SDMockCommit
{
    public class Enquiry_SourceData : SourceDataForPregnant
    {
        public PregnantInfo SourceData;

        public Enquiry_SourceData(PregnantInfo pe)
        {
            this.SourceData = pe;
        }

        //public string IdCard => SourceData.idcard;
        //public string PersonName => SourceData.personname;
        //public string SourceId => SourceData.Id.ToString();

        public string IdCard => SourceData.idcard;
        public string PersonName => SourceData.personname;
        public string SourceId => SourceData.Id.ToString();
        public TargetType TargetType => TargetType.PregnantInfo;
    }
}
