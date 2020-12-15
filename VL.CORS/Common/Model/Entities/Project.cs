using System;

namespace ResearchAPI.CORS.Common
{
    public class Project
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public long? DepartmentId { set; get; }
        public ViewAuthorizeType ViewAuthorizeType { set; get; }
        public string ProjectDescription { set; get; }
        public long CreatorId { set; get; }
        public DateTime? CreatedAt { set; get; }
    }

    public enum ViewAuthorizeType
    {
        None,
        CreatorOnly,
        MemberAvailable,
        DepartmentAndMemberAvailable,
    }
}
