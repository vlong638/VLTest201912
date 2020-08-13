using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Research.Models
{
    /// <summary>
    /// 页面配置
    /// </summary>
    public class DetailConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static string RootElementName = "Views";
        /// <summary>
        /// 
        /// </summary>
        public static string NodeElementName = "View";

        /// <summary>
        /// 页面名称
        /// </summary>
        public string ViewName { set; get; }

        /// <summary>
        /// 页面字段
        /// </summary>
        public List<DetailConfig_Card> cards { set; get; }
        /// <summary>
        /// 用于 详情|编辑 时的查询接口
        /// </summary>
        public string getUrl { set; get; }
        /// <summary>
        /// 用于查询接口的参数
        /// </summary>
        public List<string> getUrl_param { set; get; }
        /// <summary>
        /// 用于查询接口的参数
        /// </summary>
        public string saveUrl { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DetailConfig()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public DetailConfig(XElement element)
        {
            ViewName = element.Attribute(nameof(ViewName)).Value;
            cards = element.Descendants(DetailConfig_Card.ElementName).Select(c => new DetailConfig_Card(c)).ToList();
            getUrl = element.Attribute(nameof(getUrl))?.Value;
            getUrl_param = element.Attribute(nameof(getUrl_param))?.Value.Split(",").ToList() ?? new List<string>();
            saveUrl = element.Attribute(nameof(saveUrl))?.Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DetailConfig_Card
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Group";

        /// <summary>
        /// 
        /// </summary>
        public DetailConfig_Card()
        {
            text = "";
            content = new List<List<DetailConfig_Card_Content>>();
            isTextArea = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public DetailConfig_Card(XElement element)
        {
            text = element.Attribute(nameof(text))?.Value;
            var innerContent = element.Descendants(DetailConfig_Card_Content.ElementName)
                .Select(c => new DetailConfig_Card_Content(c))
                .ToList() ?? new List<DetailConfig_Card_Content>();
            this.content = new List<List<DetailConfig_Card_Content>>() { innerContent };
            isTextArea = false;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string text { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public List<List<DetailConfig_Card_Content>> content { set; get; }

        //以下内容弃用
        //"isTextArea": "<是否文本域>", 
        public bool isTextArea { set; get; }
        //"lineNum": "<文本域行数(高度)>",
        //"areaText": "<文本域字段中文名>",
        //"areaParam": "<文本域字段名>",
        //"areaValue": "<文本域默认值>",
        //"areaRequired": "<文本域是否必填>"
    }
    /// <summary>
    /// 
    /// </summary>
    public class DetailConfig_Card_Content
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Section";

        /// <summary>
        /// 
        /// </summary>
        public DetailConfig_Card_Content()
        {
            width = 0;
            child = new List<DetailConfig_Card_Content_Child>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public DetailConfig_Card_Content(XElement element)
        {
            width = element.Attribute(nameof(width))?.Value.ToInt() ?? 0;
            child = element.Descendants(DetailConfig_Card_Content_Child.ElementName)
                .Select(c => new DetailConfig_Card_Content_Child(c))
                .ToList() ?? new List<DetailConfig_Card_Content_Child>();
        }

        /// <summary>
        /// 标题
        /// </summary>
        public int width { set; get; }
        /// <summary>
        /// 比较操作符
        /// </summary>
        public List<DetailConfig_Card_Content_Child> child { set; get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DetailConfig_Card_Content_Child
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Item";

        /// <summary>
        /// 
        /// </summary>
        public DetailConfig_Card_Content_Child()
        {
            width = 0;
            attr = new DetailConfig_Card_Content_Child_Attribute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public DetailConfig_Card_Content_Child(XElement element)
        {
            width = element.Attribute(nameof(width))?.Value.ToInt() ?? 0;
            attr = new DetailConfig_Card_Content_Child_Attribute(element);
        }

        /// <summary>
        /// 所占单位[12个单位为一行]
        /// </summary>
        public int width { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DetailConfig_Card_Content_Child_Attribute attr { set; get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DetailConfig_Card_Content_Child_Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Attribute";

        /// <summary>
        /// 
        /// </summary>
        public DetailConfig_Card_Content_Child_Attribute()
        {
            text = "";
            type = 0;
            range = false;
            lineNum = 0;
            multiple = false;
            required = false;
            param = "";
            value = "";
            options = new List<Config_Option>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public DetailConfig_Card_Content_Child_Attribute(XElement element)
        {
            text = element.Attribute(nameof(text))?.Value;
            type = element.Attribute(nameof(type))?.Value.ToInt() ?? 0;
            range = element.Attribute(nameof(range))?.Value.ToBool() ?? false;
            lineNum = element.Attribute(nameof(lineNum))?.Value.ToInt() ?? 0;
            multiple = element.Attribute(nameof(multiple))?.Value.ToBool() ?? false;
            required = element.Attribute(nameof(required))?.Value.ToBool() ?? false;
            param = element.Attribute(nameof(param))?.Value;
            value = element.Attribute(nameof(value))?.Value;
            options = new GetListConfigModel_Search_Options(element.Attribute(nameof(options))?.Value);
        }

        //"text": "<字段中文名>",
        public string text { set; get; }
        //"type": "<类型[1-文本框|2-数字框|3-下拉框|4-单选框|5-复选框|6-开关|7-日期选择框|8-文本域]>",
        public int type { set; get; }
        //"range": "<是否开启范围输入[适用于日期选择框和数字框]>",
        public bool range { set; get; }
        //"lineNum": "<文本域行数>",
        public int lineNum { set; get; }
        //"multiple": "<是否多选[适用于下拉框]>",
        public bool multiple { set; get; }
        //"required": "<是否必填>",
        public bool required { set; get; }
        //"param": "<字段名>",
        public string param { set; get; }
        //"value": "<默认值>",
        public string value { set; get; }
        //"options": "<下拉框内容|待选项[{name: '张三', value: 1}]>"
        /// <summary>
        /// 下拉项的下拉项
        /// </summary>
        public List<Config_Option> options { set; get; }
    }
}
