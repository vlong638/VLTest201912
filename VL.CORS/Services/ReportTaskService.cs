using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.ExcelSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ServiceSolution;
using Autobots.Infrastracture.Common.TransactionSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Controllers;
using ResearchAPI.CORS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using VLAutobots.Infrastracture.Common.FileSolution;
using static ResearchAPI.CORS.Common.DomainConstraits;

namespace ResearchAPI.CORS.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportTaskService
    {
        Log4NetLogger Logger = Log4NetLogger.GetLogger();
        APIContext APIContext { get; set; }
        DbContext ResearchDbContext { set; get; }

        BusinessEntityPropertyRepository BusinessEntityPropertyRepository { set; get; }
        CustomBusinessEntityPropertyRepository CustomBusinessEntityPropertyRepository { set; get; }
        CustomBusinessEntityRepository CustomBusinessEntityRepository { set; get; }
        CustomBusinessEntityWhereRepository CustomBusinessEntityWhereRepository { set; get; }
        DataStatisticsRepository DataStatisticsRepository { set; get; }
        FavoriteProjectRepository FavoriteProjectRepository { set; get; }
        ProjectDepartmentRepository ProjectDepartmentRepository { set; get; }
        ProjectRepository ProjectRepository { set; get; }
        ProjectLogRepository ProjectLogRepository { set; get; }
        ProjectScheduleRepository ProjectScheduleRepository { set; get; }
        ProjectTaskRepository ProjectTaskRepository { set; get; }
        ProjectTaskWhereRepository ProjectTaskWhereRepository { set; get; }
        ProjectMemberRepository ProjectMemberRepository { set; get; }
        ProjectIndicatorRepository ProjectIndicatorRepository { set; get; }
        RoleRepository RoleRepository { set; get; }
        SharedRepository SharedRepository { set; get; }
        UserRepository UserRepository { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public ReportTaskService(DbContext DbContext)
        {
            ResearchDbContext = DbContext;
            Init(DbContext);
        }
        /// <summary>
        /// 
        /// </summary>
        public ReportTaskService(APIContext apiContext)
        {
            APIContext = apiContext;
            ResearchDbContext = APIContext?.GetDBContext(APIContraints.ResearchDbContext);
            Init(ResearchDbContext);
        }

        private void Init(DbContext DbContext)
        {
            //repositories
            BusinessEntityPropertyRepository = new BusinessEntityPropertyRepository(DbContext);
            CustomBusinessEntityPropertyRepository = new CustomBusinessEntityPropertyRepository(DbContext);
            CustomBusinessEntityRepository = new CustomBusinessEntityRepository(DbContext);
            CustomBusinessEntityWhereRepository = new CustomBusinessEntityWhereRepository(DbContext);
            DataStatisticsRepository = new DataStatisticsRepository(DbContext);
            FavoriteProjectRepository = new FavoriteProjectRepository(DbContext);
            ProjectDepartmentRepository = new ProjectDepartmentRepository(DbContext);
            ProjectRepository = new ProjectRepository(DbContext);
            ProjectLogRepository = new ProjectLogRepository(DbContext);
            ProjectScheduleRepository = new ProjectScheduleRepository(DbContext);
            ProjectTaskRepository = new ProjectTaskRepository(DbContext);
            ProjectTaskWhereRepository = new ProjectTaskWhereRepository(DbContext);
            ProjectMemberRepository = new ProjectMemberRepository(DbContext);
            ProjectIndicatorRepository = new ProjectIndicatorRepository(DbContext);
            RoleRepository = new RoleRepository(DbContext);
            SharedRepository = new SharedRepository(DbContext);
            UserRepository = new UserRepository(DbContext);
        }

        internal ServiceResult<List<VLKeyValue<string, long>>> GetFavoriteProjects(long userid)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = ProjectRepository.GetFavoriteProjects(userid);
                return new List<VLKeyValue<string, long>>(
                    result.Select(c => new VLKeyValue<string, long>(c.ProjectName, c.ProjectId)).ToList()
                );
            }, Logger);
        }

        internal ServiceResult<Dictionary<string, long>> GetProjectMemberIdAndName(long projectId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var users = ProjectMemberRepository.GetUsersByProjectId(projectId);
                return users.ToDictionary(key => key.Name, value => value.Id);
            }, Logger);
        }

        internal ServiceResult<GetProjectModel> GetProject(long projectId, long userId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = new GetProjectModel();
                var project = ProjectRepository.GetById(projectId);
                if (project == null)
                {
                    throw new NotImplementedException("项目不存在");
                }
                project.MapTo(result);
                result.ProjectName = project.Name;
                result.AdminIds = ProjectRepository.GetUserIdsByProjectIdAndRoleId(projectId, DomainConstraits.AdminRoleId.Value);
                result.MemberIds = ProjectRepository.GetUserIdsByProjectIdAndRoleId(projectId, DomainConstraits.MemberRoleId.Value);
                result.DepartmentIds = ProjectDepartmentRepository.GetDepartmentIdsByProjectId(projectId);
                result.IsFavorite = FavoriteProjectRepository.GetOne(new FavoriteProject(project.Id, userId)) != null;
                result.CreatorId = project.CreatorId;
                return result;
            }, Logger);
        }

        internal ServiceResult<Dictionary<long, string>> GetAllUsersIdAndName()
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = DomainConstraits.Users;
                return result;
            }, Logger);
        }

        internal ServiceResult<Dictionary<long, string>> GetUsersDictionary()
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = UserRepository.GetAllUsers();
                return result.ToDictionary(c => c.Id, c => c.Name);
            }, Logger);
        }

        internal ServiceResult<Dictionary<long, string>> GetProjectRolesDictionary()
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = RoleRepository.GetAllProjectRoles();
                return result.ToDictionary(c => c.Id, c => c.Name);
            }, Logger);
        }

        internal ServiceResult<List<User>> GetAllUsers()
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = UserRepository.GetAllUsers();
                return result;
            }, Logger);
        }

        internal ServiceResult<bool> DeleteProject(long projectId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = ProjectRepository.DeleteById(projectId);
                return result;
            }, Logger);
        }

        internal ServiceResult<VLPagerResult<List<Dictionary<string, object>>>> GetPagedResultBySQLConfig(GetCommonSelectRequest request)
        {
            var sqlConfig = ConfigHelper.GetSQLConfigByDirectoryName("ProjectList");
            sqlConfig.PageIndex = request.page;
            sqlConfig.PageSize = request.limit;
            sqlConfig.UpdateWheres(request.search);
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var list = SharedRepository.GetCommonSelect(sqlConfig.Source, sqlConfig.Skip, sqlConfig.Limit);
                var count = SharedRepository.GetCommonSelectCount(sqlConfig.Source);
                sqlConfig.Source.DoTransforms(ref list);
                return new VLPagerResult<List<Dictionary<string, object>>>() { List = list.ToList(), Count = count };
            }, Logger);
        }

        internal ServiceResult<int> DeleteProjectIndicators(List<long> indicatorIds)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = ProjectIndicatorRepository.DeleteByIds(indicatorIds);
                return result;
            }, Logger);
        }

        internal ServiceResult<List<BusinessEntityPropertyModel>> CreateCustomIndicator(CreateCustomIndicatorRequest request, BusinessEntityTemplate template)
        {
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                List<BusinessEntityPropertyModel> results = new List<BusinessEntityPropertyModel>();
                foreach (var periodTemplate in request.PeriodTemplates)
                {
                    //自定义实体对象
                    var templateBE = new CustomBusinessEntity()
                    {
                        DisplayName = template.BusinessEntity.DisplayName,
                        TemplateId = template.Id
                    };
                    //自定义实体属性
                    var templateBEProperties = template.BusinessEntity.Properties.Select(c => new CustomBusinessEntityProperty()
                    {
                        EntityName = templateBE.Name,
                        Name = c.SourceName,
                        DisplayName = c.DisplayName,
                        ColumnType = c.ColumnType,
                        EnumType = c.EnumType,
                        TemplateId = template.Id,
                        TemplatePropertyId = c.Id,
                    }).ToList();
                    //自定义实体条件
                    //普通传参
                    var templateBEWheres = request.Search.Select(c => new CustomBusinessEntityWhere()
                    {
                        ComponentName = c.Key,
                        Value = c.Value,
                        DisplayName = template.SQLConfig.Wheres.First(d => d.ComponentName == c.Key).DisplayName,
                        Operator = "eq",
                    }).ToList();
                    //多区间传参
                    templateBEWheres.Add(new CustomBusinessEntityWhere()
                    {
                        ComponentName = periodTemplate.StartAtComponentName,
                        Value = periodTemplate.StartAt,
                        DisplayName = template.SQLConfig.Wheres.First(d => d.ComponentName == periodTemplate.StartAtComponentName).DisplayName,
                        Operator = "eq",
                    });
                    templateBEWheres.Add(new CustomBusinessEntityWhere()
                    {
                        ComponentName = periodTemplate.EndAtComponentName,
                        Value = periodTemplate.EndAt,
                        DisplayName = template.SQLConfig.Wheres.First(d => d.ComponentName == periodTemplate.EndAtComponentName).DisplayName,
                        Operator = "eq",
                    });
                    //项目属性
                    var itemPrefix = "";
                    var itemId = request.Search.FirstOrDefault(c => c.Key == "检验类别")?.Value ?? "";
                    if (!itemId.IsNullOrEmpty())
                    {
                        itemPrefix = "_" + DomainConstraits.RenderIdToText(itemId, DomainConstraits.LabResultItems);
                    }
                    var projectProperties = request.Properties.Select(c =>
                    {
                        var templateProperty = template.BusinessEntity.Properties.FirstOrDefault(d => d.Id == c.TemplatePropertyId);
                        if (templateProperty == null)
                            throw new NotImplementedException($"无效的模板属性:{ c.TemplatePropertyId}");
                        return new ProjectIndicator()
                        {
                            TemplateId = template.Id,
                            TemplatePropertyId = templateProperty.Id,
                            ProjectId = request.ProjectId,
                            PropertySourceName = templateProperty.SourceName,
                            PropertyDisplayName = $"{periodTemplate.StartAt}_{periodTemplate.EndAt}{itemPrefix}_{templateProperty.DisplayName}_{request.GetRuleDisplayName()}",
                        };
                    }).ToList();
                    results.AddRange(CreateCustomProjectIndicator(templateBE, templateBEProperties, templateBEWheres, projectProperties));
                }
                return results;
            }, Logger);
        }

        internal ServiceResult<List<VLKeyValue<string, string>>> GetStatistics(List<long> categories)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                return DataStatisticsRepository.GetByCategories(categories).Select(c => new VLKeyValue<string, string>(((int)c.Category).ToString(), c.Value)).ToList();
            }, Logger);
        }

        private List<BusinessEntityPropertyModel> CreateCustomProjectIndicator(CustomBusinessEntity templateBE, List<CustomBusinessEntityProperty> templateBEProperties, List<CustomBusinessEntityWhere> templateBEWheres, List<ProjectIndicator> projectProperties)
        {
            var entityId = CustomBusinessEntityRepository.InsertOne(templateBE);
            templateBEProperties.ForEach(c =>
            {
                c.BusinessEntityId = entityId;
                c.Id = CustomBusinessEntityPropertyRepository.InsertOne(c);
            });
            templateBEWheres.ForEach(c =>
            {
                c.BusinessEntityId = entityId;
                c.Id = CustomBusinessEntityWhereRepository.InsertOne(c);
            });
            projectProperties.ForEach(c =>
            {
                c.BusinessEntityId = entityId;
                c.BusinessEntityPropertyId = templateBEProperties.First(d => d.TemplatePropertyId == c.TemplatePropertyId).Id;
                c.EntitySourceName = entityId.ToString();
                c.Id = ProjectIndicatorRepository.InsertOne(c);
            });
            return projectProperties.Select(c => new BusinessEntityPropertyModel()
            {
                TemplatePropertyId = c.Id,
            }).ToList();
        }

        internal ServiceResult<List<GetProjectOperateHistoryModel>> GetProjectOperateHistory(GetProjectOperateHistoryRequest request)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = ProjectLogRepository.GetProjectLogs(request).Select(d => new GetProjectOperateHistoryModel()
                {
                    OperateAt = d.CreatedAt,
                    OperatorSummary = d.Text,
                }).ToList();
                return result;
            }, Logger);
        }

        internal ServiceResult<bool> DeleteProjectIndicator(long indicatorId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var result = ProjectIndicatorRepository.DeleteById(indicatorId);
                return result;
            }, Logger);
        }

        internal ServiceResult<long> CreateProject(CreateProjectRequest request, long userid)
        {
            var project = new Project()
            {
                Name = request.ProjectName,
                CreatedAt = DateTime.Now,
                CreatorId = userid,
                ProjectDescription = request.ProjectDescription,
                ViewAuthorizeType = request.ViewAuthorizeType,
            };
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var projectId = ProjectRepository.Insert(project);
                var members = new List<ProjectMember>();
                members.Add(new ProjectMember(projectId, project.CreatorId, DomainConstraits.OwnerRoleId.Value));
                foreach (var adminId in request.AdminIds ?? new List<long>())
                    members.Add(new ProjectMember(projectId, adminId, DomainConstraits.AdminRoleId.Value));
                foreach (var memberId in request.MemberIds ?? new List<long>())
                    members.Add(new ProjectMember(projectId, memberId, DomainConstraits.MemberRoleId.Value));
                ProjectMemberRepository.BatchInsert(members);
                var projectDepartments = request.DepartmentIds.Select(c => new ProjectDepartment() { ProjectId = projectId, DepartmentId = c }).ToList();
                ProjectDepartmentRepository.BatchInsert(projectDepartments);
                return projectId;
            }, Logger);
        }

        internal ServiceResult<bool> UpdateIndicatorName(long indicatorId, string name)
        {
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var result = ProjectIndicatorRepository.UpdateIndicatorName(indicatorId, name) > 0;
                return result;
            }, Logger);
        }

        internal ServiceResult<bool> EditProject(EditProjectRequest request, long userid)
        {
            var project = new Project()
            {
                Id = request.ProjectId,
                Name = request.ProjectName,
                CreatedAt = DateTime.Now,
                CreatorId = userid,
                ProjectDescription = request.ProjectDescription,
                ViewAuthorizeType = request.ViewAuthorizeType,
            };
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var oldProject = ProjectRepository.GetById(project.Id);
                if (oldProject == null)
                    throw new NotImplementedException("项目不存在");
                var result = ProjectRepository.Update(project);
                if (!result)
                    throw new NotImplementedException("项目更新失败");
                var projectId = project.Id;
                var members = new List<ProjectMember>();
                members.Add(new ProjectMember(projectId, project.CreatorId, DomainConstraits.OwnerRoleId.Value));
                foreach (var adminId in request.AdminIds)
                    members.Add(new ProjectMember(projectId, adminId, DomainConstraits.AdminRoleId.Value));
                foreach (var memberId in request.MemberIds)
                    members.Add(new ProjectMember(projectId, memberId, DomainConstraits.MemberRoleId.Value));
                ProjectMemberRepository.DeleteByProjectId(projectId);
                ProjectMemberRepository.BatchInsert(members);
                var projectDepartments = request.DepartmentIds.Select(c => new ProjectDepartment() { ProjectId = projectId, DepartmentId = c }).ToList();
                ProjectDepartmentRepository.DeleteByProjectId(projectId);
                ProjectDepartmentRepository.BatchInsert(projectDepartments);
                return true;
            }, Logger);
        }

        internal ServiceResult<List<GetProjectIndicatorModel>> GetProjectIndicators(long projectId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var projectIndicators = ProjectIndicatorRepository.GetProjectIndicatorDisplayModelByProjectId(projectId);
                return projectIndicators.Select(c =>
                {
                    var m = new GetProjectIndicatorModel();
                    c.MapTo(m);
                    m.DisplayName = c.PropertyDisplayName;
                    m.BusinessEntityPropertyId = c.BusinessEntityPropertyId;
                    if (c.IsTemplate())
                    {
                        m.EntityName = c.TemplateDisplayName;
                        m.PropertyName = c.TemplatePropertyDisplayName;
                        m.ColumnType = c.TemplatePropertColumnType;
                    }
                    else
                    {
                        m.PropertyName = DomainConstraits.RenderIdToText(c.BusinessEntityPropertyId, DomainConstraits.BusinessEntityPropertyDisplayDic);
                        m.EntityName = DomainConstraits.RenderIdToText(c.BusinessEntityId, DomainConstraits.BusinessEntityDisplayDic);
                        var coProperty = DomainConstraits.BusinessEntityProperties.FirstOrDefault(d => d.Id == c.BusinessEntityPropertyId);
                        m.ColumnType = coProperty?.ColumnType ?? ColumnType.String;
                        m.EnumType = coProperty?.EnumType;
                    }
                    return m;
                }).ToList();
            }, Logger);
        }

        internal ServiceResult<List<Role>> GetAllRoles()
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                return RoleRepository.GetAllProjectRoles();
            }, Logger);
        }

        internal ServiceResult<bool> AddFavoriteProject(long projectId, long userId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var favoriteProject = new FavoriteProject(projectId, userId);
                var exist = FavoriteProjectRepository.GetOne(favoriteProject);
                if (exist != null)
                    throw new NotImplementedException("已添加收藏");
                var project = ProjectRepository.GetById(projectId);
                if (project == null)
                    throw new NotImplementedException("项目不存在");
                return FavoriteProjectRepository.InsertOne(favoriteProject) > 0;
            }, Logger);
        }

        internal ServiceResult<ProjectTask> GetTaskById(long taskId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var task = ProjectTaskRepository.GetById(taskId);
                if (task == null)
                    throw new NotImplementedException("队列不存在");
                return task;
            }, Logger);
        }

        internal ServiceResult<bool> DeleteFavoriteProject(long projectId, long userId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var favoriteProject = new FavoriteProject(projectId, userId);
                var exist = FavoriteProjectRepository.GetOne(favoriteProject);
                if (exist == null)
                    throw new NotImplementedException("收藏不存在");
                return FavoriteProjectRepository.DeleteOne(favoriteProject) > 0;
            }, Logger);
        }

        internal ServiceResult<long> AddProjectLog(long userId, long projectId, ActionType actionType, string text)
        {
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var projectLog = new ProjectLog()
                {
                    OperatorId = userId,
                    ProjectId = projectId,
                    ActionType = actionType,
                    Text = text,
                };
                return ProjectLogRepository.InsertOne(projectLog);
            }, Logger);
        }

        internal ServiceResult<bool> AddProjectIndicators(AddIndicatorsRequest request)
        {
            //Data
            var projectIndicators = request.BusinessEntityPropertyIds.Select(c =>
            {
                var projectIndicator = new ProjectIndicator();
                projectIndicator.ProjectId = request.ProjectId;
                projectIndicator.BusinessEntityId = request.BusinessEntityId;
                projectIndicator.BusinessEntityPropertyId = c;
                projectIndicator.EntitySourceName = RenderIdToText(request.BusinessEntityId, BusinessEntitySourceDic);
                projectIndicator.PropertySourceName = RenderIdToText(c, BusinessEntityPropertySourceDic);
                projectIndicator.PropertyDisplayName = RenderIdToText(c, BusinessEntityPropertyDisplayDic);
                return projectIndicator;
            }).ToList();
            //Logic
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                ProjectIndicatorRepository.DeleteByEntityId(request.ProjectId, request.BusinessEntityId);
                var successCount = ProjectIndicatorRepository.InsertBatch(projectIndicators);
                return successCount == request.BusinessEntityPropertyIds.Count();
            }, Logger);
        }

        internal ServiceResult<long> CreateTask(CreateTaskRequest request)
        {
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var old = ProjectTaskRepository.GetByName(request.ProjectId, request.TaskName);
                if (old != null)
                {
                    throw new NotImplementedException("队列名称重复");
                }
                var task = new ProjectTask()
                {
                    Name = request.TaskName,
                    ProjectId = request.ProjectId,
                };
                task.Id = ProjectTaskRepository.Insert(task);
                if (request.CopyTaskId.HasValue && request.CopyTaskId > 0)
                {
                    var projectTaskWheres = ProjectTaskWhereRepository.GetByTaskId(request.CopyTaskId.Value);
                    var current = projectTaskWheres.FirstOrDefault(c => c.ParentId == null);
                    var groupedCondition = new EditTaskV2GroupedCondition(current, projectTaskWheres);
                    EditTaskV2(task.Id, groupedCondition);
                }
                return task.Id;
            }, Logger);
        }

        internal ServiceResult<bool> EditTaskV2(EditTaskV2Request request)
        {
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var taskId = request.TaskId;
                var groupedCondition = request.GroupedCondition;
                EditTaskV2(taskId, groupedCondition);
                return true;
            }, Logger);
        }

        private void EditTaskV2(long taskId, EditTaskV2GroupedCondition groupedCondition)
        {
            //ProjectTask
            var projectTask = ProjectTaskRepository.GetById(taskId);
            if (projectTask == null)
            {
                throw new NotImplementedException("队列不存在");
            }
            //ProjectIndicator
            var projectIndicators = ProjectIndicatorRepository.GetProjectIndicatorDisplayModelByProjectId(projectTask.ProjectId);
            if (projectIndicators.Count == 0)
            {
                throw new NotImplementedException("无项目指标");
            }
            //ProjectTaskWhere
            ProjectTaskWhereRepository.DeleteByTaskId(taskId);
            groupedCondition.CreateTaskWhere(null, projectTask, projectIndicators, ProjectTaskWhereRepository);
        }

        internal ServiceResult<bool> EditTask(EditTaskRequest request)
        {
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var projectTask = ProjectTaskRepository.GetById(request.TaskId);
                if (projectTask == null)
                {
                    throw new NotImplementedException("队列不存在");
                }
                var projectIndicators = ProjectIndicatorRepository.GetProjectIndicatorDisplayModelByProjectId(projectTask.ProjectId);
                if (projectIndicators.Count == 0)
                {
                    throw new NotImplementedException("无项目指标");
                }
                ProjectTaskWhereRepository.DeleteByTaskId(request.TaskId);
                var wheres = request.Wheres.Select(c =>
                {
                    var indicator = projectIndicators.FirstOrDefault(d => d.Id == c.IndicatorId);
                    if (indicator == null)
                    {
                        throw new NotImplementedException("项目指标缺失");
                    }
                    var item = new ProjectTaskWhere()
                    {
                        ProjectId = projectTask.ProjectId,
                        TaskId = projectTask.Id,
                        IndicatorId = indicator.Id,
                        BusinessEntityId = indicator.BusinessEntityId,
                        BusinessEntityPropertyId = indicator.BusinessEntityPropertyId,
                        Operator = c.Operator,
                        Value = c.Value,
                    };
                    if (!indicator.IsTemplate())
                    {
                        item.EntityName = RenderIdToText(indicator.BusinessEntityId, BusinessEntitySourceDic);
                        item.PropertyName = RenderIdToText(indicator.BusinessEntityPropertyId, BusinessEntityPropertySourceDic);
                    }
                    else
                    {
                        item.EntityName = indicator.EntitySourceName;
                        item.PropertyName = indicator.PropertySourceName;
                    }
                    return item;
                }).ToList();
                wheres.ForEach(c =>
                {
                    c.Id = ProjectTaskWhereRepository.Insert(c);
                });
                return true;
            }, Logger);
        }

        internal ServiceResult<bool> StartSchedule(long scheduleId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var schedule = ProjectScheduleRepository.GetById(scheduleId);
                if (schedule == null)
                    throw new NotImplementedException("计划不存在");
                try
                {
                    Logger.Info("开始数据导出");
                    //更新处理任务状态
                    ProjectScheduleRepository.UpdateSchedule(schedule.Id, ScheduleStatus.Started, "", "任务正在执行中");

                    //必要信息整理
                    var task = ProjectTaskRepository.GetById(schedule.TaskId);
                    if (task == null)
                        throw new NotImplementedException("任务不存在");
                    var project = ProjectRepository.GetById(schedule.ProjectId);
                    if (project == null)
                        throw new NotImplementedException("项目不存在");
                    var projectIndicators = ProjectIndicatorRepository.GetByProjectId(schedule.ProjectId);
                    if (projectIndicators.Count == 0)
                    {
                        throw new NotImplementedException("指标不存在");
                    }

                    var taskProperties = ProjectIndicatorRepository.GetByProjectId(project.Id);
                    var taskWheres = ProjectTaskWhereRepository.GetByTaskId(schedule.TaskId);
                    var root = taskWheres.FirstOrDefault(c => c.ParentId == null);
                    var groupedCondition = root != null ? new BOGroupedCondition(root, taskProperties, taskWheres) : null;
                    var customBusinessEntityIndicators = projectIndicators.Where(c => c.BusinessEntityId.ToString().StartsWith("3"));
                    var customBusinessEntities = customBusinessEntityIndicators.Count() > 0
                        ? CustomBusinessEntityRepository.GetByIds(customBusinessEntityIndicators.Select(c => c.BusinessEntityId).Distinct().ToList())
                        : new List<CustomBusinessEntity>();
                    var customBusinessEntityWheres = customBusinessEntityIndicators.Count() > 0
                        ? CustomBusinessEntityWhereRepository.GetByBusinessEntityIds(customBusinessEntityIndicators.Select(c => c.BusinessEntityId).Distinct().ToList())
                        : new List<CustomBusinessEntityWhere>();
                    var defaultRouters = ConfigHelper.GetRouters("Configs/XMLConfigs/BusinessEntities", "Routers.xml");
                    var templates = ConfigHelper.GetBusinessEntityTemplates();

                    //内核处理
                    DataTable dataTable = null;
                    var reportTask = new ReportTask(task.Name);
                    reportTask.Update(projectIndicators, taskWheres, groupedCondition, customBusinessEntities, customBusinessEntityWheres, defaultRouters, templates, reportTask);
                    Logger.Info("引擎装载成功");
                    //string.Join("\r\n",parameters.Select(c=> "declare @"+c.Key+" nvarchar(50); set @"+c.Key+" = '"+ c.Value+"'"))

                    //将核心报表结果处理拆分成几个环节
                    //1.临时表准备
                    //2.核心数据报表
                    //3.临时表清理
                    //4.结果输出

                    var parameters = reportTask.GetParameters();
                    var mainSQL = reportTask.GetSQL();
                    //1.临时表准备
                    var tempTables = reportTask.TempTables;
                    var sqlLog = new StringBuilder();
                    for (int i = 0; i < tempTables.Count(); i++)
                    {
                        var tempTable = tempTables[i];
                        sqlLog.AppendLine(tempTable.SQL);
                    }
                    sqlLog.Append(mainSQL);
                    for (int i = 0; i < tempTables.Count(); i++)
                    {
                        var tempTable = tempTables[i];
                        sqlLog.AppendLine($"drop table {tempTable.Alias}");
                    }
                    Logger.LogSQL(sqlLog.ToString(), parameters);
                    for (int i = 0; i < tempTables.Count(); i++)
                    {
                        var tempTable = tempTables[i];
                        var result = SharedRepository.CreateTempTable(tempTable.SQL, parameters);
                        ProjectScheduleRepository.UpdateSchedule(schedule.Id, ScheduleStatus.Started, "", $"执行中,创建临时表{i + 1}/{tempTables.Count()}");
                    }
                    //2.核心数据报表
                    dataTable = SharedRepository.GetDataTable(mainSQL, parameters);
                    //3.临时表清理
                    for (int i = 0; i < tempTables.Count(); i++)
                    {
                        var tempTable = tempTables[i];
                        var result = SharedRepository.DropTempTable(tempTable.Alias);
                        ProjectScheduleRepository.UpdateSchedule(schedule.Id, ScheduleStatus.Started, "", $"执行中,清理临时表{i + 1}/{tempTables.Count()}");
                    }
                    Logger.Info("查询数据成功");
                    //4.结果输出
                    //转译处理结果
                    var repeatCount = 0;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        //TODO Fix
                        var matchedColumn = projectIndicators.FirstOrDefault(c => c.EntitySourceName + "_" + c.PropertySourceName == column.ColumnName);
                        matchedColumn = matchedColumn ?? projectIndicators.FirstOrDefault(c => c.BusinessEntityId + "_" + c.PropertySourceName == column.ColumnName);
                        var tempColumnName = matchedColumn.PropertyDisplayName;
                        if (dataTable.Columns.Contains(tempColumnName))
                        {
                            tempColumnName += ++repeatCount;
                        }
                        column.ColumnName = tempColumnName;
                    }
                    //输出处理结果
                    var projectIndicators2 = projectIndicators;
                    var filePath = $"{schedule.Id}_{DateTime.Now.ToString("yyyy_MM_dd_mm_hh_ss")}.xlsx";
                    var fullPath = Path.Combine(FileHelper.GetDirectory("Export"), filePath);
                    Logger.Info("开始数据导出");
                    ExcelHelper.ExportDataTableToExcel(dataTable, fullPath);
                    Logger.Info("导出数据成功");
                    //更新处理任务状态
                    ProjectScheduleRepository.UpdateSchedule(schedule.Id, ScheduleStatus.Completed, "Export/" + filePath, "");
                }
                catch (Exception ex)
                {
                    //更新处理任务状态
                    ProjectScheduleRepository.UpdateSchedule(schedule.Id, ScheduleStatus.Failed, "", ex.ToString());
                    throw ex;
                }
                return true;
            }, Logger);
        }

        internal ServiceResult<bool> EditTaskName(EditTaskNameRequest request)
        {
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var task = ProjectTaskRepository.GetById(request.TaskId);
                var old = ProjectTaskRepository.GetByName(task.ProjectId, request.TaskName);
                if (old != null)
                {
                    throw new NotImplementedException("队列名称重复");
                }

                return ProjectTaskRepository.UpdateName(request.TaskId, request.TaskName) > 0;
            }, Logger);
        }

        internal ServiceResult<bool> DeleteTask(long taskId)
        {
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                //ProjectIndicatorRepository.DeleteByTaskId(taskId);
                //ProjectTaskWhereRepository.DeleteByTaskId(taskId);
                return ProjectTaskRepository.DeleteById(taskId);
            }, Logger);
        }

        internal ServiceResult<List<GetTaskV2Model>> GetTasksV2(long projectId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var tasks = ProjectTaskRepository.GetByProjectId(projectId);
                var taskProperties = ProjectIndicatorRepository.GetByProjectId(projectId);
                var taskWheres = ProjectTaskWhereRepository.GetByProjectId(projectId);
                var taskSchedules = ProjectScheduleRepository.GetByProjectId(projectId);
                var result = tasks.Select(d =>
                {
                    var schedule = taskSchedules.FirstOrDefault(e => e.TaskId == d.Id);
                    var model = new GetTaskV2Model()
                    {
                        ProjectId = d.ProjectId,
                        TaskId = d.Id,
                        TaskName = d.Name,
                        ScheduleStatus = schedule?.Status ?? ScheduleStatus.None,
                        ScheduleStatusName = schedule?.Status.GetDescription(),
                        LastCompletedAt = schedule?.LastCompletedAt,
                        ResultFile = schedule?.ResultFile,
                    };
                    var currentTaskWheres = taskWheres.Where(q => q.TaskId == d.Id).ToList();
                    var current = currentTaskWheres.FirstOrDefault(c => c.ParentId == null);
                    if (current != null)
                    {
                        model.GroupedCondition = new GetTaskV2GroupedCondition(taskProperties, new EditTaskV2GroupedCondition(current, currentTaskWheres));
                    }
                    return model;
                }).ToList();
                return result;
            }, Logger);
        }

        internal ServiceResult<List<GetTaskModel>> GetTasks(long projectId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var tasks = ProjectTaskRepository.GetByProjectId(projectId);
                var taskProperties = ProjectIndicatorRepository.GetByProjectId(projectId);
                var taskWheres = ProjectTaskWhereRepository.GetByProjectId(projectId);
                var taskSchedules = ProjectScheduleRepository.GetByProjectId(projectId);
                var result = tasks.Select(d =>
                {
                    var schedule = taskSchedules.FirstOrDefault(e => e.TaskId == d.Id);
                    var model = new GetTaskModel()
                    {
                        ProjectId = d.ProjectId,
                        TaskId = d.Id,
                        TaskName = d.Name,
                        ScheduleStatus = schedule?.Status ?? ScheduleStatus.None,
                        ScheduleStatusName = schedule?.Status.GetDescription(),
                        LastCompletedAt = schedule?.LastCompletedAt,
                        ResultFile = schedule?.ResultFile,
                        Wheres = taskWheres.Where(e => e.TaskId == d.Id)
                    .Select(e =>
                    {
                        var taskPropertiesDic = taskProperties.ToDictionary(key => key.BusinessEntityPropertyId, value => value.PropertyDisplayName);
                        var item = new GetTaskWhereModel();
                        e.MapTo(item);
                        item.OperatorName = item.Operator.GetDescription();
                        item.DisplayName = RenderIdToText(item.BusinessEntityPropertyId, taskPropertiesDic);
                        return item;
                    }).ToList(),
                    };
                    return model;
                }).ToList();
                return result;
            }, Logger);
        }

        internal ServiceResult<List<VLKeyValue<string, string>>> GetTaskNameAndIds(long projectId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                return ProjectTaskRepository.GetTaskNameAndIds(projectId);
            }, Logger);
        }

        internal ServiceResult<long> CommitTask(CommitTaskRequest request)
        {
            var schedule = new ProjectSchedule();
            schedule.ProjectId = request.ProjectId;
            schedule.TaskId = request.TaskId;
            schedule.StartedAt = request.IsStartNow ? DateTime.Now : request.StartAt;
            schedule.Status = ScheduleStatus.Ready;
            schedule.ResultFile = "";
            return ResearchDbContext.DbGroup.DelegateTransaction(c =>
            {
                var old = ProjectScheduleRepository.GetLastestByTaskId(schedule.TaskId);
                if (old != null && old.IsWorking())
                {
                    throw new NotImplementedException("任务已在执行中");
                }
                schedule.Id = ProjectScheduleRepository.InsertOne(schedule);
                return schedule.Id;
            }, Logger);
        }

        internal ServiceResult<GetTaskStatusModel> GetTaskStatus(long taskId)
        {
            return ResearchDbContext.DbGroup.DelegateNonTransaction(c =>
            {
                var taskStatus = ProjectScheduleRepository.GetTaskStatus(taskId);
                if (taskStatus == null)
                {
                    throw new NotImplementedException("执行任务不存在");
                }
                return taskStatus;
            }, Logger);
        }
    }

}
