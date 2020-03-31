using Dapper.Contrib.Extensions;

namespace VL.API.Common.Repositories
{
    public class DapperRepository<TEntity> :DbRepository
        where TEntity : class
    {
        public DapperRepository(DbGroup dbGroup) : base(dbGroup)
        {
        }

        public long Insert(TEntity entity)
        {
            return dbGroup.Connection.Insert(entity, dbGroup.Transaction);
        }

        public bool Delete(TEntity entity)
        {
            return dbGroup.Connection.Delete(entity, dbGroup.Transaction);
        }

        public bool DeleteById(long id)
        {
            var entity = dbGroup.Connection.Get<TEntity>(id, dbGroup.Transaction);
            if (entity == null)
                return false;
            return dbGroup.Connection.Delete(entity, dbGroup.Transaction);
        }

        public bool Update(TEntity entity)
        {
            return dbGroup.Connection.Update(entity, dbGroup.Transaction);
        }

        public TEntity GetById(long id)
        {
            return dbGroup.Connection.Get<TEntity>(id, dbGroup.Transaction);
        }
    }
}
