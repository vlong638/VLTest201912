using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ControllerSolution;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.Research.Common;
using VL.Research.Models;
using VL.Research.Services;

namespace VL.Research.Controllers
{
    /// <summary>
    /// 
    /// </summary>
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

        #region JsonConfig

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

        #region XMLConfig

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static ViewConfig GetViewConfigByName(string viewName)
        {
            ViewConfig tableConfig;
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ViewConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(ViewConfig.NodeElementName);
            var tableConfigs = tableElements.Select(c => new ViewConfig(c));
            tableConfig = tableConfigs.FirstOrDefault(c => c.ViewName == viewName);
            return tableConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static SQLConfig GetSQLConfigByName(string viewName)
        {
            SQLConfig tableConfig;
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "SQLConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(SQLConfig.NodeElementName);
            var tableConfigs = tableElements.Select(c => new SQLConfig(c));
            tableConfig = tableConfigs.FirstOrDefault(c => c.ViewName == viewName);
            return tableConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static MenuConfig GetMenuConfigByName(string viewName)
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
        /// 获取 列表配置
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="request">请求参数实体</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public APIResult<GetListConfigModel> GetListConfig([FromServices] UserService userService, GetListConfigRequest request)
        {
            if (request.CustomConfigId > 0)
            {
                var serviceResult = userService.GetUserMenuById(request.CustomConfigId);
                if (!serviceResult.IsSuccess)
                {
                    return Error(data: new GetListConfigModel(), "无效的用户配置");
                }
                var viewConfig = serviceResult.PagedData.ViewConfig.FromJson<ViewConfig>();
                var result = new GetListConfigModel()
                {
                    CustomConfigId = request.CustomConfigId,
                    //ViewConfig = viewConfig,
                };
                return Success(result);
            }
            else
            {
                return Success(LoadDefaultConfig(request));
            }
        }

        /// <summary>
        /// 获取 列表配置
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="request">请求参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public APIResult<GetListConfigModel> GetListConfig_LayUI([FromServices] UserService userService, GetListConfigRequest request)
        {
            if (request.CustomConfigId > 0)
            {
                var serviceResult = userService.GetUserMenuById(request.CustomConfigId);
                if (!serviceResult.IsSuccess)
                {
                    return Error(data: new GetListConfigModel(), "无效的用户配置");
                }
                var viewConfig = serviceResult.PagedData.ViewConfig.FromJson<ViewConfig>();
                var result = new GetListConfigModel()
                {
                    CustomConfigId = request.CustomConfigId,
                    ViewConfig = viewConfig,
                };
                return Success(result);
            }
            else
            {
                return Success(LoadDefaultConfig(request));
            }
        }

        private GetListConfigModel LoadDefaultConfig(GetListConfigRequest request)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ViewConfig.xml");
            XDocument doc = XDocument.Load(path);
            var viewElements = doc.Descendants(ViewConfig.NodeElementName);
            var viewConfigs = viewElements.Select(c => new ViewConfig(c));
            var viewConfig = viewConfigs.FirstOrDefault(c => c.ViewName == request.ListName);
            viewConfig.Properties.RemoveAll(c => !c.IsNeedOnPage);
            var result = new GetListConfigModel()
            {
                CustomConfigId = request.CustomConfigId,
                ViewConfig = viewConfig,
                search = viewConfig.Wheres.Select(c => new GetListConfigModel_Search()
                {
                    name = c.ComponentName,
                    text = c.DisplayName,
                    type = c.DisplayType.ToInt().Value,
                    value = c.DisplayValues ?? "",
                    options = new List<GetListConfigModel_Search_Option>(),
                }).ToList(),
                table = new GetListConfigModel_TableConfg()
                {
                    url = viewConfig.ViewURL,
                    add_btn = new GetListConfigModel_TableConfg_AddButton()
                    {
                        text = "新增",
                        type = "newPage",
                        url = "",//新增提交的页面
                        defaultParam = new List<string>(),
                    },
                    line_toolbar = new List<GetListConfigModel_TableConfg_ToolBar>(),
                    toolbar_viewModel = new GetListConfigModel_TableConfg_ViewModel(),
                    page = true,
                    limit = 20,
                    initSort = new GetListConfigModel_TableConfg_InitSort(),
                    cols = new List<List<GetListConfigModel_TableConfg_Col>>()
                    {
                        viewConfig.Properties.Select(c => new GetListConfigModel_TableConfg_Col()
                        {
                            field = c.ColumnName,
                            title = c.DisplayName,
                            align = "center",
                            templet ="",
                            width=c.DisplayWidth,
                            @fixed="",
                            sort=c.IsSortable,
                            colspan="",
                            rowspan="",
                        }).ToList()
                    },
                    where = new List<GetListConfigModel_TableConfg_Where>(),
                },
            };
            return result;
        }

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
                ViewConfig = request.ViewConfig.ToJson(),
            };
            if (request.CustomConfigId > 0)
            {
                var serviceResult = userService.UpdateUserMenu(userMenu);
                if (!serviceResult.IsSuccess)
                    return Error(serviceResult.PagedData ? 1L : 0L, serviceResult.Messages);
                return Success(serviceResult.PagedData ? 1L : 0L);
            }
            else
            {
                var serviceResult = userService.CreateUserMenu(userMenu);
                if (!serviceResult.IsSuccess)
                    return Error(serviceResult.PagedData, serviceResult.Messages);
                return Success(serviceResult.PagedData);
            }
        }

        /// <summary>
        /// 获取 列表配置(当前用户)
        /// </summary>
        /// <param name="userService"></param>
        /// <returns></returns>
        [HttpGet]
        public APIResult<List<UserMenu>> GetListMenu([FromServices] UserService userService)
        {
            var userId = GetCurrentUser().UserId;
            var serviceResult = userService.GetUserMenus(userId);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.PagedData, serviceResult.Messages);
            return Success(serviceResult.PagedData);
        }

        /// <summary>
        /// 获取 列表配置(当前用户)
        /// </summary>
        /// <param name="userService"></param>
        /// <returns></returns>
        [HttpGet]
        public APIResult<List<MenuItem>> GetListMenu_LayUI([FromServices] UserService userService)
        {
            MenuConfig menuConfig = SystemController.GetMenuConfigByName("PregnantInfo");
            return Success(menuConfig.MenuItems);

            //var userId = GetCurrentUser().UserId;
            //var serviceResult = userService.GetUserMenus(userId);
            //if (!serviceResult.IsSuccess)
            //    return Error(DefaultMenuItems, serviceResult.Messages);
            //var menuItems = DefaultMenuItems.Select(c => c).ToList();
            //menuItems.AddRange(serviceResult.Data.Select(c=>new MenuItem(ser)))

            //return Success(serviceResult.Data);
        }

        //static List<MenuItem> DefaultMenuItems = new List<MenuItem>()
        //{
        //    new MenuItem("1","","业务列表","",""),
        //    new MenuItem("11","1","孕妇档案","","../Home/PregnantInfoList"),
        //    new MenuItem("2","","自定义查询","",""),
        //    new MenuItem("3","","账户管理","",""),
        //    new MenuItem("31","3","用户管理","","../Home/AccountList"),
        //    new MenuItem("32","3","角色管理","",""),
        //    new MenuItem("4","","个人中心","",""),
        //    new MenuItem("41","4","修改密码","",""),
        //};

        #endregion
    }
}
