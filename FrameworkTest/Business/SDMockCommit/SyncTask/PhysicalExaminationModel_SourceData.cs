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
    public class PhysicalExaminationModel_SourceData : SourceDataForPregnant
    {
        public PhysicalExaminationModel SourceData;

        public PhysicalExaminationModel_SourceData(PhysicalExaminationModel pe)
        {
            this.SourceData = pe;
        }

        public string IdCard => SourceData.idcard;
        public string PersonName => SourceData.pi_personname;
        public string SourceId => SourceData.Id.ToString();
        public TargetType TargetType => TargetType.PhysicalExamination;
    }
}
