using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("A_UserRole")]
    public class UserRole
    {
        public long Id { set; get; }
        public long UserId { set; get; }
        public long RoleId { set; get; }
    }
}
