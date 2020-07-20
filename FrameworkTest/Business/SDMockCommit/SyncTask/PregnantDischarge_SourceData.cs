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
    public class PregnantDischarge_SourceData : SourceData
    {
        public PregnantDischarge SourceData;
        internal string chiefcomplaint;

        public PregnantDischarge_SourceData(PregnantDischarge pe)
        {
            this.SourceData = pe;
        }

        //public string IdCard => SourceData.idcard;
        //public string PersonName => SourceData.personname;
        //public string SourceId => SourceData.Id.ToString();

        public string IdCard => "SourceData.idcard";
        public string PersonName => "SourceData.personname";
        public string SourceId => "SourceData.Id.ToString()";
        public TargetType TargetType => TargetType.PregnantDischarge;
    }
}
