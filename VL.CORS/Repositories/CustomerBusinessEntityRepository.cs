using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class CustomerBusinessEntityRepository : Repository<CustomerBusinessEntity>
    {
        public CustomerBusinessEntityRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(CustomerBusinessEntity CustomerBusinessEntity)
        {
            return Insert(CustomerBusinessEntity);
        }

        public int InsertBatch(IEnumerable<CustomerBusinessEntity> businessEntityProperties)
        {
            int i = 0;
            foreach (var CustomerBusinessEntity in businessEntityProperties)
            {
                Insert(CustomerBusinessEntity);
                i++;
            }
            return i;
        }
    }
}