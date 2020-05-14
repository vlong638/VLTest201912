using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("[A_User]")]
    public class User
    {
        public long Id { set; get; }
        public string Password { set; get; }
        public string Name { set; get; }
    }
}
