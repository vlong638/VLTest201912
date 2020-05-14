using System.Collections.Generic;
using VLTest2015.Common.Models.RequestDTO;
using VLTest2015.DAL;
using VLVLTest2015.Common.Pager;

namespace VLTest2015.Services
{
    public class PregnantService : BaseService
    {
        PregnantInfoRepository _PregnantInfoRepository;

        public PregnantService()
        {
            _PregnantInfoRepository = new PregnantInfoRepository(_context);
        }

        /// <summary>
        /// 分页查询样例
        /// Sample PagedList
        /// </summary>
        /// <returns></returns>
        public ServiceResult<VLPageResult<PagedListOfPregnantInfoModel>> GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = DelegateTransaction(() =>
            {
                var list = _PregnantInfoRepository.GetPregnantInfoPagedList(request);
                var count = _PregnantInfoRepository.GetPregnantInfoPagedListCount(request);
                return new VLPageResult<PagedListOfPregnantInfoModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

    }
}
