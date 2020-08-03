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
                    return Error(new GetListConfigModel(), "无效的用户配置");
                }
                var viewConfig = serviceResult.Data.ViewConfig.FromJson<ViewConfig>();
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
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListPages.xml");
            XDocument doc = XDocument.Load(path);
            var viewElements = doc.Descendants(ViewConfig.NodeElementName);
            var viewConfigs = viewElements.Select(c => new ViewConfig(c));
            var viewConfig = viewConfigs.FirstOrDefault(c => c.ViewName == request.ListName);
            viewConfig.Properties.RemoveAll(c => !c.IsNeedOnPage);
            var result = new GetListConfigModel()
            {
                CustomConfigId = request.CustomConfigId,
                ViewConfig = viewConfig,
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
                return Error(serviceResult.Data, serviceResult.Messages);
            return Success(serviceResult.Data);
        }
        #endregion
    }
}
