using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest0213.SemiAutoExport
{
    /// <summary>
    /// 配置文件输出项集合
    /// </summary>
    public class ConfigEntityOutputCollection : List<ConfigEntityOutput>
    {
        public static char SplitterItem = ',';
        public static char SplitterField = '_';
        public static char SplitterValue = '|';

        public ConfigEntityOutputCollection() { }
        public ConfigEntityOutputCollection(string str)
        {
            var itemStrs = str.Split(SplitterItem);
            foreach (var itemStr in itemStrs)
            {
                this.Add(new ConfigEntityOutput(itemStr));
            }
        }

        public override string ToString()
        {
            return string.Join(SplitterItem.ToString(), this.Select(c => c.ToString()));
        }

        internal string GetFieldSQL()
        {
            return string.Join(Environment.NewLine + ",", this.Select(c => string.Join(",", c.GetFieldSQLs())));
        }

        internal List<string> GetJoinSQLs()
        {
            return this.Select(c => c.GetJoinSQL()).ToList();
        }
    }

    /// <summary>
    /// 配置文件输出项
    /// </summary>
    public class ConfigEntityOutput
    {
        public int FunctionCategory { set; get; }
        public int SubFunctionCategory { set; get; }
        public List<int> SubFunctionFields { set; get; }

        #region sql
        List<string> FieldSQLs { set; get; }
        public List<string> GetFieldSQLs()
        {
            if (FieldSQLs==null)
            {
                FieldSQLs = new List<string>();
                foreach (var subFunctionField in SubFunctionFields)
                {
                    FieldSQLs.Add(ConfigContext.GetSQL(subFunctionField));
                }
            }
            return FieldSQLs;
        }
        string JoinSQL { set; get; }
        public string GetJoinSQL()
        {
            if (string.IsNullOrEmpty(JoinSQL))
            {
                JoinSQL = ConfigContext.GetSQL(SubFunctionCategory);
            }
            return JoinSQL;
        } 
        #endregion

        public ConfigEntityOutput() { }
        public ConfigEntityOutput(string str)
        {
            var values = str.Split(ConfigEntityOutputCollection.SplitterField);
            if (values.Count() == 3)
            {
                if (int.TryParse(values[0], out int functionCategory))
                {
                    FunctionCategory = functionCategory;
                }
                else
                {
                    throw new NotImplementedException("数据格式不服务规范");
                }
                if (int.TryParse(values[1], out int subFunctionCategory))
                {
                    SubFunctionCategory = subFunctionCategory;
                }
                else
                {
                    throw new NotImplementedException("数据格式不服务规范");
                }
                if (string.IsNullOrEmpty(values[2]))
                {
                    throw new NotImplementedException("数据格式不服务规范");
                }
                var subFunctionFieldStrs = values[2].Split(ConfigEntityOutputCollection.SplitterValue);
                foreach (var subFunctionFieldStr in subFunctionFieldStrs)
                {
                    if (int.TryParse(subFunctionFieldStr, out int subFunctionField))
                    {
                        SubFunctionFields.Add(subFunctionField);
                    }
                }
            }
        }

        public override string ToString()
        {
            return $@"{FunctionCategory
                + ConfigEntityConditionCollection.SplitterField.ToString()
                + SubFunctionCategory
                + ConfigEntityConditionCollection.SplitterField.ToString()
                + string.Join(ConfigEntityConditionCollection.SplitterValue.ToString(), SubFunctionFields)
                }";
        }
    }
}
