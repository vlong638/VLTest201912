using Dapper.Contrib.Extensions;

namespace VLTest2015.Services
{
    [Table("T_PregnantInfo")]
    public class T_PregnantInfo
    {
        public long Id { set; get; }
        public string PatientCode { set; get; }
        public string Name { set; get; }
        public int Sex { set; get; }
    }
}
