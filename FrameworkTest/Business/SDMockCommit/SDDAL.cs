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

        internal static SyncOrder GetSyncForFS(DbGroup group, TargetType TargetType, string sourceId)
        {
            return group.Connection.Query<SyncOrder>("select * from SyncForFS where TargetType = @TargetType and sourceId = @sourceId", new { TargetType, sourceId }, transaction: group.Transaction).FirstOrDefault();
        } 
        #endregion

        #region PhysicalExamination
        #endregion

        #region ProfessionalExamination
        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreateByIdCard(DbGroup dbGroup, string idcard)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id,vr.uterinecontraction,vr.amnioticfluidcharacter
,vr.weight,pi.bmi,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal 
,vr.dbp,vr.sbp,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis,vr.highriskdic
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS s3 on s3.TargetType = 3 and s3.SourceId = vr.Id
left join SyncForFS se on se.TargetType = 4 and se.SourceId = vr.Id			
where s3.id is not null and s3.SyncStatus in (2,11) 
and se.id is null 
and vr.visitdate = convert(nvarchar,getdate(),23)	
and vr.idcard = @idcard
", new { idcard }, transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id,vr.uterinecontraction,vr.amnioticfluidcharacter
,vr.weight,pi.bmi,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal,pi.createage,pi.birthday
,vr.dbp,vr.sbp,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis,vr.highriskdic
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS s3 on s3.TargetType = 3 and s3.SourceId = vr.Id
left join SyncForFS s4 on s4.TargetType = 4 and s4.SourceId = vr.Id			
where s3.id is not null and s3.SyncStatus in (2,11) 
and s4.id is null 
and vr.visitdate = convert(nvarchar,getdate(),23)
", transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdateByIdCard(DbGroup dbGroup, string idcard)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id,vr.uterinecontraction,vr.amnioticfluidcharacter
,vr.weight,pi.bmi,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal 
,vr.dbp,vr.sbp,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis,vr.highriskdic
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS s4 on s4.TargetType = 4 and s4.SourceId = vr.Id			
where vr.visitdate = convert(nvarchar,getdate(),23)
and s4.id is not null and s4.SyncStatus in (2,11) 
and vr.updatetime > DATEADD( SECOND,10 ,se.SyncTime)
and vr.idcard = @idcard
", new { idcard }, transaction: dbGroup.Transaction).ToList();
        }

        internal static List<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<ProfessionalExaminationModel>($@"
SELECT top 1
vr.id,vr.uterinecontraction,vr.amnioticfluidcharacter
,vr.weight,pi.bmi,pi.idcard,pi.personname,pi.lastmenstrualperiod,pi.dateofprenatal,pi.createage,pi.birthday
,vr.dbp,vr.sbp,vr.uterus,vr.palacemouth,vr.suggestion,vr.generalcomment,vr.followupappointment,vr.brokenwater,vr.multifetal
,vr.chiefcomplaint,vr.presenthistory,vr.heightfundusuterus,vr.abdomencircumference,vr.xianjie,vr.edemastatus
,vr.diagnosisinfo,vr.maindiagnosis,vr.secondarydiagnosis,vr.highriskdic
FROM PregnantInfo pi 
LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
left join SyncForFS s4 on s4.TargetType = 4 and s4.SourceId = vr.Id			
where vr.visitdate = convert(nvarchar,getdate(),23)
and s4.id is not null and s4.SyncStatus in (2,11) 
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
left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
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
            //left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
            //where s.id is null 
            //and pi.createtime < convert(nvarchar, getdate(),23) 
            //and pi.updatetime> convert(nvarchar, getdate(),23)
            //", transaction: dbGroup.Transaction).ToList();
        }

        internal static IEnumerable<PregnantInfo> GetPregnantInfoForCreate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantInfo>($@"
select Top 1
s.id sid,
pi.createtime, pi.updatetime,
pi.*
from PregnantInfo pi
left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
where s.id is null 
and pi.createtime>= convert(nvarchar, getdate(),23) 
union
(
    select
    Top 1
    s.id sid,
    pi.createtime, pi.updatetime,
    pi.*
    from PregnantInfo pi
    left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
    where s.id is null 
    and pi.createtime < convert(nvarchar, getdate(),23) 
    and pi.updatetime >= convert(nvarchar, getdate(),23)
)
union
(

	select 
	Top 1
	se.id sid,
	pi.createtime, pi.updatetime,
	pi.*
	FROM PregnantInfo pi 
	LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
	left join SyncForFS s4 on s4.TargetType = 4 and s4.SourceId = vr.Id			
	left join SyncForFS s1 on s1.TargetType = 1 and s1.SourceId = pi.Id			
	where vr.visitdate >='2020-07-13' and s4.id is null and s1.id is null
	and pi.createtime < convert(nvarchar, getdate(),23) 
	and pi.updatetime < convert(nvarchar, getdate(),23)
)
", transaction: dbGroup.Transaction).ToList();
            //TODO 需要扩展编辑过初诊 但没有档案同步记录的数据 走11状态码

        }

        internal static IEnumerable<PregnantInfo> GetPregnantInfoForUpdate(DbGroup dbGroup)
        {
            return dbGroup.Connection.Query<PregnantInfo>($@"
select Top 1 s.id sid
,s.SyncTime
,pi.createtime,pi.updatetime
,pi.* from PregnantInfo pi
left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
where s.id is not null and s.SyncStatus = 2
and pi.updatetime > DATEADD( SECOND,10 ,s.SyncTime)
", transaction: dbGroup.Transaction).ToList();

            //in (2, 11)
        } 
        #endregion

    }
}
