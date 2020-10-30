using Dapper;
using System.Collections.Generic;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using BBee.Models;

namespace BBee.Repositories
{
    public class LabCheckRepository : Repository<LabCheck>
    {
        public string TableName { set; get; } = nameof(LabCheck);

        public LabCheckRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<LabCheck> GetAll()
        {
            return context.DbGroup.Connection.Query<LabCheck>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        }

        //internal IEnumerable<PagedListOfLabCheckModel> GetLabCheckPagedList(GetPagedListOfLabCheckRequest request)
        //{
        //    var sql = request.ToListSQL();
        //    var pars = request.GetParams();
        //    return _context.DbGroup.Connection.Query<PagedListOfLabCheckModel>(sql, pars, transaction: _transaction).ToList();
        //}

        //internal int GetLabCheckPagedListCount(GetPagedListOfLabCheckRequest request)
        //{
        //    var sql = request.ToCountSQL();
        //    var pars = request.GetParams();
        //    return _context.DbGroup.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        //}
    }
}