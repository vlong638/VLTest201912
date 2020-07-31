using Dapper;
using System.Collections.Generic;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Research.Models;

namespace VL.Research.Repositories
{
    public class LabOrderRepository : Repository<LabOrder>
    {
        public string TableName { set; get; } = nameof(LabOrder);

        public LabOrderRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<LabOrder> GetAll()
        {
            return _context.DbGroup.Connection.Query<LabOrder>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        }

        //internal IEnumerable<PagedListOfLabOrderModel> GetLabOrderPagedList(GetPagedListOfLabOrderRequest request)
        //{
        //    var sql = request.ToListSQL();
        //    var pars = request.GetParams();
        //    return _context.Connection.Query<PagedListOfLabOrderModel>(sql, pars, transaction: _transaction).ToList();
        //}

        //internal int GetLabOrderPagedListCount(GetPagedListOfLabOrderRequest request)
        //{
        //    var sql = request.ToCountSQL();
        //    var pars = request.GetParams();
        //    return _context.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        //}
    }
}