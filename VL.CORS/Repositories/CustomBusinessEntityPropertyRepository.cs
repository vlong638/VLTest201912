using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class CustomBusinessEntityPropertyRepository : Repository<CustomBusinessEntityProperty>
    {
        public CustomBusinessEntityPropertyRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(CustomBusinessEntityProperty CustomerBusinessEntityProperty)
        {
            return Insert(CustomerBusinessEntityProperty);
        }

        public int InsertBatch(IEnumerable<CustomBusinessEntityProperty> businessEntityProperties)
        {
            int i = 0;
            foreach (var CustomerBusinessEntityProperty in businessEntityProperties)
            {
                Insert(CustomerBusinessEntityProperty);
                i++;
            }
            return i;
        }

        public List<CustomBusinessEntityProperty> GetByBusinessEntityIds(List<long> businessEntityIds)
        {
            return _connection.Query<CustomBusinessEntityProperty>("select * from [CustomBusinessEntityProperty] where businessEntityId in @businessEntityIds"
                , new { businessEntityIds }, transaction: _transaction).ToList();
        }
    }
}