using System.Collections.Generic;
using System.Data;
using VL.Consolo_Core.Common.PagerSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.Research.Common;
using VL.Research.Repositories;

namespace VL.Research.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SharedService : BaseService
    {
        APIContext dbContext;
        SharedRepository sharedRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public SharedService(APIContext dbContext)
        {
            this.dbContext = dbContext;
            sharedRepository = new SharedRepository(dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()));
        }

        /// <summary>
        /// 通用查询模型
        /// </summary>
        public ServiceResult<VLPagerTableResult<List<Dictionary<string, object>>>> GetCommonSelectBySQLConfig(SQLConfig sqlConfig)
        {
            var adbContext = dbContext.GetDBContext(sqlConfig.Source.DBSourceType);
            var result = adbContext.DelegateNonTransaction((g) =>
            {
                sharedRepository = new SharedRepository(adbContext);
                var list = sharedRepository.GetCommonSelect(sqlConfig.Source, sqlConfig.Skip, sqlConfig.Limit);
                var count = sharedRepository.GetCommonSelectCount(sqlConfig);
                //Transform
                sqlConfig.Source.DoTransforms(ref list);
                return new VLPagerTableResult<List<Dictionary<string, object>>>() { SourceData = list.ToList(), Count = count, CurrentIndex = sqlConfig.PageIndex };
            });
            return result;
        }
        internal ServiceResult<DataTable> GetCommonSelectByExportConfig(Consolo_Core.Common.ExcelExportSolution.SQLConfigSource sourceConfig)
        {
            var result = dbContext.GetDBContext(DBSourceType.DefaultConnectionString.ToString()).DelegateTransaction((g) =>
            {
                return sharedRepository.GetCommonSelect(sourceConfig);
            });
            return result;
        }

        /// <summary>
        /// 获取通用的导出数据
        /// </summary>
        /// <param name="sourceConfig"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        internal ServiceResult<DataTable> GetCommonSelectByExportConfig(Consolo_Core.Common.ExcelExportSolution.SQLConfigSource sourceConfig, DBSourceType source)
        {
            var adbContext = dbContext.GetDBContext(source.ToString());
            var result = adbContext.DelegateNonTransaction((g) =>
            {
                sharedRepository = new SharedRepository(adbContext);
                var datatable = sharedRepository.GetCommonSelect(sourceConfig);
                return datatable;
            });
            return result;
        }

        /// <summary>
        /// 早产儿管理情况统计报 指标更新
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal ServiceResult<bool> UpdateIndicatorsForPrematureBabyManagement(DBSourceType source)
        {
            var adbContext = dbContext.GetDBContext(source.ToString());
            var result = adbContext.DelegateTransaction((g) =>
            {
                sharedRepository = new SharedRepository(adbContext);
                //生成完整列表
                sharedRepository.GenerateSourceForPrematureBabyManagement();
                //更新各项指标
                sharedRepository.GenerateIndicatorsForPrematureBabyManagement();
                return true;
            });
            return result;
        }

        #region HuZhou

        #endregion
    }
}
