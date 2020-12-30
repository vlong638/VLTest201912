using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class ProjectLogRepository : Repository<ProjectLog>
    {
        public ProjectLogRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectLog"></param>
        /// <returns></returns>
        public long InsertOne(ProjectLog ProjectLog)
        {
            return _connection.Execute("INSERT INTO [ProjectLog]([ProjectId], [OperatorId], [CreatedAt], [ActionType], [Text]) VALUES (@ProjectId, @OperatorId, GetDate(), @ActionType, @Text);"
                , ProjectLog, transaction: _transaction);
        }

        public int InsertBatch(IEnumerable<ProjectLog> businessEntityProperties)
        {
            int i = 0;
            foreach (var ProjectLog in businessEntityProperties)
            {
                Insert(ProjectLog);
                i++;
            }
            return i;
        }

        internal List<ProjectLog> GetByProjectId(long projectId)
        {
            return _connection.Query<ProjectLog>("select * from [ProjectLog] where projectId = @projectId order by id desc"
                , new { projectId }, transaction: _transaction).ToList();
        }

        internal List<ProjectLog> GetProjectLogs(GetProjectOperateHistoryRequest request)
        {
            return _connection.Query<ProjectLog>(@$"select * from [ProjectLog] 
where projectId = @projectId 
{(request.OperateTimeStart.HasValue ? " and CreatedAt>=@OperateTimeStart" : "")}
{(request.OperateTimeEnd.HasValue ? " and CreatedAt<=@OperateTimeEnd" : "")}
{(request.OperatorId.HasValue && request.OperatorId != 0 ? " and OperatorId =@OperatorId" : "")}
order by id desc"
                , new
                {
                    projectId = request.ProjectId,
                    OperateTimeStart = request.OperateTimeStart,
                    OperateTimeEnd = request.OperateTimeEnd,
                    OperatorId = request.OperatorId,
                }, transaction: _transaction).ToList();
        }
    }
}