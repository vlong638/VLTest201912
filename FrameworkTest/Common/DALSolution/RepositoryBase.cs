using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.DALSolution
{
    public class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected DbContext _context;
        protected IDbConnection _connection { get { return _context.DbGroup.Connection; } }
        protected IDbCommand _command { get { return _context.DbGroup.Command; } }
        protected IDbTransaction _transaction { get { return _command.Transaction; } }

        public RepositoryBase(DbContext context)
        {
            this._context = context;
        }

        public long Insert(TEntity entity)
        {
            return _connection.Insert(entity, _transaction);
        }

        public bool Delete(TEntity entity)
        {
            return _connection.Delete(entity, _transaction);
        }

        public bool DeleteById(long id)
        {
            var entity = _connection.Get<TEntity>(id, _transaction);
            if (entity == null)
                return false;
            return _connection.Delete(entity, _transaction);
        }

        public bool Update(TEntity entity)
        {
            return _connection.Update(entity, _transaction);
        }

        public TEntity GetById(long id)
        {
            return _connection.Get<TEntity>(id, _transaction);
        }
    }
}
