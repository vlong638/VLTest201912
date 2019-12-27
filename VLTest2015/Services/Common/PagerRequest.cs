using System.Collections.Generic;

namespace VLTest2015.Services
{
    /// <summary>
    /// 分页规格
    /// </summary>
    public abstract class PagerRequest
    {
        public int Page { set; get; }
        public int Rows { set; get; }

        #region 便捷扩展

        public int Skip
        {
            get
            {
                var skip = (Page - 1) * Rows;
                return skip > 0 ? skip : 0;
            }
        }

        public int Limit
        {
            get { return Rows; }
        }

        public string GetLimitCondition(string indexer)
        {
            return $"order by {indexer} offset {Skip} rows fetch next {Limit} rows only ";
        }

        public virtual string GetWhereCondition()
        {
            return "";
        }

        public abstract Dictionary<string, object> GetParameters();

        #endregion
    }
    /// <summary>
    /// 分页规格
    /// </summary>
    public abstract class PagerRequest<T>: PagerRequest
    {
        public T Data { set; get; }
    }
}