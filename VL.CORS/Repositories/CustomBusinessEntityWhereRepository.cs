using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class CustomBusinessEntityWhereRepository : Repository<CustomBusinessEntityWhere>
    {
        public CustomBusinessEntityWhereRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(CustomBusinessEntityWhere CustomBusinessEntityWhere)
        {
            return Insert(CustomBusinessEntityWhere);
        }

        public int InsertBatch(IEnumerable<CustomBusinessEntityWhere> businessEntityProperties)
        {
            int i = 0;
            foreach (var CustomBusinessEntityWhere in businessEntityProperties)
            {
                Insert(CustomBusinessEntityWhere);
                i++;
            }
            return i;
        }

        public List<CustomBusinessEntityWhere> GetByBusinessEntityIds(List<long> businessEntityIds)
        {
            return _connection.Query<CustomBusinessEntityWhere>("select * from [CustomBusinessEntityWhere] where BusinessEntityId in @businessEntityIds"
                , new { businessEntityIds }, transaction: _transaction).ToList();
        }
    }
}