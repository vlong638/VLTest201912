using Dapper;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using FS.SyncManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FS.SyncManager.Repositories
{
    public class PregnantInfoRepository : RepositoryBase<PregnantInfo>
    {
        public PregnantInfoRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 获取孕妇档案分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>88888
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
}