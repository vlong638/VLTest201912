using System;
using System.Collections.Generic;

namespace Research.Common
{
    ///// <summary>
    ///// 数据集单元集合
    ///// </summary>
    //public class BusinessEntitySet
    //{
    //    public BusinessEntitySet(string displayName)
    //    {
    //        DisplayName = displayName;
    //    }

    //    public string DisplayName { get; set; }
    //    public List<BusinessEntity> Properties { set; get; }
    //}
    /// <summary>
    /// 报告单元
    /// </summary>
    public class ReportTask
    {
        public ReportTask(string reportName)
        {
            Name = reportName;
        }

        public long Id { set; get; }
        public string Name { set; get; }
        public List<BusinessEntityProperty> Properties { get; set; } = new List<BusinessEntityProperty>();
        /// <summary>
        /// 纳入标准
        /// </summary>
        public List<IWhere> MainConditions { get; set; } = new List<IWhere>();
        /// <summary>
        /// 排除标准
        /// </summary>
        public List<IWhere> ExceptionConditions { get; set; } = new List<IWhere>();

        internal List<Router> GetRouters()
        {
            throw new NotImplementedException();
        }

        internal Dictionary<string, object> GetParameters()
        {
            throw new NotImplementedException();
        }
    }
}
