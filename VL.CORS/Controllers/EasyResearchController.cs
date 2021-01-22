using Autobots.Infrastracture.Common.ControllerSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ResearchAPI.CORS.Controllers
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
    [VLAuthentication]
    [Route("api/[controller]/[action]")]
    public class EasyResearchController : APIBaseController
    {
        #region 通用,Dropdown

        /// <summary>
        /// 下拉项
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<VLKeyValue<string, string>>> GetDropdowns([FromServices] AccountService accountService, [FromServices] ReportTaskService reportTaskService, [FromBody] GetDropdownsRequest request)
        {
            List<VLKeyValue<string, string>> values = new List<VLKeyValue<string, string>>();
            switch (request.Type)
            {
                case "SystemRole":
                    var result1624 = accountService.GetAllRolesIdAndName();
                    foreach (var item in result1624.Data)
                    {
                        values.Add(new VLKeyValue<string, string>(item.Value, item.Key.ToString()));
                    }
                    return Success(values);
                case "SystemAuthority":
                    var authorities = typeof(SystemAuthority).GetAllEnums<SystemAuthority>();
                    values.AddRange(authorities.Select(c => new VLKeyValue<string, string>(c.ToString(), ((int)c).ToString())));
                    return Success(values);
                case "BusinessType"://业务类别
                    values.AddRange(DomainConstraits.BusinessTypes.Select(c => new VLKeyValue<string, string>(c.Name, c.Id.ToString())));
                    return Success(values);
                case "Member"://项目成员
                    var result = reportTaskService.GetAllUsersIdAndName();
                    foreach (var user in result.Data)
                    {
                        values.Add(new VLKeyValue<string, string>(user.Value, user.Key.ToString()));
                    }
                    return Success(values);
                case "LabOrder":
                    values.AddRange(DomainConstraits.LabOrders.Select(c => new VLKeyValue<string, string>(c.Value, c.Key)));
                    return Success(values);
                default:
                    values = ConfigHelper.GetJsonConfig<string, string>(request.Type);
                    return Success(values);
            }
        }

        /// <summary>
        /// 下拉项
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<VLKeyValue<string, string, string, string>>> GetTreeDropdowns([FromServices] AccountService accountService, [FromBody] GetDropdownsRequest request)
        {
            return Success(GetTreeDropdowns(request.Type));
        }

        static internal List<VLKeyValue<string, string, string, string>> GetTreeDropdowns(string type)
        {
            List<VLKeyValue<string, string, string, string>> values = new List<VLKeyValue<string, string, string, string>>();
            switch (type)
            {
                case "SystemAuthority":
                    var authorities = typeof(SystemAuthority).GetAllEnums<SystemAuthority>();
                    var kvs = authorities.Select(c => new VLKeyValue<string, string>(c.ToString(), ((int)c).ToString())).ToList();
                    var pkvs = new List<VLKeyValue<string, string>>(){
                        new VLKeyValue<string, string>( "项目管理", "101" ),
                        new VLKeyValue<string, string>( "用户角色管理", "999" ),
                    };
                    values.AddRange(kvs.Select(c =>
                    {
                        var parent = pkvs.FirstOrDefault(p => c.Value.StartsWith(p.Value));
                        return new VLKeyValue<string, string, string, string>(parent.Key, parent.Value, c.Key, c.Value);
                    }));
                    values.AddRange(pkvs.Select(c =>
                    {
                        return new VLKeyValue<string, string, string, string>("", "", c.Key, c.Value);
                    }));
                    return values;
                default:
                    throw new NotImplementedException("未实现");
            }
        }

        /// <summary>
        /// 下拉项
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<List<VLKeyValue<string, string>>> GetDropdownsWithParent([FromServices] ReportTaskService service, [FromBody] GetDropdownsWithParentRequest request)
        {
            var type = request.Type;
            var parentId = request.ParentId;
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
                case "TemplateProperty":
                    switch (parentId)
                    {
                        case "202012221031": //Template_孕周检验.xml
                        case "202001221034": //Template_孕周检验.xml
                            var template = DomainConstraits.Templates.FirstOrDefault(c => c.Id == parentId.ToLong());
                            if (template==null)
                            {
                                throw new NotImplementedException("模板不存在");
                            }
                            var properties = template.BusinessEntity.Properties
                                .Select(c => new VLKeyValue<string, string>() { Key = c.DisplayName, Value = c.Id.ToString() })
                                .ToList();
                            return Success(properties);
                        default:
                            throw new NotImplementedException("未支持该parentId");
                    }
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
        [EnableCors("AllCors")]
        [VLAuthorize(SystemAuthority.查看项目列表)]
        public APIResult<VLPagerResult<List<Dictionary<string, object>>>> GetPagedProjects([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] GetCommonSelectRequest request)
        {
            var currentUser = context.GetCurrentUser();
            if (request.search == null)
            {
                request.search = new List<VLKeyValue>();
            }
            request.search.Add(new VLKeyValue("UserId", currentUser.UserId.ToString()));
            var result = service.GetPagedResultBySQLConfig(request);
            return new APIResult<VLPagerResult<List<Dictionary<string, object>>>>(result);
        }

        /// <summary>
        /// 1.1.3.获取项目详情
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]

        public APIResult<GetProjectModel> GetProject([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] SimpleProjectRequest request)
        {
            var currentUser = context.GetCurrentUser();
            var result = service.GetProject(request.ProjectId, currentUser.UserId);
            return new APIResult<GetProjectModel>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        public class SimpleProjectRequest
        {
            /// <summary>
            /// 项目Id
            /// </summary>
            public long ProjectId { set; get; }
        }

        /// <summary>
        /// 1.1.4.删除项目
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> DeleteProject([FromServices] ReportTaskService service, [FromBody] SimpleProjectRequest request)
        {
            var result = service.DeleteProject(request.ProjectId);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.2.1.新建项目
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<long> CreateProject([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] CreateProjectRequest request)
        {
            var userId = context.GetCurrentUser().UserId;
            var result = service.CreateProject(request, userId);
            if (result.IsSuccess)
            {
                var projectId = result.Data;
                //新建的项目自动收藏
                service.AddFavoriteProject(projectId, userId);

                //{用户}设置了项目名称{项目名称}，
                var userName = context.GetCurrentUser().UserName;
                var text = $"{userName}设置了项目名称:{request.ProjectName}";
                service.AddProjectLog(userId, projectId, ActionType.EditProjectName, text);

                //{用户}添加了项目管理员{用户名}，
                var adminIds = request.AdminIds;
                var adminNames = DomainConstraits.RenderIdToText(adminIds, DomainConstraits.Users);
                if (adminNames != null && adminNames.Count > 0)
                {
                    text = $"{userName}添加了项目管理员:{string.Join(",", adminNames)}";
                    service.AddProjectLog(userId, projectId, ActionType.AddProjectManager, text);
                }

                //{用户}添加了项目成员{用户名}，
                var memberIds = request.MemberIds;
                var memberNames = DomainConstraits.RenderIdToText(memberIds, DomainConstraits.Users);
                if (memberNames != null && memberNames.Count > 0)
                {
                    text = $"{userName}添加了项目成员:{string.Join(",", memberNames)}";
                    service.AddProjectLog(userId, projectId, ActionType.AddProjectMember, text);
                }

                //{用户}添加了关联科室{科室名称}，
                var departmentIds = request.DepartmentIds;
                var departmentNames = DomainConstraits.RenderIdToText(departmentIds, DomainConstraits.Departments);
                if (departmentNames!=null && departmentNames.Count>0)
                {
                    text = $"{userName}添加了关联科室:{string.Join(",", departmentNames)}";
                    service.AddProjectLog(userId, projectId, ActionType.AddProjectDepartment, text);
                }

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
        public APIResult<bool> AddFavoriteProject([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] SimpleProjectRequest request)
        {
            var userId = context.GetCurrentUser().UserId;
            var result = service.AddFavoriteProject(request.ProjectId, userId);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.2.6.解除收藏项目
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> DeleteFavoriteProject([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] SimpleProjectRequest request)
        {
            var userId = context.GetCurrentUser().UserId;
            var result = service.DeleteFavoriteProject(request.ProjectId, userId);
            return new APIResult<bool>(result);
        }

        /// <summary>
        /// 1.3.2.编辑项目
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> EditProject([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] EditProjectRequest request)
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
        public APIResult<List<GetProjectOperateHistoryModel>> GetProjectOperateHistory([FromServices] ReportTaskService service, [FromBody] GetProjectOperateHistoryRequest request)
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
        public APIResult<GetBriefProjectModel> GetBriefProject([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] GetBriefProjectRequest request)
        {
            var currentUser = context.GetCurrentUser();
            var serviceResult = service.GetProject(request.ProjectId, currentUser.UserId);
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
        public APIResult<List<GetProjectIndicatorModel>> GetProjectIndicators([FromServices] ReportTaskService service, [FromBody] SimpleProjectRequest request)
        {
            var result = service.GetProjectIndicators(request.ProjectId);
            return new APIResult<List<GetProjectIndicatorModel>>(result);
        }

        /// <summary>
        /// 1.4.5.删除指标(批量)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<int> DeleteProjectIndicators([FromServices] ReportTaskService service, [FromBody] DeleteProjectIndicatorsRequest request)
        {
            var result = service.DeleteProjectIndicators(request.IndicatorIds);
            return new APIResult<int>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        public class DeleteProjectIndicatorsRequest
        {
            /// <summary>
            /// 项目Id
            /// </summary>
            public List<long> IndicatorIds { set; get; }
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
                case TargetArea.PregnantLabResult:
                    var template = DomainConstraits.Templates.First(c => c.BusinessEntity.Id == 202012221031);
                    var result = service.CreateCustomIndicator(request, template);
                    return new APIResult<List<BusinessEntityPropertyModel>>(result);
                case TargetArea.PregnantVisitRecord:
                    template = DomainConstraits.Templates.First(c => c.BusinessEntity.Id == 202001221034);
                    result = service.CreateCustomIndicator(request, template);
                    return new APIResult<List<BusinessEntityPropertyModel>>(result);
                case TargetArea.Child:
                    break;
                default:
                    break;
            }
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
        /// 01/22 编辑队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> EditTaskV2([FromServices] ReportTaskService service, [FromBody] EditTaskV2Request request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public class EditTaskV2Request
        {
            /// <summary>
            /// 
            /// </summary>
            public long TaskId { set; get; }
            /// <summary>
            /// 
            /// </summary>
            public List<EditTaskWhereModel> Wheres { set; get; }
        }

        /// <summary>
        /// 组合条件
        /// </summary>
        public class GroupedConditions
        {
            /// <summary>
            /// And Or
            /// </summary>
            public bool IsAnd { set; get; }

            /// <summary>
            /// 条件项目
            /// </summary>
            public List<EditTaskWhereModel> Conditions { set; get; }
        }

        /// <summary>
        /// 1.5.3.删除队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<bool> DeleteTask([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] SimpleTaskRequest request)
        {
            var task = service.GetTaskById(request.TaskId);
            if (!task.IsSuccess)
                return new APIResult<bool>(false, task.Messages);
            var serviceResult = service.DeleteTask(request.TaskId);
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
        /// 
        /// </summary>
        public class SimpleTaskRequest
        {
            /// <summary>
            /// 项目Id
            /// </summary>
            public long TaskId { set; get; }
        }

        /// <summary>
        /// 1.5.4.执行队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<long> CommitTask([FromServices] APIContext context, [FromServices] ReportTaskService service, [FromBody] CommitTaskRequest request)
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
                else
                {
                    return new APIResult<long>(serviceResult.Data, result.Messages);
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
        public APIResult<List<GetTaskModel>> GetTasks([FromServices] ReportTaskService service, [FromBody] SimpleProjectRequest request)
        {
            var serviceResult = service.GetTasks(request.ProjectId);
            return new APIResult<List<GetTaskModel>>(serviceResult);
        }

        /// <summary>
        /// 1.5.6.查看队列状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllCors")]
        public APIResult<GetTaskStatusModel> GetTaskStatus([FromServices] ReportTaskService service, [FromBody] SimpleTaskRequest request)
        {
            var serviceResult = service.GetTaskStatus(request.TaskId);
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
