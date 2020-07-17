using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.PagerSolution;
using FrameworkTest.Common.ServiceSolution;
using FrameworkTest.Common.ValuesSolution;
using FS.SyncManager.Models;
using FS.SyncManager.Repositories;
using System;
using System.Collections.Generic;

namespace FS.SyncManager.Service
{
    public class SyncService : BaseService
    {
        DbContext DbContext;

        public SyncService(DbContext context)
        {
            DbContext = context;
        }

        #region PregnantInfo
        PregnantInfoRepository PregnantInfoRepository { get { return new PregnantInfoRepository(DbContext); } }
        internal ServiceResult<VLPageResult<Dictionary<string,object>>> GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                var list = PregnantInfoRepository.GetPregnantInfoPagedList(request);
                var count = PregnantInfoRepository.GetPregnantInfoPagedListCount(request);
                return new VLPageResult<Dictionary<string, object>>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }
        #endregion

        #region VisitRecord
        VisitRecordRepository VisitRecordRepository { get { return new VisitRecordRepository(DbContext); } }
        internal ServiceResult<VLPageResult<Dictionary<string, object>>> GetPagedListOfVisitRecord(GetPagedListOfVisitRecordRequest request)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                var list = VisitRecordRepository.GetVisitRecordPagedList(request);
                var count = VisitRecordRepository.GetVisitRecordPagedListCount(request);
                return new VLPageResult<Dictionary<string, object>>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }
        #endregion

        #region SyncOrder
        SyncOrderRepository SyncOrderRepository { get { return new SyncOrderRepository(DbContext); } }
        internal ServiceResult<VLPageResult<Dictionary<string, object>>> GetPagedListOfSyncOrder(GetPagedListOfSyncOrderRequest request)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                var list = SyncOrderRepository.GetSyncOrderPagedList(request);
                var count = SyncOrderRepository.GetSyncOrderPagedListCount(request);
                return new VLPageResult<Dictionary<string, object>>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }
        #endregion
    }
}