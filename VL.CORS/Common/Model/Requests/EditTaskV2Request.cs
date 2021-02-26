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
        public EditTaskV2GroupedCondition GroupedCondition { set; get; }
    }

    /// <summary>
    /// 组合条件
    /// </summary>
    public class EditTaskV2GroupedCondition
    {
        /// <summary>
        /// 
        /// </summary>
        public EditTaskV2GroupedCondition()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="taskProperties"></param>
        /// <param name="taskWheres"></param>
        public EditTaskV2GroupedCondition(ProjectTaskWhere current, List<ProjectIndicator> taskProperties, List<ProjectTaskWhere> taskWheres)
        {
            switch (current.WhereCategory)
            {
                case ProjectTaskWhereCategory.GroupAnd:
                    IsAnd = true;
                    break;
                case ProjectTaskWhereCategory.GroupOr:
                    IsAnd = false;
                    break;
                default:
                    throw new NotImplementedException("无效的操作类型");
            }
            var wheres = taskWheres.Where(c => c.ParentId == current.Id && c.WhereCategory == ProjectTaskWhereCategory.Indicator);
            foreach (var where in wheres)
            {
                WhereConditions.Add(new EditTaskWhereCondition()
                {
                    IndicatorId = where.IndicatorId,
                    Operator = where.Operator.ToString(),
                    Value = where.Value,
                });
            }
            var groups = taskWheres.Where(c => c.ParentId == current.Id && c.WhereCategory != ProjectTaskWhereCategory.Indicator);
            foreach (var group in groups)
            {
                GroupedConditions.Add(new EditTaskV2GroupedCondition(group, taskProperties, taskWheres));
            }
        }

        /// <summary>
        /// 组合方式: 
        /// true为And 
        /// false为Or
        /// </summary>
        public bool IsAnd { set; get; }
        /// <summary>
        /// 条件项目
        /// </summary>
        public List<EditTaskWhereCondition> WhereConditions { set; get; } = new List<EditTaskWhereCondition>();
        /// <summary>
        /// 条件项目
        /// </summary>
        public List<EditTaskV2GroupedCondition> GroupedConditions { set; get; } = new List<EditTaskV2GroupedCondition>();

        internal bool CreateTaskWhere(ProjectTaskWhere c, ProjectTask projectTask, List<ProjectIndicatorDisplayModel> projectIndicators, ProjectTaskWhereRepository projectTaskWhereRepository)
        {
            //GroupedCondition
            var currentGroup = new ProjectTaskWhere()
            {
                ParentId = c?.Id ?? null,
                ProjectId = projectTask.ProjectId,
                TaskId = projectTask.Id,
                WhereCategory = IsAnd ? ProjectTaskWhereCategory.GroupAnd : ProjectTaskWhereCategory.GroupOr,
            };
            currentGroup.Id = projectTaskWhereRepository.InsertOne(currentGroup);
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
                    WhereCategory = ProjectTaskWhereCategory.Indicator,
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
