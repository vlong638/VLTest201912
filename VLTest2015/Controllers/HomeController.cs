using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace VLTest2015.Controllers
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

        static Dictionary<string, string> JsonConfigs = new Dictionary<string, string>();
        /// <summary>
        /// 获取下拉项        /// </summary>
        /// <param name="type"></param>
        /// <param name="isForceChange"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetDropDowns(string type,bool isForceChange)
        {
            //List<DropDownItem> names = new List<DropDownItem>()
            //{
            //    new DropDownItem("张三","张三"),
            //    new DropDownItem("李四","李四"),
            //    new DropDownItem("王五","王五"),
            //};
            //var file = (Path.Combine(AppContext.BaseDirectory, "JsonConfigs", "PersonNames.json"));
            //System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(names));


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
    }
}