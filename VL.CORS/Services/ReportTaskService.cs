﻿using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.ExcelSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ServiceSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Http;
using ResearchAPI.Controllers;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using VLAutobots.Infrastracture.Common.FileSolution;
using static ResearchAPI.CORS.Common.DomainConstraits;

namespace ResearchAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportTaskService
    {
        APIContext APIContext { get; set; }
        DbContext ResearchDbContext { set; get; }

        CustomBusinessEntityPropertyRepository CustomBusinessEntityPropertyRepository { set; get; }
        CustomBusinessEntityRepository CustomBusinessEntityRepository { set; get; }
        CustomBusinessEntityWhereRepository CustomBusinessEntityWhereRepository { set; get; }
        BusinessEntityPropertyRepository BusinessEntityPropertyRepository { set; get; }
        FavoriteProjectRepository FavoriteProjectRepository { set; get; }
        ProjectDepartmentRepository ProjectDepartmentRepository { set; get; }
        ProjectRepository ProjectRepository { set; get; }
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
        public ReportTaskService(APIContext apiContext)
        {
            APIContext = apiContext;
            ResearchDbContext = APIContext?.GetDBContext(APIContraints.ResearchDbContext);

            //repositories
            CustomBusinessEntityPropertyRepository = new CustomBusinessEntityPropertyRepository(ResearchDbContext);
            CustomBusinessEntityRepository = new CustomBusinessEntityRepository(ResearchDbContext);
            CustomBusinessEntityWhereRepository = new CustomBusinessEntityWhereRepository(ResearchDbContext);
            BusinessEntityPropertyRepository = new BusinessEntityPropertyRepository(ResearchDbContext);
            FavoriteProjectRepository = new FavoriteProjectRepository(ResearchDbContext);
            ProjectDepartmentRepository = new ProjectDepartmentRepository(ResearchDbContext);
            ProjectRepository = new ProjectRepository(ResearchDbContext);
            ProjectScheduleRepository = new ProjectScheduleRepository(ResearchDbContext);
            ProjectTaskRepository = new ProjectTaskRepository(ResearchDbContext);
            ProjectTaskWhereRepository = new ProjectTaskWhereRepository(ResearchDbContext);
            ProjectMemberRepository = new ProjectMemberRepository(ResearchDbContext);
            ProjectIndicatorRepository = new ProjectIndicatorRepository(ResearchDbContext);
            RoleRepository = new RoleRepository(ResearchDbContext);
            SharedRepository = new SharedRepository(ResearchDbContext);
            UserRepository = new UserRepository(ResearchDbContext);
        }

        internal ServiceResult<List<VLKeyValue<string, long>>> GetFavoriteProjects(long userid)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = ProjectRepository.GetFavoriteProjects(userid);
                return new List<VLKeyValue<string, long>>(
                    result.Select(c => new VLKeyValue<string, long>(c.ProjectName, c.ProjectId)).ToList()
                );
            });
        }

        internal ServiceResult<GetProjectModel> GetProject(long projectId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
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
                result.IsFavorite = FavoriteProjectRepository.GetOne(new FavoriteProject(project.Id, project.CreatorId)) != null;
                return result;
            });
        }

        internal ServiceResult<Dictionary<long, string>> GetAllUsersIdAndName()
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = DomainConstraits.Users;
                return result;
            });
        }

        internal ServiceResult<Dictionary<long, string>> GetUsersDictionary()
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = UserRepository.GetAllUsers();
                var dic = new Dictionary<long, string>();
                foreach (var item in result)
                {
                    dic.Add(item.Id, item.Name);
                }
                return dic;
            });
        }

        internal ServiceResult<Dictionary<long, string>> GetRolesDictionary()
        {
            var result = RoleRepository.GetAllRoles();
            var dic = new Dictionary<long, string>();
            foreach (var item in result)
            {
                dic.Add(item.Id, item.Name);
            }
            return new ServiceResult<Dictionary<long, string>>(dic);
        }

        internal ServiceResult<List<User>> GetAllUsers()
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = UserRepository.GetAllUsers();
                return result;
            });
        }

        internal ServiceResult<bool> DeleteProject(long projectId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = ProjectRepository.DeleteById(projectId);
                return result;
            });
        }

        internal ServiceResult<VLPagerResult<List<Dictionary<string, object>>>> GetPagedResultBySQLConfig(GetCommonSelectRequest request)
        {
            var sqlConfig = ConfigHelper.GetSQLConfigByDirectoryName("ProjectList");
            sqlConfig.PageIndex = request.page;
            sqlConfig.PageSize = request.limit;
            sqlConfig.UpdateWheres(request.search);
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var list = SharedRepository.GetCommonSelect(sqlConfig.Source, sqlConfig.Skip, sqlConfig.Limit);
                var count = SharedRepository.GetCommonSelectCount(sqlConfig.Source);
                sqlConfig.Source.DoTransforms(ref list);
                return new VLPagerResult<List<Dictionary<string, object>>>() { List = list.ToList(), Count = count };
            });
        }

        internal ServiceResult<int> DeleteProjectIndicators(List<long> indicatorIds)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = ProjectIndicatorRepository.DeleteByIds(indicatorIds);
                return result;
            });
        }

        internal ServiceResult<List<BusinessEntityPropertyModel>> CreateCustomIndicator(CreateCustomIndicatorRequest request, BusinessEntityTemplate template)
        {
            var entity = new CustomBusinessEntity()
            {
                Name = "t" + template.Id,
                DisplayName = request.Name,
                TemplateId = template.Id
            };
            var properties = template.BusinessEntity.Properties.Select(c => new CustomBusinessEntityProperty()
            {
                EntityName = entity.Name,
                Name = c.SourceName,
                DisplayName = c.DisplayName,
            }).ToList();
            var wheres = request.Search.Select(c => new CustomBusinessEntityWhere()
            {
                ComponentName = c.Key,
                Value = c.Value,
                DisplayName = template.SQLConfig.Wheres.First(d => d.ComponentName == c.Key).DisplayName,
                Operator = "eq",
            }).ToList();
            var selectedProperties = request.Properties.Select(c => new ProjectIndicator()
            {
                ProjectId = request.ProjectId,
                EntitySourceName = entity.Name,
                PropertySourceName = c.ColumnName,
                PropertyDisplayName = properties.First(d => d.Name == c.ColumnName).DisplayName,
            }).ToList();
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var entityId = CustomBusinessEntityRepository.InsertOne(entity);
                properties.ForEach(c =>
                {
                    c.BusinessEntityId = entityId;
                    c.Id = CustomBusinessEntityPropertyRepository.InsertOne(c);
                });
                wheres.ForEach(c =>
                {
                    c.BusinessEntityId = entityId;
                    c.Id = CustomBusinessEntityWhereRepository.InsertOne(c);
                });
                selectedProperties.ForEach(c =>
                {
                    c.BusinessEntityId = entityId;
                    c.BusinessEntityPropertyId = properties.First(d => d.Name == c.PropertySourceName).Id;
                    c.EntitySourceName = entity.Name;
                    c.PropertySourceName = c.PropertySourceName;
                    c.PropertyDisplayName = entity.DisplayName;
                    c.Id = ProjectIndicatorRepository.InsertOne(c);
                });
                return selectedProperties.Select(c => new BusinessEntityPropertyModel() { Id = c.Id, ColumnName = c.PropertySourceName }).ToList();
            });
        }

        internal ServiceResult<bool> DeleteProjectIndicator(long indicatorId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = ProjectIndicatorRepository.DeleteById(indicatorId);
                return result;
            });
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
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var projectId = ProjectRepository.Insert(project);
                var members = new List<ProjectMember>();
                members.Add(new ProjectMember(projectId, project.CreatorId, DomainConstraits.OwnerRoleId.Value));
                foreach (var adminId in request.AdminIds ?? new List<long>())
                    members.Add(new ProjectMember(projectId, adminId, DomainConstraits.AdminRoleId.Value));
                foreach (var memberId in request.MemberIds ?? new List<long>())
                    members.Add(new ProjectMember(projectId, memberId, DomainConstraits.MemberRoleId.Value));
                ProjectMemberRepository.AddProjectMembers(members);
                var projectDepartments = request.DepartmentIds.Select(c => new ProjectDepartment() { ProjectId = projectId, DepartmentId = c }).ToList();
                ProjectDepartmentRepository.AddProjectDepartments(projectDepartments);
                return projectId;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indicatorId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal ServiceResult<bool> UpdateIndicatorName(long indicatorId, string name)
        {
            throw new NotImplementedException();
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
            return ResearchDbContext.DelegateTransaction(c =>
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
                ProjectMemberRepository.AddProjectMembers(members);
                var projectDepartments = request.DepartmentIds.Select(c => new ProjectDepartment() { ProjectId = projectId, DepartmentId = c }).ToList();
                ProjectDepartmentRepository.DeleteByProjectId(projectId);
                ProjectDepartmentRepository.AddProjectDepartments(projectDepartments);
                return true;
            });
        }

        internal ServiceResult<List<GetProjectIndicatorModel>> GetProjectIndicators(long projectId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                return ProjectIndicatorRepository.GetByProjectId(projectId)
                .Select(c =>
                {
                    var m = new GetProjectIndicatorModel();
                    c.MapTo(m);
                    m.DisplayName = c.PropertyDisplayName;

                    //自定义字段将无匹配内容,目前自定义字段只允许输入字符,默认为字符
                    var coProperty= DomainConstraits.BusinessEntityProperties.FirstOrDefault(d => d.Id == c.BusinessEntityPropertyId);
                    m.ColumnType = coProperty?.ColumnType ?? ColumnType.String;
                    m.EnumType = coProperty?.EnumType;
                    return m;
                }).ToList();
            });
        }

        internal ServiceResult<List<Role>> GetAllRoles()
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                return RoleRepository.GetAllRoles();
            });
        }

        internal ServiceResult<bool> AddFavoriteProject(long projectId, long userId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var favoriteProject = new FavoriteProject(projectId, userId);
                var exist = FavoriteProjectRepository.GetOne(favoriteProject);
                if (exist != null)
                    throw new NotImplementedException("已添加收藏");
                var project = ProjectRepository.GetById(projectId);
                if (project == null)
                    throw new NotImplementedException("项目不存在");
                return FavoriteProjectRepository.InsertOne(favoriteProject) > 0;
            });
        }

        internal ServiceResult<bool> DeleteFavoriteProject(long projectId, long userId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var favoriteProject = new FavoriteProject(projectId, userId);
                var exist = FavoriteProjectRepository.GetOne(favoriteProject);
                if (exist == null)
                    throw new NotImplementedException("收藏不存在");
                return FavoriteProjectRepository.DeleteOne(favoriteProject) > 0;
            });
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
                projectIndicator.EntitySourceName = DomainConstraits.RenderIdsToText(request.BusinessEntityId, PKVType.BusinessEntitySource);
                projectIndicator.PropertySourceName = DomainConstraits.RenderIdsToText(c, PKVType.BusinessEntityPropertySource);
                projectIndicator.PropertyDisplayName = DomainConstraits.RenderIdsToText(c, PKVType.BusinessEntityProperty);
                return projectIndicator;
            }).ToList();
            //Logic
            return ResearchDbContext.DelegateTransaction(c =>
            {
                ProjectIndicatorRepository.DeleteByEntityId(request.ProjectId, request.BusinessEntityId);
                var successCount = ProjectIndicatorRepository.InsertBatch(projectIndicators);
                return successCount == request.BusinessEntityPropertyIds.Count();
            });
        }

        internal ServiceResult<long> CreateTask(CreateTaskRequest request)
        {
            return ResearchDbContext.DelegateTransaction(c => 
            {
                if (request.CopyTaskId>0)
                {
                    throw new NotImplementedException("未支持队列复制");
                }
                else
                {
                    var task = new ProjectTask()
                    {
                        Name = request.TaskName,
                        ProjectId = request.ProjectId,
                    };
                    task.Id = ProjectTaskRepository.Insert(task);
                    return task.Id;
                }
            });
        }

        internal ServiceResult<bool> EditTask(EditTaskRequest request)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var projectTask = ProjectTaskRepository.GetById(request.TaskId);
                if (projectTask == null)
                {
                    throw new NotImplementedException("队列不存在");
                }
                var projectIndicators = ProjectIndicatorRepository.GetByProjectId(projectTask.ProjectId);
                if (projectIndicators.Count==0)
                {
                    throw new NotImplementedException("项目指标缺失");
                }
                ProjectTaskWhereRepository.DeleteByTaskId(request.TaskId);
                var wheres = request.Wheres.Select(c => {
                    var indicator = projectIndicators.First(d => d.Id == c.IndicatorId);
                    var item = new ProjectTaskWhere()
                    {
                        ProjectId = projectTask.ProjectId,
                        TaskId = projectTask.Id,
                        BusinessEntityId = indicator.BusinessEntityId,
                        BusinessEntityPropertyId = indicator.BusinessEntityPropertyId,
                        EntityName = RenderIdToText(indicator.BusinessEntityId, BusinessEntitySourceDic),
                        PropertyName = RenderIdToText(indicator.BusinessEntityPropertyId, BusinessEntityPropertySourceDic),
                        Operator = (ProjectTaskWhereOperator)Enum.Parse(typeof(ProjectTaskWhereOperator), c.Operator),
                        Value = c.Value,
                    };
                    return item;
                }).ToList();
                wheres.ForEach(c =>
                {
                    c.Id = ProjectTaskWhereRepository.Insert(c);
                });
                return true;
            });
        }

        internal ServiceResult<bool> StartSchedule(long scheduleId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var schedule = ProjectScheduleRepository.GetById(scheduleId);
                if (schedule == null)
                    throw new NotImplementedException("计划不存在");
                var task = ProjectTaskRepository.GetById(schedule.TaskId);
                if (task == null)
                    throw new NotImplementedException("任务不存在");
                var project = ProjectRepository.GetById(schedule.ProjectId);
                if (project == null)
                    throw new NotImplementedException("项目不存在");
                var projectIndicators = ProjectIndicatorRepository.GetByProjectId(schedule.ProjectId);
                if (projectIndicators.Count==0)
                {
                    throw new NotImplementedException("指标不存在");
                }
                var taskWheres = ProjectTaskWhereRepository.GetByTaskId(schedule.TaskId);
                var customBusinessEntityIndicators = projectIndicators.Where(c => c.BusinessEntityId.ToString().StartsWith("3"));
                var customBusinessEntities = customBusinessEntityIndicators.Count() > 0
                    ? CustomBusinessEntityRepository.GetByIds(customBusinessEntityIndicators.Select(c => c.BusinessEntityId).Distinct().ToList())
                    : new List<CustomBusinessEntity>();
                var customBusinessEntityWheres = customBusinessEntityIndicators.Count() > 0
                    ? CustomBusinessEntityWhereRepository.GetByBusinessEntityIds(customBusinessEntityIndicators.Select(c => c.BusinessEntityId).Distinct().ToList())
                    : new List<CustomBusinessEntityWhere>();
                var routers = ConfigHelper.GetRouters("Configs/XMLConfigs/BusinessEntities", "Routers.xml");
                var templates = ConfigHelper.GetBusinessEntityTemplates();

                //内核处理
                var reportTask = new ReportTask(task.Name);
                foreach (var entityName in projectIndicators.Select(c => c.EntitySourceName).Distinct())
                {
                    var router = routers.FirstOrDefault(c => c.To == entityName);
                    if (router!=null)
                    {
                        reportTask.Routers.Add(router);
                    }
                    else
                    {
                        var template = templates.FirstOrDefault(c => "t" + c.Id == entityName);
                        if (template!=null)
                        {
                            router = template.Router;
                            router.To = entityName;
                            router.IsFromTemplate = true;
                            reportTask.Routers.Add(router);
                        }
                    }
                }
                var templateIds = customBusinessEntities.Select(c => c.TemplateId);
                reportTask.Templates.AddRange(templates.Where(c => templateIds.Contains(c.Id)));
                reportTask.Properties.AddRange(projectIndicators.Select(c => new COBusinessEntityProperty()
                {
                    SourceName = c.PropertySourceName,
                    DisplayName = c.PropertyDisplayName,
                    From = c.EntitySourceName,
                }));
                reportTask.Conditions.AddRange(taskWheres.Select(c => new Field2ValueWhere()
                {
                    EntityName = c.EntityName,
                    FieldName = c.PropertyName,
                    Value = c.Value,
                }));
                reportTask.TemplateConditions.AddRange(customBusinessEntityWheres.Select(c => new Field2ValueWhere()
                {
                    EntityName = customBusinessEntities.First(d=>d.Id == c.BusinessEntityId).Name,
                    FieldName = c.ComponentName,
                    Value = c.Value,
                }));
                //string.Join("\r\n",parameters.Select(c=> "declare @"+c.Key+" nvarchar(50); set @"+c.Key+" = '"+ c.Value+"'"))
                var parameters = reportTask.GetParameters();
                var sql = reportTask.GetSQL();
                var dataTable = SharedRepository.GetDataTable(sql, parameters);
                //转译处理结果
                foreach (var column in dataTable.Columns)
                {
                }
                //输出处理结果
                var projectIndicators2 = projectIndicators;
                var filePath = $"{schedule.Id}_{DateTime.Now.ToString("yyyy_MM_dd_mm_hh_ss")}.xls";
                var fullPath = Path.Combine(FileHelper.GetDirectory("Export"), filePath);
                ExcelHelper.ExportDataTableToExcel(dataTable, fullPath);
                //更新处理任务状态
                ProjectScheduleRepository.UpdateResultFile(schedule.Id, "Export/" + filePath);
                return true;
            });
        }

        internal ServiceResult<bool> EditTaskName(EditTaskNameRequest request)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                return ProjectTaskRepository.UpdateName(request.TaskId, request.TaskName) > 0;
            });
        }

        internal ServiceResult<bool> DeleteTask(long taskId)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                return ProjectTaskRepository.DeleteById(taskId);
            });
        }

        internal ServiceResult<List<GetTaskModel>> GetTasks(long projectId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var tasks = ProjectTaskRepository.GetByProjectId(projectId);
                var taskWheres = ProjectTaskWhereRepository.GetByProjectId(projectId);
                var taskSchedules = ProjectScheduleRepository.GetByProjectId(projectId);
                var result = tasks.Select(d => new GetTaskModel()
                {
                    ProjectId = d.ProjectId,
                    TaskId = d.Id,
                    TaskName = d.Name,
                    LastCompletedAt = taskSchedules.FirstOrDefault(e => e.TaskId == d.Id)?.LastCompletedAt,
                    ResultFile = taskSchedules.FirstOrDefault(e => e.TaskId == d.Id)?.ResultFile,
                    Wheres = taskWheres.Where(e => e.TaskId == d.Id)
                    .Select(d =>
                    {
                        var item = new GetTaskWhereModel();
                        d.MapTo(item);
                        item.OperatorName = item.Operator.GetDescription();
                        item.DisplayName = DomainConstraits.RenderIdsToText(item.BusinessEntityPropertyId, PKVType.BusinessEntityProperty);
                        return item;
                    }).ToList(),
                }).ToList();
                return result;
            });
        }

        internal ServiceResult<List<VLKeyValue<string,string>>> GetTaskNameAndIds(long projectId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                return ProjectTaskRepository.GetTaskNameAndIds(projectId);
            });
        }

        internal ServiceResult<long> CommitTask(CommitTaskRequest request)
        {
            var schedule = new ProjectSchedule();
            schedule.ProjectId = request.ProjectId;
            schedule.TaskId = request.TaskId;
            schedule.StartedAt = request.IsStartNow ? DateTime.Now : request.StartAt;
            schedule.Status = ScheduleStatus.Ready;
            schedule.ResultFile = "";
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var old = ProjectScheduleRepository.GetLastestByTaskId(schedule.TaskId);
                if (old != null && old.IsWorking())
                {
                    throw new NotImplementedException("任务已在执行中");
                }
                schedule.Id = ProjectScheduleRepository.InsertOne(schedule);
                return schedule.Id;
            });
        }

        internal ServiceResult<GetTaskStatusModel> GetTaskStatus(long taskId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var taskStatus = ProjectScheduleRepository.GetTaskStatus(taskId);
                if (taskStatus==null)
                {
                    throw new NotImplementedException("执行任务不存在");
                }
                return taskStatus;
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DbContextEX
    {
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateTransaction<T>(this DbContext context, Func<DbContext, T> exec)
        {
            try
            {
                if (context.DbGroup.Connection.State != ConnectionState.Open)
                    context.DbGroup.Connection.Open();
                context.DbGroup.Transaction = context.DbGroup.Connection.BeginTransaction();
                context.DbGroup.Command.Transaction = context.DbGroup.Transaction;
                try
                {
                    var result = exec(context);
                    context.DbGroup.Transaction.Commit();
                    return new ServiceResult<T>(result);
                }
                catch (Exception ex)
                {
                    context.DbGroup.Transaction.Rollback();
                    Log4NetLogger.Error("DelegateTransaction Exception", ex);

                    return new ServiceResult<T>(default(T), code: 500, ex.Message);
                }
                finally
                {
                    context.DbGroup.Connection.Close();
                }
            }
            catch (Exception e)
            {
                Log4NetLogger.Error("打开数据库连接配置失败,当前数据库连接," + context.DbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), code: 500, e.Message);
            }
        }
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateNonTransaction<T>(this DbContext context, Func<DbContext, T> exec)
        {
            try
            {
                if (context.DbGroup.Connection.State != ConnectionState.Open)
                    context.DbGroup.Connection.Open();
                try
                {
                    var result = exec(context);
                    return new ServiceResult<T>(result);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Error("DelegateTransaction Exception", ex);
                    return new ServiceResult<T>(default(T), ex.Message);
                }
                finally
                {
                    context.DbGroup.Connection.Close();
                }
            }
            catch (Exception e)
            {
                //集成Log4Net
                Log4NetLogger.Error("打开数据库连接配置失败,当前数据库连接," + context.DbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), e.Message);
            }
        }
    }
}
