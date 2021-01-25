using ResearchAPI.CORS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class EditTaskV2Request
    {
        /// <summary>
        /// 
        /// </summary>
        public long TaskId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public GroupedCondition GroupedCondition { set; get; }
    }

    /// <summary>
    /// 组合条件
    /// </summary>
    public class GroupedCondition
    {
        /// <summary>
        /// 组合方式: 
        /// true为And 
        /// false为Or
        /// </summary>
        public bool IsAnd { set; get; }
        /// <summary>
        /// 条件项目
        /// </summary>
        public List<EditTaskWhereModel> WhereConditions { set; get; } = new List<EditTaskWhereModel>();
        /// <summary>
        /// 条件项目
        /// </summary>
        public List<GroupedCondition> GroupedConditions { set; get; } = new List<GroupedCondition>();

        internal bool CreateTaskWhere(ProjectTaskWhere c, ProjectTask projectTask, List<ProjectIndicatorDisplayModel> projectIndicators, ProjectTaskWhereRepository projectTaskWhereRepository)
        {
            //GroupedCondition
            var currentGroup = new ProjectTaskWhere()
            {
                ParentId = c?.Id,
                ProjectId = projectTask.ProjectId,
                TaskId = projectTask.Id,
                WhereCategory = IsAnd ? ProjectTaskWhereCategory.GroupAnd : ProjectTaskWhereCategory.GroupOr,
            };
            currentGroup.Id = projectTaskWhereRepository.Insert(currentGroup);
            //WhereConditions
            foreach (var where in WhereConditions)
            {
                var indicator = projectIndicators.FirstOrDefault(d => d.Id == where.IndicatorId);
                if (indicator == null)
                {
                    throw new NotImplementedException("项目指标缺失");
                }
                var item = new ProjectTaskWhere()
                {
                    ParentId = currentGroup.Id,
                    ProjectId = projectTask.ProjectId,
                    TaskId = projectTask.Id,
                    IndicatorId = indicator.Id,
                    BusinessEntityId = indicator.BusinessEntityId,
                    BusinessEntityPropertyId = indicator.BusinessEntityPropertyId,
                    Operator = (ProjectTaskWhereOperator)Enum.Parse(typeof(ProjectTaskWhereOperator), where.Operator),
                    Value = where.Value,
                };
                if (!indicator.IsTemplate())
                {
                    item.EntityName = DomainConstraits.RenderIdToText(indicator.BusinessEntityId, DomainConstraits.BusinessEntitySourceDic);
                    item.PropertyName = DomainConstraits.RenderIdToText(indicator.BusinessEntityPropertyId, DomainConstraits.BusinessEntityPropertySourceDic);
                }
                else
                {
                    item.EntityName = indicator.EntitySourceName;
                    item.PropertyName = indicator.PropertySourceName;
                }
                projectTaskWhereRepository.Insert(item);
            }
            //GroupedConditions
            foreach (var group in GroupedConditions)
            {
                group.CreateTaskWhere(currentGroup, projectTask, projectIndicators, projectTaskWhereRepository);
            }
            return true;
        }
    }
}
