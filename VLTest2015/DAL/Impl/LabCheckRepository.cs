using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VLTest2015.Services;
using System;
using VLTest2015.Common.Models.RequestDTO;

namespace VLTest2015.DAL
{
    public class LabCheckRepository : Repository<LabCheck>
    {
        public string TableName { set; get; } = nameof(LabCheck);

        public LabCheckRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<LabCheck> GetAll()
        {
            return _context.Connection.Query<LabCheck>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        }

        //internal IEnumerable<PagedListOfLabCheckModel> GetLabCheckPagedList(GetPagedListOfLabCheckRequest request)
        //{
        //    var sql = request.ToListSQL();
        //    var pars = request.GetParams();
        //    return _context.Connection.Query<PagedListOfLabCheckModel>(sql, pars, transaction: _transaction).ToList();
        //}

        //internal int GetLabCheckPagedListCount(GetPagedListOfLabCheckRequest request)
        //{
        //    var sql = request.ToCountSQL();
        //    var pars = request.GetParams();
        //    return _context.Connection.ExecuteScalar<int>(sql, pars, transaction: _transaction);
        //}
    }
}