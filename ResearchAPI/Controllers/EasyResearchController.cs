using Autobots.Infrastracture.Common.ControllerSolution;
using Microsoft.AspNetCore.Mvc;
using ResearchAPI.Common;
using ResearchAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResearchAPI.Controllers
{
    /// <summary>
    /// 科研内核接口
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EasyResearchController : ControllerBase
    {
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
        /// 新建 项目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult<bool>> CreateReportProject(CreateReportProjectRequest request)
        {
            var result = new APIResult<bool>(true);
            return await Task.FromResult(result);
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
        public List<BusinessEntityProperty> Properties { set; get; }
        public List<BusinessEntityWhere> Wheres { set; get; }
    }

    public class EditCustomBusinessEntityRequest
    {
        public long CustomBBusinessEntityId { set; get; }
        public long TemplateId { set; get; }
        public string DisplayName { set; get; }
        public List<BusinessEntityProperty> Properties { set; get; }
        public List<BusinessEntityWhere> Wheres { set; get; }
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
        Error =3,
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
