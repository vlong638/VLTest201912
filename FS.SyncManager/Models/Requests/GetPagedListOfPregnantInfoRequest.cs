using System.Collections.Generic;
using System.Linq;

namespace FS.SyncManager
{
    /// <summary>
    /// 分页入参规范
    /// </summary>
    public class VLPageRequest
    {
        #region 分页
        public virtual int PageIndex { set; get; }
        public virtual int PageSize { set; get; }
        public int Skip
        {
            get
            {
                var skip = (PageIndex - 1) * PageSize;
                return skip > 0 ? skip : 0;
            }
        }
        public int Limit
        {
            get { return PageSize > 0 ? PageSize : 10; }
        }

        public string GetLimitCondition()
        {
            return $"offset {Skip} rows fetch next {Limit} rows only;";
        }
        #endregion

        #region 排序
        /// <summary>
        /// 排序 true：asc,false:desc
        /// </summary>
        public virtual Dictionary<string, bool> Orders { get; set; } = new Dictionary<string, bool>();

        public string GetOrderCondition()
        {
            if (Orders.Count == 0)
                return "";
            return "order by " + string.Join(",", Orders.Keys.Select(c => c + " " + (Orders[c] ? "asc" : "desc")));
        }
        #endregion
    }

    public interface IQueriablePagedList
    {
        string GetWhereCondition();
        Dictionary<string, object> GetParams();
        string ToListSQL();
        string ToCountSQL();
    }

    public class GetPagedListOfPregnantInfoRequest : VLPageRequest, IQueriablePagedList
    {
        #region OrientInput
        public string name { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        #endregion

        public GetPagedListOfPregnantInfoRequest()
        { 
        }
        public GetPagedListOfPregnantInfoRequest(string name, int page, int rows, string sort, string order)
        {
            this.name = name;
            this.page = page;
            this.rows = rows;
            this.sort = sort;
            this.order = order;
        }

        public string PersonName { get { return name; } }
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

            if (!string.IsNullOrEmpty(PersonName))
            {
                args.Add(nameof(PersonName), $"%{PersonName}%");
            }
            return args;
        }
        public string GetWhereCondition()
        {
            if (wheres.Count == 0)
            {
                if (!string.IsNullOrEmpty(PersonName))
                {
                    wheres.Add($"{nameof(PersonName)} Like @PersonName");
                }
            }
            return wheres.Count == 0 ? "" : "where " + string.Join(" and", wheres);
        }
        public string ToCountSQL()
        {
            return $@"
select count(*)
from {PregnantInfo.TableName}
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
from {PregnantInfo.TableName}
{GetWhereCondition()}
{GetOrderCondition()}
{GetLimitCondition()}
";
        } 

        #endregion
    }
}