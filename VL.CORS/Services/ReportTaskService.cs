using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ServiceSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Http;
using ResearchAPI.CORS.Common;
using ResearchAPI.Controllers;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static ResearchAPI.Controllers.EasyResearchController;

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
        CustomBusinessEntityRepository CustomerBusinessEntityRepository { set; get; }
        CustomBusinessEntityWhereRepository CustomBusinessEntityWhereRepository { set; get; }
        BusinessEntityPropertyRepository BusinessEntityPropertyRepository { set; get; }
        FavoriteProjectRepository FavoriteProjectRepository { set; get; }
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
            CustomerBusinessEntityRepository = new CustomBusinessEntityRepository(ResearchDbContext);
            CustomBusinessEntityWhereRepository = new CustomBusinessEntityWhereRepository(ResearchDbContext);
            BusinessEntityPropertyRepository = new BusinessEntityPropertyRepository(ResearchDbContext);
            FavoriteProjectRepository = new FavoriteProjectRepository(ResearchDbContext);
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


        internal ServiceResult<bool> ExecuteReportTask(long taskId)
        {
            throw new NotImplementedException();
        }

        internal ServiceResult<long> CreateCustomBusinessEntity(CreateCustomBusinessEntityRequest request)
        {
            throw new NotImplementedException();
        }

        internal ServiceResult<bool> EditCustomBusinessEntity(EditCustomBusinessEntityRequest request)
        {
            throw new NotImplementedException();
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
                var project = ProjectRepository.GetAvailableProjectById(projectId);
                if (project == null)
                {
                    throw new NotImplementedException("项目不存在");
                }
                result.ProjectId = project.Id;
                result.ProjectName = project.Name;
                result.DepartmentId = project.DepartmentId;
                result.ViewAuthorizeType = project.ViewAuthorizeType;
                result.ProjectDescription = project.ProjectDescription;
                result.CreatorId = project.CreatorId;
                result.CreatedAt = project.CreatedAt;
                result.LastModifiedAt = project.LastModifiedAt;
                result.LastModifiedBy = project.LastModifiedBy;
                var adminIds = ProjectRepository.GetUserIdsByProjectIdAndRoleId(projectId, DomainConstraits.AdminRoleId.Value);
                result.AdminIds = adminIds;
                var memberIds = ProjectRepository.GetUserIdsByProjectIdAndRoleId(projectId, DomainConstraits.MemberRoleId.Value);
                result.MemberIds = memberIds;
                var isFavorite = FavoriteProjectRepository.GetOne(new FavoriteProject(project.Id, project.CreatorId));
                result.IsFavorite = isFavorite != null;
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
                var count = SharedRepository.GetCommonSelectCount(sqlConfig);
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

        internal ServiceResult<List<BusinessEntityPropertyDTO>> CreateCustomIndicator(CreateCustomIndicatorRequest request, BusinessEntityTemplate template)
        {
            var entity = new CustomBusinessEntity()
            {
                Name = "t" + template.Id,
                TemplateId = template.Id
            };
            var properties = template.BusinessEntity.Properties.Select(c => new CustomBusinessEntityProperty()
            {
                EntityName = entity.Name,
                Name = c.ColumnName,
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
                EntityName = entity.Name,
                PropertyName = c.ColumnName,
                DisplayName = properties.First(d => d.Name == c.ColumnName).DisplayName,
            }).ToList();
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var entityId = CustomerBusinessEntityRepository.InsertOne(entity);
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
                    c.BusinessEntityPropertyId = properties.First(d => d.Name == c.PropertyName).Id;
                    c.Id = ProjectIndicatorRepository.InsertOne(c);
                });
                return selectedProperties.Select(c => new BusinessEntityPropertyDTO() { Id = c.Id, ColumnName = c.PropertyName }).ToList();
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
                DepartmentId = request.DepartmentId,
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
                ProjectMemberRepository.CreateProjectMembers(members);
                return projectId;
            });
        }

        internal ServiceResult<bool> EditProject(EditProjectRequest request, long userid)
        {
            var project = new Project()
            {
                Id = request.ProjectId,
                Name = request.ProjectName,
                CreatedAt = DateTime.Now,
                CreatorId = userid,
                DepartmentId = request.DepartmentId,
                ProjectDescription = request.ProjectDescription,
                ViewAuthorizeType = request.ViewAuthorizeType,
            };
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var oldProject = ProjectRepository.GetAvailableProjectById(project.Id);
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
                ProjectMemberRepository.CreateProjectMembers(members);
                return true;
            });
        }
        internal ServiceResult<List<GetProjectIndicatorModel>> GetProjectIndicators(long projectId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                return ProjectIndicatorRepository.GetByProjectId(projectId);
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
                var project = ProjectRepository.GetAvailableProjectById(projectId);
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
            var projectIndicators = request.Properties.Select(c =>
            {
                var projectIndicator = new ProjectIndicator();
                c.MapTo(projectIndicator);
                projectIndicator.ProjectId = request.ProjectId;
                projectIndicator.BusinessEntityId = request.BusinessEntityId;
                return projectIndicator;
            }).ToList();
            //Logic
            return ResearchDbContext.DelegateTransaction(c =>
            {
                ProjectIndicatorRepository.DeleteByEntityId(request.ProjectId, request.BusinessEntityId);
                var successCount = ProjectIndicatorRepository.InsertBatch(projectIndicators);
                return successCount == request.Properties.Count();
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
                ProjectTaskWhereRepository.DeleteByTaskId(request.TaskId);
                var wheres = request.Wheres.Select(c => new ProjectTaskWhere()
                {
                    ProjectId = projectTask.ProjectId,
                    TaskId = projectTask.Id,
                    BusinessEntityId = c.BusinessEntityId,
                    EntityName =c.EntityName,
                    PropertyName = c.PropertyName,
                    Operator = c.Operator,
                    Value = c.Value,
                }).ToList();
                wheres.ForEach(c =>
                {
                    c.Id = ProjectTaskWhereRepository.Insert(c);
                });
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
                var result = tasks.Select(c => new GetTaskModel() {
                    ProjectId = c.ProjectId,
                    TaskId = c.Id,
                    TaskName = c.Name,
                    Wheres = taskWheres.Where(d => d.TaskId == c.Id)
                    .Select(d => {
                        var item = new GetTaskWhereModel();
                        d.MapTo(item);
                        return item;
                    }).ToList(),
                }).ToList();
                return result;
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class APIContext
    {
        /// <summary>
        /// 
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public HttpContext HttpContext { set { HttpContextAccessor.HttpContext = value; } get { return HttpContextAccessor.HttpContext; } }

        ///// <summary>
        ///// 
        ///// </summary>
        //public RedisCache RedisCache { set; get; }
        ///// <summary>
        ///// 获取当前用户信息
        ///// </summary>
        ///// <returns></returns>
        //public CurrentUser GetCurrentUser()
        //{
        //    var sessionId = CurrentUser.GetSessionId(HttpContext);
        //    return CurrentUser.GetCurrentUser(RedisCache, sessionId);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public APIContext(IHttpContextAccessor httpContext, RedisCache redisCache) : base()
        //{
        //    HttpContextAccessor = httpContext;
        //    RedisCache = redisCache;
        //}

        /// <summary>
        /// 
        /// </summary>
        public APIContext(IHttpContextAccessor httpContext) : base()
        {
            HttpContextAccessor = httpContext;
        }

        #region Common

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetWebPath()
        {
            var request = HttpContext.Request;
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public DbContext GetDBContext(string source)
        {
            var connectionString = APIContraints.DBConfig.ConnectionStrings.FirstOrDefault(c => c.Key == source);
            if (connectionString == null)
            {
                throw new NotImplementedException("尚未支持该类型的dbContext构建");
            }
            return new DbContext(DBHelper.GetDbConnection(connectionString.Value));
        }

        internal CurrentUser GetCurrentUser()
        {
            var currentUser = new CurrentUser();
            currentUser.UserId = TestContext.UserId;
            return currentUser;
        }

        #endregion
    }

    public class CurrentUser
    {
        public long UserId { set; get; }
    }

    /// <summary>
    /// 静态量,常量
    /// </summary>
    public class APIContraints
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        public static DBConfig DBConfig { set; get; }

        public static string ResearchDbContext { set; get; } = "ResearchConnectionString";
    }

    /// <summary>
    /// 配置样例
    /// </summary>
    public class DBConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public List<VLKeyValue> ConnectionStrings { set; get; }
    }

    public class VLPagerResultDataTable
    {

        /// <summary>
        /// 总数
        /// </summary>
        public int Count { set; get; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentIndex { set; get; }
        /// <summary>
        /// 内容
        /// </summary>
        public DataTable List { set; get; }
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
