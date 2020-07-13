using FrameworkTest.Common.ServiceSolution;
using System.Data;

namespace VLTest2015.Services
{
    public class SyncService : BaseService
    {

        public SyncService()
        {
        }

        /// <summary>
        /// 孕妇档案列表
        /// </summary>
        /// <param name="request"></param>
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

        /// <summary>
        /// 孕妇档案列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResult<VLPageSingleResult<DataTable>> GetConfigurablePagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = DelegateTransaction(() =>
            {
                var list = _PregnantInfoRepository.GetConfigurablePregnantInfoPagedList(request);
                var count = _PregnantInfoRepository.GetPregnantInfoPagedListCount(request);
                return new VLPageSingleResult<DataTable>() { DataTable = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        /// <summary>
        /// 孕妇档案详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResult<PregnantInfo> GetPregnantInfoByPregnantInfoId(long pregnantInfoId)
        {
            var result = DelegateTransaction(() =>
            {
                var pregnantInfo = _PregnantInfoRepository.GetPregnantInfoById(pregnantInfoId);
                return pregnantInfo;
            });
            return result;
        }

        /// <summary>
        /// 产检列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResult<VLPageResult<PagedListOfVisitRecordModel>> GetPagedListOfVisitRecord(GetPagedListOfVisitRecordRequest request)
        {
            var result = DelegateTransaction(() =>
            {
                var list = _VisitRecordRepository.GetVisitRecordPagedList(request);
                var count = _VisitRecordRepository.GetVisitRecordPagedListCount(request);
                return new VLPageResult<PagedListOfVisitRecordModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        /// <summary>
        /// 检查单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResult<VLPageResult<PagedListOfLabOrderModel>> GetPagedListOfLabOrder(GetPagedListOfLabOrderRequest request)
        {
            var result = DelegateTransaction(() =>
            {
                var list = _LabOrderRepository.GetLabOrderPagedList(request);
                var count = _LabOrderRepository.GetLabOrderPagedListCount(request);
                return new VLPageResult<PagedListOfLabOrderModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        ///// <summary>
        ///// 检查单详情(带检验项)
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public ServiceResult<VLPageResult<PagedListOfLabOrderModel>> GetPagedListOfLabOrder(GetPagedListOfLabOrderRequest request)
        //{
        //    var result = DelegateTransaction(() =>
        //    {
        //        var list = _LabOrderRepository.GetLabOrderPagedList(request);
        //        var count = _LabOrderRepository.GetLabOrderPagedListCount(request);
        //        return new VLPageResult<PagedListOfLabOrderModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
        //    });
        //    return result;
        //}
    }
}
