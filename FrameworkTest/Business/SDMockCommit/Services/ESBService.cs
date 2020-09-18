﻿using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;

namespace FrameworkTest.Business.SDMockCommit
{
    public class HISService
    {
        public static string ConntectingString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=201.201.201.81)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=MyOracleSID)));User Id=jhmz;Password=jhmz;";
        private DbContext DBContext;

        public HISService(DbContext context)
        {
            this.DBContext = context;
        }

        #region BirthDefect

        internal List<BirthDefect> GetBirthDefects(string idcard)
        {
            return HISDAL.GetBirthDefects(DBContext.DbGroup, idcard);
        }

        #endregion
    }

    public class ESBService
    {
        public static string ConntectingString = "Data Source=201.201.201.89;Initial Catalog=HELEESB;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
        private DbContext DBContext;

        public ESBService(DbContext context)
        {
            this.DBContext = context;
        }

        #region SyncOrder

        public SyncOrder GetSyncOrder(TargetType TargetType, string sourceId)
        {
            return ESBDAL.GetSyncForFS(DBContext.DbGroup, TargetType, sourceId);
        }

        public long SaveSyncOrder(SyncOrder syncForFS)
        {
            if (syncForFS.Id > 0)
            {
                ESBDAL.UpdateSyncForFS(DBContext.DbGroup, syncForFS);
                return syncForFS.Id;
            }
            else
            {
                return ESBDAL.InsertSyncForFS(DBContext.DbGroup, syncForFS);
            }
        }

        #endregion

        #region PregnantDischarge

        internal int UpdatePregnantDischarge()
        {
            return ESBDAL.UpdatePregnantDischarge(DBContext.DbGroup);
        }

        internal IEnumerable<PregnantDischargeModel> GetPregnantDischargesToCreate()
        {
            return ESBDAL.GetPregnantDischargesToCreate(DBContext.DbGroup);
        }

        internal IEnumerable<PregnantDischargeModel> GetPregnantDischargesToUpdate()
        {
            return ESBDAL.GetPregnantDischargesToUpdate(DBContext.DbGroup);
        }

        internal object GetBirthDefects(string inp_no, string visit_id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ChildDischarge

        internal IEnumerable<ChildDischargeModel> GetChildDischargesToCreate()
        {
            return ESBDAL.GetChildDischargesToCreate(DBContext.DbGroup);
        }

        internal IEnumerable<ChildDischargeModel> GetChildDischargesToUpdate()
        {
            return ESBDAL.GetChildDischargesToUpdate(DBContext.DbGroup);
        }

        #endregion

        #region Diagnosis

        internal IEnumerable<Diagnosis> GetDiagnosisByPatientIdAndINPNo(string patientId, string visitId)
        {
            return ESBDAL.GetDiagnosisByPatientIdAndVisitId(DBContext.DbGroup, patientId, visitId);
        }

        #endregion

        #region Advice

        internal List<Advice> GetAdvicesByPatientId(string patientId)
        {
            return ESBDAL.GetAdvicesByPatientId(DBContext.DbGroup, patientId);
        }

        #endregion

        #region Inspection

        internal List<Inspection> GetInspectionsByPatientId(string patientId)
        {
            return ESBDAL.GetInspectionsByPatientId(DBContext.DbGroup, patientId);
        }

        #endregion
    }
}
