using Autobots.Infrastracture.Common.DBSolution;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Autobots.Infrastracture.Common.RepositorySolution
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public void UpdateDbContext (DbContext context){
            this.context = context;
        }
        protected DbContext context;
        protected IDbConnection _connection { get { return context.DbGroup.Connection; } }
        protected IDbCommand _command { get { return context.DbGroup.Command; } }
        protected IDbTransaction _transaction { get { return _command.Transaction; } }

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public virtual long Insert(TEntity entity)
        {
            return _connection.Insert(entity, _transaction);
        }

        public virtual bool Delete(TEntity entity)
        {
            return _connection.Delete(entity, _transaction);
        }

        public virtual bool DeleteById(long id)
        {
            var entity = _connection.Get<TEntity>(id, _transaction);
            if (entity == null)
                return false;
            return _connection.Delete(entity, _transaction);
        }

        public virtual bool Update(TEntity entity)
        {
            return _connection.Update(entity, _transaction);
        }

        public virtual TEntity GetById(long id)
        {
            return _connection.Get<TEntity>(id, _transaction);
        }
    }
}