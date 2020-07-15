using FrameworkTest.Common.PagerSolution;
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
            return wheres.Count == 0 ? "" : "where " + string.Join(" and", wheres);
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
select {string.Join(",", FieldNames)}
from {VisitRecord.TableName}
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
";
        } 

        #endregion
    }
}