using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.PagerSolution;
using FrameworkTest.Common.ServiceSolution;
using FS.SyncManager.Models;
using FS.SyncManager.Repositories;
using System.Collections.Generic;

namespace FS.SyncManager.Service
{
    public class SyncService : BaseService
    {
        DbContext DbContext;

        public SyncService()
        { }
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

        internal ServiceResult<bool> DeleteSyncOrderById(long syncOrderId)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                return SyncOrderRepository.DeleteById(syncOrderId);
            });
            return result;
        }

        internal ServiceResult<bool> DeleteSyncOrderByIds(List<long> syncOrderIds)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                foreach (var syncOrderId in syncOrderIds)
                {
                     SyncOrderRepository.DeleteById(syncOrderId);
                }
                return true;
            });
            return result;
        }
        internal ServiceResult<bool> RetrySyncById(long syncOrderId)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                SyncOrderRepository.UpdateToRetry(syncOrderId);
                return true;
            });
            return result;
        }

        internal ServiceResult<bool> RetrySyncByIds(List<long> syncOrderIds)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                SyncOrderRepository.UpdateToRetry(syncOrderIds);
                return true;
            });
            return result;
        }
        
        #endregion

        #region MotherDischarge
        MotherDischargeRepository MotherDischargeRepository { get { return new MotherDischargeRepository(DbContext); } }
        internal ServiceResult<VLPageResult<Dictionary<string, object>>> GetPagedListOfMotherDischarge(GetPagedListOfMotherDischargeRequest request)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                var list = MotherDischargeRepository.GetMotherDischargePagedList(request);
                var count = MotherDischargeRepository.GetMotherDischargePagedListCount(request);
                return new VLPageResult<Dictionary<string, object>>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }
        #endregion

        #region ChildDischarge
        ChildDischargeRepository ChildDischargeRepository { get { return new ChildDischargeRepository(DbContext); } }
        internal ServiceResult<VLPageResult<Dictionary<string, object>>> GetPagedListOfChildDischarge(GetPagedListOfChildDischargeRequest request)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                var list = ChildDischargeRepository.GetChildDischargePagedList(request);
                var count = ChildDischargeRepository.GetChildDischargePagedListCount(request);
                return new VLPageResult<Dictionary<string, object>>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }
        #endregion
    }
}