using System.ComponentModel.DataAnnotations.Schema;

namespace VL.Research.Models
{
    [Table("A_Role")]
    public class Role
    {
        public long Id { set; get; }
        public string Name { set; get; }
    }
}
