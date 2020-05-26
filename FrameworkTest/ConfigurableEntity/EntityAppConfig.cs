using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.ConfigurableEntity
{
    class EntityAppConfig
    {
        public long Id { set; get; }
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 说明(显示,编辑时易理解)
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 列名(显示)
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 数据类型(显示,校验)
        /// </summary>
        public string DataType { set; get; }
        /// <summary>
        /// 最大值(校验)
        /// </summary>
        public long MaxValue { set; get; }
        /// <summary>
        /// 最小值(校验)
        /// </summary>
        public long MinValue { set; get; }
        /// <summary>
        /// 是否允许空(校验)
        /// </summary>
        public bool IsNullable { set; get; }
    }
}
