using System.Web;
using System.Web.Optimization;

namespace FS.SyncManager
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/vl").Include(
                        "~/bin/Common/Scripts/Common-{version}.js"
                        , "~/bin/Common/Scripts/Common_EasyUI-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            #region easyUI
            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                        "~/Scripts/jquery.easyui-{version}.js"
                        , "~/Scripts/jquery.easyui-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/easyui").Include(
                         "~/Content/easyui/color.css"
                        , "~/Content/easyui/demo.css"
                        , "~/Content/easyui/easyui.css"
                        , "~/Content/easyui/icon.css")); 
            #endregion

            //@* easyUI *@
            //<link rel="stylesheet" type="text/css" href="https://www.jeasyui.com/easyui/themes/default/easyui.css">
            //<link rel="stylesheet" type="text/css" href="https://www.jeasyui.com/easyui/themes/icon.css">
            //<link rel="stylesheet" type="text/css" href="https://www.jeasyui.com/easyui/themes/color.css">
            //<link rel="stylesheet" type="text/css" href="https://www.jeasyui.com/easyui/demo/demo.css">

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            #region bootstrap

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css")); 

            #endregion
        }
    }
}
