using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("RoleAuthority")]
    public class RoleAuthority
    {
        public long RoleId { set; get; }
        public long AuthorityId { set; get; }
    }
}
