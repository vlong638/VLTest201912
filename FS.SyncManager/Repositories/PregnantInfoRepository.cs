using Dapper;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
using FS.SyncManager.Models;
using System.Collections.Generic;
using System.Data;

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
        internal List<Dictionary<string, object>> GetPregnantInfoPagedList(GetPagedListOfPregnantInfoRequest request)
        {
            var sql = request.ToListSQL();
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
        internal int GetPregnantInfoPagedListCount(GetPagedListOfPregnantInfoRequest request)
        {
            var sql = request.ToCountSQL();
            var pars = request.GetParams();
            return _context.DbGroup.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }
    }
    public class MotherDischargeRepository : RepositoryBase<MotherDischarge>
    {
        public MotherDischargeRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 获取孕妇档案分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>88888
        internal List<Dictionary<string, object>> GetMotherDischargePagedList(GetPagedListOfMotherDischargeRequest request)
        {
            var sql = request.ToListSQL();
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
        internal int GetMotherDischargePagedListCount(GetPagedListOfMotherDischargeRequest request)
        {
            var sql = request.ToCountSQL();
            var pars = request.GetParams();
            return _context.DbGroup.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }
    }
}