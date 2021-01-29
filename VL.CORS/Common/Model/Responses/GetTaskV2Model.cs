using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetTaskV2Model
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 队列Id
        /// </summary>
        public long TaskId { set; get; }
        /// <summary>
        /// 队列名称
        /// </summary>
        public string TaskName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public GetTaskV2GroupedCondition GroupedCondition { set; get; }

        /// <summary>
        /// 执行状态
        /// </summary>
        public ScheduleStatus ScheduleStatus { set; get; }
        /// <summary>
        /// 执行状态_文本
        /// </summary>
        public string ScheduleStatusName { set; get; }
        /// <summary>
        /// 可导出文件
        /// </summary>
        public string ResultFile { set; get; }
        /// <summary>
        /// 上一次执行完成时间
        /// </summary>
        public DateTime? LastCompletedAt { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetTaskV2WhereCondition
    {
        /// <summary>
        /// 
        /// </summary>
        public long IndicatorId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string IndicatorName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Operator { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string OperatorName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { set; get; }
    }

    /// <summary>
    /// 组合条件
    /// </summary>
    public class GetTaskV2GroupedCondition
    {
        /// <summary>
        /// 
        /// </summary>
        public GetTaskV2GroupedCondition()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="taskProperties"></param>
        /// <param name="taskWheres"></param>
        public GetTaskV2GroupedCondition(ProjectTaskWhere current, List<ProjectIndicator> taskProperties, List<ProjectTaskWhere> taskWheres)
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
                WhereConditions.Add(new GetTaskV2WhereCondition()
                {
                    IndicatorId = where.IndicatorId,
                    IndicatorName = DomainConstraits.RenderIdToText(where.IndicatorId, taskProperties.ToDictionary(key => key.Id, value => value.PropertyDisplayName)),
                    Operator = where.Operator.ToString(),
                    OperatorName = where.Operator.GetDescription(),
                    Value = where.Value,
                });
            }
            var groups = taskWheres.Where(c => c.ParentId == current.Id && c.WhereCategory != ProjectTaskWhereCategory.Indicator);
            foreach (var group in groups)
            {
                GroupedConditions.Add(new GetTaskV2GroupedCondition(group, taskProperties, taskWheres));
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
        public List<GetTaskV2WhereCondition> WhereConditions { set; get; } = new List<GetTaskV2WhereCondition>();
        /// <summary>
        /// 条件项目
        /// </summary>
        public List<GetTaskV2GroupedCondition> GroupedConditions { set; get; } = new List<GetTaskV2GroupedCondition>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class BOWhereCondition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public BOWhereCondition(ProjectTaskWhere c)
        {
            Field2ValueWhere = new Field2ValueWhere()
            {
                EntityName = c.EntityName,
                FieldName = c.PropertyName,
                Operator = (WhereOperator)Enum.Parse(typeof(WhereOperator), ((int)c.Operator).ToString()),
                Value = c.Value,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Field2ValueWhere Field2ValueWhere { set; get; }

        internal string GetSQL()
        {
            return Field2ValueWhere.ToSQL();
        }
    }

    /// <summary>
    /// 组合条件
    /// </summary>
    public class BOGroupedCondition 
    {
        /// <summary>
        /// 
        /// </summary>
        public ProjectTaskWhere ProjectTaskWhere { set; get; }

        /// <summary>
        /// 组合方式: 
        /// true为And 
        /// false为Or
        /// </summary>
        public bool IsAnd { set; get; }
        /// <summary>
        /// 条件项目
        /// </summary>
        public List<BOWhereCondition> WhereConditions { set; get; } = new List<BOWhereCondition>();
        /// <summary>
        /// 条件项目
        /// </summary>
        public List<BOGroupedCondition> GroupedConditions { set; get; } = new List<BOGroupedCondition>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="taskProperties"></param>
        /// <param name="taskWheres"></param>
        public BOGroupedCondition(ProjectTaskWhere current, List<ProjectIndicator> taskProperties, List<ProjectTaskWhere> taskWheres) 
        {
            ProjectTaskWhere = current;
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
            WhereConditions = new List<BOWhereCondition>();
            var wheres = taskWheres.Where(c => c.ParentId == current.Id && c.WhereCategory == ProjectTaskWhereCategory.Indicator);
            foreach (var where in wheres)
            {
                WhereConditions.Add(new BOWhereCondition(where));
            }
            GroupedConditions = new List<BOGroupedCondition>();
            var groups = taskWheres.Where(c => c.ParentId == current.Id && c.WhereCategory != ProjectTaskWhereCategory.Indicator);
            foreach (var group in groups)
            {
                GroupedConditions.Add(new BOGroupedCondition(group, taskProperties, taskWheres));
            }
        }

        internal string GetSQL()
        {
            List<string> items = new List<string>();
            items.AddRange(WhereConditions.Select(c => c.GetSQL()));
            items.AddRange(GroupedConditions.Select(c=>c.GetSQL()));
            if (items.Count==0)
            {
                return "";
            }
            return $"({string.Join(IsAnd ? " and " : " or ", items)})";
        }
    }
}

