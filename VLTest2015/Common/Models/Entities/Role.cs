using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("A_Role")]
    public class Role
    {
        public long Id { set; get; }
        public string Name { set; get; }
    }
}
