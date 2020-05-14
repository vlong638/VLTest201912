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

        internal IEnumerable<PagedListOfPregnantInfoModel> GetPregnantInfoPagedList(GetPagedListOfPregnantInfoRequest request)
        {
            var sql = request.ToListSQL();
            var pars = request.GetParams();
            return _context.Connection.Query<PagedListOfPregnantInfoModel>(sql, pars, transaction: _transaction).ToList();
        }

        internal int GetPregnantInfoPagedListCount(GetPagedListOfPregnantInfoRequest request)
        {
            var sql = request.ToCountSQL();
            var pars = request.GetParams();
            return _context.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }
    }
}