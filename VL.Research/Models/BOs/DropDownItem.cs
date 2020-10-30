using Dapper.Contrib.Extensions;

namespace BBee.Models
{
    /// <summary>
    /// 下拉项
    /// </summary>
    public class DropDownItem
    {
        /// <summary>
        /// 下拉项
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public DropDownItem(string text, string value)
        {
            this.text = text;
            this.value = value;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string text { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string value { set; get; }
    }
}
