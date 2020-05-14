using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("A_UserAuthority")]
    public class UserAuthority
    {
        public long Id { set; get; }
        public long UserId { set; get; }
        public long AuthorityId { set; get; }
    }
}
