using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.PagerSolution;
using FrameworkTest.Common.ServiceSolution;
using FS.SyncManager.Models;
using FS.SyncManager.Repositories;

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
        internal ServiceResult<VLPageResult<PagedListOfPregnantInfoModel>> GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                var list = PregnantInfoRepository.GetPregnantInfoPagedList(request);
                var count = PregnantInfoRepository.GetPregnantInfoPagedListCount(request);
                return new VLPageResult<PagedListOfPregnantInfoModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        }
        #endregion

        #region VisitRecord
        VisitRecordRepository VisitRecordRepository { get { return new VisitRecordRepository(DbContext); } }
        internal ServiceResult<VLPageResult<PagedListOfVisitRecordModel>> GetPagedListOfVisitRecord(GetPagedListOfVisitRecordRequest request)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                var list = VisitRecordRepository.GetVisitRecordPagedList(request);
                var count = VisitRecordRepository.GetVisitRecordPagedListCount(request);
                return new VLPageResult<PagedListOfVisitRecordModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;
        } 
        #endregion
    }
}