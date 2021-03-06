﻿using Autobots.Infrastracture.Common.ControllerSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchAPI.Common;
using ResearchAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResearchAPI.Controllers
{
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
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EasyResearchController : ControllerBase
    {
        #region 通用

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
        public APIResult<List<VLKeyValue<string,long>>> GetDropdowns(string type)
        {
            throw new NotImplementedException();
            //var file = (Path.Combine(AppContext.BaseDirectory, "JsonConfigs", type + ".json"));
            //if (!System.IO.File.Exists(file))
            //{
            //    List<DropDownItem> values = new List<DropDownItem>()
            //    {
            //        new DropDownItem("请联系管理员配置","请联系管理员配置"),
            //    };
            //    System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(values));
            //    return Success(values);
            //}
            //var data = System.IO.File.ReadAllText(file);
            //var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DropDownItem>>(data);
            //return Success(entity);
        }

        #endregion

        #region 项目

        /// <summary>
        /// 1.1.0.获取项目列表(菜单)
        /// </summary>
        /// <returns>Key:项目名称,Value:项目Id</returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<List<VLKeyValue<string, long>>> GetProjectsForMenu()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetPagedProjectsRequest : VLPagerRequest
        {
            /// <summary>
            /// 项目名称
            /// </summary>
            public string ProjectName { set; get; }        
            /// <summary>
            /// 科室Id
            /// </summary>
            public long DepartmentId { set; get; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public string CreateTime{ set; get; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetPagedProjectsModel
        {
            /// <summary>
            /// 项目Id
            /// </summary>
            public long ProjectId { set; get; }
            /// <summary>
            /// 项目名称
            /// </summary>
            public string ProjectName { set; get; }
            /// <summary>
            /// 关联科室
            /// </summary>
            public long DepartmentId { set; get; }
            /// <summary>
            /// 创建者
            /// </summary>
            public long CreatorId { set; get; }
            /// <summary>
            /// 创建日期
            /// </summary>
            public DateTime CreatedAt { set; get; }
            /// <summary>
            /// 最近更新时间
            /// </summary>
            public DateTime LastModifiedAt { set; get; }
        }

        /// <summary>
        /// 1.1.1.获取项目列表
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<VLPagerResult<GetPagedProjectsModel>> GetPagedProjects(GetPagedProjectsRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetProjectModel
        {
            /// <summary>
            /// 项目Id
            /// </summary>
            public long ProjectId { set; get; }
            /// <summary>
            /// 项目名称
            /// </summary>
            public string ProjectName { set; get; }
            /// <summary>
            /// 项目管理人员
            /// </summary>
            public List<long> AdminIds { set; get; }
            /// <summary>
            /// 项目成员
            /// </summary>
            public List<long> MemberIds { set; get; }
            /// <summary>
            /// 关联科室
            /// </summary>
            public long DepartmentId { set; get; }
            /// <summary>
            /// 项目查看权限
            /// </summary>
            public int ViewAuthorizeType { set; get; }
            /// <summary>
            /// 项目描述
            /// </summary>
            public string ProjectDescription { set; get; }
        }

        /// <summary>
        /// 1.1.3.获取项目详情
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<GetProjectModel> GetProject(int projectId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 1.1.4.删除项目
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<bool> DeleteProject(int projectId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 项目信息
        /// </summary>
        public class ProjectDTO
        {
            /// <summary>
            /// 项目名称
            /// </summary>
            public string ProjectName { set; get; }
            /// <summary>
            /// 项目管理人员
            /// </summary>
            public List<long> AdminIds { set; get; }
            /// <summary>
            /// 项目成员
            /// </summary>
            public List<long> MemberIds { set; get; }
            /// <summary>
            /// 关联科室
            /// </summary>
            public long DepartmentId { set; get; }
            /// <summary>
            /// 项目查看权限
            /// </summary>
            public int ViewAuthorizeType { set; get; }
            /// <summary>
            /// 项目描述
            /// </summary>
            public string ProjectDescription { set; get; }
        }

        /// <summary>
        /// 新建项目
        /// </summary>
        public class CreateProjectRequest: ProjectDTO
        {
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        public class EditProjectRequest: ProjectDTO
        {
            /// <summary>
            /// 项目Id
            /// </summary>
            public long ProjectId { set; get; }
        }

        /// <summary>
        /// 1.2.1.新建项目
        /// </summary>
        /// <returns>项目Id</returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<long> CreateProject(CreateProjectRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 1.2.5.收藏项目
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<bool> CollectProject(int projectId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 1.3.2.编辑项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<bool> EditProject(EditProjectRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetProjectOperateHistoryResponse
        {
            /// <summary>
            /// 操作时间
            /// </summary>
            public DateTime OperateAt { set; get; }
            /// <summary>
            /// 操作概要描述
            /// </summary>
            public string OperatorSummary { set; get; }
        }

        /// <summary>
        /// 1.3.4.获取操作记录 GetProjectOperateHistory
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<GetProjectOperateHistoryResponse> GetProjectOperateHistory(long projectId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetBiefProjectResponse
        {
            /// <summary>
            /// 创建日期
            /// </summary>
            public DateTime CreatedAt { set; get; }
            /// <summary>
            /// 最近更新时间
            /// </summary>
            public DateTime LastModifiedAt { set; get; }
            /// <summary>
            /// 创建者
            /// </summary>
            public long CreatorName { set; get; }
            /// <summary>
            /// 关联科室
            /// </summary>
            public long DepartmentId { set; get; }
            /// <summary>
            /// 项目管理人员
            /// </summary>
            public List<long> AdminIds { set; get; }
            /// <summary>
            /// 项目成员
            /// </summary>
            public List<long> MemberIds { set; get; }
            /// <summary>
            /// 项目描述
            /// </summary>
            public string ProjectDescription { set; get; }
        }

        /// <summary>
        /// 1.4.1.获取项目概要信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<GetProjectOperateHistoryResponse> GetBiefProject(long projectId)
        {
            throw new NotImplementedException();
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
        public APIResult<List<GetIndicatorsResponse>> GetIndicators(long projectId)
        {
            throw new NotImplementedException();
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
