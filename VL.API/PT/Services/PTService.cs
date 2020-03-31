using VL.API.Common.Services;
using VL.API.PT.Entities;

namespace VL.API.PT.Services
{
    public class PTService: ServiceBase
    {
        public PregnantInfo GetPregnantInfoById(int id)
        {
            return ServiceContext.Repository_PregnantInfo.GetById(id);
        }
    }
}
