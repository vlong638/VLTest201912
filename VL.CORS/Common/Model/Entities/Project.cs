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
        public long? LastModifiedBy { get; set; }
    }

    /// <summary>
    /// 项目查看权限
    /// </summary>
    public enum ViewAuthorizeType
    {
        None,
        Public = 1, //所有人可见
        MemberAvailable = 2,//成员可见
        DepartmentAndMemberAvailable = 3,//成员与科室可见
    }
}
