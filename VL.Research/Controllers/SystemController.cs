using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ControllerSolution;
using VL.Consolo_Core.Common.ExcelExportSolution;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.Research.Common;
using VL.Research.Models;
using VL.Research.Services;

namespace VL.Research.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SystemController : APIBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        public SystemController()
        {
        }

        #region 下拉项,JsonConfig

        static Dictionary<string, string> JsonConfigs = new Dictionary<string, string>();

        /// <summary>
        /// 获取 下拉项        
        /// </summary>
        /// <param name="type">类型名称</param>
        /// <param name="isForceChange">是否强制获取最新</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public APIResult<List<DropDownItem>> GetDropDowns(string type, bool isForceChange)
        {
            var file = (Path.Combine(AppContext.BaseDirectory, "JsonConfigs", type + ".json"));
            if (!System.IO.File.Exists(file))
            {
                List<DropDownItem> values = new List<DropDownItem>()
                {
                    new DropDownItem("请联系管理员配置","请联系管理员配置"),
                };
                System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(values));
                return Success(values);
            }
            var data = System.IO.File.ReadAllText(file);
            var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DropDownItem>>(data);
            return Success(entity);
        }

        #endregion

        #region 菜单栏,MenuConfig
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MenuConfig GetMenuConfig()
        {
            MenuConfig tableConfig;
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "MenuConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(MenuConfig.RootElementName);
            var tableConfigs = tableElements.Select(c => new MenuConfig(c));
            tableConfig = tableConfigs.FirstOrDefault();
            return tableConfig;
        }
        /// <summary>
        /// 获取 菜单栏(当前用户)
        /// </summary>
        /// <param name="userService"></param>
        /// <returns></returns>
        [HttpGet]
        public APIResult<List<UserMenu>> GetListMenu([FromServices] UserService userService)
        {
            var userId = GetCurrentUser().UserId;
            var serviceResult = userService.GetUserMenus(userId);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Success(serviceResult.Data);
        }

        /// <summary>
        /// 获取 菜单栏(当前用户)
        /// </summary>
        /// <param name="userService"></param>
        /// <returns></returns>
        [HttpGet]
        public APIResult<List<MenuItem>> GetListMenu_LayUI([FromServices] UserService userService)
        {
            MenuConfig menuConfig = SystemController.GetMenuConfig();
            return Success(menuConfig.MenuItems);
        }
        #endregion

        #region 个性化,ListConfig

        ///// <summary>
        ///// 获取 列表配置
        ///// </summary>
        ///// <param name="userService"></param>
        ///// <param name="request">请求参数实体</param>
        ///// <returns></returns>
        //[HttpPost]
        //[AllowAnonymous]
        //public APIResult<GetListConfigModel> GetListConfig([FromServices] UserService userService, GetListConfigRequest request)
        //{
        //    if (request.CustomConfigId > 0)
        //    {
        //        var serviceResult = userService.GetUserMenuById(request.CustomConfigId);
        //        if (!serviceResult.IsSuccess)
        //        {
        //            return Error(data: new GetListConfigModel(), "无效的用户配置");
        //        }
        //        var ListConfig = serviceResult.Data.ListConfig.FromJson<ListConfig>();
        //        var result = new GetListConfigModel()
        //        {
        //            CustomConfigId = request.CustomConfigId,
        //            //ListConfig = ListConfig,
        //        };
        //        return Success(result);
        //    }
        //    else
        //    {
        //        return Success(LoadDefaultConfig(request));
        //    }
        //}

        ///// <summary>
        ///// 获取 列表配置
        ///// </summary>
        ///// <param name="userService"></param>
        ///// <param name="request">请求参数实体</param>
        ///// <returns></returns>
        //[HttpPost]
        //public APIResult<GetListConfigModel> GetListConfig_LayUI([FromServices] UserService userService, GetListConfigRequest request)
        //{
        //    if (request.CustomConfigId > 0)
        //    {
        //        var serviceResult = userService.GetUserMenuById(request.CustomConfigId);
        //        if (!serviceResult.IsSuccess)
        //        {
        //            return Error(data: new GetListConfigModel(), "无效的用户配置");
        //        }
        //        var ListConfig = serviceResult.Data.ListConfig.FromJson<ListConfig>();
        //        var result = new GetListConfigModel()
        //        {
        //            CustomConfigId = request.CustomConfigId,
        //            ListConfig = ListConfig,
        //        };
        //        return Success(result);
        //    }
        //    else
        //    {
        //        return Success(LoadDefaultConfig(request));
        //    }
        //}

        //private GetListConfigModel LoadDefaultConfig(GetListConfigRequest request)
        //{
        //    if (request.ViewName.IsNullOrEmpty())
        //        return null;
        //    var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListConfig.xml");
        //    XDocument doc = XDocument.Load(path);
        //    var viewElements = doc.Descendants(ListConfig.NodeElementName);
        //    var listConfigs = viewElements.Select(c => new ListConfig(c));
        //    var listConfig = listConfigs.FirstOrDefault(c => c.ViewName == request.ViewName);
        //    listConfig.Properties.RemoveAll(c => !c.IsNeedOnPage);
        //    var result = new GetListConfigModel()
        //    {
        //        CustomConfigId = request.CustomConfigId,
        //        ListConfig = listConfig,
        //        search = listConfig.Wheres.Select(c => new GetListConfigModel_Search(c)).ToList(),
        //        table = new GetListConfigModel_TableConfg()
        //        {
        //            url = listConfig.ViewURL,
        //            add_btn = new GetListConfigModel_TableConfg_AddButton()
        //            {
        //                text = listConfig.AddButton.text,
        //                type = listConfig.AddButton.type,
        //                url = listConfig.AddButton.url,
        //                area = listConfig.AddButton.area,
        //                defaultParam = listConfig.AddButton.defaultParam
        //            },
        //            line_toolbar = listConfig.ToolBars.Select(c => new GetListConfigModel_TableConfg_ToolBar()
        //            {
        //                text = c.Text,
        //                type = c.Type,
        //                desc = c.Description,
        //                url = c.URL,
        //                @params = c.Params,
        //                area = c.Area,
        //                yesFun = c.YesFun,
        //                defaultParam = c.DefaultParams,
        //            }).ToList(),
        //            toolbar_viewModel = new GetListConfigModel_TableConfg_ViewModel(),
        //            page = true,
        //            limit = 20,
        //            initSort = new GetListConfigModel_TableConfg_InitSort(),
        //            cols = new List<List<GetListConfigModel_TableConfg_Col>>()
        //            {
        //                listConfig.Properties.Select(c => new GetListConfigModel_TableConfg_Col()
        //                {
        //                    field = c.ColumnName,
        //                    title = c.DisplayName,
        //                    align = "center",
        //                    templet ="",
        //                    width= c.DisplayWidth,
        //                    @fixed="",
        //                    sort= c.IsSortable,
        //                    colspan="",
        //                    rowspan="",
        //                }).ToList()
        //            },
        //            where = new List<VLNameValue>()
        //            {
        //                new VLNameValue(){
        //                    name = "target",
        //                    value = request.ViewName,
        //                }
        //            },
        //        },
        //        export = new GetListConfigModel_Export()
        //        {
        //            url = listConfig.Export.URL,
        //            defaultParam = new List<Consolo_Core.Common.ValuesSolution.VLKeyValue>()
        //            {
        //                new Consolo_Core.Common.ValuesSolution.VLKeyValue("target",request.ViewName)
        //            },
        //        },
        //    };
        //    return result;
        //}

        /// <summary>
        /// 保存 列表配置
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="request">请求参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public APIResult<long> SaveListConfig([FromServices] UserService userService, SaveListConfigRequest request)
        {
            var userMenu = new UserMenu()
            {
                Id = request.CustomConfigId,
                UserId = GetCurrentUser().UserId,
                MenuName = request.ListName,
                URL = request.URL,
                ListConfig = request.ListConfig.ToJson(),
            };
            if (request.CustomConfigId > 0)
            {
                var serviceResult = userService.UpdateUserMenu(userMenu);
                if (!serviceResult.IsSuccess)
                    return Error(serviceResult.Data ? 1L : 0L, serviceResult.Messages);
                return Success(serviceResult.Data ? 1L : 0L);
            }
            else
            {
                var serviceResult = userService.CreateUserMenu(userMenu);
                if (!serviceResult.IsSuccess)
                    return Error(serviceResult.Data, serviceResult.Messages);
                return Success(serviceResult.Data);
            }
        }

        #endregion

        #region 详情页,DetailConfig

        /// <summary>
        /// 请求参数实体
        /// </summary>
        public class GetDetailConfigRequest
        {
            /// <summary>
            /// 列表名称
            /// </summary>
            public string ViewName { set; get; }
            /// <summary>
            /// 自定义配置Id
            /// </summary>
            public long CustomConfigId { set; get; }
        }

        /// <summary>
        /// 获取 列表配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public APIResult<DetailConfig> GetDetailConfig([FromServices] APIContext apiContext, [FromServices] UserService userService, string viewName)
        {
            return Success(LoadDefaultDetailConfig(viewName));
        }

        private DetailConfig LoadDefaultDetailConfig(string viewName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "DetailConfig.xml");
            XDocument doc = XDocument.Load(path);
            var viewElements = doc.Descendants(DetailConfig.NodeElementName);
            var detailConfigs = viewElements.Select(c => new DetailConfig(c));
            var detailConfig = detailConfigs.FirstOrDefault(c => c.ViewName == viewName);
            return detailConfig;
        }

        #endregion

        #region 导出,CommonExport
        /// <summary>
        /// 通用导出
        /// </summary>
        [HttpPost]
        //[AllowAnonymous]
        public APIResult<string> CommonExportAll([FromServices] APIContext apiContext, [FromServices] SharedService sharedService, [FromBody] GetCommonSelectRequest request)
        {
            return commonExport(apiContext, sharedService, request);
        }

        private APIResult<string> commonExport(APIContext apiContext, SharedService sharedService, GetCommonSelectRequest request)
        {
            var target = request.search.FirstOrDefault(c => c.Key == "target").Value;
            var search = request.search;
            var path = Path.Combine(AppContext.BaseDirectory, @"XMLConfig", target, "ExportConfig_列表.xml");

            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(ExportConfig.NodeElementName);
            var configs = tableElements.Select(c => new ExportConfig(c));
            var config = configs.FirstOrDefault();
            if (config == null)
            {
                throw new NotImplementedException("无效的导出配置");
            }
            var modelPath = Path.Combine(AppContext.BaseDirectory, @"XMLConfig", target, config.FileName);
            var filename = DateTime.Now.ToString("yyyyMMdd_HHmmss") + config.FileName;
            var outputPath = Path.Combine(AppContext.BaseDirectory, @"XMLConfig", target, filename);
            if (!System.IO.File.Exists(modelPath))
            {
                throw new NotImplementedException("模板文件不存在");
            }
            using (FileStream s = System.IO.File.OpenRead(modelPath))
            {
                var workbook = new XSSFWorkbook(s);
                foreach (var sheetConfig in config.Sheets)
                {
                    sheetConfig.UpdateWheres(search);
                    var sheet = workbook.GetSheet(sheetConfig.SheetName);
                    if (sheet != null)
                    {
                        sheetConfig.DataSources = new Dictionary<string, DataTable>();
                        //获取数据
                        foreach (var sourceConfig in sheetConfig.Sources)
                        {
                            DBSourceType sourceType = sourceConfig.DBSourceType.ToEnum<DBSourceType>();
                            var result = sharedService.GetCommonSelectByExportConfig(sourceConfig, sourceType);
                            if (!result.IsSuccess)
                            {
                                throw new NotImplementedException("数据源存在异常:" + result.Message);
                            }
                            var data = result.Data;
                            sheetConfig.DataSources[sourceConfig.SourceName] = data;
                        }
                        //Transform
                        foreach (var sourceConfig in sheetConfig.Sources)
                        {
                            var data = sheetConfig.DataSources[sourceConfig.SourceName];
                            sourceConfig.DoTransforms(ref data);
                            sheetConfig.DataSources[sourceConfig.SourceName] = data;
                        }
                        //Mapping
                        foreach (var sourceConfig in sheetConfig.Sources)
                        {
                            var data = sheetConfig.DataSources[sourceConfig.SourceName];
                            sourceConfig.DoMappings(ref data, sheetConfig.DataSources);
                            sheetConfig.DataSources[sourceConfig.SourceName] = data;
                        }
                        //Render
                        sheetConfig.Render(sheet);
                    }
                }
                using (Stream stream = System.IO.File.OpenWrite(outputPath))
                {
                    workbook.Write(stream);
                }
            }
            var webPath = $@"{apiContext.GetWebPath()}/Home/CommonExportAll?outputPath={outputPath}";
            return Success(webPath);
        }

        #endregion

        #region 列表,CommonSelect

        #region 视图配置
        /// <summary>
        /// 获取 列表配置
        /// </summary>
        [HttpPost]
        public APIResult<GetListConfigModel> GetListConfigByDirectoryName([FromServices]APIContext apiContext, [FromServices] UserService userService, GetListConfigRequest request)
        {
            if (request.ViewName.IsNullOrEmpty())
                return null;
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", request.ViewName, "ListConfig.xml");
            XDocument doc = XDocument.Load(path);
            var viewElements = doc.Descendants(ListConfig.NodeElementName);
            var listConfigs = viewElements.Select(c => new ListConfig(c));
            var listConfig = listConfigs.FirstOrDefault();
            GetListConfigModel result = ToGetListConfigModel(apiContext, request, listConfig);

            return Success(result);
        }

        /// <summary>
        /// 获取 列表配置
        /// </summary>
        [HttpPost]
        public APIResult<GetListConfigModel> GetListConfigByViewName([FromServices] APIContext apiContext, [FromServices] UserService userService, GetListConfigRequest request)
        {
            if (request.ViewName.IsNullOrEmpty())
                return null;
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListConfig.xml");
            XDocument doc = XDocument.Load(path);
            var viewElements = doc.Descendants(ListConfig.NodeElementName);
            var listConfigs = viewElements.Select(c => new ListConfig(c));
            var listConfig = listConfigs.FirstOrDefault(c => c.ViewName == request.ViewName);
            GetListConfigModel result = ToGetListConfigModel(apiContext,request, listConfig);
            return Success(result);
        }

        private static GetListConfigModel ToGetListConfigModel(APIContext apiContext, GetListConfigRequest request, ListConfig listConfig)
        {
            listConfig.Properties.RemoveAll(c => !c.IsNeedOnPage);
            var defaultParams = GetNameValue(request.DefaultParams);
            foreach (var defaultParam in defaultParams)
            {
                var where = listConfig.Wheres.FirstOrDefault(c => c.ComponentName == defaultParam.name);
                if (where != null)
                    where.Value = defaultParam.value;
            }
            var model = new GetListConfigModel()
            {
                CustomConfigId = request.CustomConfigId,
                ListConfig = listConfig,
                search = listConfig.Wheres.Select(c => new GetListConfigModel_Search(c)).ToList(),
                table = new GetListConfigModel_TableConfg()
                {
                    url = listConfig.ViewURL,
                    add_btn = listConfig.AddButton.url.IsNullOrEmpty() ? null : new GetListConfigModel_TableConfg_AddButton()
                    {
                        text = listConfig.AddButton.text,
                        type = listConfig.AddButton.type,
                        url = listConfig.AddButton.url,
                        area = listConfig.AddButton.area,
                        defaultParam = listConfig.AddButton.defaultParam
                    },
                    line_toolbar = listConfig.ToolBars.Select(c => new GetListConfigModel_TableConfg_ToolBar()
                    {
                        text = c.Text,
                        type = c.Type,
                        desc = c.Description,
                        url = c.URL,
                        @params = c.Params,
                        area = c.Area,
                        yesFun = c.YesFun,
                        defaultParam = c.DefaultParams,
                    }).ToList(),
                    toolbar_viewModel = new GetListConfigModel_TableConfg_ViewModel(),
                    page = true,
                    limit = 20,
                    initSort = new GetListConfigModel_TableConfg_InitSort(),
                    cols = new List<List<GetListConfigModel_TableConfg_Col>>()
                    {
                        listConfig.Properties.Select(c => new GetListConfigModel_TableConfg_Col()
                        {
                            field = c.ColumnName,
                            title = c.DisplayName,
                            align = "center",
                            templet ="",
                            width= c.DisplayWidth,
                            @fixed="",
                            sort= c.IsSortable,
                            colspan="",
                            rowspan="",
                        }).ToList()
                    },
                    where = new List<VLNameValue>()
                    {
                        new VLNameValue(){
                            name = "target",
                            value = request.ViewName,
                        }
                    },
                },
                export = new GetListConfigModel_Export()
                {
                    url = listConfig.Export.URL,
                    defaultParam = new List<VLKeyValue>()
                    {
                        new Consolo_Core.Common.ValuesSolution.VLKeyValue( "target",request.ViewName)
                    },
                },
            };
            var hiddenParams = GetNameValue(request.HiddenParams);
            model.table.where.AddRange(hiddenParams);
            return model;
        }

        /// <summary>
        /// A_111|B_222
        /// </summary>
        /// <param name="defaultParams"></param>
        /// <returns></returns>
        static List<VLNameValue> GetNameValue(string defaultParams)
        {
            List<VLNameValue> result = new List<VLNameValue>();
            if (defaultParams.IsNullOrEmpty())
                return result;
            var pairs = defaultParams.Split("|");
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split("_");
                if (keyValue.Count() == 2)
                {
                    result.Add(new VLNameValue(keyValue[0], keyValue[1]));
                }
            }
            return result;
        }

        #endregion

        #region 数据源

        /// <summary>
        /// 获取 通用分页列表
        /// </summary>
        [HttpPost]
        //[VLActionFilter(Authority.查看孕妇档案列表)]
        public APIResult<List<Dictionary<string, object>>, int> GetCommonSelectByViewName([FromServices] SharedService sharedService, GetCommonSelectRequest request)
        {
            var viewName = request.search.First(c => c.Key.ToLower() == "viewname").Value;
            var listConfig = ConfigHelper.GetListConfigByTagName(viewName);
            var sqlConfig = ConfigHelper.GetSQLConfigByTagName(viewName);
            sqlConfig.PageIndex = request.page;
            sqlConfig.PageSize = request.limit;
            sqlConfig.UpdateWheres(request.search);
            sqlConfig.UpdateOrderBy(request.field, request.order);
            return commonSelect(sharedService, request, listConfig, sqlConfig);
        }

        /// <summary>
        /// 获取 通用查询 分页列表
        /// </summary>
        [HttpPost]
        //[VLActionFilter(Authority.查看孕妇档案列表)]
        public APIResult<List<Dictionary<string, object>>, int> GetCommonSelectByDirectoryName([FromServices] SharedService sharedService, GetCommonSelectRequest request)
        {
            var viewName = request.search.First(c => c.Key.ToLower() == "viewname").Value;
            var listConfig = ConfigHelper.GetListConfigByDirectoryName(viewName);
            var sqlConfig = ConfigHelper.GetSQLConfigByDirectoryName(viewName);
            sqlConfig.PageIndex = request.page;
            sqlConfig.PageSize = request.limit;
            sqlConfig.UpdateWheres(request.search);
            sqlConfig.UpdateOrderBy(request.field, request.order);
            return commonSelect(sharedService, request, listConfig, sqlConfig);
        }

        private APIResult<List<Dictionary<string, object>>, int> commonSelect(SharedService sharedService, GetCommonSelectRequest request, ListConfig listConfig, SQLConfig sqlConfig)
        {
            //获取数据
            var serviceResult = sharedService.GetCommonSelectBySQLConfig(sqlConfig);
            if (!serviceResult.IsSuccess)
                return Error<List<Dictionary<string, object>>, int>(null, 0, messages: serviceResult.Messages);
            //更新显示映射(枚举,函数,脱敏)
            listConfig.UpdateValues(serviceResult.Data.SourceData);
            return Success(serviceResult.Data.SourceData, serviceResult.Data.Count, serviceResult.Messages);
        }
        #endregion

        #endregion

        #region  CommonListForFYPT

        ///// <summary>
        ///// 获取 通用查询 分页列表
        ///// </summary>
        //[HttpPost]
        ////[VLAuthentication(Authority.查看孕妇档案列表)]
        ////[Authorize]
        //public APIResult<List<Dictionary<string, object>>, int> GetCommonSelectForFYPT([FromServices] APIContext apiContext, [FromServices] SharedService sharedService, GetCommonSelectRequest request)
        //{
        //    return commonSelect(sharedService, request);
        //}



        ///// <summary>
        ///// 获取 通用查询 分页列表
        ///// </summary>
        //[HttpPost]
        ////[VLAuthentication(Authority.查看孕妇档案列表)]
        ////[Authorize]
        //public APIResult<List<Dictionary<string, object>>, int> GetCommonSelectForSZXT([FromServices] APIContext apiContext, [FromServices] SharedService sharedService, GetCommonSelectRequest request)
        //{
        //    var dbSourceType = DBSourceType.SZXTConnectionString;
        //    return commonSelect(sharedService, request, dbSourceType);
        //}

        ///// <summary>
        ///// 通用导出
        ///// </summary>
        //[HttpPost]
        ////[AllowAnonymous]
        //public APIResult<string> CommonExportAllForSZXT([FromServices] APIContext apiContext, [FromServices] SharedService sharedService, [FromBody] GetCommonSelectRequest request)
        //{
        //    var sourceType = DBSourceType.SZXTConnectionString;
        //    return commonExport(apiContext, sharedService, request, sourceType);
        //}

        #endregion

        #region Test

        /// <summary>
        /// 
        /// </summary>
        public void Log4Net()
        {
            Log4NetLogger.Info("Message From Log4Net()");
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GlobalException()
        {
            throw new NotImplementedException("GlobalException");
            return;
        }

        #endregion
    }
}
