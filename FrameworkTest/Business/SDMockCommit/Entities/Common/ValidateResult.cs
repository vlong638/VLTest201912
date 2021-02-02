using FrameworkTest.Common.ValuesSolution;
using System;
using System.ComponentModel;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateResult
    {
        public ValidateResult(ValidateResultCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public ValidateResultCode Code { set; get; }
        public string Message { set; get; }

        internal static string GetNullOrEmptyMessage(PropertyDescriptor propertyDescriptor)
        {
            return $"字段`{propertyDescriptor.GetDescription()}`数据不可为空";
        }
    }

    public enum ValidateResultCode
    {
        None = 0,
        Success =1,
        [Description("数据不可为空")]
        IsNullOrEmpty =2,
    }
}
