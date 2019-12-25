using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("UserRole")]
    public class TUserRole
    {
        public long UserId { set; get; }
        public long RoleId { set; get; }
    }
}
