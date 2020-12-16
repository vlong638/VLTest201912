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
    /// <summary>
    /// 
    /// </summary>
    public class ReportTaskService
    {
        static ReportTaskDomain Domain { set; get; }

        public APIContext APIContext { get; private set; }

        public ReportTaskService(APIContext apiContext)
        {
            APIContext = apiContext;
            Domain = new ReportTaskDomain(APIContext);
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

            var adminId = Domain.GetUserIdsByProjectAndRoleName(projectId, "项目管理员");
            result.AdminIds = adminId;
            var memberId = Domain.GetUserIdsByProjectAndRoleName(projectId, "项目成员");
            result.MemberIds = memberId;
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
            var result = Domain.GetPagedResultBySQLConfig(sqlConfig);
            return new ServiceResult<VLPagerResult<List<Dictionary<string, object>>>>(result);
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

        public static string DefaultConnectionString { set; get; } = "DefaultConnectionString";
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
        APIContext APIContext { set; get; }
        DbContext DefaultDbContext { set; get; }
        ProjectRepository ProjectRepository { set; get; }
        SharedRepository SharedRepository { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ReportTaskDomain(APIContext dbContext)
        {
            APIContext = dbContext;
            DefaultDbContext = dbContext.GetDBContext(APIContraints.DefaultConnectionString);
            ProjectRepository = new ProjectRepository(DefaultDbContext);
            SharedRepository = new SharedRepository(DefaultDbContext);
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
            return ProjectRepository.GetUserFavoriteProjects(userid);
        }

        internal Project GetProject(int projectId)
        {
            return ProjectRepository.GetById(projectId);
        }

        internal bool DeleteProject(int projectId)
        {
            return ProjectRepository.DeleteById(projectId);
        }

        internal List<long> GetUserIdsByProjectAndRoleName(int projectId, string roleName)
        {
            return ProjectRepository.GetUserIdsByProjectAndRoleName(projectId, roleName);
        }

        internal VLPagerResult<List<Dictionary<string, object>>> GetPagedResultBySQLConfig(SQLConfigV2 sqlConfig)
        {
            return DefaultDbContext.DelegateTransaction(c =>
            {
                var list = SharedRepository.GetCommonSelect(sqlConfig.Source, sqlConfig.Skip, sqlConfig.Limit);
                var count = SharedRepository.GetCommonSelectCount(sqlConfig);
                sqlConfig.Source.DoTransforms(ref list);
                return new VLPagerResult<List<Dictionary<string, object>>>() { List = list.ToList(), Count = count };
            });
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
        public static T DelegateTransaction<T>(this DbContext context, Func<DbContext, T> exec)
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
                    return result;
                }
                catch (Exception ex)
                {
                    context.DbGroup.Transaction.Rollback();
                    context.DbGroup.Connection.Close();

                    Log4NetLogger.Error("DelegateTransaction Exception", ex);
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error($"当前数据库连接:{context.DbGroup.Connection.ConnectionString},DelegateTransaction Exception:", ex);
                return default(T);
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
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      