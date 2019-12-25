using Dapper.Contrib.Extensions;
using System.Data;

namespace VLTest2015.DAL
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected IDbConnection _connection;

        public Repository(IDbConnection connection)
        {
            this._connection = connection;
        }

        public long Insert(TEntity entity)
        {
            return _connection.Insert(entity);
        }

        public int Insert(TEntity[] entitys)
        {
            return (int)this._connection.Insert(entitys);
        }

        public bool Delete(TEntity entity)
        {
            return _connection.Delete(entity);
        }

        public bool DeleteById(long id)
        {
            var entity = _connection.Get<TEntity>(id);
            if (entity == null)
                return false;
            return _connection.Delete(entity);
        }

        public bool Update(TEntity entity)
        {
            return _connection.Update(entity);
        }

        public TEntity GetById(long id)
        {
            return _connection.Get<TEntity>(id);
        }
    }
}