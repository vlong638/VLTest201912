using System;
using System.Collections.Generic;
using System.Data;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ExcelExportSolution;
using VL.Consolo_Core.Common.PagerSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.Research.Common;
using VL.Research.Models;
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
            sharedRepository = new SharedRepository(dbContext.CommonDbContext);
        }

        /// <summary>
        /// 通用查询模型
        /// </summary>
        public ServiceResult<VLPagerTableResult<List<Dictionary<string, object>>>> GetCommonSelect(SQLConfig sqlConfig)
        {
            var result = dbContext.CommonDbContext.DelegateTransaction((g) =>
            {
                var list = sharedRepository.GetCommonSelect(sqlConfig);
                var count = sharedRepository.GetCommonSelectCount(sqlConfig);
                return new VLPagerTableResult<List<Dictionary<string, object>>>() { SourceData = list.ToList(), Count = count, CurrentIndex = sqlConfig.PageIndex };
            });
            return result;
        }

        /// <summary>
        /// 通用查询模型
        /// </summary>
        public ServiceResult<VLPagerTableResult<List<Dictionary<string, object>>>> GetCommonSelectForFYPT(SQLConfig sqlConfig)
        {
            var result = dbContext.FYPTDbContext.DelegateTransaction((g) =>
            {
                sharedRepository = new SharedRepository(dbContext.FYPTDbContext);
                var list = sharedRepository.GetCommonSelect(sqlConfig);
                var count = sharedRepository.GetCommonSelectCount(sqlConfig);
                return new VLPagerTableResult<List<Dictionary<string, object>>>() { SourceData = list.ToList(), Count = count, CurrentIndex = sqlConfig.PageIndex };
            });
            return result;
        }

        internal ServiceResult<DataTable> GetCommonSelect(ExportSource sourceConfig)
        {
            var result = dbContext.CommonDbContext.DelegateTransaction((g) =>
            {
                return sharedRepository.GetCommonSelect(sourceConfig);
            });
            return result;
        }
    }
}
