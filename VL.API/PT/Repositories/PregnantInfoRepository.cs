using VL.API.Common.Repositories;
using VL.API.PT.Entities;

namespace VL.API.PT.Repositories
{
    public class PregnantInfoRepository : DapperRepository<PregnantInfo>
    {
        public PregnantInfoRepository(DbGroup dbGroup) : base(dbGroup)
        {
        }
    }
}
