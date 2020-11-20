using System.ComponentModel.DataAnnotations.Schema;

namespace BBee.Models
{
    [Table("Role")]
    public class Role
    {
        public long Id { set; get; }
        public string Name { set; get; }
    }
}
