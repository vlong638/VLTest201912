using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectIndicatorRepository : Repository<ProjectIndicator>
    {
        public ProjectIndicatorRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(ProjectIndicator ProjectIndicator)
        {
            return Insert(ProjectIndicator);
        }

        internal int InsertBatch(IEnumerable<ProjectIndicator> businessEntityProperties)
        {
            int i = 0;
            foreach (var ProjectIndicator in businessEntityProperties)
            {
                Insert(ProjectIndicator);
                i++;
            }
            return i;
        }

        internal int DeleteByIds(IEnumerable<long> indicatorIds)
        {
            return _connection.Execute("delete from [ProjectIndicator] where id in @IndicatorIds"
                , new { indicatorIds }, transaction: _transaction);
        }

        internal int DeleteByEntityId(long projectId, long businessEntityId)
        {
            return _connection.Execute("delete from [ProjectIndicator] where ProjectId = @projectId and BusinessEntityId = @businessEntityId"
                , new { projectId, businessEntityId }, transaction: _transaction);
        }

        internal List<ProjectIndicator> GetByProjectId(long projectId)
        {
            return _connection.Query<ProjectIndicator>("select * from [ProjectIndicator] where ProjectId = @ProjectId"
                , new { projectId }, transaction: _transaction).ToList();
        }

        internal List<ProjectIndicatorDisplayModel> GetProjectIndicatorDisplayModelByProjectId(long projectId)
        {
            return _connection.Query<ProjectIndicatorDisplayModel>(@"
select cbe.displayName TemplateDisplayName
,cbep.displayname TemplatePropertyDisplayName
,cbep.ColumnType TemplatePropertColumnType
,cbep.EnumType TemplatePropertEnumType
,pi.* from [ProjectIndicator] pi
left join CustomBusinessEntity cbe on pi.BusinessEntityId =cbe.id
left join CustomBusinessEntityProperty cbep on pi.BusinessEntityPropertyId =cbep.id
where ProjectId = @ProjectId
"
                , new { projectId }, transaction: _transaction).ToList();
        }

        internal int UpdateIndicatorName(long indicatorId, string name)
        {
            return _connection.Execute("update ProjectIndicator set PropertyDisplayName = @name where id = @indicatorId"
                , new { name, indicatorId }, transaction: _transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        public class ProjectIndicatorDisplayModel : ProjectIndicator
        {
            /// <summary>
            /// 模板的业务对象显示名称
            /// </summary>
            public string TemplateDisplayName { set; get; }
            /// <summary>
            /// 模板的指标显示名称
            /// </summary>
            public string TemplatePropertyDisplayName { set; get; }
            /// <summary>
            /// 模板的指标字段类型
            /// </summary>
            public ColumnType TemplatePropertColumnType { set; get; }
            /// <summary>
            /// 模板的指标枚举类型
            /// </summary>
            public string TemplatePropertEnumType { set; get; }
        }
    }
}