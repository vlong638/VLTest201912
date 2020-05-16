using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VLTest2015.Services;
using System;
using VLTest2015.Common.Models.RequestDTO;

namespace VLTest2015.DAL
{
    public class PregnantInfoRepository : Repository<T_PregnantInfo>
    {
        public string TableName { set; get; } = nameof(T_PregnantInfo);

        public PregnantInfoRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<T_PregnantInfo> GetAll()
        {
            return _context.Connection.Query<T_PregnantInfo>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        }

        /// <summary>
        /// 根据 PregnantInfoId 查询 PregnantInfo
        /// </summary>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        public T_PregnantInfo GetPregnantInfoById(long pregnantInfoId)
        {
            return _context.Connection.Query<T_PregnantInfo>($"select * from [{TableName}] where Id = @PregnantInfoId;", new { pregnantInfoId }, transaction: _transaction).FirstOrDefault();
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
            return _context.Connection.Query<PagedListOfPregnantInfoModel>(sql, pars, transaction: _transaction).ToList();
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
            return _context.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }
    }
}