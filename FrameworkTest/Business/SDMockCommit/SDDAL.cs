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

        #region SyncOrder
        public static bool UpdateSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Update(syncForFS, transaction: group.Transaction);
        }

        internal static SyncOrder GetSyncForFS(DbGroup group, SourceType sourceType, string sourceId)
        {
            return group.Connection.Query<SyncOrder>("select * from SyncForFS where sourceType = @sourceType and sourceId = @sourceId", new { sourceType, sourceId }, transaction: group.Transaction).FirstOrDefault();
        } 
        #endregion

        #region PhysicalExamination
        #endregion

        #region ProfessionalExamination
        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreateByIdCard(DbGroup dbGroup, string idcard)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id
,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal 
,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS sp on sp.SourceType = 3 and sp.SourceId = vr.Id
left join SyncForFS se on se.SourceType = 4 and se.SourceId = vr.Id			
where vr.idcard = @idcard
", new { idcard }, transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id
,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal 
,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS sp on sp.SourceType = 3 and sp.SourceId = vr.Id
left join SyncForFS se on se.SourceType = 4 and se.SourceId = vr.Id			
where sp.id is not null and sp.SyncStatus in (2,11) 
and se.id is null 
and vr.visitdate = convert(nvarchar,getdate(),23)
", transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id
,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal 
,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS se on se.SourceType = 4 and se.SourceId = vr.Id			
where vr.visitdate = convert(nvarchar,getdate(),23)
and se.id is not null and se.SyncStatus in (2,11) 
and vr.updatetime > DATEADD( SECOND,10 ,se.SyncTime)
", transaction: dbGroup.Transaction).ToList();
        } 
//and vr.idcard = '142328199610271518'
        #endregion

        #region MyRegion
        internal static List<PregnantInfo> GetPregnantInfoForCreateOrUpdate(DbGroup dbGroup)
        {
            //指定日期之后 非今天 当年会与常规新建和更新重复
            return dbGroup.Connection.Query<PregnantInfo>($@"
select
Top 1
s.id sid,
pi.createtime, pi.updatetime,
pi.*
from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is null 
and pi.createtime < convert(nvarchar, getdate(),23) 
and pi.updatetime> '2020-07-01'
", transaction: dbGroup.Transaction).ToList();

            //当天
            //            return dbGroup.Connection.Query<PregnantInfo>($@"
            //select Top 1 
            //s.id sid,
            //pi.createtime, pi.updatetime,
            //pi.*
            //from PregnantInfo pi
            //left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
            //where s.id is null 
            //and pi.createtime < convert(nvarchar, getdate(),23) 
            //and pi.updatetime> convert(nvarchar, getdate(),23)
            //", transaction: dbGroup.Transaction).ToList();
        }

        internal static IEnumerable<PregnantInfo> GetPregnantInfoForCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantInfo>($@"
select Top 1 s.id sid,
pi.createtime,
pi.* from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is null and pi.createtime>'2020-06-29 09:00:00'
order by pi.createtime
", transaction: dbGroup.Transaction).ToList();
        }

        internal static IEnumerable<PregnantInfo> GetPregnantInfoForUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantInfo>($@"
select Top 1 s.id sid
,s.SyncTime
,pi.createtime,pi.updatetime
,pi.* from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is not null and s.SyncStatus in (2,11)
and pi.updatetime > DATEADD( SECOND,10 ,s.SyncTime)
", transaction: dbGroup.Transaction).ToList();
        } 
        #endregion

    }
}
