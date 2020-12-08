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
        /// 新建队列任务
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
        /// 更新队列任务
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
        public string ReportName { set; get; }
        public List<BusinessEntityProperty> Properties { set; get; }
        public List<Field2FieldWhere> F2FWheres { set; get; }
    }

    public class CreateReportTaskRequest : SaveReportTaskRequest
    {
    }

    public class UpdateReportTaskRequest : SaveReportTaskRequest
    {
        public long TaskId { set; get; }
    }
}
