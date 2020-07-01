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
    public class SourceData_PhysicalExaminationModel : PhysicalExaminationModel, SourceData
    {
        public string SourceId => Id.ToString();
        public SourceType SourceType => SourceType.PregnantInfo;
    }

    public class SyncTask_PhysicalExaminationModel : SyncTaskBO<SourceData_PhysicalExaminationModel>
    {
        public override List<SourceData_PhysicalExaminationModel> GetSourceDatas(ServiceContext context, UserInfo userInfo)
        {
           return context.SDService.GetPhysicalExaminationsToCreate();
        }

        public override bool DoCommit(UserInfo userInfo, SourceData_PhysicalExaminationModel sourceData, StringBuilder logger, ref string errorMessage)
        {
            throw new NotImplementedException();
        }

        public override bool IsExist(UserInfo userInfo, SourceData_PhysicalExaminationModel SourceData, StringBuilder logger, ref string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
