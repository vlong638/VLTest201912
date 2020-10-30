using System.ComponentModel.DataAnnotations.Schema;

namespace BBee.Models
{
    [Table("[A_User]")]
    public class User
    {
        public long Id { set; get; }
        public string Password { set; get; }
        public string Name { set; get; }
    }
}
