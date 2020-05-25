using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleTest0213.SemiAutoExport
{      /// <summary>
       /// 配置文件
       /// </summary>
    public class ConfigEntity
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 特殊备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 输出项,存储结构
        /// </summary>
        public string Outputs { set; get; }
        /// <summary>
        /// 条件项,存储结构
        /// </summary>
        public string Conditions { set; get; }

        #region compute fields

        /// <summary>
        /// 输出项,逻辑结构
        /// </summary>
        public ConfigEntityOutputCollection ConfigEntityOutputs { set; get; }
        /// <summary>
        /// 条件项,逻辑结构
        /// </summary>
        public ConfigEntityConditionCollection ConfigEntityConditions { set; get; }

        internal string GetSQL()
        {
            var sqls1 = ConfigEntityConditions.GetJoinSQLs();
            var sqls2 = ConfigEntityOutputs.GetJoinSQLs();
            return $@"
select {ConfigEntityOutputs.GetFieldSQL()}
from Main
{string.Join(Environment.NewLine, sqls1.Concat(sqls2).Distinct())}
{ConfigEntityConditions.GetCondtions()}";
        }
        #endregion
    }
}
