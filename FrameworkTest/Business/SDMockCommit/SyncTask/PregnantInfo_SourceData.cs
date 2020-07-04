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
        public PregnantInfo Data;

        public PregnantInfo_SourceData(PregnantInfo pe)
        {
            this.Data = pe;
        }

        public string IdCard => Data.idcard;
        public string PersonName => Data.personname;
        public string SourceId => Data.Id.ToString();
        public SourceType SourceType => SourceType.PregnantInfo;
    }
}
