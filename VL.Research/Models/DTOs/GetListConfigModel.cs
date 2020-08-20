using System.Collections.Generic;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.Research.Common;

namespace VL.Research.Models
{
    /// <summary>
    /// 页面配置 模型
    /// </summary>
    public class GetListConfigModel
    {
        /// <summary>
        /// 个性化配置Id
        /// </summary>
        public long CustomConfigId { set; get; }

        /// <summary>
        /// 页面配置
        /// </summary>
        public ListConfig ListConfig { set; get; }

        /// <summary>
        /// 模版ID
        /// </summary>
        public long modelId { set; get; }
        /// <summary>
        /// 模版名称
        /// </summary>
        public string modelName { set; get; }
        /// <summary>
        /// 保存模版URL
        /// </summary>
        public string saveModelUrl { set; get; }
        /// <summary>
        /// 删除模版URL
        /// </summary>
        public string deleteModelUrl { set; get; }
        /// <summary>
        /// 搜索项配置
        /// </summary>
        public List<GetListConfigModel_Search> search { set; get; }
        /// <summary>
        /// 列表控件配置
        /// </summary>
        public GetListConfigModel_TableConfg table { set; get; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_Search
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string text { set; get; }
        /// <summary>
        /// 类型 1-文本框|2-数字框|3-单选下拉框|4-树状下拉框|5-日期选择框,默认:1
        /// </summary>
        public int type { set; get; }
        /// <summary>
        /// 输入项的输入值
        /// </summary>
        public string value { set; get; }
        /// <summary>
        /// 下拉项的下拉项
        /// </summary>
        public List<Config_Option> options { set; get; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class Config_Option
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string value { set; get; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool @checked { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetListConfigModel_Search_Options : List<Config_Option>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public GetListConfigModel_Search_Options(string options)
        {
            if (options.IsNullOrEmpty())
                return;

            var splits1 = options.Split("|");
            var defaultValue = splits1.Length > 1 ? splits1[0] : "";
            var optionStr = splits1.Length > 1 ? splits1[1] : options;
            var splitOptions = optionStr.Split(",");
            foreach (var splitOption in splitOptions)
            {
                var keyValue = splitOption.Split(":");

                var key = keyValue[0];
                var value = keyValue[1];
                var isDefaultValue = key == defaultValue;
                this.Add(new Config_Option()
                {
                    name = value,
                    value = key,
                    @checked = isDefaultValue,
                });
            }
        }
    }

    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_TableConfg
    {
        /// <summary>
        /// getList接口
        /// </summary>
        public string url { set; get; }
        /// <summary>
        /// 新增按钮配置
        /// </summary>
        public GetListConfigModel_TableConfg_AddButton add_btn { set; get; }
        /// <summary>
        /// 新增按钮配置
        /// </summary>
        public List<GetListConfigModel_TableConfg_ToolBar> line_toolbar { set; get; }
        /// <summary>
        /// 列表内容配置
        /// </summary>
        public GetListConfigModel_TableConfg_ViewModel toolbar_viewModel { set; get; }
        /// <summary>
        /// 是否分页[true|false][默认:true]
        /// </summary>
        public bool page { set; get; }
        /// <summary>
        /// 每页显示的条数[默认:10]
        /// </summary>
        public int limit { set; get; }
        /// <summary>
        /// 列表 排序
        /// </summary>
        public GetListConfigModel_TableConfg_InitSort initSort { set; get; }
        /// <summary>
        /// 列表 字段
        /// </summary>
        public List<List<GetListConfigModel_TableConfg_Col>> cols { set; get; }
        /// <summary>
        /// 列表 查询条件初始值
        /// </summary>
        public List<GetListConfigModel_TableConfg_Where> where { set; get; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_TableConfg_AddButton
    {
        /// <summary>
        /// 接口
        /// </summary>
        public string url { set; get; }
        /// <summary>
        /// 默认参数
        /// </summary>
        public string defaultParam { set; get; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string text { set; get; }
        /// <summary>
        /// window-弹窗|newPage-新页面
        /// </summary>
        public string type { set; get; }
        /// <summary>
        /// 窗体大小
        /// </summary>
        public List<string> area { get; set; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_TableConfg_ToolBar
    {

        /// <summary>
        /// 接口
        /// </summary>
        public string url { set; get; }
        /// <summary>
        /// 用于url的参数
        /// </summary>
        public List<string> @params { set; get; }
        /// <summary>
        /// 默认参数
        /// </summary>
        public List<string> defaultParam { set; get; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string text { set; get; }
        /// <summary>
        /// window-弹窗|newPage-新页面|confirm-提示
        /// </summary>
        public string type { set; get; }
        /// <summary>
        /// 提示文本
        /// </summary>
        public string desc { set; get; }
        /// <summary>
        /// 弹窗宽高 { 宽 } or { 宽,高 }
        /// </summary>
        public List<string> area { set; get; }
        /// <summary>
        /// 弹窗确认调用函数
        /// </summary>
        public string yesFun { get;  set; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_TableConfg_ViewModel_Model
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string text { set; get; }
        /// <summary>
        /// 1-文本框|2-文本域|3-下拉框|4-单选框|5-日期选择框
        /// </summary>
        public string type { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string param { set; get; }
        /// <summary>
        /// 默认值|下拉项内容
        /// </summary>
        public string value { set; get; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_TableConfg_ViewModel
    {
        /// <summary>
        /// 视图字段配置
        /// </summary>
        public List<GetListConfigModel_TableConfg_ViewModel_Model> model { set; get; } = new List<GetListConfigModel_TableConfg_ViewModel_Model>();
        /// <summary>
        /// 用于 详情|编辑 时的查询接口
        /// </summary>
        public string get_url { set; get; }
        /// <summary>
        /// 用于url的参数
        /// </summary>
        public List<string> @params { set; get; } = new List<string>();
        /// <summary>
        /// 用于 保存|编辑 时的保存接口
        /// </summary>
        public string save_url { set; get; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_TableConfg_InitSort
    {
        /// <summary>
        /// 需要排序的字段名
        /// </summary>
        public string field { set; get; }
        /// <summary>
        /// 排序方式[desc|asc
        /// </summary>
        public string type { set; get; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_TableConfg_Col
    {
        /// <summary>
        /// "field": "参数名1",
        /// </summary>
        public string field { set; get; }
        /// <summary>
        /// "title": "参数中文名",
        /// </summary>
        public string title { set; get; }
        /// <summary>
        /// "align": "内容对齐方式[center|right|left][默认:center]",
        /// </summary>
        public string align { set; get; }
        /// <summary>
        /// "templet": "模版[就是这里写什么,就显示什么]",
        /// </summary>
        public string templet { set; get; }
        /// <summary>
        /// "width": "单元格宽度[数字|百分比][默认:自动]",
        /// </summary>
        public string width { set; get; }
        /// <summary>
        /// "type": "列类型[normal-常规列|radio-单选框列|numbers-序号列][默认:normal]",
        /// </summary>
        public string type { set; get; }
        /// <summary>
        /// "fixed": "固定列[left|true-固定在左|right-固定在右][注意：如果是固定在左，该列必须放在表头最前面；如果是固定在右，该列必须放在表头最后面]",
        /// </summary>
        public string @fixed { set; get; }
        /// <summary>
        /// "sort": "是否允许排序[true|false][默认:false]",
        /// </summary>
        public bool sort { set; get; }
        /// <summary>
        /// "colspan": "单元格所占列数[用于多级表头][这种情况下不用设置field和width]",
        /// </summary>
        public string colspan { set; get; }
        /// <summary>
        /// "rowspan": "单元格所占行数[用于多级表头]"
        /// </summary>
        public string rowspan { set; get; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_TableConfg_Where
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string value { set; get; }
    }
}
