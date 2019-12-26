using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("UserRole")]
    public class UserRole
    {
        public long UserId { set; get; }
        public long RoleId { set; get; }
    }
}
