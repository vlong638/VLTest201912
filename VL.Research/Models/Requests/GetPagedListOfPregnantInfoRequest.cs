using System.Collections.Generic;
using VL.Consolo_Core.Common.PagerSolution;

namespace VL.Research.Models
{
    public class CommonSelectRequest : VLPagerRequest, IQueriablePagedList
    {
        /// <summary>
        /// 孕妇姓名
        /// </summary>
        public string PersonName { set; get; }

        /// <summary>
        /// 待查询的字段
        /// </summary>
        public List<string> FieldNames { get; set; } = new List<string>() { "*" };
        /// <summary>
        /// 参数项集合
        /// </summary>
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();




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
    }

    public class GetPagedListOfPregnantInfoRequest : VLPagerRequest, IQueriablePagedList
    {
        /// <summary>
        /// 孕妇姓名
        /// </summary>
        public string PersonName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> FieldNames { get; internal set; } = new List<string>() { "*" };

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
    }
}