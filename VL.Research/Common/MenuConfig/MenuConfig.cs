using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Research.Common
{
    /// <summary>
    /// 查询sql配置
    /// </summary>
    public class MenuConfig
    {
        #region 预设配置
        /// <summary>
        /// 
        /// </summary>
        public static string RootElementName = "Menus";
        /// <summary>
        /// 
        /// </summary>
        public static string NodeElementName = "Menu";


        /// <summary>
        /// 页面字段
        /// </summary>
        public List<MenuItem> MenuItems { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public MenuConfig()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public MenuConfig(XElement element)
        {
            MenuItems = element.Descendants(NodeElementName).Select(c => new MenuItem(c)).ToList();
        }
        #endregion
    }

    /// <summary>
    /// 页面菜单
    /// </summary>
    public class MenuItem
    {

        /// <summary>
        /// /
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="url"></param>
        public MenuItem(string id, string parentId, string text, string icon, string url)
        {
            this.id = id;
            this.parentId = parentId;
            this.text = text;
            this.icon = icon;
            this.url = url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public MenuItem(XElement element)
        {
            this.id = element.Attribute(nameof(id)).Value;
            this.parentId = element.Attribute(nameof(parentId)).Value;
            this.text = element.Attribute(nameof(text)).Value;
            this.icon = element.Attribute(nameof(icon)).Value;
            this.url = element.Attribute(nameof(url)).Value;
        }

        /// <summary>
        /// 节点Id
        /// </summary>
        public string id { set; get; }
        /// <summary>
        /// 上级节点id
        /// </summary>
        public string parentId { set; get; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string text { set; get; }
        /// <summary>
        /// 图标
        /// </summary>
        public string icon { set; get; }
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string url { set; get; }
    }
}
