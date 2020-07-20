using FrameworkTest.Common.PagerSolution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.SyncManager.Models
{
    public class GetPagedListOfVisitRecordRequest : VLPageRequest, IQueriablePagedList
    {
        #region OrientInput
        public int page { get; set; }
        public int rows { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        #endregion

        public GetPagedListOfVisitRecordRequest()
        { 
        }

        public override int PageIndex { get { return page; } }
        public override int PageSize { get { return rows; } }
        public List<string> FieldNames { get; set; } = new List<string>() { "*" };
        public override Dictionary<string, bool> Orders { get { return sort == null ? new Dictionary<string, bool>() : (new Dictionary<string, bool>() { { sort, (order == "asc") } }); } }

        #region IQueriablePagedList
        public string PersonName { set; get; }
        public string VisitDate { set; get; }

        Dictionary<string, object> args = new Dictionary<string, object>();
        List<string> wheres = new List<string>();

        public Dictionary<string, object> GetParams()
        {
            if (args.Count > 0)
                return args;

            if (!string.IsNullOrEmpty(PersonName))
            {
                args.Add(nameof(PersonName), $"%{PersonName}%");
            }
            if (!string.IsNullOrEmpty(VisitDate))
            {
                args.Add(nameof(VisitDate), VisitDate);
            }
            return args;
        }
        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                if (!string.IsNullOrEmpty(PersonName))
                {
                    wheres.Add($"pi.{nameof(PersonName)} like @PersonName");
                }
                if (!string.IsNullOrEmpty(VisitDate))
                {
                    wheres.Add($"vr.{nameof(VisitDate)} = @VisitDate");
                }
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and ", wheres);
        }
        public string ToCountSQL()
        {
            return $@"
select count(*)
from {VisitRecord.TableName} vr
left join PregnantInfo pi on vr.IdCard = pi.IdCard
{GetWhereCondition()}
";
        }

        public string ToListSQL()
        {
            if (Orders.Count == 0)
            {
                Orders.Add("vr.Id", false);
            }
            return $@"
select 
s3.Id as SyncIdToPhysicalExamination,s3.SyncTime as LastSyncTimeToPhysicalExamination,s3.SyncStatus as SyncStatusToPhysicalExamination,s3.ErrorMessage as SyncMessageToPhysicalExamination
,s4.Id as SyncIdToProfessionalExamination,s4.SyncTime as LastSyncTimeToProfessionalExamination,s4.SyncStatus as SyncStatusToProfessionalExamination,s4.ErrorMessage as SyncMessageToProfessionalExamination
,TSource.* from
(
    select 
    pi.PersonName,
    {string.Join(",", FieldNames.Select(c => "vr." + c))}
    from {VisitRecord.TableName} vr
    left join PregnantInfo pi on vr.IdCard = pi.IdCard
    {GetWhereCondition()}
    {GetOrderCondition()}
    {GetLimitCondition()}
) as TSource
left join SyncForFS s3 on TSource.Id =s3.SourceId and s3.TargetType = 3
left join SyncForFS s4 on TSource.Id =s4.SourceId and s4.TargetType = 4
";
        } 

        #endregion
    }
}