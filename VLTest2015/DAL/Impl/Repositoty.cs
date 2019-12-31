using Dapper.Contrib.Extensions;
using System.Data;

namespace VLTest2015.DAL
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected IDbConnection _connection;
        protected IDbCommand _command;
        protected IDbTransaction _transaction { get { return _command.Transaction; } }

        public Repository(Services.BaseContext context)
        {
            this._connection = context._connection;
            this._command = context._command;
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