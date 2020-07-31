using Dapper;

using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Research.Models;

namespace VL.Research.Repositories
{
    public class PregnantInfoRepository : Repository<PregnantInfo>
    {
        public PregnantInfoRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<PregnantInfo> GetAll()
        {
            return _context.DbGroup.Connection.Query<PregnantInfo>($"select * from [{PregnantInfo.TableName}] order by Id desc;", transaction: _transaction);
        }

        /// <summary>
        /// 根据 PregnantInfoId 查询 PregnantInfo
        /// </summary>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        public PregnantInfo GetPregnantInfoById(long pregnantInfoId)
        {
            return _context.DbGroup.Connection.Query<PregnantInfo>($"select * from [{PregnantInfo.TableName}] where Id = @PregnantInfoId;", new { pregnantInfoId }, transaction: _transaction).FirstOrDefault();
        }

        ///// <summary>
        ///// 获取孕妇档案分页列表
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //internal IEnumerable<PagedListOfPregnantInfoModel> GetPregnantInfoPagedList(GetPagedListOfPregnantInfoRequest request)
        //{
        //    var sql = request.ToListSQL();
        //    var pars = request.GetParams();
        //    return _context.Connection.Query<PagedListOfPregnantInfoModel>(sql, pars, transaction: _transaction).ToList();
        //}

        ///// <summary>
        ///// 获取孕妇档案分页列表
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //internal DataTable GetConfigurablePregnantInfoPagedList(GetPagedListOfPregnantInfoRequest request)
        //{
        //    var sql = request.ToListSQL();
        //    var pars = request.GetParams();

        //    DataTable table = new DataTable("MyTable");
        //    var reader=  _context.Connection.ExecuteReader(sql, pars, transaction: _transaction);
        //    table.Load(reader);
        //    return table;

        //}

        ///// <summary>
        ///// 获取孕妇档案分页计数
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //internal int GetPregnantInfoPagedListCount(GetPagedListOfPregnantInfoRequest request)
        //{
        //    var sql = request.ToCountSQL();
        //    var pars = request.GetParams();
        //    return _context.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        //}
    }
}