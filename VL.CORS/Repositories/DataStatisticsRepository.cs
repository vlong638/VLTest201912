using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class DataStatisticsRepository : Repository<DataStatistics>
    {
        public DataStatisticsRepository(DbContext context) : base(context)
        {
        }


        internal List<DataStatistics> GetByCategories(List<long> categories)
        {
            return _connection.Query<DataStatistics>(@$"
select * from [DataStatistics] 
where Category in @categories
"
                , new { categories }, transaction: _transaction).ToList();
        }
    }
}