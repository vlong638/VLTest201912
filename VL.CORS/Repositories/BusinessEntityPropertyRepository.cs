using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class BusinessEntityPropertyRepository : Repository<BusinessEntityProperty>
    {
        public BusinessEntityPropertyRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(BusinessEntityProperty businessEntityProperty)
        {
            return Insert(businessEntityProperty);
        }

        public int InsertBatch(IEnumerable<BusinessEntityProperty> businessEntityProperties)
        {
            int i = 0;
            foreach (var businessEntityProperty in businessEntityProperties)
            {
                Insert(businessEntityProperty);
                i++;
            }
            return i;
        }

        internal BusinessEntityProperty GetOne(BusinessEntityProperty BusinessEntityProperty)
        {
            return _connection.Query<BusinessEntityProperty>("select * from [BusinessEntityProperty] where ProjectId = @ProjectId and UserId = @UserId"
                , BusinessEntityProperty, transaction: _transaction).FirstOrDefault();
        }

        internal int DeleteOne(BusinessEntityProperty BusinessEntityProperty)
        {
            return _connection.Execute("delete from [BusinessEntityProperty] where ProjectId = @ProjectId and UserId = @UserId"
                , BusinessEntityProperty, transaction: _transaction);
        }
    }
}