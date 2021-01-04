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

namespace ResearchAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TestContext
    {
        public const long UserId = 1;

        public const string UserName = "管理员";
    }

    /// <summary>
    /// 科研内核接口
    /// </summary>
    //[ApiController]
    [Route("api/[controller]/[action]")]
    public class EasyResearchController : APIBaseController
    {
        #region 通用,Dropdown

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="service"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public APIResult<bool> InitData([FromServices] ReportTaskService service)
        //{
        //    DomainConstraits.InitData(service);
        //    return new APIResult<bool>(true);
        //}

        /// <summary>
        /// 下拉项
        /// 1.1.2.下拉项_科室
        /// 1.2.2.下拉项_项目成员
        /// 1.2.3.下拉项_关联科室
        /// </summary>
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
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<VLKeyValue<string, string>>> GetDropdownsWithParent([FromServices] ReportTaskService service, string type, string parentId)
        {
            List<VLKeyValue<string, string>> values = new List<VLKeyValue<string, string>>();
            switch (type)
            {
                case "Operator":
                    switch (parentId)
                    {
                        case "1":
                            values = ConfigHelper.GetJsonConfig<string, string>("Operator2String");
                            break;
                        case "2":
                            values = ConfigHelper.GetJsonConfig<string, string>("Operator2DateTime");
                            break;
                        case "3":
                            values = ConfigHelper.GetJsonConfig<string, string>("Operator2Int");
                            break;
                        case "4":
                            values = ConfigHelper.GetJsonConfig<string, string>("Operator2Enum");
                            break;
                        default:
                            break;
                    }
                    return Success(values);
                case "ProjectTask":
                    values.AddRange(service.GetTaskNameAndIds(parentId.ToLong().Value).Data);
                    return Success(values);
                case "ProjectMember":
                    var result = service.GetProjectMemberIdAndName(parentId.ToLong().Value);
                    foreach (var user in result.Data)
                    {
                        values.Add(new VLKeyValue<string, string>(user.Key, user.Value.ToString()));
                    }
                    return Success(values);
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
            //TODO加入访问Id的控制
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
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<long> CreateProject([FromServices] APIContext context, [FromServices] ReportTaskService service, CreateProjectRequest request)
        {
            var userid = context.GetCurrentUser().UserId;
            var result = service.CreateProject(request, userid);
            if (result.IsSuccess)
            {
                //{用户}设置了项目名称{项目名称}，
                var projectId = result.Data;
                var userId = context.GetCurrentUser().UserId;
                var userName = context.GetCurrentUser().UserName;
                var text = $"{userName}设置了项目名称:{request.ProjectName}";
                service.AddProjectLog(userId, projectId, ActionType.EditProjectName, text);

                //{用户}添加了项目管理员{用户名}，
                var adminIds = request.AdminIds;
                var adminNames = DomainConstraits.RenderIdToText(adminIds, DomainConstraits.Users);
                text = $"{userName}添加了项目管理员:{string.Join(",", adminNames)}";
                service.AddProjectLog(userId, projectId, ActionType.AddProjectManager, text);

                //{用户}添加了项目成员{用户名}，
                var memberIds = request.MemberIds;
                var memberNames = DomainConstraits.RenderIdToText(memberIds, DomainConstraits.Users);
                text = $"{userName}添加了项目成员:{string.Join(",", memberNames)}";
                service.AddProjectLog(userId, projectId, ActionType.AddProjectMember, text);

                //{用户}添加了关联科室{科室名称}，
                var departmentIds = request.DepartmentIds;
                var departmentNames = DomainConstraits.RenderIdToText(departmentIds, DomainConstraits.Departments);
                text = $"{userName}添加了关联科室:{string.Join(",", departmentNames)}";
                service.AddProjectLog(userId, projectId, ActionType.AddProjectDepartment, text);

                //{用户}修改项目查看权限为{权限名称}，
                var viewAuthorizeType = request.ViewAuthorizeType;
                var viewAuthorizeTypeName = DomainConstraits.RenderIdToText(viewAuthorizeType, DomainConstraits.ViewAuthorizeTypes);
                text = $"{userName}设置了项目查看权限:{viewAuthorizeTypeName}";
                service.AddProjectLog(userId, projectId, ActionType.SetProjectViewAtuhorityType, text);
            }
            return new APIResult<long>(result);
        }

        /// <summary>
        /// 1.2.5.收藏项目
        /// </summary>
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
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<GetProjectOperateHistoryModel>> GetProjectOperateHistory([FromServices] ReportTaskService service,[FromBody] GetProjectOperateHistoryRequest request)
        {
            var serviceResult = service.GetProjectOperateHistory(request);
            return new APIResult<List<GetProjectOperateHistoryModel>>(serviceResult);
        }

        /// <summary>
        /// 1.4.1.获取项目概要信息
        /// </summary>
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
            result.DepartmentNames = DomainConstraits.RenderIdToText(result.DepartmentIds, DomainConstraits.Departments);
            result.AdminNames = DomainConstraits.RenderIdToText(result.AdminIds, DomainConstraits.Users);
            result.CreateName = DomainConstraits.RenderIdToText(result.CreatorId, DomainConstraits.Users);
            result.MemberNames = DomainConstraits.RenderIdToText(result.MemberIds, DomainConstraits.Users);
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
            return new APIResult<List<GetProjectIndicatorModel>>(result);
        }

        /// <summary>
        /// 1.4.5.删除指标(批量)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<int> DeleteProjectIndicators([FromServices] ReportTaskService service, [FromBody] List<long> indicatorIds)
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
        public APIResult<bool> DeleteProjectIndicator([FromServices] ReportTaskService service, [FromBody] DeleteProjectIndicatorRequest request)
        {
            var result = service.DeleteProjectIndicator(request.IndicatorId);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.4.15.编辑指标名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> UpdateIndicatorName([FromServices] ReportTaskService service, [FromBody] UpdateIndicatorNameRequest request)
        {
            var result = service.UpdateIndicatorName(request.IndicatorId, request.Name);
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

        ///// <summary>
        ///// 1.4.11.保存自定义指标
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[EnableCors("AllCors")]
        //public APIResult<List<BusinessEntityPropertyModel>> CreateCustomIndicator([FromServices] ReportTaskService service, [FromBody] CreateCustomIndicatorRequest request)
        //{
        //    switch (request.TargetArea)
        //    {
        //        case TargetArea.None:
        //            break;
        //        case TargetArea.Pregnant:
        //            var template = ConfigHelper.GetBusinessEntityTemplate("Configs\\XMLConfigs\\BusinessEntities", "Template_孕周检验.xml");
        //            request.Properties = new List<BusinessEntityPropertyModel>() {
        //                new BusinessEntityPropertyModel(){ ColumnName = "Value"},
        //            };
        //            var result = service.CreateCustomIndicator(request, template);
        //            return new APIResult<List<BusinessEntityPropertyModel>>(result);
        //        case TargetArea.Woman:
        //            break;
        //        case TargetArea.Child:
        //            break;
        //        default:
        //            break;
        //    }
        //    throw new NotImplementedException();
        //}

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
        public APIResult<long> CreateTask([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] CreateTaskRequest request)
        {
            var serviceResult = service.CreateTask(request);
            if (serviceResult.IsSuccess)
            {
                var projectId = request.ProjectId;
                var taskName = request.TaskName;
                var userId = context.GetCurrentUser().UserId;
                var userName = context.GetCurrentUser().UserName;
                var text = $"{userName}添加了科研队列:{taskName}";
                service.AddProjectLog(userId, projectId, ActionType.AddTask, text);
            }
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
        public APIResult<bool> DeleteTask([FromServices] APIContext context, [FromServices] ReportTaskService service, long taskId)
        {
            var task = service.GetTaskById(taskId);
            if (!task.IsSuccess)
                return new APIResult<bool>(false, task.Messages);
            var serviceResult = service.DeleteTask(taskId);
            if (serviceResult.IsSuccess)
            {
                var projectId = task.Data.ProjectId;
                var taskName = task.Data.Name;
                var userId = context.GetCurrentUser().UserId;
                var userName = context.GetCurrentUser().UserName;
                var text = $"{userName}删除了科研队列:{taskName}";
                service.AddProjectLog(userId, projectId, ActionType.AddTask, text);
            }
            return new APIResult<bool>(serviceResult);
        }

        /// <summary>
        /// 1.5.4.执行队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<long> CommitTask([FromServices] APIContext context, [FromServices] ReportTaskService service, CommitTaskRequest request)
        {
            var task = service.GetTaskById(request.TaskId);
            if (!task.IsSuccess)
                return new APIResult<long>(0, task.Messages);
            var serviceResult = service.CommitTask(request);
            if (serviceResult.IsSuccess)
            {
                var result = service.StartSchedule(serviceResult.Data);
                if (result.IsSuccess)
                {
                    var projectId = request.ProjectId;
                    var taskName = task.Data.Name;
                    var userId = context.GetCurrentUser().UserId;
                    var userName = context.GetCurrentUser().UserName;
                    var text = $"{userName}执行了科研队列:{taskName}";
                    service.AddProjectLog(userId, projectId, ActionType.AddTask, text);
                }
            }
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
                serviceResult.Data.ScheduleStatusName = serviceResult.Data.ScheduleStatus.GetDescription();// DomainConstraits.RenderIdToText(serviceResult.Data.ScheduleStatus,);
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
        [ProducesResponseType(typeof(FileResult), 0)]
        public FileResult Download(string path)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, path);
            if (!System.IO.File.Exists(fullPath))
            {
                return null;
            }
            FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            MemoryStream ms = new MemoryStream(data);
            var mediaType = path.EndsWith(".xls") || path.EndsWith(".xlsx") ? "application/msexcel" : "";
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
