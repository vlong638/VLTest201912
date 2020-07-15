using FrameworkTest.Common.ControllerSolution;
using FrameworkTest.ConfigurableEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FS.SyncManager.Controllers
{
    public class HomeController : Controller
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

        #region XMLConfig

        public class GetListConfigRequest
        {
            public string ListName { set; get; }
            public long CustomConfigId { set; get; }
        }

        public class GetListConfigResponse
        {
            public long CustomConfigId { set; get; }
            public EntityAppConfig ViewConfig { set; get; }
        }

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
                //var serviceResult = UserService.GetUserMenuById(request.CustomConfigId);
                //if (!serviceResult.IsSuccess)
                //{
                //    return Error(serviceResult.Data, "无效的用户配置");
                //}
                //var viewConfig = serviceResult.Data.EntityAppConfig.FromJson<EntityAppConfig>();
                //var result = new GetListConfigResponse()
                //{
                //    CustomConfigId = request.CustomConfigId,
                //    ViewConfig = viewConfig,
                //};
                //return Json(new APIResult<GetListConfigResponse>(result));
                return null;
            }
            else
            {
                var viewConfig = LoadDefaultConfig(request.ListName);
                viewConfig.Properties.RemoveAll(c => !c.IsNeedOnPage);
                var result = new GetListConfigResponse()
                {
                    CustomConfigId = request.CustomConfigId,
                    ViewConfig = viewConfig,
                };
                return Json(new APIResult<GetListConfigResponse>(result));
            }
        }

        public static EntityAppConfig LoadDefaultConfig(string listName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListPages.xml");
            XDocument doc = XDocument.Load(path);
            var viewElements = doc.Descendants(EntityAppConfig.NodeElementName);
            var viewConfigs = viewElements.Select(c => new EntityAppConfig(c));
            var viewConfig = viewConfigs.FirstOrDefault(c => c.ViewName == listName);
            return viewConfig;
        }

        #endregion
    }
}