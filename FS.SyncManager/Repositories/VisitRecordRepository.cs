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
    public class VisitRecordRepository : RepositoryBase<VisitRecord>
    {
        public VisitRecordRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 获取孕妇档案分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal IEnumerable<PagedListOfVisitRecordModel> GetVisitRecordPagedList(GetPagedListOfVisitRecordRequest request)
        {
            var sql = request.ToListSQL();
            var pars = request.GetParams();
            return _context.DbGroup.Connection.Query<PagedListOfVisitRecordModel>(sql, pars, transaction: _transaction).ToList();
        }
        /// <summary>
        /// 获取孕妇档案分页计数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal int GetVisitRecordPagedListCount(GetPagedListOfVisitRecordRequest request)
        {
            var sql = request.ToCountSQL();
            var pars = request.GetParams();
            return _context.DbGroup.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }
    }
}