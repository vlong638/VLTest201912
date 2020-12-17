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
    public static class DomainConstraits
    {

        private static long? _AdminRoleId;
        internal static long GetAdminRoleId(Func<Dictionary<long, string>> source)
        {
            if (_AdminRoleId == null)
            {
                _AdminRoleId = GetRoles(source).First(c => c.Value == "项目管理员").Key;
            }
            return _AdminRoleId.Value;
        }

        private static long? _MemberRoleId;
        internal static long GetMemberRoleId(Func<Dictionary<long, string>> source)
        {
            if (_MemberRoleId == null)
            {
                _MemberRoleId = GetRoles(source).First(c => c.Value == "项目成员").Key;
            }
            return _MemberRoleId.Value;
        }

        private static long? _OwenerRoleId;
        internal static long GetOwnerRoleId(Func<Dictionary<long, string>> source)
        {
            if (_OwenerRoleId == null)
            {
                _OwenerRoleId = GetRoles(source).First(c => c.Value == "项目创建人").Key;
            }
            return _OwenerRoleId.Value;
        }

        private static object _Roles;
        public static Dictionary<T, string> GetRoles<T>(Func<Dictionary<T, string>> source)
        {
            if (_Roles == null)
            {
                _Roles = source();
            }
            return (Dictionary<T, string>)_Roles;
        }

        private static object _Users;
        public static Dictionary<T, string> GetUsers<T>(Func<Dictionary<T, string>> source)
        {
            if (_Users == null)
            {
                _Users = source();
            }
            return (Dictionary<T, string>)_Users;
        }

        private static object _Departments;
        public static Dictionary<T, string> GetDepartments<T>(Func<Dictionary<T, string>> source)
        {
            if (_Departments == null)
            {
                _Departments = source();
            }
            return (Dictionary<T, string>)_Departments;
        }

        private static object _ViewAuthorizeTypes;
        public static Dictionary<T, string> GetViewAuthorizeTypes<T>(Func<Dictionary<T, string>> source)
        {
            if (_ViewAuthorizeTypes == null)
            {
                _ViewAuthorizeTypes = source();
            }
            return (Dictionary<T, string>)_ViewAuthorizeTypes;
        }

        internal static string RenderIdsToText<T>(T id, KVType kvType, Func<Dictionary<T, string>> source)
        {
            List<T> ids = new List<T>() { id };
            var values = RenderIdsToText(ids, kvType, source);
            return values.First();
        }
        internal static List<string> RenderIdsToText<T>(List<T> ids, KVType kvType, Func<Dictionary<T, string>> source)
        {
            Dictionary<T, string> dic = null;
            switch (kvType)
            {
                case KVType.ViewAuthorizeType:
                    dic = GetViewAuthorizeTypes(source);
                    break;
                case KVType.Department:
                    dic = GetDepartments(source);
                    break;
                case KVType.User:
                    dic = GetUsers(source);
                    break;
                case KVType.Role:
                    dic = GetRoles(source);
                    break;
                default:
                    break;
            }
            var values = ids.Select(c => dic.ContainsKey(c) ? dic[c] : c.ToString()).ToList();
            return values;
        }
    }

    public enum KVType
    {
        ViewAuthorizeType,
        Department,
        User,
        Role,
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReportTaskService
    {
        APIContext APIContext { get; set; }
        DbContext ResearchDbContext { set; get; }

        FavoriteProjectRepository FavoriteProjectRepository { set; get; }
        ProjectRepository ProjectRepository { set; get; }
        ProjectMemberRepository ProjectMemberRepository { set; get; }
        RoleRepository RoleRepository { set; get; }
        SharedRepository SharedRepository { set; get; }
        UserRepository UserRepository { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ReportTaskService(APIContext apiContext)
        {
            APIContext = apiContext;
            ResearchDbContext = APIContext.GetDBContext(APIContraints.ResearchDbContext);

            //repositories
            FavoriteProjectRepository = new FavoriteProjectRepository(ResearchDbContext);
            ProjectRepository = new ProjectRepository(ResearchDbContext);
            ProjectMemberRepository = new ProjectMemberRepository(ResearchDbContext);
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
                var adminRoleId = DomainConstraits.GetAdminRoleId(() => GetRolesDictionary());
                var memberRoleId = DomainConstraits.GetMemberRoleId(() => GetRolesDictionary());
                var adminIds = ProjectRepository.GetUserIdsByProjectIdAndRoleId(projectId, adminRoleId);
                result.AdminIds = adminIds;
                var memberIds = ProjectRepository.GetUserIdsByProjectIdAndRoleId(projectId, memberRoleId);
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
                var result = DomainConstraits.GetUsers(() => GetUsersDictionary());
                return result;
            });
        }

        internal Dictionary<long, string> GetUsersDictionary()
        {
            var result = UserRepository.GetAllUsers();
            var dic = new Dictionary<long, string>();
            foreach (var item in result)
            {
                dic.Add(item.Id, item.Name);
            }
            return dic;
        }

        internal Dictionary<long, string> GetRolesDictionary()
        {
            var result = RoleRepository.GetAllRoles();
            var dic = new Dictionary<long, string>();
            foreach (var item in result)
            {
                dic.Add(item.Id, item.Name);
            }
            return dic;
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
                var ownerRoleId = DomainConstraits.GetOwnerRoleId(() => GetRolesDictionary());
                var adminRoleId = DomainConstraits.GetAdminRoleId(() => GetRolesDictionary());
                var memberRoleId = DomainConstraits.GetMemberRoleId(() => GetRolesDictionary());
                var members = new List<ProjectMember>();
                members.Add(new ProjectMember(projectId, project.CreatorId, ownerRoleId));
                foreach (var adminId in request.AdminIds ?? new List<long>())
                    members.Add(new ProjectMember(projectId, adminId, adminRoleId));
                foreach (var memberId in request.MemberIds ?? new List<long>())
                    members.Add(new ProjectMember(projectId, memberId, memberRoleId));
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
                var ownerRoleId = DomainConstraits.GetOwnerRoleId(() => GetRolesDictionary());
                var adminRoleId = DomainConstraits.GetAdminRoleId(() => GetRolesDictionary());
                var memberRoleId = DomainConstraits.GetMemberRoleId(() => GetRolesDictionary());
                var members = new List<ProjectMember>();
                members.Add(new ProjectMember(projectId, project.CreatorId, ownerRoleId));
                foreach (var adminId in request.AdminIds)
                    members.Add(new ProjectMember(projectId, adminId, adminRoleId));
                foreach (var memberId in request.MemberIds)
                    members.Add(new ProjectMember(projectId, memberId, memberRoleId));
                ProjectMemberRepository.DeleteByProjectId(projectId);
                ProjectMemberRepository.CreateProjectMembers(members);
                return true;
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

                    return new ServiceResult<T>(default(T), ex.Message);
                }
                finally
                {
                    context.DbGroup.Connection.Close();
                }
            }
            catch (Exception e)
            {
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
