using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class CustomBusinessEntityRepository : Repository<CustomBusinessEntity>
    {
        public CustomBusinessEntityRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(CustomBusinessEntity CustomerBusinessEntity)
        {
            return Insert(CustomerBusinessEntity);
        }

        public int InsertBatch(IEnumerable<CustomBusinessEntity> businessEntityProperties)
        {
            int i = 0;
            foreach (var CustomerBusinessEntity in businessEntityProperties)
            {
                Insert(CustomerBusinessEntity);
                i++;
            }
            return i;
        }

        public List<CustomBusinessEntity> GetByIds(List<long> ids)
        {
            return _connection.Query<CustomBusinessEntity>("select * from [CustomBusinessEntity] where id in @ids"
                , new { ids }, transaction: _transaction).ToList();
        }
    }
}