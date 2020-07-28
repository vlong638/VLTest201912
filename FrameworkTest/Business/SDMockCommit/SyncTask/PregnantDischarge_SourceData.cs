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
    public class PregnantDischarge_SourceData : SourceDataForESB
    {
        public PregnantDischargeModel SourceData;

        public PregnantDischarge_SourceData(PregnantDischargeModel pe)
        {
            this.SourceData = pe;
        }

        public string inp_no => SourceData.inp_no;
        public string idcard => SourceData.idcard;
        public string PersonName => SourceData.xingming;
        public string SourceId => SourceData.inp_no;
        public TargetType TargetType => TargetType.PregnantDischarge;
    }
}
