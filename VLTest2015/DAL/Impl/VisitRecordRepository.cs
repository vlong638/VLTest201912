using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VLTest2015.Services;
using System;
using VLTest2015.Common.Models.RequestDTO;

namespace VLTest2015.DAL
{
    public class VisitRecordRepository : Repository<T_VisitRecord>
    {
        public string TableName { set; get; } = nameof(T_VisitRecord);

        public VisitRecordRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<T_VisitRecord> GetAll()
        {
            return _context.Connection.Query<T_VisitRecord>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        }

        internal IEnumerable<PagedListOfVisitRecordModel> GetVisitRecordPagedList(GetPagedListOfVisitRecordRequest request)
        {
            var sql = request.ToListSQL();
            var pars = request.GetParams();
            return _context.Connection.Query<PagedListOfVisitRecordModel>(sql, pars, transaction: _transaction).ToList();
        }

        internal int GetVisitRecordPagedListCount(GetPagedListOfVisitRecordRequest request)
        {
            var sql = request.ToCountSQL();
            var pars = request.GetParams();
            return _context.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }
    }
}