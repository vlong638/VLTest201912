using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("[User]")]
    public class TUser
    {
        public long Id { set; get; }
        public string Password { set; get; }
        public string Name { set; get; }
    }
}
