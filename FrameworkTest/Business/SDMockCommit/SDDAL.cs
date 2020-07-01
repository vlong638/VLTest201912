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
    public class SDDAL
    {
        public static long InsertSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Insert(syncForFS, transaction: group.Transaction);
        }

        public static bool UpdateSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Update(syncForFS, transaction: group.Transaction);
        }

        internal static SyncOrder GetSyncForFS(DbGroup group, SourceType sourceType, string sourceId)
        {
            return group.Connection.Query<SyncOrder>("select * from SyncForFS where sourceType = @sourceType and sourceId = @sourceId", new { sourceType, sourceId }, transaction: group.Transaction).FirstOrDefault();
        }

        internal static List<SourceData_PhysicalExaminationModel> GetPhysicalExaminationsToCreate(DbGroup group)
        {
            throw new NotImplementedException();
//            return group.Connection.Query<PhysicalExaminationModel>($@"
//select 
//T1.*
//,pi_data.personname pi_personname
//,pi_data.weight pi_weight
//,pi_data.height pi_height
//,pi_data.bmi pi_bmi
//,vr_data.Id
//,vr_data.weight
//,vr_data.temperature
//,vr_data.heartrate
//,vr_data.dbp
//,vr_data.sbp
//from 
//(
//		SELECT 
//		vr.idcard
//		,max(vr.visitdate) lastestvisitdate
//		,min(vr.visitdate) firstvisitdate		
//		FROM (
//				SELECT top 1
//				pi.idcard
//				FROM PregnantInfo pi 
//				LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
//				left join SyncForFS sp on sp.SourceType = 1 and sp.SourceId = pi.Id
//				left join SyncForFS se on se.SourceType = 3 and se.SourceId = vr.Id			
//				where sp.id is not null and sp.SyncStatus in (2,11) 
//				and se.id is null 
//				and vr.visitdate = convert(nvarchar,getdate(),23)
//		)  as toCreate 
//		LEFT JOIN MHC_VisitRecord vr on toCreate.idcard = vr.idcard 
//		GROUP BY vr.idcard
//) as T1
//left join PregnantInfo pi_data on pi_data.idcard = T1.idcard
//left join MHC_VisitRecord vr_data on vr_data.idcard = T1.idcard and vr_data.visitdate = T1.lastestvisitdate
//", transaction: group.Transaction).ToList();
        }
    }
}
