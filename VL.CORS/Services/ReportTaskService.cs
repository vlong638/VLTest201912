using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ServiceSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Http;
using ResearchAPI.Common;
using ResearchAPI.Controllers;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ResearchAPI.Services
{
    public static class DomainConstraits {
        private static Dictionary<string, long> roles;

        public static Dictionary<string, long> GetRoles(ReportTaskDomain domain)
        {
            if (roles == null)
            {
                var result = domain.GetRoles();
                roles = new Dictionary<string, long>();
                foreach (var role in result)
                {
                    roles.Add(role.Name, role.Id);
                }
            }
            return roles;
        }

        private static long adminRoleId;
        internal static long GetAdminRoleId(ReportTaskDomain domain)
        {
            if (adminRoleId == 0)
            {
                adminRoleId = GetRoles(domain).First(c => c.Key == "项目管理员").Value;
            }
            return adminRoleId;
        }

        private static long memberRoleId;
        internal static long GetMemberRoleId(ReportTaskDomain domain)
        {
            if (memberRoleId == 0)
            {
                memberRoleId = GetRoles(domain).First(c => c.Key == "项目成员").Value;
            }
            return memberRoleId;
        }

        private static long owenerRoleId;
        internal static long GetOwnerRoleId(ReportTaskDomain domain)
        {
            if (owenerRoleId == 0)
            {
                owenerRoleId = GetRoles(domain).First(c => c.Key == "项目创建人").Value;
            }
            return owenerRoleId;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReportTaskService
    {
        ReportTaskDomain Domain { set; get; }

        APIContext APIContext { get; set; }
        DbContext ResearchDbContext { set; get; }
        internal RoleRepository RoleRepository { set; get; }
        internal ProjectRepository ProjectRepository { set; get; }
        internal ProjectMemberRepository ProjectMemberRepository { set; get; }
        internal SharedRepository SharedRepository { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ReportTaskService(APIContext apiContext)
        {
            APIContext = apiContext;
            ResearchDbContext = APIContext.GetDBContext(APIContraints.ResearchDbContext);
            RoleRepository = new RoleRepository(ResearchDbContext);
            ProjectRepository = new ProjectRepository(ResearchDbContext);
            ProjectMemberRepository = new ProjectMemberRepository(ResearchDbContext);
            SharedRepository = new SharedRepository(ResearchDbContext);
            Domain = new ReportTaskDomain(this);
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

        internal ServiceResult<List<VLKeyValue<string, long>>> GetUserFavoriteProjects(long userid)
        {
            var result = Domain.GetUserFavoriteProjects(userid);
            return new ServiceResult<List<VLKeyValue<string, long>>>(
                result.Select(c => new VLKeyValue<string, long>(c.ProjectName, c.ProjectId)).ToList()
            );
        }

        internal ServiceResult<GetProjectModel> GetProject(int projectId)
        {
            var result = new GetProjectModel();
            var project = Domain.GetProject(projectId);
            if (project == null)
            {
                return new ServiceResult<GetProjectModel>(null, "无效的项目");
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
            var adminRoleId = DomainConstraits.GetAdminRoleId(Domain);
            var memberRoleId = DomainConstraits.GetMemberRoleId(Domain);
            var adminIds = Domain.GetUserIdsByProjectAndRoleName(projectId, adminRoleId);
            result.AdminIds = adminIds;
            var memberIds = Domain.GetUserIdsByProjectAndRoleName(projectId, memberRoleId);
            result.MemberIds = memberIds;
            return new ServiceResult<GetProjectModel>(result);
        }

        internal ServiceResult<bool> DeleteProject(int projectId)
        {
            var result = Domain.DeleteProject(projectId);
            return new ServiceResult<bool>(result);
        }

        internal ServiceResult<VLPagerResult<List<Dictionary<string, object>>>> GetPagedResultBySQLConfig(GetCommonSelectRequest request)
        {
            var sqlConfig = ConfigHelper.GetSQLConfigByDirectoryName("ProjectList");
            sqlConfig.PageIndex = request.page;
            sqlConfig.PageSize = request.limit;
            sqlConfig.UpdateWheres(request.search);
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var result = Domain.GetPagedResultBySQLConfig(sqlConfig);
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
                var projectId = Domain.CreateProject(project, request.AdminIds, request.MemberIds);
                return projectId;
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

    /// <summary>
    /// 
    /// </summary>
    public class ReportTaskDomain
    {
        private ReportTaskService Service;

        public ReportTaskDomain(ReportTaskService reportTaskService)
        {
            this.Service = reportTaskService;
        }

        internal long CreateCustomBusinessEntity(CreateCustomBusinessEntityRequest request)
        {
            throw new NotImplementedException();
        }

        internal ReportTask GetReportTask(long taskId)
        {
            var reportTask = new ReportTask("母亲检验信息");
            reportTask.Properties.Add(new BusinessEntityProperty("姓名", "PregnantInfo", "PersonName"));
            reportTask.Properties.Add(new BusinessEntityProperty("生日", "PregnantInfo", "Birthday"));
            //reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检查名称", customBusinessEntity.ReportName, "ExamName"));
            //reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检查日期", customBusinessEntity.ReportName, "ExamTime"));
            //reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检验名称", customBusinessEntity.ReportName, "ItemName"));
            //reportTask.Properties.Add(new BusinessEntityProperty("空腹血糖-检验结果", customBusinessEntity.ReportName, "Value"));
            //reportTask.MainConditions.Add(new Field2ValueWhere(customBusinessEntity.ReportName, "检验类别", WhereOperator.Equal, "0148"));
            //reportTask.MainConditions.Add(new Field2ValueWhere(customBusinessEntity.ReportName, "Value", WhereOperator.GreatOrEqualThan, "10"));
            //reportTask.CustomBusinessEntities.Add(customBusinessEntity);
            //reportTask.CustomRouters.Add(customBusinessEntityRouter);
            return reportTask;
        }

        internal List<UserFavoriteProjectModel> GetUserFavoriteProjects(long userid)
        {
            return Service.ProjectRepository.GetUserFavoriteProjects(userid);
        }

        internal Project GetProject(int projectId)
        {
            return Service.ProjectRepository.GetById(projectId);
        }

        internal bool DeleteProject(int projectId)
        {
            return Service.ProjectRepository.DeleteById(projectId);
        }

        internal List<long> GetUserIdsByProjectAndRoleName(long projectId, long roleId)
        {
            return Service.ProjectRepository.GetUserIdsByProjectIdAndRoleId(projectId, roleId);
        }

        internal VLPagerResult<List<Dictionary<string, object>>> GetPagedResultBySQLConfig(SQLConfigV2 sqlConfig)
        {
            var list = Service.SharedRepository.GetCommonSelect(sqlConfig.Source, sqlConfig.Skip, sqlConfig.Limit);
            var count = Service.SharedRepository.GetCommonSelectCount(sqlConfig);
            sqlConfig.Source.DoTransforms(ref list);
            return new VLPagerResult<List<Dictionary<string, object>>>() { List = list.ToList(), Count = count };
        }

        internal long CreateProject(Project project, List<long> adminIds, List<long> memberIds)
        {
            var projectId = Service.ProjectRepository.Insert(project);
            var ownerRoleId = DomainConstraits.GetOwnerRoleId(this);
            var adminRoleId = DomainConstraits.GetAdminRoleId(this);
            var memberRoleId = DomainConstraits.GetMemberRoleId(this);
            var members = new List<ProjectMember>();
            members.Add(new ProjectMember(projectId, project.CreatorId, ownerRoleId));
            foreach (var adminId in adminIds)
                members.Add(new ProjectMember(projectId, adminId, adminRoleId));
            foreach (var memberId in memberIds)
                members.Add(new ProjectMember(projectId, memberId, memberRoleId));
            CreateProjectMembers(members);
            return projectId;
        }

        internal List<Role> GetRoles()
        {
            return Service.RoleRepository.GetAllRoles();
        }

        internal bool CreateProjectMembers(List<ProjectMember> members)
        {
            //TODO 可以优化为批量处理

            foreach (var member in members)
            {
                Service.ProjectMemberRepository.Insert(member);
            }
            return true;
        }
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
                context.DbGroup.Connection.Open();
                context.DbGroup.Transaction = context.DbGroup.Connection.BeginTransaction();
                context.DbGroup.Command.Transaction = context.DbGroup.Transaction;
                try
                {
                    var result = exec(context);
                    context.DbGroup.Transaction.Commit();
                    context.DbGroup.Connection.Close();
                    return new ServiceResult<T>(result);
                }
                catch (Exception ex)
                {
                    context.DbGroup.Transaction.Rollback();
                    context.DbGroup.Connection.Close();

                    //集成Log4Net
                    Log4NetLogger.Error("DelegateTransaction Exception", ex);

                    return new ServiceResult<T>(default(T), ex.Message);
                }
            }
            catch (Exception e)
            {
                //集成Log4Net
                Log4NetLogger.Error("打开数据库连接配置失败,当前数据库连接," + context.DbGroup.Connection.ConnectionString);
                return new ServiceResult<T>(default(T), e.Message);
            }
        }
        /// <summary>
        /// 扩展事务(服务层)通用处理
        /// </summary>
        public static ServiceResult<T> DelegateNonTransaction<T>(this DbContext context, Func<DbContext, T> exec)
        {
            try
            {
                context.DbGroup.Connection.Open();
                try
                {
                    var result = exec(context);
                    context.DbGroup.Connection.Close();
                    return new ServiceResult<T>(result);
                }
                catch (Exception ex)
                {
                    context.DbGroup.Connection.Close();

                    Log4NetLogger.Error("DelegateTransaction Exception", ex);

                    return new ServiceResult<T>(default(T), ex.Message);
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
