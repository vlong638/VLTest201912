using Dapper;
using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Research.Models;

namespace VL.Research.Repositories
{
    public class VisitRecordRepository : Repository<VisitRecord>
    {
        public string TableName { set; get; } = nameof(VisitRecord);

        public VisitRecordRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<VisitRecord> GetAll()
        {
            return _context.DbGroup.Connection.Query<VisitRecord>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        }

        //internal IEnumerable<PagedListOfVisitRecordModel> GetVisitRecordPagedList(GetPagedListOfVisitRecordRequest request)
        //{
        //    var sql = request.ToListSQL();
        //    var pars = request.GetParams();
        //    return _context.Connection.Query<PagedListOfVisitRecordModel>(sql, pars, transaction: _transaction).ToList();
        //}

        //internal int GetVisitRecordPagedListCount(GetPagedListOfVisitRecordRequest request)
        //{
        //    var sql = request.ToCountSQL();
        //    var pars = request.GetParams();
        //    return _context.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        //}
    }
}