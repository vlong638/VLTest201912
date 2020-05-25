using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest0213.SemiAutoExport
{
    /// <summary>
    /// XML配置,字段
    /// </summary>
    public class FileConfigField
    {
        public int SubFunctionField { set; get; }
        public string FieldSQL { set; get; }

        public FileConfigField(int subFunctionField, string fieldSQL)
        {
            SubFunctionField = subFunctionField;
            FieldSQL = fieldSQL;
        }
        public FileConfigField(int subFunctionField, string fieldSQL, ControlType controlType, Dictionary<int, string> keyValues) : this(subFunctionField, fieldSQL)
        {
            ControlType = controlType;
            KeyValues = keyValues;
        }

        #region 录入相关配置

        public ControlType ControlType { set; get; }
        public Dictionary<int, string> KeyValues { set; get; }

        #endregion
    }
}
