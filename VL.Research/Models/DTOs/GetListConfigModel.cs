﻿using Newtonsoft.Json;
using NPOI.SS.Formula.Atp;
using System;
using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.ValuesSolution;
using BBee.Common;

namespace BBee.Models
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
        /// <summary>
        /// 
        /// </summary>
        public GetListConfigModel_Export export { set; get; }
        public List<string> importJSFile { get; internal set; }
    }

    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_Export
    {
        /// <summary>
        /// 接口
        /// </summary>
        public string url { set; get; }
        /// <summary>
        /// 默认参数
        /// </summary>
        public List<VLKeyValue> defaultParam { set; get; }
    }

    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class GetListConfigModel_Search
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public GetListConfigModel_Search(ListConfigWhere c)
        {
            name = c.ComponentName;
            text = c.DisplayName;
            type = c.DisplayType.ToInt().Value;
            if (type == 5)
            {
                names = new List<string>() { c.ComponentName + "Start", c.ComponentName + "End" };
            }
            value = c.Value ?? "";
            required = c.Required;
            options = new GetListConfigModel_Search_Options(c.Options);
            treeOptions = new GetListConfigModel_Search_TreeOptions(c.TreeOptions);
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public List<string> names { set; get; }
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
        /// 是否必填
        /// </summary>
        public bool required { get; set; }

        /// <summary>
        /// 下拉项的下拉项
        /// </summary>
        public List<Config_Option> options { set; get; }
        /// <summary>
        /// 下拉项的下拉项
        /// </summary>
        public List<TreeConfig_Option> treeOptions { set; get; }
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
    /// 页面配置 模型子项
    /// </summary>
    public class TreeConfig_Option
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { set; get; }
        /// <summary>
        /// 上级Id
        /// </summary>
        public string parentId { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string title { set; get; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool spread { set; get; }
        /// <summary>
        /// 子项
        /// </summary>
        public List<TreeConfig_Option> children { set; get; } = new List<TreeConfig_Option>();
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
                if (keyValue.Count() != 2)
                    throw new NotImplementedException("错误的 Options 配置");
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
    /// 
    /// </summary>
    public class GetListConfigModel_Search_TreeOptions : List<TreeConfig_Option>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public GetListConfigModel_Search_TreeOptions(string options)
        {
            if (options.IsNullOrEmpty())
                return;

            var splitItems = options.Split("|");
            List<TreeConfig_Option> optionsTemp = new List<TreeConfig_Option>();
            foreach (var splitItem in splitItems)
            {
                var values = splitItem.Split(",");
                if (values.Count() != 4)
                    throw new NotImplementedException("错误的 TreeOptions 配置");

                optionsTemp.Add(new TreeConfig_Option()
                {
                    id  = values[0],
                    title= values[1],
                    parentId= values[2],
                    spread = values[3].ToBool().Value,
                });
            }
            //父子关系处理
            var treeOptions =BuildTree(optionsTemp);
            this.AddRange(treeOptions);
        }

        private List<TreeConfig_Option> BuildTree(List<TreeConfig_Option> optionsTemp)
        {
            List<TreeConfig_Option> treeOptions = new List<TreeConfig_Option>();
            foreach (var option in optionsTemp.Where(c => string.IsNullOrEmpty(c.parentId) || c.parentId == "0"))
            {
                BuildTree(option, optionsTemp);
                treeOptions.Add(option);
            }
            return treeOptions;
        }

        private void BuildTree(TreeConfig_Option rootOption, List<TreeConfig_Option> optionsTemp)
        {
            foreach (var option in optionsTemp)
            {
                if (option.parentId ==rootOption.id)
                {
                    BuildTree(option, optionsTemp);
                    rootOption.children.Add(option);
                }
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
        public List<VLNameValue> where { set; get; }

        /// <summary>
        /// 操作栏位置
        /// </summary>
        public string actionBar_position { get; set; }
        public string actionBar_width { get; internal set; }
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
        /// <summary>
        /// 弹窗传递的搜索参数
        /// </summary>
        public List<string> bringParam { get; set; }
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
        /// <summary>
        /// "colGroup": 用于多级表头中的多级叶节点
        /// </summary>
        public string colGroup { get; set; }
    }
    /// <summary>
    /// 页面配置 模型子项
    /// </summary>
    public class VLNameValue
    {
        /// <summary>
        /// 
        /// </summary>
        public VLNameValue()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public VLNameValue(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

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
