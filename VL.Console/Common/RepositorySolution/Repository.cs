using Dapper.Contrib.Extensions;
using System.Data;
using VL.Consolo_Core.Common.DBSolution;

namespace VL.Consolo_Core.Common.RepositorySolution
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected DbContext _context;
        protected IDbConnection _connection { get { return _context.DbGroup.Connection; } }
        protected IDbCommand _command { get { return _context.DbGroup.Command; } }
        protected IDbTransaction _transaction { get { return _command.Transaction; } }

        public Repository(DbContext context)
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