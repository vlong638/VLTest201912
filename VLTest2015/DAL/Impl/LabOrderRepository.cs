using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VLTest2015.Services;
using System;
using VLTest2015.Common.Models.RequestDTO;

namespace VLTest2015.DAL
{
    public class LabOrderRepository : Repository<LabOrder>
    {
        public string TableName { set; get; } = nameof(LabOrder);

        public LabOrderRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<LabOrder> GetAll()
        {
            return _context.Connection.Query<LabOrder>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        }

        internal IEnumerable<PagedListOfLabOrderModel> GetLabOrderPagedList(GetPagedListOfLabOrderRequest request)
        {
            var sql = request.ToListSQL();
            var pars = request.GetParams();
            return _context.Connection.Query<PagedListOfLabOrderModel>(sql, pars, transaction: _transaction).ToList();
        }

        internal int GetLabOrderPagedListCount(GetPagedListOfLabOrderRequest request)
        {
            var sql = request.ToCountSQL();
            var pars = request.GetParams();
            return _context.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        }
    }
}