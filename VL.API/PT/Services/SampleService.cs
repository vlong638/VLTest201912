using VL.API.Common.Models;
using VL.API.Common.Models.Entities;
using VL.API.Common.Services;

namespace VL.API.PT.Services
{
    /// <summary>
    /// sample
    /// </summary>
    public class SampleService : ServiceBase
    {
        /// <summary>
        /// 单数据库查询样例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult<PregnantInfo> GetPregnantInfoById(int id)
        {
            var entity = ServiceContext.Repository_PregnantInfo.GetById(id);
            return new ServiceResult<PregnantInfo>(entity);
        }

        /// <summary>
        /// 单数据库新增样例
        /// </summary>
        /// <param name="pregnant"></param>
        /// <returns></returns>
        public ServiceResult<int> CreatePregnantInfo(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return Error(-1);

            var result = DelegateTransaction(ServiceContext.DbGroup_Pregnant, () =>
             {
                 pregnant.Id = (int)ServiceContext.Repository_PregnantInfo.Insert(pregnant);
                 return pregnant.Id;
             });
            return result;
        }

        /// <summary>
        /// 单数据库更新样例
        /// </summary>
        /// <param name="pregnant"></param>
        /// <returns></returns>
        public ServiceResult<bool> UpdatePregnantInfo(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return Error(false);

            var result = DelegateTransaction(ServiceContext.DbGroup_Pregnant, () =>
            {
                var result = ServiceContext.Repository_PregnantInfo.Update(pregnant);
                return result;
            });
            return result;
        }

        /// <summary>
        /// 多数据库事务样例
        /// </summary>
        /// <param name="pregnant"></param>
        /// <returns></returns>
        internal ServiceResult<bool> MultipleSourceSample(PregnantInfo pregnant)
        {
            //对于实体创建和更新业务总是进行数据有效性校验
            var validResult = pregnant.Validate();
            if (!validResult.IsValidated)
                return Error(false);

            var result = DelegateTransaction(ServiceContext.DbGroup_Pregnant, ServiceContext.DbGroup_Sample01, () =>
            {
                var result = ServiceContext.Repository_PregnantInfo.Update(pregnant);
                return result;
            });
            return result;
        }

        /// <summary>
        /// 分页查询样例
        /// </summary>
        /// <returns></returns>
        public ServiceResult<VLPageResult<PregnantInfo>> GetPagedListSample(GetPagedListSampleRequest request)
        {
            var result = DelegateTransaction(ServiceContext.DbGroup_Pregnant, () =>
            {
                var list = ServiceContext.Repository_PregnantInfo.GetPregnantInfoPagedList(request);
                var count = ServiceContext.Repository_PregnantInfo.GetPregnantInfoPagedListCount(request);
                return new VLPageResult<PregnantInfo>() { Data = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        /// <summary>
        /// 带搜索的分页查询样例
        /// </summary>
        /// <returns></returns>
        public ServiceResult<VLPageResult<PregnantInfo>> GetComplexPagedListSample(GetComplexPagedListSampleRequest request)
        {
            var result = DelegateTransaction(ServiceContext.DbGroup_Pregnant, () =>
            {
                var list = ServiceContext.Repository_PregnantInfo.GetComplexPregnantInfoPagedList(request);
                var count = ServiceContext.Repository_PregnantInfo.GetComplexPregnantInfoPagedListCount(request);
                return new VLPageResult<PregnantInfo>() { Data = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }
    }
}
