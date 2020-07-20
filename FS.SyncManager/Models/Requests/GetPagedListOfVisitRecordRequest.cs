using FrameworkTest.Common.PagerSolution;
using System;
using System.Collections.Generic;

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

        public string VisitDate { set; get; }
        public override int PageIndex { get { return page; } }
        public override int PageSize { get { return rows; } }
        public List<string> FieldNames { get; set; } = new List<string>() { "*" };
        public override Dictionary<string, bool> Orders { get { return sort == null ? new Dictionary<string, bool>() : (new Dictionary<string, bool>() { { sort, (order == "asc") } }); } }

        #region IQueriablePagedList

        Dictionary<string, object> args = new Dictionary<string, object>();
        List<string> wheres = new List<string>();

        public Dictionary<string, object> GetParams()
        {
            if (args.Count > 0)
                return args;

            if (!string.IsNullOrEmpty(VisitDate))
            {
                args.Add(nameof(VisitDate), $"{VisitDate}");
            }
            return args;
        }
        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                if (!string.IsNullOrEmpty(VisitDate))
                {
                    wheres.Add($"{nameof(VisitDate)} = @VisitDate");
                }
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and ", wheres);
        }
        public string ToCountSQL()
        {
            return $@"
select count(*)
from {VisitRecord.TableName}
{GetWhereCondition()}
";
        }

        public string ToListSQL()
        {
            if (Orders.Count == 0)
            {
                Orders.Add("Id", false);
            }
            return $@"
select 
pi.PersonName
,s3.Id as SyncIdToPhysicalExamination,s3.SyncTime as LastSyncTimeToPhysicalExamination,s3.SyncStatus as SyncStatusToPhysicalExamination,s3.ErrorMessage as SyncMessageToPhysicalExamination
,s4.Id as SyncIdToProfessionalExamination,s4.SyncTime as LastSyncTimeToProfessionalExamination,s4.SyncStatus as SyncStatusToProfessionalExamination,s4.ErrorMessage as SyncMessageToProfessionalExamination
,TSource.* from
(
    select {string.Join(",", FieldNames)}
    from {VisitRecord.TableName}
    {GetWhereCondition()}
    {GetOrderCondition()}
    {GetLimitCondition()}
) as TSource
left join SyncForFS s3 on TSource.Id =s3.SourceId and s3.TargetType = 3
left join SyncForFS s4 on TSource.Id =s4.SourceId and s4.TargetType = 4
left join PregnantInfo pi on TSource.IdCard = pi.IdCard
";
        } 

        #endregion
    }
}