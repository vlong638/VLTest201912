﻿using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class CustomerBusinessEntityPropertyRepository : Repository<CustomerBusinessEntityProperty>
    {
        public CustomerBusinessEntityPropertyRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(CustomerBusinessEntityProperty CustomerBusinessEntityProperty)
        {
            return Insert(CustomerBusinessEntityProperty);
        }

        public int InsertBatch(IEnumerable<CustomerBusinessEntityProperty> businessEntityProperties)
        {
            int i = 0;
            foreach (var CustomerBusinessEntityProperty in businessEntityProperties)
            {
                Insert(CustomerBusinessEntityProperty);
                i++;
            }
            return i;
        }
    }
}