using System;
using System.Collections.Generic;
using System.Data;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ExcelExportSolution;
using VL.Consolo_Core.Common.PagerSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Consolo_Core.Common.ValuesSolution;
using BBee.Common;
using BBee.Models;
using BBee.Repositories;

namespace BBee.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class PregnantService : BaseService
    {
        APIContext dbContext;
        PregnantInfoRepository pregnantInfoRepository;
        VisitRecordRepository visitRecordRepository;
        LabOrderRepository labOrderRepository;
        LabCheckRepository labCheckRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public PregnantService(APIContext dbContext)
        {
            this.dbContext = dbContext;
            pregnantInfoRepository = new PregnantInfoRepository(dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()));
            visitRecordRepository = new VisitRecordRepository(dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()));
            labOrderRepository = new LabOrderRepository(dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()));
            labCheckRepository = new LabCheckRepository(dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()));
        }

        ///// <summary>
        ///// 通用查询模型
        ///// </summary>
        ///// <param name="request">请求参数实体</param>
        ///// <returns></returns>
        //public ServiceResult<VLPagerTableResult<List<Dictionary<string, object>>>> GetCommonSelect(CommonSelectRequest request)
        //{
        //    var result = dbContext.DelegateTransaction((g) =>
        //    {
        //        var list = pregnantInfoRepository.GetConfigurablePregnantInfoPagedList(request);
        //        var count = pregnantInfoRepository.GetPregnantInfoPagedListCount(request);
        //        return new VLPagerTableResult<List<Dictionary<string, object>>>() { SourceData = list.ToList(), Count = count, CurrentIndex = request.PageIndex };
        //    });
        //    return result;
        //}

        /// <summary>
        /// 孕妇档案列表
        /// </summary>
        /// <param name="request">请求参数实体</param>
        /// <returns></returns>
        public ServiceResult<VLPagerResult<PagedListOfPregnantInfoModel>> GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()).DelegateTransaction((g) =>
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
        /// <param name="request">请求参数实体</param>
        /// <returns></returns>
        public ServiceResult<VLPagerTableResult<List<Dictionary<string, object>>>> GetConfigurablePagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()).DelegateTransaction((g) =>
            {
                var list = pregnantInfoRepository.GetConfigurablePregnantInfoPagedList(request);
                var count = pregnantInfoRepository.GetPregnantInfoPagedListCount(request);
                return new VLPagerTableResult<List<Dictionary<string, object>>>() { SourceData = list.ToList(), Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        /// <summary>
        /// 孕妇档案详情
        /// </summary>
        /// <param name="pregnantInfoId">孕妇档案Id</param>
        /// <returns></returns>
        public ServiceResult<PregnantInfo> GetPregnantInfoByPregnantInfoId(long pregnantInfoId)
        {
            var result = dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()).DelegateTransaction((g) =>
            {
                var pregnantInfo = pregnantInfoRepository.GetPregnantInfoById(pregnantInfoId);
                return pregnantInfo;
            });
            return result;
        }

        /// <summary>
        /// 产检列表
        /// </summary>
        /// <param name="request">请求参数实体</param>
        /// <returns></returns>
        public ServiceResult<VLPagerResult<PagedListOfVisitRecordModel>> GetPagedListOfVisitRecord(GetPagedListOfVisitRecordRequest request)
        {
            var result = dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()).DelegateTransaction((g) =>
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
        /// <param name="request">请求参数实体</param>
        /// <returns></returns>
        public ServiceResult<VLPagerResult<PagedListOfLabOrderModel>> GetPagedListOfLabOrder(GetPagedListOfLabOrderRequest request)
        {
            var result = dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()).DelegateTransaction((g) =>
            {
                var list = labOrderRepository.GetLabOrderPagedList(request);
                var count = labOrderRepository.GetLabOrderPagedListCount(request);
                return new VLPagerResult<PagedListOfLabOrderModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }

        public void GetCommonSelect(VL.Consolo_Core.Common.ExcelExportSolution.SQLConfigSource sourceConfig)
        {
            throw new NotImplementedException();
        }
    }
}
