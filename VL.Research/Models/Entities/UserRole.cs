using Dapper.Contrib.Extensions;

namespace BBee.Models
{
    [Table("A_UserRole")]
    public class UserRole
    {
        public long Id { set; get; }
        public long UserId { set; get; }
        public long RoleId { set; get; }
    }
}
