using Autobots.Infrastracture.Common.ControllerSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ResearchAPI.CORS.Common;
using ResearchAPI.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchAPI.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class TestContext
    {
        public const long UserId = 1;
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
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult<bool> InitData([FromServices] ReportTaskService service)
        {
            DomainConstraits.InitData(service);
            return new APIResult<bool>(true);
        }

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
        public APIResult<List<VLKeyValue<string, string>>> GetDropdowns([FromServices] ReportTaskService service, string type)
        {
            List<VLKeyValue<string, string>> values = new List<VLKeyValue<string, string>>();
            switch (type)
            {
                case "BusinessType":
                    values.AddRange(DomainConstraits.BusinessTypes.Select(c => new VLKeyValue<string, string>(c.Name, c.Id.ToString())));
                    return Success(values);
                case "Member":
                    var result = service.GetAllUsersIdAndName();
                    foreach (var user in result.Data)
                    {
                        values.Add(new VLKeyValue<string, string>(user.Value, user.Key.ToString()));
                    }
                    return Success(values);
                case "LabOrder":
                    values.AddRange(DomainConstraits.LabOrders.Select(c => new VLKeyValue<string, string>(c.Value, c.Key)));
                    return Success(values);
                default:
                    values = ConfigHelper.GetJsonConfig<string, string>(type);
                    return Success(values);
            }
        }

        /// <summary>
        /// 下拉项
        /// </summary>
        /// <param name="service"></param>
        /// <param name="type"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<VLKeyValue<string, string>>> GetDropdownsWithParent([FromServices] ReportTaskService service, string type, string parentId)
        {
            List<VLKeyValue<string, string>> values = new List<VLKeyValue<string, string>>();
            switch (type)
            {
                case "BusinessEntity":
                    values.AddRange(DomainConstraits.BusinessEntities
                        .Where(c => c.BusinessTypeId == parentId.ToLong())
                        .Select(c => new VLKeyValue<string, string>(c.Name, c.Id.ToString())));
                    return Success(values);
                case "BusinessEntityProperty":
                    values.AddRange(DomainConstraits.BusinessEntityProperties
                        .Where(c => c.BusinessEntityId == parentId.ToLong())
                        .Select(c => new VLKeyValue<string, string>(c.DisplayName, c.Id.ToString())));
                    return Success(values);
                case "LabResult":
                    values.AddRange(DomainConstraits.LabResults
                        .First(c => c.Key == parentId)
                        .Select(c => new VLKeyValue<string, string>(c.Value, c.Key)));
                    return Success(values);
                default:
                    throw new NotImplementedException("未支持该类型");
            }
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
        public APIResult<GetProjectOperateHistoryModel> GetProjectOperateHistory(long projectId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 1.4.1.获取项目概要信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<GetBriefProjectModel> GetBriefProject([FromServices] ReportTaskService service, long projectId)
        {
            var serviceResult = service.GetProject(projectId);
            if (!serviceResult.IsSuccess)
                return new APIResult<GetBriefProjectModel>(null, serviceResult.Code, serviceResult.Message);
            var result = new GetBriefProjectModel(serviceResult.Data);
            result.ViewAuthorizeTypeName = DomainConstraits.RenderIdToText(result.ViewAuthorizeType, DomainConstraits.ViewAuthorizeTypes);
            if (result.DepartmentId.HasValue)
                result.DepartmentName = DomainConstraits.RenderIdToText(result.DepartmentId.Value, DomainConstraits.Departments);
            result.AdminNames = DomainConstraits.RenderIdsToText<long>(result.AdminIds, DomainConstraits.Users);
            result.CreateName = DomainConstraits.RenderIdToText(result.CreatorId.Value, DomainConstraits.Users);
            result.MemberNames = DomainConstraits.RenderIdsToText(result.MemberIds, DomainConstraits.Users);
            return new APIResult<GetBriefProjectModel>(result);
        }

        /// <summary>
        /// 1.4.2.获取指标集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<GetProjectIndicatorModel>> GetProjectIndicators([FromServices] ReportTaskService service, long projectId)
        {
            var result = service.GetProjectIndicators(projectId);
            result.Data.ForEach(c => c.BusinessEntityName = DomainConstraits.RenderIdToText<long>(c.BusinessEntityId, DomainConstraits.BusinessEntityDic));
            return new APIResult<List<GetProjectIndicatorModel>>(result);
        }

        /// <summary>
        /// 1.4.5.删除指标(批量)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<int> DeleteProjectIndicators([FromServices] ReportTaskService service, List<long> indicatorIds)
        {
            var result = service.DeleteProjectIndicators(indicatorIds);
            return new APIResult<int>(result);
        }

        /// <summary>
        /// 1.4.6.删除指标
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> DeleteProjectIndicator([FromServices] ReportTaskService service, long indicatorId)
        {
            var result = service.DeleteProjectIndicator(indicatorId);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.4.11.保存自定义指标
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<BusinessEntityPropertyModel>> CreateCustomIndicator([FromServices] ReportTaskService service, [FromBody] CreateCustomIndicatorRequest request)
        {
            switch (request.TargetArea)
            {
                case TargetArea.None:
                    break;
                case TargetArea.Pregnant:
                    var template = ConfigHelper.GetBusinessEntityTemplate("Configs\\XMLConfigs\\BusinessEntities", "Template_孕周检验.xml");
                    request.Properties = new List<BusinessEntityPropertyModel>() {
                        new BusinessEntityPropertyModel(){ ColumnName = "Value"},
                    };
                    var result = service.CreateCustomIndicator(request, template);
                    return new APIResult<List<BusinessEntityPropertyModel>>(result);
                case TargetArea.Woman:
                    break;
                case TargetArea.Child:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        public class GetBriefProjectModel : GetProjectModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="model"></param>
            public GetBriefProjectModel(GetProjectModel model)
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

        /// <summary>
        /// 1.5.1.新增队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<long> CreateTask([FromServices] ReportTaskService service, [FromBody] CreateTaskRequest request)
        {
            var serviceResult = service.CreateTask(request);
            return new APIResult<long>(serviceResult);
        }

        /// <summary>
        /// 1.5.2.编辑队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> EditTask([FromServices] ReportTaskService service, [FromBody] EditTaskRequest request)
        {
            var serviceResult = service.EditTask(request);
            return new APIResult<bool>(serviceResult);
        }

        /// <summary>
        /// 1.5.3.删除队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> DeleteTask([FromServices] ReportTaskService service, long taskId)
        {
            var serviceResult = service.DeleteTask(taskId);
            return new APIResult<bool>(serviceResult);
        }

        /// <summary>
        /// 1.5.4.执行队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<long> CommitTask([FromServices] ReportTaskService service, CommitTaskRequest request)
        {
            var serviceResult = service.CommitTask(request);
            return new APIResult<long>(serviceResult);
        }

        /// <summary>
        /// 1.5.5.查看队列集合
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<GetTaskModel>> GetTasks([FromServices] ReportTaskService service, long projectId)
        {
            var serviceResult = service.GetTasks(projectId);
            serviceResult.Data.ForEach(c =>
            {
                c.Wheres.ForEach(d =>
                {
                    ////TODO 补全显示用内容
                    //d.OperatorName = DomainConstraits.RenderIdToText();
                    //d.DisplayName = DomainConstraits.RenderIdToText();
                });
            });
            return new APIResult<List<GetTaskModel>>(serviceResult);
        }

        /// <summary>
        /// 1.5.6.查看队列状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<GetTaskStatusModel> GetTaskStatus([FromServices] ReportTaskService service, long taskId)
        {
            var serviceResult = service.GetTaskStatus(taskId);
            if (serviceResult.IsSuccess)
            {
                //TODO
                //serviceResult.Data.ScheduleStatusName = DomainConstraits.RenderIdsToText();
                //serviceResult.Data.ProcessingRate = Redis.GetCurrentProcessingRate(taskId);
                serviceResult.Data.ProcessingRate = (serviceResult.Data.ScheduleStatus == ScheduleStatus.Completed ? 100 : 50);
            }
            return new APIResult<GetTaskStatusModel>(serviceResult);
        }

        /// <summary>
        /// 1.5.7.导出
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(FileResult),0)]
        public FileResult Download(string path)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, path);
            if (!System.IO.File.Exists(fullPath))
            {
                return null;
            }
            FileStream fs = new FileStream(fullPath, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            MemoryStream ms = new MemoryStream(data);
            var mediaType = path.EndsWith(".xls")|| path.EndsWith(".xlsx") ? "application/msexcel" : "";
            var actionresult = new FileStreamResult(ms, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(mediaType));
            actionresult.FileDownloadName = path;
            return actionresult;
        }

        /// <summary>
        /// 1.5.9.编辑队列名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> EditTaskName([FromServices] ReportTaskService service, [FromBody] EditTaskNameRequest request)
        {
            var serviceResult = service.EditTaskName(request);
            return new APIResult<bool>(serviceResult);
        }

        #endregion
    }
}
