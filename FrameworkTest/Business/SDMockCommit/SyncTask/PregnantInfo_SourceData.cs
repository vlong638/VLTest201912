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
    public class PregnantInfo_SourceData : SourceData
    {
        public PregnantInfo SourceData;

        public PregnantInfo_SourceData(PregnantInfo pe)
        {
            this.SourceData = pe;
        }

        public string IdCard => SourceData.idcard;
        public string PersonName => SourceData.personname;
        public string SourceId => SourceData.Id.ToString();
        public SourceType SourceType => SourceType.PregnantInfo;
    }
}
