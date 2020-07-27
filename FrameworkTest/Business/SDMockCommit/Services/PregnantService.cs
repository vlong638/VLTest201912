using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FrameworkTest.Business.SDMockCommit
{
    public class PregnantService
    {
        public static string ConntectingString = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";

        private DbContext DBContext;

        public PregnantService(DbContext context)
        {
            this.DBContext = context;
        }


        #region SyncOrder

        public SyncOrder GetSyncOrder(TargetType TargetType, string sourceId)
        {
            return PregnantDAL.GetSyncForFS(DBContext.DbGroup, TargetType, sourceId);
        }

        public long SaveSyncOrder(SyncOrder syncForFS)
        {
            if (syncForFS.Id > 0)
            {
                PregnantDAL.UpdateSyncForFS(DBContext.DbGroup, syncForFS);
                return syncForFS.Id;
            }
            else
            {
                return PregnantDAL.InsertSyncForFS(DBContext.DbGroup, syncForFS);
            }
        }

        #endregion

        #region PregnantInfo

        public IEnumerable<PregnantInfo> GetPregnantInfoForUpdate()
        {
            return PregnantDAL.GetPregnantInfoForUpdate(DBContext.DbGroup);
        }

        public IEnumerable<PregnantInfo> GetPregnantInfoForCreate()
        {
            return PregnantDAL.GetPregnantInfoForCreate(DBContext.DbGroup);
        }

        /// <summary>
        /// 0703 开放边界
        /// </summary>
        /// <returns></returns>
        public List<PregnantInfo> GetPregnantInfoForCreateOrUpdate()
        {
            return PregnantDAL.GetPregnantInfoForCreateOrUpdate(DBContext.DbGroup);
        }

        #endregion

        #region Enquiry

        internal IEnumerable<PregnantInfo> GetPregnantInfosToCreateEnquiries()
        {
            return PregnantDAL.GetPregnantInfosToCreateEnquiries(DBContext.DbGroup);
        }

        internal IEnumerable<PregnantInfo> GetPregnantInfosToUpdateEnquiries()
        {
            return PregnantDAL.GetPregnantInfosToUpdateEnquiries(DBContext.DbGroup);
        }

        #endregion

        #region PhysicalExamination

        public List<PhysicalExaminationModel> GetPhysicalExaminationsToCreate()
        {
            return PregnantDAL.GetPhysicalExaminationsToCreate(DBContext.DbGroup);
        }

        public IEnumerable<PhysicalExaminationModel> GetPhysicalExaminationsToUpdate()
        {
            return PregnantDAL.GetPhysicalExaminationsToUpdate(DBContext.DbGroup);
        }

        #endregion

        #region ProfessionalExamination

        public List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreate()
        {
            //return SDDAL.GetProfessionalExaminationsToCreateByIdCard(DBContext.DbGroup, "142328199610271518");
            return PregnantDAL.GetProfessionalExaminationsToCreate(DBContext.DbGroup);
        }

        public IEnumerable<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdate()
        {
            //return SDDAL.GetProfessionalExaminationsToUpdateByIdCard(DBContext.DbGroup, "142328199610271518");
            return PregnantDAL.GetProfessionalExaminationsToUpdate(DBContext.DbGroup);
        }

        #endregion

        #region HighRisk

        internal List<HighRiskEntity> GetLatestHighRisksByIdCard(string idcard)
        {
            var highRsiksStr = PregnantDAL.GetLatestHighRisksByIdCard(DBContext.DbGroup, idcard);
            return highRsiksStr?.FromJson<List<HighRiskEntity>>() ?? new List<HighRiskEntity>();
        }

        #endregion

    }
}
