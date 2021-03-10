using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{

    public class SyncManageRepository : Repository<SyncManage>
    {
        public SyncManageRepository(DbContext context) : base(context)
        {
        }

        public SyncManage GetBy(string from,OperateType operateType,OperateStatus operateStatus)
        {
            return _connection.Query<SyncManage>("select top 1 * from [SyncManage] where [From] like @From and OperateType = @OperateType and OperateStatus = @OperateStatus order by issuetime desc;"
                , new { from, operateType, operateStatus }, transaction: _transaction)
                .FirstOrDefault();
        }
    }
}