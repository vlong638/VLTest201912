using Dapper;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
using FS.SyncManager.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FS.SyncManager.Repositories
{
    public class SyncOrderRepository : RepositoryBase<SyncOrder>
    {
        public SyncOrderRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 获取`同步记录`分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal List<Dictionary<string, object>> GetSyncOrderPagedList(GetPagedListOfSyncOrderRequest request)
        {
            var sql = request.ToListSQL();
            _context.VLLogger.Log("获取`同步记录`分页列表:" + sql);
            var pars = request.GetParams();
            var table = new DataTable();
            using (var reader = _context.DbGroup.Connection.ExecuteReader(sql, pars, transaction: _transaction))
            {
                table.Load(reader);
            }
            return table.ToList();
        }
        /// <summary>
        /// 获取孕妇档案分页计数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal int GetSyncOrderPagedListCount(GetPagedListOfSyncOrderRequest request)
        {
            var sql = request.ToCountSQL();
            var pars = request.GetParams();
            return _context.DbGroup.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }

        internal List<SyncOrder> GetByIds(List<long> syncOrderIds)
        {
            return _context.DbGroup.Connection.Query<SyncOrder>("select * from SyncForFS where id in @ids ", new { ids = syncOrderIds }, transaction: _transaction).ToList();
        }

        internal int UpdateToRetry(long syncOrderId)
        {
            return _context.DbGroup.Connection.Execute("update SyncForFS set SyncStatus = 3 where id = @id ", new { id = syncOrderId }, transaction: _transaction);
        }

        internal int UpdateToRetry(List<long> syncOrderIds)
        {
            return _context.DbGroup.Connection.Execute("update SyncForFS set SyncStatus = 3 where ids in @ids ", new { ids = syncOrderIds }, transaction: _transaction);
        }
    }
}