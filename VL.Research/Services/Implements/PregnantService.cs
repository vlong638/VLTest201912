using System.Data;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.PagerSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Research.Repositories;

namespace VL.Research.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PregnantService : BaseService
    {
        DbContext dbContext;
        PregnantInfoRepository pregnantInfoRepository;
        VisitRecordRepository visitRecordRepository;
        LabOrderRepository labOrderRepository;
        LabCheckRepository labCheckRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public PregnantService(DbContext dbContext)
        {
            this.dbContext = dbContext;
            pregnantInfoRepository = new PregnantInfoRepository(dbContext);
            visitRecordRepository = new VisitRecordRepository(dbContext);
            labOrderRepository = new LabOrderRepository(dbContext);
            labCheckRepository = new LabCheckRepository(dbContext);
        }

        /// <summary>
        /// 孕妇档案列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResult<VLPagerResult<PagedListOfPregnantInfoModel>> GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = dbContext.DelegateTransaction((g) =>
            {
                var list = pregnantInfoRepository.GetPregnantInfoPagedList(request);
                var count = pregnantInfoRepository.GetPregnantInfoPagedListCount(request);
                return new VLPagerResult<PagedListOfPregnantInfoModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        /// <summary>
        /// 孕妇档案列表
        /// </summary>
        /// <returns></returns>
        public ServiceResult<VLPagerTableResult<DataTable>> GetConfigurablePagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = dbContext.DelegateTransaction((g) =>
            {
                var list = pregnantInfoRepository.GetConfigurablePregnantInfoPagedList(request);
                var count = pregnantInfoRepository.GetPregnantInfoPagedListCount(request);
                return new VLPagerTableResult<DataTable>() { DataTable = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        /// <summary>
        /// 孕妇档案详情
        /// </summary>
        /// <returns></returns>
        public ServiceResult<PregnantInfo> GetPregnantInfoByPregnantInfoId(long pregnantInfoId)
        {
            var result = dbContext.DelegateTransaction((g) =>
            {
                var pregnantInfo = pregnantInfoRepository.GetPregnantInfoById(pregnantInfoId);
                return pregnantInfo;
            });
            return result;
        }

        /// <summary>
        /// 产检列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResult<VLPagerResult<PagedListOfVisitRecordModel>> GetPagedListOfVisitRecord(GetPagedListOfVisitRecordRequest request)
        {
            var result = dbContext.DelegateTransaction((g) =>
            {
                var list = visitRecordRepository.GetVisitRecordPagedList(request);
                var count = visitRecordRepository.GetVisitRecordPagedListCount(request);
                return new VLPagerResult<PagedListOfVisitRecordModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        /// <summary>
        /// 检查单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResult<VLPagerResult<PagedListOfLabOrderModel>> GetPagedListOfLabOrder(GetPagedListOfLabOrderRequest request)
        {
            var result = dbContext.DelegateTransaction((g) =>
            {
                var list = labOrderRepository.GetLabOrderPagedList(request);
                var count = labOrderRepository.GetLabOrderPagedListCount(request);
                return new VLPagerResult<PagedListOfLabOrderModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }
    }
}
