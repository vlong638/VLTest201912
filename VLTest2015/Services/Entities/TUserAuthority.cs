using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("UserAuthority")]
    public class TUserAuthority
    {
        public long UserId { set; get; }
        public long AuthorityId { set; get; }
    }
}
