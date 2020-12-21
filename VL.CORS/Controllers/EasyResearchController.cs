using Autobots.Infrastracture.Common.ControllerSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ResearchAPI.Common;
using ResearchAPI.CORS.Common;
using ResearchAPI.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ResearchAPI.Controllers
{

    public static class VLAutoMappler
    {
        /// <summary>
        /// 注意,只自动匹配 '类型'和'名称'一致的属性
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void MapTo(this object from, object to)
        {
            PropertyInfo[] fromProperties = from.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            PropertyInfo[] toProperties = to.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var fromProperty in fromProperties)
            {
                var matchedProperty = toProperties.FirstOrDefault(c => c.Name == fromProperty.Name);
                if (matchedProperty.PropertyType == fromProperty.PropertyType)
                {
                    matchedProperty.SetValue(to, fromProperty.GetValue(from));
                }
            }
        }
    }

    public class TestContext
    {
        public const long UserId = 1;
    }

    public class APIBaseController : Controller
    {

        #region APIResult,便捷方法
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        internal APIResult<T> Success<T>(T data)
        {
            return new APIResult<T>(data);
        }
        /// <summary>
        /// 
        /// </summary>
        internal APIResult<T> Success<T>(T data, params string[] messages)
        {
            return new APIResult<T>(data, messages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(T data, IList<string> messages)
        {
            return new APIResult<T>(data, messages.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(T data, int code, IList<string> messages)
        {
            return new APIResult<T>(data, code, messages.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(params string[] messages)
        {
            return new APIResult<T>(default(T), messages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(T data, params string[] messages)
        {
            return new APIResult<T>(data, messages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(T data, int code, params string[] messages)
        {
            return new APIResult<T>(data, code, messages);
        }
        #endregion
    }

    /// <summary>
    /// 下拉项
    /// </summary>
    public class DropDownItem
    {
        /// <summary>
        /// 下拉项
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public DropDownItem(string text, string value)
        {
            this.text = text;
            this.value = value;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string text { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string value { set; get; }
    }

    /// <summary>
    /// 科研内核接口
    /// </summary>
    //[ApiController]
    [Route("api/[controller]/[action]")]
    public class EasyResearchController : APIBaseController
    {
        #region 通用,Dropdown

        /// <summary>
        /// 下拉项
        /// 1.1.2.下拉项_科室
        /// 1.2.2.下拉项_项目成员
        /// 1.2.3.下拉项_关联科室
        /// </summary>
        /// <param name="type">类型名称</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<VLKeyValue<string, long>>> GetDropdowns([FromServices] ReportTaskService service, string type)
        {
            List<VLKeyValue<string, long>> values = new List<VLKeyValue<string, long>>();
            switch (type)
            {
                case "BusinessType":
                    values.AddRange(DomainConstraits.GetBusinessTypes(() => LoadByXMLConfig())
                        .Select(c => new VLKeyValue<string, long>(c.Name, c.Id)));
                    return Success(values);
                case "Member":
                    var result = service.GetAllUsersIdAndName();
                    foreach (var user in result.Data)
                    {
                        values.Add(new VLKeyValue<string, long>(user.Value, user.Key));
                    }
                    return Success(values);
                default:
                    values = LoadKeyValueFile<long>(type);
                    return Success(values);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<VLKeyValue<string, long>>> GetDropdownsWithParent([FromServices] ReportTaskService service, string type,long parentId)
        {
            List<VLKeyValue<string, long>> values = new List<VLKeyValue<string, long>>();
            switch (type)
            {
                case "BusinessEntity":
                    values.AddRange(DomainConstraits.GetBusinessEntities(() => LoadByXMLConfig())
                        .Select(c => new VLKeyValue<string, long>(c.Name, c.Id)));
                    return Success(values);
                case "BusinessEntityProperty":
                    values.AddRange(DomainConstraits.GetBusinessEntityProperties(() => LoadByXMLConfig())
                        .Select(c => new VLKeyValue<string, long>(c.DisplayName, c.Id)));
                    return Success(values);
                default:
                    throw new NotImplementedException("未支持该类型");
            }
        }
        private static List<COBusinessEntities> LoadByXMLConfig()
        {
            var businessEntitiesCollection = new List<COBusinessEntities>();
            var directory = @"Configs/XMLConfigs/BusinessEntities";
            var files = Directory.GetFiles(directory);
            var bsfiles = files.Select(c => Path.GetFileName(c)).Where(c => c.StartsWith("BusinessEntities"));
            foreach (var bsfile in bsfiles)
            {
                var businessEntities = ConfigHelper.GetBusinessEntities(directory, bsfile);
                businessEntitiesCollection.Add(businessEntities);
            }
            return businessEntitiesCollection;
        }


        private List<VLKeyValue<string, T>> LoadKeyValueFile<T>(string type)
        {
            List<VLKeyValue<string, T>> values = new List<VLKeyValue<string, T>>();
            var file = (Path.Combine(AppContext.BaseDirectory, "Configs/JsonConfigs", type + ".json"));
            if (!System.IO.File.Exists(file))
            {
                values.Add(new VLKeyValue<string, T>("请联系管理员配置", default(T)));
                System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(values));
                return values;
            }
            var data = System.IO.File.ReadAllText(file);
            values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VLKeyValue<string, T>>>(data);
            return values;
        }

        private Dictionary<T, string> GetDictionary<T>(string type)
        {
            var kvs = LoadKeyValueFile<T>(type);
            var result = new Dictionary<T, string>();
            foreach (var kv in kvs)
            {
                result.Add(kv.Value, kv.Key);
            }
            return result;
        }

        #endregion

        #region 项目,Project

        /// <summary>
        /// 1.1.0.获取项目列表(菜单)
        /// </summary>
        /// <returns>Key:项目名称,Value:项目Id</returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<VLKeyValue<string, long>>> GetFavoriteProjects([FromServices] APIContext context, [FromServices] ReportTaskService service)
        {
            var userid = context.GetCurrentUser().UserId;
            var result = service.GetFavoriteProjects(userid);
            return new APIResult<List<VLKeyValue<string, long>>>(result);
        }

        /// <summary>
        /// 1.1.1.获取项目列表
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<VLPagerResult<List<Dictionary<string, object>>>> GetPagedProjects([FromServices] ReportTaskService service, [FromBody] GetCommonSelectRequest request)
        {
            var result = service.GetPagedResultBySQLConfig(request);
            return new APIResult<VLPagerResult<List<Dictionary<string, object>>>>(result);
        }

        /// <summary>
        /// 1.1.3.获取项目详情
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<GetProjectModel> GetProject([FromServices] ReportTaskService service, long projectId)
        {
            var result = service.GetProject(projectId);
            return new APIResult<GetProjectModel>(result);
        }

        /// <summary>
        /// 1.1.4.删除项目
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> DeleteProject([FromServices] ReportTaskService service, long projectId)
        {
            var result = service.DeleteProject(projectId);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.2.1.新建项目
        /// </summary>
        /// <returns>项目Id</returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<long> CreateProject([FromServices] APIContext context, [FromServices] ReportTaskService service, CreateProjectRequest request)
        {
            var userid = context.GetCurrentUser().UserId;
            var result = service.CreateProject(request, userid);
            return new APIResult<long>(result);
        }

        /// <summary>
        /// 1.2.5.收藏项目
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> AddFavoriteProject([FromServices] APIContext context, [FromServices] ReportTaskService service, long projectId)
        {
            var userId = context.GetCurrentUser().UserId;
            var result = service.AddFavoriteProject(projectId, userId);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.2.6.解除收藏项目
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> DeleteFavoriteProject([FromServices] APIContext context, [FromServices] ReportTaskService service, long projectId)
        {
            var userId = context.GetCurrentUser().UserId;
            var result = service.DeleteFavoriteProject(projectId, userId);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.3.2.编辑项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> EditProject([FromServices] APIContext context, [FromServices] ReportTaskService service, EditProjectRequest request)
        {
            var userid = context.GetCurrentUser().UserId;
            var result = service.EditProject(request, userid);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.3.4.获取操作记录 GetProjectOperateHistory
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<GetProjectOperateHistoryResponse> GetProjectOperateHistory(long projectId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 1.4.1.获取项目概要信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<GetBiefProjectResponse> GetBiefProject([FromServices] ReportTaskService service, long projectId)
        {
            var serviceResult = service.GetProject(projectId);
            if (!serviceResult.IsSuccess)
                return new APIResult<GetBiefProjectResponse>(null, serviceResult.Code, serviceResult.Message);
            var result = new GetBiefProjectResponse(serviceResult.Data);
            result.ViewAuthorizeTypeName = DomainConstraits.RenderIdsToText(result.ViewAuthorizeType, KVType.ViewAuthorizeType, () => GetDictionary<int>(KVType.ViewAuthorizeType.ToString()));
            if (result.DepartmentId.HasValue)
                result.DepartmentName = DomainConstraits.RenderIdsToText(result.DepartmentId.Value, KVType.Department, () => GetDictionary<long>(KVType.Department.ToString()));
            result.AdminNames = DomainConstraits.RenderIdsToText(result.AdminIds, KVType.User, () => service.GetUsersDictionary());
            result.CreateName = DomainConstraits.RenderIdsToText(result.CreatorId.Value, KVType.User, () => service.GetUsersDictionary());
            result.MemberNames = DomainConstraits.RenderIdsToText(result.MemberIds, KVType.User, () => service.GetUsersDictionary());
            return new APIResult<GetBiefProjectResponse>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetBiefProjectResponse : GetProjectModel
        {
            public GetBiefProjectResponse(GetProjectModel model)
            {
                model.MapTo(this);

                //this.ProjectId = model.ProjectId;
                //this.ProjectName = model.ProjectName;
                //this.AdminIds = model.AdminIds;
                //this.MemberIds = model.MemberIds;
                //this.CreatorId = model.CreatorId;
                //this.DepartmentId = model.DepartmentId;
                //this.ViewAuthorizeType = model.ViewAuthorizeType;
                //this.ProjectDescription = model.ProjectDescription;
                //this.CreatedAt = model.CreatedAt;
                //this.LastModifiedAt = model.LastModifiedAt;
                //this.LastModifiedBy = model.LastModifiedBy;
                //this.IsFavorite = model.IsFavorite;
            }

            /// <summary>
            /// 项目查看权限 名称
            /// </summary>
            public string ViewAuthorizeTypeName { set; get; }
            /// <summary>
            /// 创建者 名称
            /// </summary>
            public string CreateName { set; get; }
            /// <summary>
            /// 关联科室 名称
            /// </summary>
            public string DepartmentName { set; get; }
            /// <summary>
            /// 项目管理人员 名称
            /// </summary>
            public List<string> AdminNames { set; get; }
            /// <summary>
            /// 项目成员 名称
            /// </summary>
            public List<string> MemberNames { set; get; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetIndicatorsResponse
        {
            /// <summary>
            /// 指标名称
            /// </summary>
            public string IndicatorName { set; get; }
            /// <summary>
            /// 指标类别
            /// </summary>
            public int IndicatorType { set; get; }
            /// <summary>
            /// 项目名称
            /// </summary>
            public string ProjectName { set; get; }
            /// <summary>
            /// 规则
            /// </summary>
            public string Rule { set; get; }

        }

        /// <summary>
        /// 1.4.2.获取指标集合 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<GetIndicatorsResponse>> GetIndicators(long projectId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 1.4.13.保存指标
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> AddProjectIndicators([FromServices] ReportTaskService service, [FromBody] AddIndicatorsRequest request)
        {
            var serviceResult = service.AddProjectIndicators(request);
            return new APIResult<bool>(serviceResult);
        }

        #endregion

        /// <summary>
        /// 新建用户指标单元
        /// </summary>
        [HttpPost]
        public async Task<APIResult<long>> CreateCustomBusinessEntity([FromServices] ReportTaskService service, CreateCustomBusinessEntityRequest request)
        {
            var serviceResult = service.CreateCustomBusinessEntity(request);
            var apiResult = new APIResult<long>(serviceResult);
            return await Task.FromResult(apiResult);
        }

        /// <summary>
        /// 编辑用户指标单元
        /// </summary>
        [HttpPost]
        public async Task<APIResult<bool>> EditCustomBusinessEntity([FromServices] ReportTaskService service, EditCustomBusinessEntityRequest request)
        {
            var serviceResult = service.EditCustomBusinessEntity(request);
            var apiResult = new APIResult<bool>(serviceResult);
            return await Task.FromResult(apiResult);
        }

        /// <summary>
        /// 获取 指标单元
        /// </summary>
        [HttpPost]
        public async Task<APIResult<List<GetBusinessEntityModel>>> GetBusinessEntities([FromServices] ReportTaskService service)
        {
            var apiResult = new APIResult<List<GetBusinessEntityModel>>(new List<GetBusinessEntityModel>());
            return await Task.FromResult(apiResult);
        }

        /// <summary>
        /// 获取 指标属性
        /// </summary>
        [HttpPost]
        public async Task<APIResult<List<GetBusinessEntityPropertiesModel>>> GetBusinessEntityProperties([FromServices] ReportTaskService service)
        {
            var apiResult = new APIResult<List<GetBusinessEntityPropertiesModel>>(new List<GetBusinessEntityPropertiesModel>());
            return await Task.FromResult(apiResult);
        }

        /// <summary>
        /// 更新 项目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult<bool>> UpdateReportProject(UpdateReportProjectRequest request)
        {
            var result = new APIResult<bool>(true);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 新建 项目执行项,即队列
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult<bool>> CreateReportTask(CreateReportTaskRequest request)
        {
            var result = new APIResult<bool>(true);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 更新 项目执行项,即队列
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult<bool>> UpdateReportTask(UpdateReportTaskRequest request)
        {
            var result = new APIResult<bool>(true);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        [HttpPost]
        public async Task<APIResult<bool>> ExecuteReportTask([FromServices] ReportTaskService service, long taskId)
        {
            var serviceResult = service.ExecuteReportTask(taskId);
            var apiResult = new APIResult<bool>(serviceResult);
            return await Task.FromResult(apiResult);
        }

        /// <summary>
        /// 获取任务执行状态描述
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult<ReportTaskStatusResponse>> GetReportTaskStatus(long taskId)
        {
            var result = new APIResult<ReportTaskStatusResponse>(new ReportTaskStatusResponse());
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 下载任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>文件路径</returns>
        [HttpPost]
        public async Task<APIResult<string>> DownloadReportTask(long taskId)
        {
            var result = new APIResult<string>("");
            return await Task.FromResult(result);
        }
    }

    public class GetBusinessEntityPropertiesModel
    {
        /// <summary>
        /// 指标单元Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        /// <summary>
        /// 指标单元名称
        /// </summary>
        public string BusinessEntityName { set; get; }
        /// <summary>
        /// 是否用户自定义
        /// </summary>
        public bool IsCustom { set; get; }
        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool IsSelected { set; get; }
    }

    public class GetBusinessEntityModel
    {
        /// <summary>
        /// 指标单元Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        /// <summary>
        /// 指标单元名称
        /// </summary>
        public string BusinessEntityName { set; get; }
        /// <summary>
        /// 是否用户自定义
        /// </summary>
        public bool IsCustom { set; get; }
        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool IsSelected { set; get; }
    }

    public class CreateCustomBusinessEntityRequest
    {
        public long TemplateId { set; get; }
        public string DisplayName { set; get; }
        public List<COBusinessEntityProperty> Properties { set; get; }
        public List<COBusinessEntityWhere> Wheres { set; get; }
    }

    public class EditCustomBusinessEntityRequest
    {
        public long CustomBBusinessEntityId { set; get; }
        public long TemplateId { set; get; }
        public string DisplayName { set; get; }
        public List<COBusinessEntityProperty> Properties { set; get; }
        public List<COBusinessEntityWhere> Wheres { set; get; }
    }

    public class ReportTaskStatusResponse
    {
        public int ExecuteRate { set; get; }
        public ExecuteStatus ExecuteStatus { set; get; }

    }

    public enum ExecuteStatus
    {
        None = 0,
        Executing = 1,
        Done = 2,
        Error = 3,
    }

    public class SaveReportTaskRequest
    {
        public List<Field2FieldWhere> F2FWheres { set; get; }
    }

    public class CreateReportTaskRequest : SaveReportTaskRequest
    {
    }

    public class UpdateReportTaskRequest : SaveReportTaskRequest
    {
        public long TaskId { set; get; }
    }

    public class SaveReportProjectRequest
    {
        public string ReportName { set; get; }
        public List<BusinessEntityPropertyDTO> Properties { set; get; }
    }

    public class CreateReportProjectRequest : SaveReportProjectRequest
    {
    }

    public class UpdateReportProjectRequest : SaveReportProjectRequest
    {
        public long TaskId { set; get; }
    }

    public class BusinessEntityPropertyDTO
    {
        /// <summary>
        /// 来源对象
        /// </summary>
        public string From { set; get; }
        /// <summary>
        /// 如果不是默认表,则填写来源的自定义对象Id
        /// </summary>
        public string CustomBusinessId { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { set; get; }
    }
}
