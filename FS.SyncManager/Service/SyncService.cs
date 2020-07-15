using Dapper;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.PagerSolution;
using FrameworkTest.Common.ServiceSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FS.SyncManager.Service
{
    public class ServiceContext
    {
        public string ConntectingStringSD = "Data Source=192.168.50.102,1433;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=ESBUSER;Password=ESBPWD";
        public DbContext Hele_DBContext { get { return DBHelper.GetDbContext(ConntectingStringSD); } }

        public SyncService SyncService
        {
            get
            {
                var context = Hele_DBContext;
                return new SyncService(context);
            }
        }
    }

    public class PregnantInfoRepository : RepositoryBase<PregnantInfo>
    {
        public PregnantInfoRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 获取孕妇档案分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal IEnumerable<PagedListOfPregnantInfoModel> GetPregnantInfoPagedList(GetPagedListOfPregnantInfoRequest request)
        {
            var sql = request.ToListSQL();
            var pars = request.GetParams();
            return _context.DbGroup.Connection.Query<PagedListOfPregnantInfoModel>(sql, pars, transaction: _transaction).ToList();
        }
        /// <summary>
        /// 获取孕妇档案分页计数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal int GetPregnantInfoPagedListCount(GetPagedListOfPregnantInfoRequest request)
        {
            var sql = request.ToCountSQL();
            var pars = request.GetParams();
            return _context.DbGroup.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }
    }

    public class SyncService : BaseService
    {
        DbContext DbContext;
        PregnantInfoRepository PregnantInfoRepository;

        public SyncService(DbContext context)
        {
            DbContext = context;
            PregnantInfoRepository = new PregnantInfoRepository(DbContext);
        }

        internal ServiceResult<VLPageResult<PagedListOfPregnantInfoModel>> GetPagedListOfPregnantInfo(GetPagedListOfPregnantInfoRequest request)
        {
            var result = DbContext.DbGroup.DelegateTransaction(() =>
            {
                var list = PregnantInfoRepository.GetPregnantInfoPagedList(request);
                var count = PregnantInfoRepository.GetPregnantInfoPagedListCount(request);
                return new VLPageResult<PagedListOfPregnantInfoModel>() { List = list, Count = count, CurrentIndex = request.PageIndex };
            });
            return result;



            //return new ServiceResult<VLPageResult<PagedListOfPregnantInfoModel>>(
            //    new VLPageResult<PagedListOfPregnantInfoModel>()
            //    {
            //        List = new List<PagedListOfPregnantInfoModel> {
            //            new PagedListOfPregnantInfoModel() {
            //                id=1,
            //                personname="张三",
            //            },
            //            new PagedListOfPregnantInfoModel() {
            //                id=2,
            //                personname="李四",
            //            },
            //       }
            //    }
            //);
        }
    }

    public class PregnantDAL
    {

    }

    public class PagedListOfPregnantInfoModel
    {
        public long id { set; get; }
        //public long InstitutionCode { set; get; } //机构编码
        //public long CreatorId { set; get; } //创建者
        public string patientid { set; get; }
        public string idcard { set; get; } //身份证号


        public string personname { set; get; } //孕妇姓名
        public int? sexcode { set; get; } //性别
        public string contactphone { set; get; } //联系人电话
        public DateTime? birthday { set; get; } //出生日期 
        public int? gravidity { set; get; } //孕次
        public int? parity { set; get; } //产次
        public int? iscreatebook { set; get; } //是否建册
        public int? GestationalWeeks { set; get; } //建册孕周
        public DateTime? lastmenstrualperiod { set; get; } //末次月经
        public DateTime? dateofprenatal { set; get; } //预产期
        public int? filestatus { set; get; } //档案状态:(结案标识)
    }
}