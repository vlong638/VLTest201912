﻿using Dapper;
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
            return SDDAL.GetSyncForFS(DBContext.DbGroup, TargetType, sourceId);
        }

        public long SaveSyncOrder(SyncOrder syncForFS)
        {
            if (syncForFS.Id > 0)
            {
                SDDAL.UpdateSyncForFS(DBContext.DbGroup, syncForFS);
                return syncForFS.Id;
            }
            else
            {
                return SDDAL.InsertSyncForFS(DBContext.DbGroup, syncForFS);
            }
        }
        #endregion

        #region PregnantInfo

        public IEnumerable<PregnantInfo> GetPregnantInfoForUpdate()
        {
            return SDDAL.GetPregnantInfoForUpdate(DBContext.DbGroup);
        }

        public IEnumerable<PregnantInfo> GetPregnantInfoForCreate()
        {
            return SDDAL.GetPregnantInfoForCreate(DBContext.DbGroup);
        }

        /// <summary>
        /// 0703 开放边界
        /// </summary>
        /// <returns></returns>
        public List<PregnantInfo> GetPregnantInfoForCreateOrUpdate()
        {
            return SDDAL.GetPregnantInfoForCreateOrUpdate(DBContext.DbGroup);
        }

        #endregion

        #region PhysicalExamination

        #endregion

        #region ProfessionalExamination

        public List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreate()
        {
            //return SDDAL.GetProfessionalExaminationsToCreateByIdCard(DBContext.DbGroup, "142328199610271518");
            return SDDAL.GetProfessionalExaminationsToCreate(DBContext.DbGroup);
        }

        public IEnumerable<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdate()
        {
            //return SDDAL.GetProfessionalExaminationsToUpdateByIdCard(DBContext.DbGroup, "142328199610271518");
            return SDDAL.GetProfessionalExaminationsToUpdate(DBContext.DbGroup);
        }

        #endregion

    }
}