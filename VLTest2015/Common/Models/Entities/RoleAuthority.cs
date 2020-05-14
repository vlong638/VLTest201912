using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("A_RoleAuthority")]
    public class RoleAuthority
    {
        public long Id { set; get; }
        public long RoleId { set; get; }
        public long AuthorityId { set; get; }
    }
}
