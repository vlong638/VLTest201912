using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using VLTest2015.Common.Controllers;
using VLTest2015.Common.Models.RequestDTO;
using VLTest2015.Services;
using VLTest2015.Utils;

namespace VLTest2015.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region JsonConfig
        static Dictionary<string, string> JsonConfigs = new Dictionary<string, string>();
        /// <summary>
        /// 获取下拉项        /// </summary>
        /// <param name="type"></param>
        /// <param name="isForceChange"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetDropDowns(string type, bool isForceChange)
        {
            var file = (Path.Combine(AppContext.BaseDirectory, "JsonConfigs", type + ".json"));
            if (!System.IO.File.Exists(file))
            {
                List<DropDownItem> values = new List<DropDownItem>()
                {
                    new DropDownItem("请联系管理员配置","请联系管理员配置"),
                };
                System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(values));
                return Json(values, JsonRequestBehavior.AllowGet);
            }
            var data = System.IO.File.ReadAllText(file);
            var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DropDownItem>>(data);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }
        public class DropDownItem
        {
            public DropDownItem(string text, string value)
            {
                this.text = text;
                this.value = value;
            }

            public string text { set; get; }
            public string value { set; get; }
        }
        #endregion

        #region XMLConfig

        /// <summary>
        /// 列表配置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isForceChange"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetListConfig(GetListConfigRequest request)
        {
            if (request.CustomConfigId > 0)
            {
                var serviceResult = UserService.GetUserMenuById(request.CustomConfigId);
                if (!serviceResult.IsSuccess)
                {
                    return Error(serviceResult.Data, "无效的用户配置");
                }
                var viewConfig = serviceResult.Data.EntityAppConfig.FromJson<EntityAppConfig>();
                var result = new GetListConfigResponse()
                {
                    CustomConfigId = request.CustomConfigId,
                    ViewConfig = viewConfig,
                };
                return Json(new APIResult<GetListConfigResponse>(result));
            }
            else
            {
                return LoadDefaultConfig(request);
            }
        }

        private JsonResult LoadDefaultConfig(GetListConfigRequest request)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListPages.xml");
            XDocument doc = XDocument.Load(path);
            var viewElements = doc.Descendants(EntityAppConfig.NodeElementName);
            var viewConfigs = viewElements.Select(c => new EntityAppConfig(c));
            var viewConfig = viewConfigs.FirstOrDefault(c => c.ViewName == request.ListName);
            viewConfig.Properties.RemoveAll(c => !c.IsNeedOnPage);
            var result = new GetListConfigResponse()
            {
                CustomConfigId = request.CustomConfigId,
                ViewConfig = viewConfig,
            };
            return Json(new APIResult<GetListConfigResponse>(result));
        }

        [HttpPost]
        public JsonResult SaveListConfig(SaveListConfigRequest request)
        {
            var userMenu = new UserMenu()
            {
                Id = request.CustomConfigId,
                UserId = GetCurrentUser().UserId,
                MenuName = request.ListName,
                URL = request.URL,
                EntityAppConfig = request.ViewConfig.ToJson(),
            };
            if (request.CustomConfigId > 0)
            {
                var serviceResult = UserService.UpdateUserMenu(userMenu);
                if (!serviceResult.IsSuccess)
                    return Error(serviceResult.Data, serviceResult.Messages);
                return Success(serviceResult.Data);
            }
            else
            {
                var serviceResult = UserService.CreateUserMenu(userMenu);
                if (!serviceResult.IsSuccess)
                    return Error(serviceResult.Data, serviceResult.Messages);
                return Success(serviceResult.Data);
            }
        }

        [HttpGet]
        public JsonResult GetListMenu()
        {
            var userId = GetCurrentUser().UserId;
            var serviceResult = UserService.GetUserMenus(userId);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Success(serviceResult.Data);
        }

        #endregion

        #region 登陆模拟

        public ActionResult RSATest()
        {
            return View();
        }

        #endregion
    }
}