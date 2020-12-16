using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class Project
    {
        public const string TableName = "Project";

        public long Id { set; get; }
        public string Name { set; get; }
        public long? DepartmentId { set; get; }
        public ViewAuthorizeType ViewAuthorizeType { set; get; }
        public string ProjectDescription { set; get; }
        public long CreatorId { set; get; }
        public DateTime? CreatedAt { set; get; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public enum ViewAuthorizeType
    {
        None,
        CreatorOnly,
        MemberAvailable,
        DepartmentAndMemberAvailable,
    }
}
