using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectIndicator
    {
        public const string TableName = "ProjectIndicator";

        public long Id { set; get; }
        public long ProjectId { set; get; }
        public long BusinessEntityId { set; get; }
        public long BusinessEntityPropertyId { set; get; }
        public long TemplateId { set; get; }
        public long TemplatePropertyId { get; internal set; }
        public string EntitySourceName { set; get; }
        public string PropertySourceName { set; get; }
        public string PropertyDisplayName { set; get; }

        internal string GetUniqueEntitySourceName()
        {
            if (IsTemplate())
            {
                return BusinessEntityId.ToString();
            }
            else
            {
                return EntitySourceName;
            }
        }

        internal bool IsTemplate()
        {
            return TemplateId > 0;
        }

        ///// <summary>
        ///// 字段别名名称
        ///// </summary>
        //public string ColumnNickName { set; get; }
    }
}